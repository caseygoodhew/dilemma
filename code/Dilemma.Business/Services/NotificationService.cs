using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Repositories;
using Dilemma.Security;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    internal class NotificationService : INotificationService
    {
        private static readonly Lazy<INotificationRepository> NotificationRepository = new Lazy<INotificationRepository>(Locator.Current.Instance<INotificationRepository>);

        private static readonly Lazy<ISecurityManager> SecurityManager = new Lazy<ISecurityManager>(Locator.Current.Instance<ISecurityManager>);

        public IEnumerable<NotificationListViewModel> GetAll()
        {
            var all = NotificationRepository.Value.GetAll<NEWNotificationViewModel>(SecurityManager.Value.GetUserId()).ToList();
            
            var byQuestions = all.Where(x => x.QuestionId != null).GetGroupedQuestions();

            if (all.Any(x => x.QuestionId == null))
            {
                throw new NotImplementedException();
            }

            return byQuestions;
        }

        public void Mute(NotificationType notificationType, int id)
        {
            NotificationRepository.Value.Mute(SecurityManager.Value.GetUserId(), notificationType, id);
        }
    }

    internal static class NotificationServiceGroupingExtensions
    {
        internal static IEnumerable<NotificationListViewModel> GetGroupedQuestions(this IEnumerable<NEWNotificationViewModel> enumerable)
        {
            var grouped = enumerable.GroupBy(x => new { QuestionId = x.QuestionId.Value, IsActioned = x.ActionedDateTime != null, x.NotificationType });

            return grouped.Select(
                x =>
                new
                    {
                        x.Key.QuestionId,
                        x.Key.NotificationType,
                        x.Key.IsActioned,
                        Items = x.OrderByDescending(i => i.CreatedDateTime)
                    })
                .Select(
                    x =>
                    new NotificationListViewModel
                        {
                            IsActioned = x.IsActioned,
                            DateTime = x.Items.First().CreatedDateTime,
                            NotificationType = x.NotificationType,
                            Occurrences = x.Items.Count(),
                            RouteDataValue = x.QuestionId
                        });
        }
    }
}