using System.Web.Mvc;

using Dilemma.Security.AccessFilters;

namespace Dilemma.Web.Controllers
{
    [QuestionSeederAccessFilter]
    public abstract class DilemmaBaseController : Controller
    {
    }
}