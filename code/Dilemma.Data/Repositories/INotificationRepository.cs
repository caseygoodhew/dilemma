using System.Collections.Generic;

using Dilemma.Common;

namespace Dilemma.Data.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<T> GetAll<T>(int forUserId) where T : class;

        void Raise(int forUserId, NotificationType notificationType, int id);

        void Mute(int forUserId, NotificationType notificationType, int id);

        void Delete(int forUserId, NotificationType notificationType, int id);
    }
}