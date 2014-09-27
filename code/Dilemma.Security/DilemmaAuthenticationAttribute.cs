﻿using System;
using System.Web.Mvc;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Security
{
    public class DilemmaAuthenticationAttribute : ActionFilterAttribute
    {
        private readonly static Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SecurityManager.Value.CookieValidation();
            base.OnActionExecuting(filterContext);
        }
    }
}
