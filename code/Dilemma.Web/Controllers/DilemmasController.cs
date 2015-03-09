using System.Web.Mvc;

namespace Dilemma.Web.Controllers
{
    public class DilemmasController : DilemmaBaseController
    {
        //
        // GET: /Dilemmas/
        public ActionResult Index()
        {
            return View();
        }
    }
}
