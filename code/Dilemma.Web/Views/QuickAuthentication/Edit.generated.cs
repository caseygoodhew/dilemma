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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/QuickAuthentication/Edit.cshtml")]
    public partial class _Views_QuickAuthentication_Edit_cshtml : System.Web.Mvc.WebViewPage<Dilemma.Business.ViewModels.DevelopmentUserViewModel>
    {
        public _Views_QuickAuthentication_Edit_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\QuickAuthentication\Edit.cshtml"
  
    ViewBag.Title = "Edit user name";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h2>Edit user name</h2>\r\n\r\n");

            
            #line 9 "..\..\Views\QuickAuthentication\Edit.cshtml"
 using (Html.BeginForm())
{
    
            
            #line default
            #line hidden
            
            #line 11 "..\..\Views\QuickAuthentication\Edit.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 11 "..\..\Views\QuickAuthentication\Edit.cshtml"
                            
    

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 14 "..\..\Views\QuickAuthentication\Edit.cshtml"
   Write(Html.ValidationSummary(true));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 16 "..\..\Views\QuickAuthentication\Edit.cshtml"
       Write(Html.LabelFor(model => model.Name, new { @class = "control-label col-md-2" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            <div");

WriteLiteral(" class=\"col-md-10\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 18 "..\..\Views\QuickAuthentication\Edit.cshtml"
           Write(Html.EditorFor(model => model.Name));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 19 "..\..\Views\QuickAuthentication\Edit.cshtml"
           Write(Html.ValidationMessageFor(model => model.Name));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"control-label col-md-2\"");

WriteLiteral(">User Id</label>\r\n            <div");

WriteLiteral(" class=\"col-md-10\"");

WriteLiteral(" style=\"padding-top: 6px\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 26 "..\..\Views\QuickAuthentication\Edit.cshtml"
           Write(Model.UserId.Default("(new user)"));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"col-md-offset-2 col-md-10\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" value=\"Save\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n    </div>\r\n");

            
            #line 36 "..\..\Views\QuickAuthentication\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<div>\r\n");

WriteLiteral("    ");

            
            #line 39 "..\..\Views\QuickAuthentication\Edit.cshtml"
Write(Html.ActionLink("Back to List", "Index"));

            
            #line default
            #line hidden
WriteLiteral("\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
