using System;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Data.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);
        
        public int CreateAnonymousUser()
        {
            using (var context = new DilemmaContext())
            {
                var user = new User
                               {
                                   CreatedDateTime = TimeSource.Value.Now,
                                   UserType = UserType.Anonymous
                               };
                
                context.Users.Add(user);
                context.SaveChangesVerbose();
                
                return user.UserId;
            }
        }
    }
}
