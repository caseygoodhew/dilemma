using Dilemma.Common;
using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.AccessFilters
{
    public class AdministrationAccessFilterAttribute : UnlockableAccessFilterAttribute
    {
        public AdministrationAccessFilterAttribute()
            : base(AllowDeny.Allow, ServerRole.Administration, "Home", "Index", "AdministrationUnlockKey")
        {
        }
    }
}