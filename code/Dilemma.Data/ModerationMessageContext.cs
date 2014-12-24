using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data
{
    internal class ModerationMessageContext : DataMessageContext<ModerationState>
    {
        public readonly Moderation Moderation;

        public readonly ModerationEntry ModerationEntry;

        public ModerationMessageContext(ModerationState messageType, DilemmaContext dataContext, Moderation moderation, ModerationEntry moderationEntry)
            : base(messageType, dataContext)
        {
            Moderation = moderation;
            ModerationEntry = moderationEntry;
        }
    }
}