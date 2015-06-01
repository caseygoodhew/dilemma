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
    using Dilemma.Common;
    using Dilemma.Web;
    using Dilemma.Web.Extensions;
    using Disposable.Common.Extensions;
    using Disposable.Web.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/QuestionNotification.cshtml")]
    public partial class _Views_Shared_DisplayTemplates_QuestionNotification_cshtml_ : System.Web.Mvc.WebViewPage<Dilemma.Business.ViewModels.QuestionViewModel>
    {
        public _Views_Shared_DisplayTemplates_QuestionNotification_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"Card Notification Card-go--container\"");

WriteLiteral(">\r\n    <a");

WriteAttribute("href", Tuple.Create(" href=\"", 114), Tuple.Create("\"", 191)
            
            #line 4 "..\..\Views\Shared\DisplayTemplates\QuestionNotification.cshtml"
, Tuple.Create(Tuple.Create("", 121), Tuple.Create<System.Object, System.Int32>(Url.Action("Index", "Answers", new { questionId = Model.QuestionId })
            
            #line default
            #line hidden
, 121), false)
);

WriteLiteral(" class=\"Card-go--trigger\"");

WriteLiteral("></a>\r\n    <div");

WriteLiteral(" class=\"Notification-score\"");

WriteLiteral(">\r\n        <div><span");

WriteLiteral(" data-icon=\"");

            
            #line 6 "..\..\Views\Shared\DisplayTemplates\QuestionNotification.cshtml"
                          Write(Model.IsMyQuestion ? "ask" : "advise");

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral("></span></div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"Notification-summary\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"Card-label--small\"");

WriteLiteral(">");

            
            #line 9 "..\..\Views\Shared\DisplayTemplates\QuestionNotification.cshtml"
                                  Write(Model.CreatedDateTime.ToRelativeText());

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n        <div");

WriteLiteral(" class=\"Notification-summary-text\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 11 "..\..\Views\Shared\DisplayTemplates\QuestionNotification.cshtml"
       Write(HttpUtility.HtmlDecode(Model.Text));

            
            #line default
            #line hidden
WriteLiteral("          \r\n        </div>\r\n    </div><!-- Notification-summary -->\r\n</div>\r\n\r\n\r\n" +
"");

        }
    }
}
#pragma warning restore 1591
