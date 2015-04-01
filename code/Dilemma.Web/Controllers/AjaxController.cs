using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Dilemma.Web.DataTransferObjects;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [AllowUserRole(UserRole.Public)]
    public class AjaxController : DilemmaBaseController
    {
        private static readonly Lazy<IQuestionService> QuestionService = Locator.Lazy<IQuestionService>();
            
        [HttpPost]
        public JsonResult Vote(VoteDto vote)
        {
            QuestionService.Value.RegisterVote(vote.AnswerId);
            
            return Json(new { success = true });
        }
	}
}