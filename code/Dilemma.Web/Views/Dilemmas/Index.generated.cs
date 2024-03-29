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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Dilemmas/Index.cshtml")]
    public partial class _Views_Dilemmas_Index_cshtml : System.Web.Mvc.WebViewPage<Dilemma.Web.ViewModels.DilemmasViewModel>
    {
        public _Views_Dilemmas_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Dilemmas\Index.cshtml"
  
    ViewBag.Title = "";

    ViewBag.ShowLuckyDip = !string.IsNullOrEmpty(ViewBag.Category);

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n    <h1");

WriteLiteral(" class=\"js-sticky\"");

WriteLiteral(">\r\n        <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" data-icon=\"advise\"");

WriteLiteral("></span>\r\n            Advise\r\n        </span>\r\n    </h1>\r\n\r\n    <div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n\r\n        <aside");

WriteLiteral(" class=\"sidebar\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 18 "..\..\Views\Dilemmas\Index.cshtml"
       Write(Html.DisplayFor(x => x.Sidebar, "Sidebar"));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </aside><!-- sidebar -->\r\n\r\n        <main");

WriteLiteral(" role=\"main\"");

WriteLiteral(" class=\"main\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"Dilemmas\"");

WriteLiteral(">\r\n\r\n");

            
            #line 24 "..\..\Views\Dilemmas\Index.cshtml"
                
            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\Dilemmas\Index.cshtml"
                 if (Model.DilemmasToAnswer.Any())
                {
                    
            
            #line default
            #line hidden
            
            #line 26 "..\..\Views\Dilemmas\Index.cshtml"
               Write(Html.DisplayForList(x => x.DilemmasToAnswer, "QuestionSummary"));

            
            #line default
            #line hidden
            
            #line 26 "..\..\Views\Dilemmas\Index.cshtml"
                                                                                    
                }
                else
                {
                    
            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\Dilemmas\Index.cshtml"
               Write(Html.Partial("DisplayTemplates/NoDilemmas"));

            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\Dilemmas\Index.cshtml"
                                                                
                }

            
            #line default
            #line hidden
WriteLiteral("\r\n                <h1");

WriteLiteral(" class=\"js-sticky h1 js-sticky--vote\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                        <span");

WriteLiteral(" data-icon=\"vote\"");

WriteLiteral("></span>\r\n                        Vote\r\n                    </span>\r\n            " +
"    </h1>\r\n          \r\n");

            
            #line 40 "..\..\Views\Dilemmas\Index.cshtml"
                
            
            #line default
            #line hidden
            
            #line 40 "..\..\Views\Dilemmas\Index.cshtml"
                 if (Model.DilemmasToVote.Any())
                {
                    
            
            #line default
            #line hidden
            
            #line 42 "..\..\Views\Dilemmas\Index.cshtml"
               Write(Html.DisplayForList(x => x.DilemmasToVote, "QuestionSummary"));

            
            #line default
            #line hidden
            
            #line 42 "..\..\Views\Dilemmas\Index.cshtml"
                                                                                  
                }
                else
                {
                    
            
            #line default
            #line hidden
            
            #line 46 "..\..\Views\Dilemmas\Index.cshtml"
               Write(Html.Partial("DisplayTemplates/NoDilemmas"));

            
            #line default
            #line hidden
            
            #line 46 "..\..\Views\Dilemmas\Index.cshtml"
                                                                
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n        </main><!-- main-->  \r\n    </div>\r\n\r\n\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        document.getElementsByTagName(\'body\')[0].className+=\' page-dilemmas\';\r" +
"\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
