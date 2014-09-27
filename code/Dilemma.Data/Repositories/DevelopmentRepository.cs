using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Data.Repositories
{
    internal class DevelopmentRepository : IDevelopmentRepository
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository = Locator.Lazy<IAdministrationRepository>();

        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();
        
        public DevelopmentRepository()
        {
            var systemConfiguration = AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>();
            
            if (!systemConfiguration.IsInternalEnvironment)
            {
                throw new UnauthorizedAccessException();
            }
        }

        public void SetUserName(int userId, string name)
        {
            using (var context = new DilemmaContext())
            {
                var devUser = context.DevelopmentUsers.FirstOrDefault(x => x.UserId == userId);

                if (devUser == null || devUser.UserId != userId)
                {
                    devUser = new DevelopmentUser { UserId = userId, CreatedDateTime = TimeSource.Value.Now };

                    if (GetAnonymousUsers(context, new[] { devUser.UserId }).Any())
                    {
                        context.DevelopmentUsers.Add(devUser);
                    }
                }
                else
                {
                    context.Entry(devUser).State = EntityState.Modified;
                }

                devUser.Name = name;

                context.SaveChangesVerbose();
            }
        }

       public IEnumerable<T> GetList<T>(IEnumerable<int> userIds) where T : class 
        {
            using (var context = new DilemmaContext())
            {
                var users = context.DevelopmentUsers.Where(x => userIds.Contains(x.UserId)).OrderByDescending(x => x.CreatedDateTime).ToList();

                var missingUsers = userIds.Where(x => !users.Select(u => u.UserId).Contains(x))
                                          .Select(x => new DevelopmentUser { UserId = x, CreatedDateTime = TimeSource.Value.Now })
                                          .ToList();

                if (missingUsers.Any())
                {
                    var validIds = GetAnonymousUsers(context, missingUsers.Select(x => x.UserId));
                    context.DevelopmentUsers.AddRange(missingUsers.Where(x => validIds.Contains(x.UserId)));
                    context.SaveChangesVerbose();
                }

                return ConverterFactory.ConvertMany<DevelopmentUser, T>(missingUsers.Concat(users));
            }
        }

        public T Get<T>(int userId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                return ConverterFactory.ConvertOne<DevelopmentUser, T>(context.DevelopmentUsers.FirstOrDefault(x => x.UserId == userId));
            }
        }

        public bool CanLogin(int userId)
        {
            using (var context = new DilemmaContext())
            {
                var user = context.Users.FirstOrDefault(x => x.UserId == userId);
                return user.UserType == UserType.Anonymous;
            }
        }

        private static IEnumerable<int> GetAnonymousUsers(DilemmaContext context, IEnumerable<int> userIds)
        {
            return
                context.Users.Where(x => userIds.Contains(x.UserId))
                    .Where(x => x.UserType == UserType.Anonymous)
                    .Select(x => x.UserId)
                    .ToList();
        }
    }
}
