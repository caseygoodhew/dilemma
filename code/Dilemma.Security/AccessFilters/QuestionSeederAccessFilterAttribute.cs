using System.Linq;
using System.Web.Mvc;

using Dilemma.Common;
using Dilemma.Data.Models;

namespace Dilemma.Security.AccessFilters
{
    public class QuestionSeederAccessFilterAttribute : UnlockableAccessFilterAttribute
    {
        public QuestionSeederAccessFilterAttribute() : base("QuestionSeederUnlockKey", "Question", "Seeder", false)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var serverRole =
                    AdministrationRepository.Value.GetServerConfiguration<ServerConfiguration>().ServerRole;

            if (serverRole != ServerRole.QuestionSeeder)
            {
                return;
            }

            var uafType = typeof(UnlockableAccessFilterAttribute);
            
            // if theres an UnlockableAccessFilterAttribute on the action, relinquish control
            var uaf = filterContext.ActionDescriptor.GetCustomAttributes(uafType, false).Cast<UnlockableAccessFilterAttribute>();
            if (uaf.Any(x => !(x is QuestionSeederAccessFilterAttribute)))
            {
                return;
            }

            // if theres an UnlockableAccessFilterAttribute on the controller, relinquish control
            uaf = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(uafType, false).Cast<UnlockableAccessFilterAttribute>();
            if (uaf.Any(x => !(x is QuestionSeederAccessFilterAttribute)))
            {
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}