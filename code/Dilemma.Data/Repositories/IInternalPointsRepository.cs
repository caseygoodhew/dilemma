using Dilemma.Common;
using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Points repository interface.
    /// </summary>
    internal interface IInternalPointsRepository
    {
        int AwardPoints(DilemmaContext context, int forUserId, PointType pointType, int? referenceId = null);

        int RemovePoints(DilemmaContext dataContext, int userId, PointType voteRegistered, int questionId);
    }
}
