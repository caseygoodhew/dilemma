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
    
    #line 1 "..\..\Views\QuickAuthentication\Index.cshtml"
    using Disposable.Common.Extensions;
    
    #line default
    #line hidden
    using Disposable.Web.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/QuickAuthentication/Index.cshtml")]
    public partial class _Views_QuickAuthentication_Index_cshtml : System.Web.Mvc.WebViewPage<System.Collections.Generic.IEnumerable<Dilemma.Business.ViewModels.DevelopmentUserViewModel>>
    {
        public _Views_QuickAuthentication_Index_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<h2>Your users</h2>\r\n\r\n<div>\r\n");

            
            #line 7 "..\..\Views\QuickAuthentication\Index.cshtml"
Write(Html.ActionLink("Create New User", "edit", "QuickAuthentication"));

            
            #line default
            #line hidden
WriteLiteral("\r\n</div>\r\n\r\n<table");

WriteLiteral(" class=\"table\"");

WriteLiteral(">\r\n    \r\n");

            
            #line 12 "..\..\Views\QuickAuthentication\Index.cshtml"
 foreach (var item in Model)
{
    var rowClass = item.IsCurrent ? "bg-info" : "";
    

            
            #line default
            #line hidden
WriteLiteral("    <tr");

WriteAttribute("class", Tuple.Create(" class=\"", 376), Tuple.Create("\"", 393)
            
            #line 16 "..\..\Views\QuickAuthentication\Index.cshtml"
, Tuple.Create(Tuple.Create("", 384), Tuple.Create<System.Object, System.Int32>(rowClass
            
            #line default
            #line hidden
, 384), false)
);

WriteLiteral(">\r\n        <td");

WriteLiteral(" style=\"padding-top: 5px;\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 18 "..\..\Views\QuickAuthentication\Index.cshtml"
       Write(item.UserId);

            
            #line default
            #line hidden
WriteLiteral("\r\n        </td>\r\n        <td>\r\n");

WriteLiteral("            ");

            
            #line 21 "..\..\Views\QuickAuthentication\Index.cshtml"
       Write(item.Name.Default("(anonymous)"));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </td>\r\n        <td");

WriteLiteral(" width=\"50\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 24 "..\..\Views\QuickAuthentication\Index.cshtml"
       Write(Html.ActionLink("Edit", "Edit", new { userId = item.UserId }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </td>\r\n        <td");

WriteLiteral(" width=\"50\"");

WriteLiteral(">    \r\n");

            
            #line 27 "..\..\Views\QuickAuthentication\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\QuickAuthentication\Index.cshtml"
             if (!item.IsCurrent)
            {
                
            
            #line default
            #line hidden
            
            #line 29 "..\..\Views\QuickAuthentication\Index.cshtml"
           Write(Html.ActionLink("Login", "login", new { userId = item.UserId }));

            
            #line default
            #line hidden
            
            #line 29 "..\..\Views\QuickAuthentication\Index.cshtml"
                                                                                
            }

            
            #line default
            #line hidden
WriteLiteral("        </td>\r\n    </tr>\r\n");

            
            #line 33 "..\..\Views\QuickAuthentication\Index.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n</table>\r\n\r\n\r\n\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
