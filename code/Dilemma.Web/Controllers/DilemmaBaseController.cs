using System;
using System.Web.Mvc;

using Dilemma.Business.Services;
using Dilemma.Security.AccessFilters;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Web.Controllers
{
    [QuestionSeederAccessFilter]
    public abstract class DilemmaBaseController : Controller
    {
        private static readonly Lazy<ISiteService> SiteService =
            Locator.Lazy<ISiteService>();

        protected DilemmaBaseController()
        {
            ViewBag.Categories = SiteService.Value.GetCategories();
        }
    }
}