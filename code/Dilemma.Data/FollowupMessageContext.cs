using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data
{
    internal class FollowupMessageContext : DataMessageContext<FollowupDataAction>
    {
        public readonly Followup Followup;

        public FollowupMessageContext(FollowupDataAction messageType, DilemmaContext dataContext, Followup followup)
            : base(messageType, dataContext)
        {
            Followup = followup;
        }
    }
}