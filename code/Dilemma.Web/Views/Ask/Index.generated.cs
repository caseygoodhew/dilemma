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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Ask/Index.cshtml")]
    public partial class _Views_Ask_Index_cshtml : System.Web.Mvc.WebViewPage<Dilemma.Web.ViewModels.AskViewModel>
    {
        public _Views_Ask_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Ask\Index.cshtml"
  
    ViewBag.Title = "Ask";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n   \r\n    <h1");

WriteLiteral(" class=\"pb10 mh15\"");

WriteLiteral("><span");

WriteLiteral(" data-icon=\"ask\"");

WriteLiteral("></span> Ask</h1>\r\n    <div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n\r\n        <aside");

WriteLiteral(" class=\"sidebar\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 12 "..\..\Views\Ask\Index.cshtml"
       Write(Html.DisplayFor(x => x.Sidebar, "Sidebar"));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </aside>\r\n\r\n        <main");

WriteLiteral(" role=\"main\"");

WriteLiteral(" class=\"main\"");

WriteLiteral(">\r\n            \r\n            <form");

WriteAttribute("action", Tuple.Create(" action=\"", 362), Tuple.Create("\"", 436)
            
            #line 17 "..\..\Views\Ask\Index.cshtml"
, Tuple.Create(Tuple.Create("", 371), Tuple.Create<System.Object, System.Int32>(Url.ActionWithCategory("Index", "Ask", (string)ViewBag.Category)
            
            #line default
            #line hidden
, 371), false)
);

WriteLiteral(" method=\"post\"");

WriteLiteral(" class=\"Card\"");

WriteLiteral(", role =\"form\">\r\n");

WriteLiteral("                ");

            
            #line 18 "..\..\Views\Ask\Index.cshtml"
           Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                <div");

WriteLiteral(" class=\"Card-section Card-divider pa10\"");

WriteLiteral(" style=\"background: #f9f9f9;\"");

WriteLiteral(">\r\n                    \r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 23 "..\..\Views\Ask\Index.cshtml"
                   Write(Html.LabelFor(x => x.Question.Text, "What's on your mind?", new { @class = "h3 pv10" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        <p");

WriteLiteral(" class=\"info\"");

WriteLiteral(">Ask your question in complete anonymity. All questions disappear within ");

            
            #line 24 "..\..\Views\Ask\Index.cshtml"
                                                                                                           Write(ViewBag.WeeksQuestionsOpen);

            
            #line default
            #line hidden
WriteLiteral(" weeks.</p>\r\n");

WriteLiteral("                        ");

            
            #line 25 "..\..\Views\Ask\Index.cshtml"
                   Write(Html.TextAreaFor(x => x.Question.Text, new { @class = "form-control", cols="30", rows="10"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                        ");

            
            #line 26 "..\..\Views\Ask\Index.cshtml"
                   Write(Html.ValidationMessageFor(x => x.Question.Text));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                    \r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 30 "..\..\Views\Ask\Index.cshtml"
                   Write(Html.LabelFor(x => x.Question.CategoryId, "Which best describes your dilemma?", new { @class = "h3 pv10" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                        ");

            
            #line 31 "..\..\Views\Ask\Index.cshtml"
                   Write(Html.DropDownListFor(x => x.Question.CategoryId, new SelectList(Model.Categories, "CategoryId", "Name"), new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                        ");

            
            #line 32 "..\..\Views\Ask\Index.cshtml"
                   Write(Html.ValidationMessageFor(x => x.Question.CategoryId));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                    \r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"Button--ask\"");

WriteLiteral(">\r\n                            <span");

WriteLiteral(" data-icon=\"ask\"");

WriteLiteral("></span>\r\n                            Share My Dilemma\r\n                        <" +
"/button>                      \r\n                    </div>\r\n      \r\n            " +
"    </div>\r\n            </form>\r\n\r\n        </main>\r\n    \r\n    </div><!-- line --" +
">\r\n\r\n\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        document.getElementsByTagName(\'body\')[0].className+=\' page-ask\';\r\n    " +
"</script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
