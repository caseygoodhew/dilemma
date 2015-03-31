using System.Web.Mvc;

namespace Dilemma.Web.Controllers
{
    public class AnswersController : DilemmaBaseController
    {
        //
        // GET: /advise/QuestionId
        [Route("advise/{questionId:int:min(1)}")]
        public ActionResult Index(int questionId)
        {
            var viewModel = XTestData.GetDilemmaDetails();
            return View(viewModel);
        }
    }
}