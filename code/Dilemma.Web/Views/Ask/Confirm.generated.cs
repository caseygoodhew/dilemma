﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Views\Ask\Confirm.cshtml"
    using Dilemma.Business.ViewModels;
    
    #line default
    #line hidden
    using Dilemma.Common;
    using Dilemma.Web;
    using Dilemma.Web.Extensions;
    using Disposable.Common.Extensions;
    using Disposable.Web.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Ask/Confirm.cshtml")]
    public partial class _Views_Ask_Confirm_cshtml : System.Web.Mvc.WebViewPage<Dilemma.Web.ViewModels.DilemmaDetailsViewModel>
    {
        public _Views_Ask_Confirm_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\Ask\Confirm.cshtml"
  
    ViewBag.IsDilemmaViewPage = true;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" class=\"section-label js-sticky\"");

WriteLiteral(">\r\n\t<span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n        <span");

WriteLiteral(" data-icon=\"advise\"");

WriteLiteral("></span>\r\n        We hear you!\r\n    </span>\r\n</h1>\r\n\r\n\r\n\r\n<div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n\r\n\t<aside");

WriteLiteral(" class=\"sidebar\"");

WriteLiteral(">\r\n");

WriteLiteral("\t\t");

            
            #line 20 "..\..\Views\Ask\Confirm.cshtml"
   Write(Html.DisplayFor(x => x.Sidebar, "Sidebar"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\t</aside>\r\n\r\n\t<main");

WriteLiteral(" role=\"main\"");

WriteLiteral(" class=\"main\"");

WriteLiteral(">\r\n\r\n");

WriteLiteral("\t\t");

            
            #line 25 "..\..\Views\Ask\Confirm.cshtml"
   Write(Html.DisplayFor(x => x.QuestionDetails.QuestionViewModel, "QuestionSummary"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\t\t\r\n\t</main><!-- main-->\r\n\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591