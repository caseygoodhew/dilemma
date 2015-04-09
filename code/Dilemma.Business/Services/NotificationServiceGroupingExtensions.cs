using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.ViewModels;

using Disposable.Common;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Groups <see cref="InternalNotificationViewModel"/>s into <see cref="NotificationListViewModel"/>s.
    /// </summary>
    internal static class NotificationServiceGroupingExtensions
    {
        /// <summary>
        /// Groups <see cref="InternalNotificationViewModel"/>s into <see cref="NotificationListViewModel"/>s.
        /// </summary>
        /// <param name="enumerable">The <see cref="InternalNotificationViewModel"/>s to group.</param>
        /// <returns>The <see cref="NotificationListViewModel"/>s.</returns>
        internal static IEnumerable<NotificationListViewModel> GetGroupedQuestions(this IEnumerable<InternalNotificationViewModel> enumerable)
        {
            var notificationViewModels = enumerable as IList<InternalNotificationViewModel> ?? enumerable.ToList();
            notificationViewModels.ToList().ForEach(x => Guard.ArgumentNotNull(x.QuestionId, "QuestionId"));
            
            var grouped = notificationViewModels.GroupBy(x => new { QuestionId = x.QuestionId.Value, IsActioned = x.ActionedDateTime != null, x.NotificationType, x.NotificationTarget });

            return grouped.Select(
                x =>
                new
                    {
                        x.Key.QuestionId,
                        x.Key.NotificationType,
                        x.Key.NotificationTarget,
                        x.Key.IsActioned,
                        Items = x.OrderByDescending(i => i.CreatedDateTime)
                    })
                .Select(
                    x =>
                    new NotificationListViewModel
                        {
                            IsActioned = x.IsActioned,
                            CreatedDateTime = x.Items.First().CreatedDateTime,
                            NotificationType = x.NotificationType,
                            NotificationTarget = x.NotificationTarget,
                            Occurrences = x.Items.Count(),
                            TotalPointsAwarded = x.Items.Where(i => i.PointsAwarded.HasValue).Sum(i => i.PointsAwarded.Value),
                            RouteDataValue = x.QuestionId
                        });
        }

        /// <summary>
        /// Groups <see cref="InternalNotificationViewModel"/>s into <see cref="NotificationListViewModel"/>s.
        /// </summary>
        /// <param name="enumerable">The <see cref="InternalNotificationViewModel"/>s to group.</param>
        /// <returns>The <see cref="NotificationListViewModel"/>s.</returns>
        internal static IEnumerable<NotificationListViewModel> GetGroupedAnswers(this IEnumerable<InternalNotificationViewModel> enumerable)
        {
            var notificationViewModels = enumerable as IList<InternalNotificationViewModel> ?? enumerable.ToList();
            notificationViewModels.ToList().ForEach(x => Guard.ArgumentNotNull(x.AnswerId, "AnswerId"));

            var grouped = notificationViewModels.GroupBy(x => new { AnswerId = x.AnswerId.Value, IsActioned = x.ActionedDateTime != null, x.NotificationType, x.NotificationTarget });

            return grouped.Select(
                x =>
                new
                {
                    x.Key.AnswerId,
                    x.Key.NotificationType,
                    x.Key.NotificationTarget,
                    x.Key.IsActioned,
                    Items = x.OrderByDescending(i => i.CreatedDateTime)
                })
                .Select(
                    x =>
                    new NotificationListViewModel
                    {
                        IsActioned = x.IsActioned,
                        CreatedDateTime = x.Items.First().CreatedDateTime,
                        NotificationType = x.NotificationType,
                        NotificationTarget = x.NotificationTarget,
                        Occurrences = x.Items.Count(),
                        TotalPointsAwarded = x.Items.Where(i => i.PointsAwarded.HasValue).Sum(i => i.PointsAwarded.Value),
                        RouteDataValue = x.AnswerId
                    });
        }

        /// <summary>
        /// Groups <see cref="InternalNotificationViewModel"/>s into <see cref="NotificationListViewModel"/>s.
        /// </summary>
        /// <param name="enumerable">The <see cref="InternalNotificationViewModel"/>s to group.</param>
        /// <returns>The <see cref="NotificationListViewModel"/>s.</returns>
        internal static IEnumerable<NotificationListViewModel> GetGroupedFollowups(this IEnumerable<InternalNotificationViewModel> enumerable)
        {
            var notificationViewModels = enumerable as IList<InternalNotificationViewModel> ?? enumerable.ToList();
            notificationViewModels.ToList().ForEach(x => Guard.ArgumentNotNull(x.FollowupId, "FollowupId"));

            var grouped = notificationViewModels.GroupBy(x => new { FollowupId = x.FollowupId.Value, IsActioned = x.ActionedDateTime != null, x.NotificationType, x.NotificationTarget });

            return grouped.Select(
                x =>
                new
                {
                    x.Key.FollowupId,
                    x.Key.NotificationType,
                    x.Key.NotificationTarget,
                    x.Key.IsActioned,
                    Items = x.OrderByDescending(i => i.CreatedDateTime)
                })
                .Select(
                    x =>
                    new NotificationListViewModel
                    {
                        IsActioned = x.IsActioned,
                        CreatedDateTime = x.Items.First().CreatedDateTime,
                        NotificationType = x.NotificationType,
                        NotificationTarget = x.NotificationTarget,
                        Occurrences = x.Items.Count(),
                        TotalPointsAwarded = x.Items.Where(i => i.PointsAwarded.HasValue).Sum(i => i.PointsAwarded.Value),
                        RouteDataValue = x.FollowupId
                    });
        }
    }
}