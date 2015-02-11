using System;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Data.Repositories
{
    internal class PointsRepository : IInternalPointsRepository
    {
        private readonly static Lazy<IAdministrationRepository> AdministrationRepository = Locator.Lazy<IAdministrationRepository>();
        
        private readonly static Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        public void AwardPoints(DilemmaContext context, int forUserId, PointType pointType, int? referenceId = null)
        {
            var points =
                AdministrationRepository.Value.GetPointConfigurations<PointConfiguration>()
                    .Single(x => x.PointType == pointType)
                    .Points;

            var forUser = context.GetOrAttachNew<User, int>(forUserId, x => x.UserId);

            Question relatedQuestion = null;

            if (referenceId.HasValue)
            {
                switch (pointType)
                {
                    case PointType.QuestionAsked:
                    case PointType.AnswerProvided:
                        relatedQuestion = context.GetOrAttachNew<Question, int>(referenceId.Value, x => x.QuestionId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("pointType");
                }
            }

            context.UserPoints.Add(
                new UserPoint
                    {
                        CreatedDateTime = TimeSource.Value.Now,
                        PointType = pointType,
                        PointsAwarded = points,
                        ForUser = forUser,
                        RelatedQuestion = relatedQuestion
                    });

            context.SaveChangesVerbose();
        }
    }
}