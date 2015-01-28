using Dilemma.Common;
using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.AccessFilters
{
    public class QuestionSeederAccessFilterAttribute : UnlockableAccessFilterAttribute
    {
        public QuestionSeederAccessFilterAttribute()
            : base(AllowDeny.Deny, ServerRole.QuestionSeeder, "Question", "Seeder", "QuestionSeederUnlockKey")
        {
        }
    }
}