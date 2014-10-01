using System.Collections.Generic;

using Dilemma.Common;

namespace Dilemma.Data.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<T> GetAll<T>(int forUserId) where T : class;

        void Mute(int forUserId, NotificationLookupBy notificationLookupBy, int id);
    }
}