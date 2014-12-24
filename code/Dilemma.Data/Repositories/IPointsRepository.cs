using Dilemma.Common;
using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Points repository interface.
    /// </summary>
    internal interface IPointsRepository
    {
        void AwardPoints(DilemmaContext context, int forUserId, PointType pointType);
    }
}
