using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.ViewModels;

using Disposable.Common;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Groups <see cref="NotificationViewModel"/>s into <see cref="NotificationListViewModel"/>s.
    /// </summary>
    internal static class NotificationServiceGroupingExtensions
    {
        /// <summary>
        /// Groups <see cref="NotificationViewModel"/>s into <see cref="NotificationListViewModel"/>s.
        /// </summary>
        /// <param name="enumerable">The <see cref="NotificationViewModel"/>s to group.</param>
        /// <returns>The <see cref="NotificationListViewModel"/>s.</returns>
        internal static IEnumerable<NotificationListViewModel> GetGroupedQuestions(this IEnumerable<NotificationViewModel> enumerable)
        {
            var notificationViewModels = enumerable as IList<NotificationViewModel> ?? enumerable.ToList();
            notificationViewModels.ToList().ForEach(x => Guard.ArgumentNotNull(x.QuestionId, "QuestionId"));
            
            var grouped = notificationViewModels.GroupBy(x => new { QuestionId = x.QuestionId.Value, IsActioned = x.ActionedDateTime != null, x.NotificationType });

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
                            CreatedDateTime = x.Items.First().CreatedDateTime,
                            NotificationType = x.NotificationType,
                            Occurrences = x.Items.Count(),
                            RouteDataValue = x.QuestionId
                        });
        }
    }
}