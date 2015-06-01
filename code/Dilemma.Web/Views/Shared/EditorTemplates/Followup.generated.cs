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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/Followup.cshtml")]
    public partial class _Views_Shared_EditorTemplates_Followup_cshtml_ : System.Web.Mvc.WebViewPage<Dilemma.Business.ViewModels.FollowupViewModel>
    {
        public _Views_Shared_EditorTemplates_Followup_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Shared\EditorTemplates\Followup.cshtml"
  
    var isActive = ViewBag.FollowupIsActive == true;
    var textareaClass = isActive ? "is-active" : "js-activate-answer-form";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<form");

WriteLiteral(" class=\"Card Answer--window\"");

WriteLiteral(">\r\n    \r\n    <div");

WriteLiteral(" class=\"Card-section Card-info\"");

WriteLiteral(">\r\n        What did you do?\r\n    </div>\r\n\r\n");

WriteLiteral("    ");

            
            #line 14 "..\..\Views\Shared\EditorTemplates\Followup.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n    <div");

WriteLiteral(" class=\"Card-section line\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" id=\"js-button-followup\"");

WriteLiteral(" class=\"Button Button--answer\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" data-icon=\"advise\"");

WriteLiteral("></span>\r\n            <span");

WriteLiteral(" class=\"Button-label\"");

WriteLiteral(">Followup</span>\r\n        </button>\r\n        <div");

WriteLiteral(" class=\"textarea-wrap\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 22 "..\..\Views\Shared\EditorTemplates\Followup.cshtml"
       Write(Html.TextAreaFor(x => x.Text, new
                                               {
                                                   @class = "form-control " + textareaClass,
                                                   rows = "2",
                                                   cols = "30",
                                                   placeholder="What do you think?"
                                               }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 29 "..\..\Views\Shared\EditorTemplates\Followup.cshtml"
       Write(Html.ValidationMessageFor(x => x.Text));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n    \r\n</form>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
