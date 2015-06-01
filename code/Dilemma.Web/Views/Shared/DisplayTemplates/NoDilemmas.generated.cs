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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/NoDilemmas.cshtml")]
    public partial class _Views_Shared_DisplayTemplates_NoDilemmas_cshtml_ : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Views_Shared_DisplayTemplates_NoDilemmas_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
  
    var showLuckDip = ViewBag.ShowLuckyDip == true;
    var panic = ViewBag.DontPanic != true;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"Card Dilemmas-empty\"");

WriteLiteral(">\r\n");

            
            #line 7 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
    
            
            #line default
            #line hidden
            
            #line 7 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
     if(showLuckDip)
    {
        if (panic)
        {

            
            #line default
            #line hidden
WriteLiteral("            <h2>Oh no! there are no dilemmas in this category right now!</h2>\r\n");

            
            #line 12 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        <p>Maybe you want to share your Dilemma? Or take a lucky dip!</p>\r\n");

            
            #line 14 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
    }
    else
    {
        if (panic) 
        {

            
            #line default
            #line hidden
WriteLiteral("            <h2>Oh no! there are no dilemmas to answer right now!</h2>\r\n");

            
            #line 20 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        <p>Maybe you want to share your Dilemma?</p>\r\n");

            
            #line 22 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n    <div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 602), Tuple.Create("\"", 680)
            
            #line 25 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
, Tuple.Create(Tuple.Create("", 609), Tuple.Create<System.Object, System.Int32>(Url.ActionWithCategory("Index", "Ask", (string)ViewBag.Category, true)
            
            #line default
            #line hidden
, 609), false)
);

WriteLiteral(" class=\"Button--ask mb20\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" data-icon=\"ask\"");

WriteLiteral("></span> \r\n            Post Your Dilemma\r\n        </a>                \r\n");

            
            #line 29 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
        
            
            #line default
            #line hidden
            
            #line 29 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
         if (showLuckDip)
        {

            
            #line default
            #line hidden
WriteLiteral("            <span>&nbsp;&nbsp;&nbsp;</span>\r\n");

WriteLiteral("            <a");

WriteAttribute("href", Tuple.Create(" href=\"", 911), Tuple.Create("\"", 950)
            
            #line 32 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
, Tuple.Create(Tuple.Create("", 918), Tuple.Create<System.Object, System.Int32>(Url.Action("Index", "Dilemmas")
            
            #line default
            #line hidden
, 918), false)
);

WriteLiteral(" class=\"Button--ask\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" data-icon=\"refresh\"");

WriteLiteral("></span> \r\n                Lucky Dip\r\n            </a>\r\n");

            
            #line 36 "..\..\Views\Shared\DisplayTemplates\NoDilemmas.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
