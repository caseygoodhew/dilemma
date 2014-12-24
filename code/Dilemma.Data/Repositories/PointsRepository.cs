using System;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Data.Repositories
{
    internal class PointsRepository : IPointsRepository
    {
        private readonly static Lazy<IInternalNotificationRepository> NotificationRepository = Locator.Lazy<IInternalNotificationRepository>();
        
        public void AwardPoints(DilemmaContext context, int forUserId, PointType pointType)
        {
            var points = context.PointConfigurations.Where(x => x.PointType == pointType).Select(x => new { x.Points }).Single();

            var user = context.Users.Single(x => x.UserId == forUserId);
            user.Points += points.Points;

            context.Users.Update(context, user);

            NotificationRepository.Value.Raise(context, forUserId, NotificationType.PointsAwarded, (int)pointType);
        }
    }
}