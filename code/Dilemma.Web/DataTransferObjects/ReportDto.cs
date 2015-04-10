using Dilemma.Common;

namespace Dilemma.Web.DataTransferObjects
{
    public class ReportDto
    {
        public int? QuestionId { get; set; }

        public int? AnswerId { get; set; }

        public int? FollowupId { get; set; }

        public ReportReason ReportReason { get; set; }
    }
}