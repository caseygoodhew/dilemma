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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/QuestionSummary.cshtml")]
    public partial class _Views_Shared_DisplayTemplates_QuestionSummary_cshtml_ : System.Web.Mvc.WebViewPage<Dilemma.Business.ViewModels.QuestionViewModel>
    {
        public _Views_Shared_DisplayTemplates_QuestionSummary_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
  
    var isDilemmaViewPage = ViewBag.IsDilemmaViewPage == true;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<article");

WriteAttribute("class", Tuple.Create(" class=\"", 137), Tuple.Create("\"", 187)
, Tuple.Create(Tuple.Create("", 145), Tuple.Create("Dilemma", 145), true)
, Tuple.Create(Tuple.Create(" ", 152), Tuple.Create("Card", 153), true)
, Tuple.Create(Tuple.Create(" ", 157), Tuple.Create("in-category-", 158), true)
            
            #line 7 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
, Tuple.Create(Tuple.Create("", 170), Tuple.Create<System.Object, System.Int32>(Model.CategoryId
            
            #line default
            #line hidden
, 170), false)
);

WriteLiteral(">\r\n\r\n    <div");

WriteLiteral(" class=\"Card-section Card-info\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"line align-center\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"Card-label--small\"");

WriteLiteral(">\r\n");

            
            #line 12 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                
            
            #line default
            #line hidden
            
            #line 12 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                 if (Model.IsClosed)
                {

            
            #line default
            #line hidden
WriteLiteral("                    ");

WriteLiteral("Closed ");

            
            #line 14 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                            Write(Model.ClosedDateTime.ToRelativeText());

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 15 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                }
                else
                {

            
            #line default
            #line hidden
WriteLiteral("                    ");

WriteLiteral("Closes ");

            
            #line 18 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                            Write(Model.ClosesDateTime.ToRelativeText());

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 19 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                \r\n            </div>\r\n            <ol");

WriteLiteral(" \r\n                class=\"Dilemma-respondents horizontal line\"");

WriteAttribute("title", Tuple.Create(" \r\n                title=\"", 732), Tuple.Create("\"", 823)
, Tuple.Create(Tuple.Create("", 758), Tuple.Create("Posted", 758), true)
, Tuple.Create(Tuple.Create(" ", 764), Tuple.Create("in", 765), true)
            
            #line 24 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
, Tuple.Create(Tuple.Create(" ", 767), Tuple.Create<System.Object, System.Int32>(Model.CategoryName
            
            #line default
            #line hidden
, 768), false)
, Tuple.Create(Tuple.Create("", 787), Tuple.Create(",", 787), true)
            
            #line 24 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
, Tuple.Create(Tuple.Create(" ", 788), Tuple.Create<System.Object, System.Int32>(Model.TotalAnswers
            
            #line default
            #line hidden
, 789), false)
, Tuple.Create(Tuple.Create(" ", 808), Tuple.Create("answers", 809), true)
, Tuple.Create(Tuple.Create(" ", 816), Tuple.Create("so", 817), true)
, Tuple.Create(Tuple.Create(" ", 819), Tuple.Create("far", 820), true)
);

WriteLiteral(">\r\n");

            
            #line 25 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                
            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                 for (var i = 0; i < @Model.MaxAnswers; i++)
                {
                    if (i < @Model.TotalAnswers)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <li");

WriteLiteral(" class=\"Dilemma-respondent\"");

WriteLiteral("></li>\r\n");

            
            #line 30 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <li");

WriteLiteral(" class=\"Dilemma-respondent Dilemma-respondent--empty\"");

WriteLiteral("></li>\r\n");

            
            #line 34 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                    }
                }

            
            #line default
            #line hidden
WriteLiteral("                \r\n            </ol>    \r\n        </div>\r\n    </div><!-- Dilemma-q" +
"uestion Card-section -->\r\n\r\n    <div");

WriteLiteral(" class=\"Card-section\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"Dilemma-question\"");

WriteLiteral(">\r\n\r\n            <div");

WriteLiteral(" class=\"Dilemma-text \"");

WriteLiteral(">\r\n                <p");

WriteLiteral(" style=\"white-space: pre\"");

WriteLiteral(">");

            
            #line 45 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                                       Write(Model.Text);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"Dilemma-text-toggle\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"Dilemma-text-toggle-more\"");

WriteLiteral(">&hellip; continue reading</span>\r\n                    <span");

WriteLiteral(" class=\"Dilemma-text-toggle-less\"");

WriteLiteral(">&hellip; show less</span>\r\n                </a>\r\n            </div><!-- Dilemma-" +
"text -->\r\n        </div>  \r\n\r\n    </div><!--  Card-section -->\r\n    \r\n");

            
            #line 55 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
    
            
            #line default
            #line hidden
            
            #line 55 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
     if (!Model.IsMyQuestion || !isDilemmaViewPage)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"Dilemma-actions Card-section line\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"u-2of5\"");

WriteLiteral(">\r\n");

            
            #line 59 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                
            
            #line default
            #line hidden
            
            #line 59 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                 if (Model.IsOpen && !isDilemmaViewPage)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2140), Tuple.Create("\"", 2406)
            
            #line 61 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
, Tuple.Create(Tuple.Create("", 2147), Tuple.Create<System.Object, System.Int32>(Url.Action("Index", "Answers", new
                                                                 {
                                                                     Model.QuestionId
                                                                 })
            
            #line default
            #line hidden
, 2147), false)
);

WriteLiteral(" class=\"Button--answer\"");

WriteLiteral(">\r\n                        <img");

WriteLiteral(" src=\"/Content/images/icons/dark-answer.png\"");

WriteLiteral(" alt=\"\"");

WriteLiteral(" width=\"20\"");

WriteLiteral(">\r\n                        <span");

WriteLiteral(" class=\"Button-label\"");

WriteLiteral(">");

            
            #line 66 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                                               Write(Model.IsMyQuestion ? "View" : "Advise");

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                    </a>\r\n");

            
            #line 68 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                }
                else if (Model.IsClosed && !Model.IsMyQuestion && !isDilemmaViewPage)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2800), Tuple.Create("\"", 3066)
            
            #line 71 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
, Tuple.Create(Tuple.Create("", 2807), Tuple.Create<System.Object, System.Int32>(Url.Action("Index", "Answers", new
                                                                 {
                                                                     Model.QuestionId
                                                                 })
            
            #line default
            #line hidden
, 2807), false)
);

WriteLiteral("  class=\"Button--vote \"");

WriteLiteral(">\r\n                        <span");

WriteLiteral(" data-icon=\"vote\"");

WriteLiteral("></span>\r\n                        <span");

WriteLiteral(" class=\"Button-label\"");

WriteLiteral(">Vote</span>\r\n                    </a>\r\n");

            
            #line 78 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n");

            
            #line 80 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
            
            
            #line default
            #line hidden
            
            #line 80 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
             if (!Model.IsMyQuestion)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"u-2of5\"");

WriteLiteral(">\r\n                    <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"Button--bookmark js-bookmark-button\"");

WriteLiteral(">\r\n                        <span");

WriteLiteral(" data-icon=\"bookmark\"");

WriteLiteral("></span>\r\n                        <span");

WriteLiteral(" class=\"Button-label is-inactive\"");

WriteLiteral(">Bookmark</span>\r\n                        <span");

WriteLiteral(" class=\"Button-label is-active\"");

WriteLiteral(">Bookmarked</span>\r\n                    </button>                  \r\n            " +
"    </div>\r\n");

WriteLiteral("                <div");

WriteLiteral(" class=\"u-1of5 right\"");

WriteLiteral(">\r\n                    <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"Button--flag js-flag-button \"");

WriteLiteral(" title=\"Report an issue with this question\"");

WriteLiteral(" data-toggle=\"modal\"");

WriteLiteral(" data-target=\"#modal-flag\"");

WriteLiteral(">\r\n                        <span");

WriteLiteral(" data-icon=\"flag\"");

WriteLiteral("></span>\r\n                        <span");

WriteLiteral(" class=\"Button-label is-inactive\"");

WriteLiteral(">Flag</span>\r\n                        <span");

WriteLiteral(" class=\"Button-label is-active\"");

WriteLiteral(">Flagged</span>\r\n                    </button>                               \r\n  " +
"              </div>\r\n");

            
            #line 96 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

WriteLiteral("        <!-- Dilemma-actions -->\r\n");

            
            #line 100 "..\..\Views\Shared\DisplayTemplates\QuestionSummary.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</article>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
