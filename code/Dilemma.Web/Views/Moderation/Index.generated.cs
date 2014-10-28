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

namespace Dilemma.Web.Views.Moderation
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
    
    #line 1 "..\..\Views\Moderation\Index.cshtml"
    using Dilemma.Web.Extensions;
    
    #line default
    #line hidden
    using Disposable.Common.Extensions;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Moderation/Index.cshtml")]
    public partial class Index : System.Web.Mvc.WebViewPage<Dilemma.Business.ViewModels.ModerationViewModel>
    {
        public Index()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\Moderation\Index.cshtml"
  
    ViewBag.Title = "Index";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h2>Index</h2>\r\n\r\n<div>\r\n    <h4>ModerationViewModel</h4>\r\n    <hr />\r\n");

            
            #line 13 "..\..\Views\Moderation\Index.cshtml"
    
            
            #line default
            #line hidden
            
            #line 13 "..\..\Views\Moderation\Index.cshtml"
     if (Model == null)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div>Nothing left to moderate.</div>\r\n");

            
            #line 16 "..\..\Views\Moderation\Index.cshtml"
    }
    else
    {


            
            #line default
            #line hidden
WriteLiteral("        <dl");

WriteLiteral(" class=\"dl-horizontal\"");

WriteLiteral(">\r\n            <dt>\r\n                CreatedDateTime\r\n            </dt>\r\n\r\n      " +
"      <dd>\r\n");

WriteLiteral("                ");

            
            #line 26 "..\..\Views\Moderation\Index.cshtml"
           Write(Model.ModerationEntries.First().CreatedDateTime.ToRelativeText());

            
            #line default
            #line hidden
WriteLiteral("\r\n            </dd>\r\n        \r\n            <dt>\r\n                Message (check m" +
"essage is correct -> might be Last())\r\n            </dt>\r\n\r\n            <dd>\r\n  " +
"              <span");

WriteLiteral(" class=\"user-text\"");

WriteLiteral(">");

            
            #line 34 "..\..\Views\Moderation\Index.cshtml"
                                   Write(Model.ModerationEntries.First().Message);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n            </dd>\r\n        </dl>\r\n");

            
            #line 37 "..\..\Views\Moderation\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" id=\"approve-wrapper\"");

WriteLiteral(">\r\n");

            
            #line 39 "..\..\Views\Moderation\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 39 "..\..\Views\Moderation\Index.cshtml"
             using (Html.BeginForm("Approve", "Moderation", FormMethod.Post, new { @class = "form-horizontal", role = "form" }))
            {
                
            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\Moderation\Index.cshtml"
           Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\Moderation\Index.cshtml"
                                        
                
            
            #line default
            #line hidden
            
            #line 42 "..\..\Views\Moderation\Index.cshtml"
           Write(Html.HiddenFor(x => x.ModerationId));

            
            #line default
            #line hidden
            
            #line 42 "..\..\Views\Moderation\Index.cshtml"
                                                    


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"col-md-12\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" value=\"Approve\"");

WriteLiteral(" />\r\n                        <input");

WriteLiteral(" id=\"begin-reject\"");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" value=\"Reject\"");

WriteLiteral(" />\r\n                    </div>\r\n                </div>\r\n");

            
            #line 50 "..\..\Views\Moderation\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n");

            
            #line 52 "..\..\Views\Moderation\Index.cshtml"
        

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" id=\"reject-wrapper\"");

WriteLiteral(" class=\"hidden\"");

WriteLiteral(">\r\n");

            
            #line 54 "..\..\Views\Moderation\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 54 "..\..\Views\Moderation\Index.cshtml"
             using (Html.BeginForm("Reject", "Moderation", FormMethod.Post, new { @class = "form-horizontal", role = "form" }))
            {
                
            
            #line default
            #line hidden
            
            #line 56 "..\..\Views\Moderation\Index.cshtml"
           Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 56 "..\..\Views\Moderation\Index.cshtml"
                                        
                
            
            #line default
            #line hidden
            
            #line 57 "..\..\Views\Moderation\Index.cshtml"
           Write(Html.HiddenFor(x => x.ModerationId));

            
            #line default
            #line hidden
            
            #line 57 "..\..\Views\Moderation\Index.cshtml"
                                                    
                
            
            #line default
            #line hidden
            
            #line 58 "..\..\Views\Moderation\Index.cshtml"
           Write(Html.TextArea("Message", new { @class = "form-control", rows="10"}));

            
            #line default
            #line hidden
            
            #line 58 "..\..\Views\Moderation\Index.cshtml"
                                                                                    


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"col-md-12\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" value=\"Reject\"");

WriteLiteral(" />\r\n                        <input");

WriteLiteral(" id=\"cancel-reject\"");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" value=\"Cancel\"");

WriteLiteral(" />\r\n                    </div>\r\n                </div>\r\n");

            
            #line 66 "..\..\Views\Moderation\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n");

            
            #line 68 "..\..\Views\Moderation\Index.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">
    $('#begin-reject').click(function () {
            
        $('#approve-wrapper').addClass('hidden');
        $('#reject-wrapper').removeClass('hidden');
    });

    $('#cancel-reject').click(function() {
        $('#approve-wrapper').removeClass('hidden');
        $('#reject-wrapper').addClass('hidden');
    });
</script>
");

});

        }
    }
}
#pragma warning restore 1591