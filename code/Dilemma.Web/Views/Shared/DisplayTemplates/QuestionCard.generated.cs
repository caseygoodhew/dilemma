﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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
    using Dilemma.Common;
    using Dilemma.Web;
    using Dilemma.Web.Extensions;
    using Disposable.Common.Extensions;
    using Disposable.Web.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/QuestionCard.cshtml")]
    public partial class _Views_Shared_DisplayTemplates_QuestionCard_cshtml_ : System.Web.Mvc.WebViewPage<Dilemma.Business.ViewModels.QuestionViewModel>
    {
        public _Views_Shared_DisplayTemplates_QuestionCard_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"Card-section\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"Dilemma-question\"");

WriteLiteral(">\r\n\r\n        <div");

WriteLiteral(" class=\"Dilemma-text \"");

WriteLiteral(">\r\n            <p");

WriteLiteral(" style=\"white-space: pre-wrap\"");

WriteLiteral(">");

            
            #line 7 "..\..\Views\Shared\DisplayTemplates\QuestionCard.cshtml"
                                        Write(HttpUtility.HtmlDecode(Model.Text));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n        </div><!-- Dilemma-text -->\r\n\r\n    </div> \r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
