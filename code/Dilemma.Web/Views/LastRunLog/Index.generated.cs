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
    
    #line 1 "..\..\Views\LastRunLog\Index.cshtml"
    using Dilemma.Web.Extensions;
    
    #line default
    #line hidden
    using Disposable.Common.Extensions;
    using Disposable.Web.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/LastRunLog/Index.cshtml")]
    public partial class _Views_LastRunLog_Index_cshtml : System.Web.Mvc.WebViewPage<Dilemma.Business.ViewModels.LastRunLogViewModel>
    {
        public _Views_LastRunLog_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\LastRunLog\Index.cshtml"
  
    ViewBag.Title = "Last Run Log";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h2>Last Run Log</h2>\r\n\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(" style=\"margin-bottom: 20px; margin-top: 20px;\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"col-md-2\"");

WriteLiteral("><strong>Expire answer slots</strong></div>\r\n    <div");

WriteLiteral(" class=\"col-md-2\"");

WriteAttribute("title", Tuple.Create(" title=\"", 321), Tuple.Create("\"", 353)
            
            #line 12 "..\..\Views\LastRunLog\Index.cshtml"
, Tuple.Create(Tuple.Create("", 329), Tuple.Create<System.Object, System.Int32>(Model.ExpireAnswerSlots
            
            #line default
            #line hidden
, 329), false)
);

WriteLiteral(">");

            
            #line 12 "..\..\Views\LastRunLog\Index.cshtml"
                                                      Write(Model.ExpireAnswerSlots.ToRelativeText("Never"));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n    <div");

WriteLiteral(" class=\"col-md-2\"");

WriteLiteral("><em>Should run every 5 minutes</em></div>\r\n    <div");

WriteLiteral(" class=\"col-md-6\"");

WriteLiteral("><a");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" href=\"ExpireAnswerSlots\"");

WriteLiteral(" role=\"button\"");

WriteLiteral(">Run Now</a></div>\r\n</div>\r\n\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(" style=\"margin-bottom: 20px\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"col-md-2\"");

WriteLiteral("><strong>Close Questions</strong></div>\r\n    <div");

WriteLiteral(" class=\"col-md-2\"");

WriteAttribute("title", Tuple.Create(" title=\"", 739), Tuple.Create("\"", 768)
            
            #line 19 "..\..\Views\LastRunLog\Index.cshtml"
, Tuple.Create(Tuple.Create("", 747), Tuple.Create<System.Object, System.Int32>(Model.CloseQuestions
            
            #line default
            #line hidden
, 747), false)
);

WriteLiteral(">");

            
            #line 19 "..\..\Views\LastRunLog\Index.cshtml"
                                                   Write(Model.CloseQuestions.ToRelativeText("Never"));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n    <div");

WriteLiteral(" class=\"col-md-2\"");

WriteLiteral("><em>Should run every 15 minutes</em></div>\r\n    <div");

WriteLiteral(" class=\"col-md-6\"");

WriteLiteral("><a");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" href=\"CloseQuestions\"");

WriteLiteral(" role=\"button\"");

WriteLiteral(">Run Now</a></div>\r\n</div>\r\n\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(" style=\"margin-bottom: 60px\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"col-md-2\"");

WriteLiteral("><strong>Retire old questions</strong></div>\r\n    <div");

WriteLiteral(" class=\"col-md-2\"");

WriteAttribute("title", Tuple.Create(" title=\"", 1154), Tuple.Create("\"", 1187)
            
            #line 26 "..\..\Views\LastRunLog\Index.cshtml"
, Tuple.Create(Tuple.Create("", 1162), Tuple.Create<System.Object, System.Int32>(Model.RetireOldQuestions
            
            #line default
            #line hidden
, 1162), false)
);

WriteLiteral(">");

            
            #line 26 "..\..\Views\LastRunLog\Index.cshtml"
                                                       Write(Model.RetireOldQuestions.ToRelativeText("Never"));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n    <div");

WriteLiteral(" class=\"col-md-2\"");

WriteLiteral("><em>Should run every few hours</em></div>\r\n    <div");

WriteLiteral(" class=\"col-md-6\"");

WriteLiteral("><a");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" href=\"RetireOldQuestions\"");

WriteLiteral(" role=\"button\"");

WriteLiteral(">Run Now</a></div>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591