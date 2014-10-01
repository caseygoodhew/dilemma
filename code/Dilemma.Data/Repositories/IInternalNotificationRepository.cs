using Dilemma.Common;
using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Repositories
{
    internal interface IInternalNotificationRepository : INotificationRepository
    {
        void Raise(DilemmaContext context, int forUserId, NotificationType notificationType, int id);

        void Delete(DilemmaContext context, NotificationLookupBy notificationLookupBy, int id);
    }
}