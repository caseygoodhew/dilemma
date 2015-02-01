using Dilemma.Common;
using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.AccessFilters
{
    public class ModerationAccessFilterAttribute : UnlockableAccessFilterAttribute
    {
        public ModerationAccessFilterAttribute()
            : base(AllowDeny.Allow, ServerRole.Moderation, "Home", "Index", "AdministrationUnlockKey")
        {
        }
    }
}