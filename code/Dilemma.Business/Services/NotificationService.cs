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
        private static readonly Lazy<INotificationRepository> NotificationRepository = Locator.Lazy<INotificationRepository>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        public IEnumerable<NotificationListViewModel> GetAll()
        {
            var all = NotificationRepository.Value.GetAll<NotificationViewModel>(SecurityManager.Value.GetUserId()).ToList();
            
            var byQuestions = all.Where(x => x.QuestionId != null).GetGroupedQuestions();

            var remaining = all.Where(x => x.ModerationId != null).Select(x => new NotificationListViewModel
                                                                                 {
                                                                                     DateTime = x.CreatedDateTime,
                                                                                     IsActioned = x.ActionedDateTime != null,
                                                                                     RouteDataValue = x.ModerationId.Value,
                                                                                     NotificationType = x.NotificationType,
                                                                                     Occurrences = 1
                                                                                 });

            return byQuestions.Concat(remaining).OrderByDescending(x => x.DateTime);
        }

        public void Mute(NotificationLookupBy notificationLookupBy, int id)
        {
            NotificationRepository.Value.Mute(SecurityManager.Value.GetUserId(), notificationLookupBy, id);
        }
    }

    internal static class NotificationServiceGroupingExtensions
    {
        internal static IEnumerable<NotificationListViewModel> GetGroupedQuestions(this IEnumerable<NotificationViewModel> enumerable)
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