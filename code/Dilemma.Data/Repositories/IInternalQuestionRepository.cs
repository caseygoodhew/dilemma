using System.Collections.Generic;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;

using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Question repository methods that should not be externally available.
    /// </summary>
    internal interface IInternalQuestionRepository : IQuestionRepository
    {
        /// <summary>
        /// To be called when the moderation state is updated.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        void OnModerationStateUpdated(IMessenger<ModerationState> messenger);

        IEnumerable<int> GetBookmarkUserIds(DilemmaContext dataContext, int questionId);

        IEnumerable<int> GetAnswererUserIds(DilemmaContext dataContext, int questionId);
    }
}