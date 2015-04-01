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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Profile/Index.cshtml")]
    public partial class _Views_Profile_Index_cshtml : System.Web.Mvc.WebViewPage<Dilemma.Web.ViewModels.MyProfileViewModel>
    {
        public _Views_Profile_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Profile\Index.cshtml"
  
    ViewBag.Title = "Profile";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n    <h1");

WriteLiteral(" class=\"mb0 mh15 pb0\"");

WriteLiteral(">\r\n      <span");

WriteLiteral(" data-icon=\"user\"");

WriteLiteral("></span> \r\n      Your Profile\r\n    </h1>\r\n\r\n    <div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n\r\n        <aside");

WriteLiteral(" class=\"sidebar\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 16 "..\..\Views\Profile\Index.cshtml"
       Write(Html.DisplayFor(x => x.Sidebar, "Sidebar", new { HideNotifications = true }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </aside><!-- sidebar -->\r\n        \r\n        <main");

WriteLiteral(" role=\"main\"");

WriteLiteral(" class=\"main\"");

WriteLiteral(">\r\n            \r\n            <h2");

WriteLiteral(" class=\"js-sticky h3\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                    Your Current Dilemmas\r\n                </span>\r\n          " +
"  </h2>\r\n\r\n            <div");

WriteLiteral(" class=\"Dilemmas\"");

WriteLiteral(">\r\n\r\n");

WriteLiteral("                ");

            
            #line 29 "..\..\Views\Profile\Index.cshtml"
           Write(Html.DisplayForList(x => x.Dilemmas, "QuestionNotification"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                <h2");

WriteLiteral(" class=\"js-sticky h3\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                        My Current Answers\r\n                    </span>\r\n     " +
"           </h2>\r\n\r\n");

WriteLiteral("                ");

            
            #line 37 "..\..\Views\Profile\Index.cshtml"
           Write(Html.DisplayForList(x => x.Answers, "QuestionNotification"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                \r\n                <h2");

WriteLiteral(" class=\"js-sticky h3\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                        Other notifications\r\n                    </span>\r\n    " +
"            </h2>\r\n\r\n");

WriteLiteral("                ");

            
            #line 45 "..\..\Views\Profile\Index.cshtml"
           Write(Html.DisplayForList(x => x.Notifications, "Notifications"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                \r\n            </div>\r\n\r\n        </main><!-- main-->   \r\n\r\n    <" +
"/div>     \r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        document.getElementsByTagName(\'body\')[0].className += \' page-profile\';" +
"\r\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
