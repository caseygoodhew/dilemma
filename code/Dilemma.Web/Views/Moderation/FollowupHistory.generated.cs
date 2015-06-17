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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Moderation/FollowupHistory.cshtml")]
    public partial class _Views_Moderation_FollowupHistory_cshtml : System.Web.Mvc.WebViewPage<Dilemma.Web.ViewModels.ModerationHistoryViewModel<Dilemma.Business.ViewModels.FollowupModerationHistoryViewModel>>
    {
        public _Views_Moderation_FollowupHistory_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Moderation\FollowupHistory.cshtml"
  
    ViewBag.Title = "Followup History";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" class=\"mb0 mh15 pb0\"");

WriteLiteral(">\r\n    Followup History\r\n</h1>\r\n\r\n\r\n<div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n    <aside");

WriteLiteral(" class=\"sidebar\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 13 "..\..\Views\Moderation\FollowupHistory.cshtml"
   Write(Html.DisplayFor(x => x.Sidebar, "Sidebar"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </aside>\r\n    \r\n    <main");

WriteLiteral(" role=\"main\"");

WriteLiteral(" class=\"main\"");

WriteLiteral(">\r\n");

            
            #line 17 "..\..\Views\Moderation\FollowupHistory.cshtml"
        
            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\Moderation\FollowupHistory.cshtml"
         if (Model.History == null)
        {
            
            
            #line default
            #line hidden
            
            #line 19 "..\..\Views\Moderation\FollowupHistory.cshtml"
       Write(Html.Partial("DisplayTemplates/NoModerationHistory"));

            
            #line default
            #line hidden
            
            #line 19 "..\..\Views\Moderation\FollowupHistory.cshtml"
                                                                 
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <h2");

WriteLiteral(" class=\"js-sticky h3\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                    The dilemma that was being followed up\r\n                </" +
"span>\r\n            </h2>\r\n");

            
            #line 28 "..\..\Views\Moderation\FollowupHistory.cshtml"


            
            #line default
            #line hidden
WriteLiteral("            <article");

WriteLiteral(" class=\"Dilemma Card\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"Card-section Card-info\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"line align-center\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"Card-label--small\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 33 "..\..\Views\Moderation\FollowupHistory.cshtml"
                       Write(Model.History.Question.CategoryName);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                        <div");

WriteLiteral(" style=\"margin-bottom: 20px\"");

WriteLiteral("></div>\r\n                    </div>\r\n                </div>\r\n\r\n");

WriteLiteral("                ");

            
            #line 39 "..\..\Views\Moderation\FollowupHistory.cshtml"
           Write(Html.DisplayFor(x => x.History.Question, "QuestionCard"));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </article>\r\n");

            
            #line 41 "..\..\Views\Moderation\FollowupHistory.cshtml"


            
            #line default
            #line hidden
WriteLiteral("            <h2");

WriteLiteral(" class=\"js-sticky h3\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                    The followup that was moderated\r\n                </span>\r\n" +
"            </h2>\r\n");

            
            #line 47 "..\..\Views\Moderation\FollowupHistory.cshtml"


            
            #line default
            #line hidden
WriteLiteral("            <article");

WriteLiteral(" class=\"Dilemma Card\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"Card-section\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"Dilemma-question\"");

WriteLiteral(">\r\n\r\n                        <div");

WriteLiteral(" class=\"Dilemma-text \"");

WriteLiteral(">\r\n                            <p");

WriteLiteral(" style=\"white-space: pre-wrap\"");

WriteLiteral(">");

            
            #line 53 "..\..\Views\Moderation\FollowupHistory.cshtml"
                                                        Write(HttpUtility.HtmlDecode(Model.History.Followup.Text));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                        </div><!-- Dilemma-text -->\r\n\r\n                    " +
"</div> \r\n                </div>\r\n            </article>\r\n");

            
            #line 59 "..\..\Views\Moderation\FollowupHistory.cshtml"
            

            
            #line default
            #line hidden
WriteLiteral("            <h2");

WriteLiteral(" class=\"js-sticky h3\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                    Moderation history\r\n                </span>\r\n            <" +
"/h2>\r\n");

            
            #line 65 "..\..\Views\Moderation\FollowupHistory.cshtml"

            
            
            #line default
            #line hidden
            
            #line 66 "..\..\Views\Moderation\FollowupHistory.cshtml"
       Write(Html.DisplayForList(x => x.History.ModerationEntries, "ModerationEntry"));

            
            #line default
            #line hidden
            
            #line 66 "..\..\Views\Moderation\FollowupHistory.cshtml"
                                                                                     
        }

            
            #line default
            #line hidden
WriteLiteral("    </main>\r\n</div>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
