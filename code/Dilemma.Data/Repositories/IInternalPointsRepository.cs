using Dilemma.Common;
using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Points repository interface.
    /// </summary>
    internal interface IInternalPointsRepository
    {
        void AwardPoints(DilemmaContext context, int forUserId, PointType pointType, int? referenceId = null);
    }
}
