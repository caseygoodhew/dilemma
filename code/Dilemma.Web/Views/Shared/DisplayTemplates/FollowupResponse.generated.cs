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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/FollowupResponse.cshtml")]
    public partial class _Views_Shared_DisplayTemplates_FollowupResponse_cshtml_ : System.Web.Mvc.WebViewPage<Dilemma.Business.ViewModels.FollowupViewModel>
    {
        public _Views_Shared_DisplayTemplates_FollowupResponse_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteAttribute("class", Tuple.Create(" class=\"", 60), Tuple.Create("\"", 145)
, Tuple.Create(Tuple.Create("", 68), Tuple.Create("Card", 68), true)
            
            #line 3 "..\..\Views\Shared\DisplayTemplates\FollowupResponse.cshtml"
, Tuple.Create(Tuple.Create(" ", 72), Tuple.Create<System.Object, System.Int32>(Model.IsMyFollowup ? "Dilemmas-empty" : "Dilemmas-empty-with-buttons"
            
            #line default
            #line hidden
, 73), false)
);

WriteLiteral(">\r\n    <p>Here\'s how it ended up</p>\r\n    <div");

WriteLiteral(" class=\"Card-label--small\"");

WriteLiteral(" style =\"margin-top: -20px; margin-bottom: 20px\">\r\n");

WriteLiteral("        ");

            
            #line 6 "..\..\Views\Shared\DisplayTemplates\FollowupResponse.cshtml"
   Write(Model.CreatedDateTime.ToRelativeText());

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n    \r\n    <div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n        <span");

WriteLiteral(" style=\"white-space: pre-wrap\"");

WriteLiteral(">");

            
            #line 10 "..\..\Views\Shared\DisplayTemplates\FollowupResponse.cshtml"
                                       Write(HttpUtility.HtmlDecode(Model.Text));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n    </div>\r\n    \r\n");

            
            #line 13 "..\..\Views\Shared\DisplayTemplates\FollowupResponse.cshtml"
    
            
            #line default
            #line hidden
            
            #line 13 "..\..\Views\Shared\DisplayTemplates\FollowupResponse.cshtml"
     if (!Model.IsMyFollowup)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"line button-section\"");

WriteLiteral(">\r\n            \r\n            <div");

WriteLiteral(" class=\"u-1of5 right\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"Button--flag js-flag-button \"");

WriteLiteral(" title=\"Report an issue with this question\"");

WriteLiteral("  data-followup-id=\"");

            
            #line 18 "..\..\Views\Shared\DisplayTemplates\FollowupResponse.cshtml"
                                                                                                                                    Write(Model.FollowupId);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" data-toggle=\"modal\"");

WriteLiteral(" data-target=\"#modal-flag\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" data-icon=\"flag\"");

WriteLiteral("></span>\r\n                    <span");

WriteLiteral(" class=\"Button-label is-inactive\"");

WriteLiteral(">Flag</span>\r\n                    <span");

WriteLiteral(" class=\"Button-label is-active\"");

WriteLiteral(">Flagged</span>\r\n                </button>                               \r\n      " +
"      </div>\r\n            \r\n        </div>\r\n");

WriteLiteral("        <!-- Dilemma-actions -->\r\n");

            
            #line 27 "..\..\Views\Shared\DisplayTemplates\FollowupResponse.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>");

        }
    }
}
#pragma warning restore 1591
