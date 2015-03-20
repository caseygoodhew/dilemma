using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Web.ViewModels;

using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    public class DilemmasController : DilemmaBaseController
    {
        private static readonly Lazy<ISiteService> SiteService =
            Locator.Lazy<ISiteService>();

        
        public ActionResult Index()
        {
            return Index(string.Empty);
        }

        //
        // GET: /Dilemmas/
        [Route("dilemmas/{category:alpha}")]
        public ActionResult Index(string category)
        {
            ViewBag.Category = string.IsNullOrEmpty(category) ? "all" : category;
            
            return View(TestData.GetDilemmasViewModel());
        }

        //
        // GET: /Dilemmas/QuestionId
        [Route("dilemmas/{questionId:int:min(1)}")]
        public ActionResult Index(int questionId)
        {
            // Not implemented
            return View();
        }
    }
}
