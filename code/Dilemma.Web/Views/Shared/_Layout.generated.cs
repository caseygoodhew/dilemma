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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/_Layout.cshtml")]
    public partial class _Views_Shared__Layout_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Views_Shared__Layout_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" http-equiv=\"X-UA-Compatible\"");

WriteLiteral(" content=\"IE=edge\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no\"" +
"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"description\"");

WriteLiteral(" content=\"\"");

WriteLiteral(">\r\n\r\n    <title>\r\n");

            
            #line 10 "..\..\Views\Shared\_Layout.cshtml"
        
            
            #line default
            #line hidden
            
            #line 10 "..\..\Views\Shared\_Layout.cshtml"
         if (string.IsNullOrEmpty(ViewBag.Title))
        {

            
            #line default
            #line hidden
WriteLiteral("            ");

WriteLiteral("OurDilemmas");

WriteLiteral("\r\n");

            
            #line 13 "..\..\Views\Shared\_Layout.cshtml"
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            ");

WriteLiteral("OurDilemmas - ");

WriteLiteral(" ");

            
            #line 16 "..\..\Views\Shared\_Layout.cshtml"
                                        
            
            #line default
            #line hidden
            
            #line 16 "..\..\Views\Shared\_Layout.cshtml"
                                   Write(ViewBag.Titles);

            
            #line default
            #line hidden
            
            #line 16 "..\..\Views\Shared\_Layout.cshtml"
                                                       
        }

            
            #line default
            #line hidden
WriteLiteral("    </title>\r\n");

WriteLiteral("    ");

            
            #line 19 "..\..\Views\Shared\_Layout.cshtml"
Write(Styles.Render("~/Content/css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 20 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/modernizr"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n    <!--icons-->\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon\"");

WriteLiteral(" sizes=\"57x57\"");

WriteLiteral(" href=\"/Content/images/icons/fav/apple-touch-icon-57x57.png\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon\"");

WriteLiteral(" sizes=\"60x60\"");

WriteLiteral(" href=\"/Content/images/icons/fav/apple-touch-icon-60x60.png\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon\"");

WriteLiteral(" sizes=\"72x72\"");

WriteLiteral(" href=\"/Content/images/icons/fav/apple-touch-icon-72x72.png\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon\"");

WriteLiteral(" sizes=\"76x76\"");

WriteLiteral(" href=\"/Content/images/icons/fav/apple-touch-icon-76x76.png\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon\"");

WriteLiteral(" sizes=\"114x114\"");

WriteLiteral(" href=\"/Content/images/icons/fav/apple-touch-icon-114x114.png\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon\"");

WriteLiteral(" sizes=\"120x120\"");

WriteLiteral(" href=\"/Content/images/icons/fav/apple-touch-icon-120x120.png\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon\"");

WriteLiteral(" sizes=\"144x144\"");

WriteLiteral(" href=\"/Content/images/icons/fav/apple-touch-icon-144x144.png\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon\"");

WriteLiteral(" sizes=\"152x152\"");

WriteLiteral(" href=\"/Content/images/icons/fav/apple-touch-icon-152x152.png\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon\"");

WriteLiteral(" sizes=\"180x180\"");

WriteLiteral(" href=\"/Content/images/icons/fav/apple-touch-icon-180x180.png\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"icon\"");

WriteLiteral(" type=\"image/png\"");

WriteLiteral(" href=\"/Content/images/icons/fav/favicon-32x32.png\"");

WriteLiteral(" sizes=\"32x32\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"icon\"");

WriteLiteral(" type=\"image/png\"");

WriteLiteral(" href=\"/Content/images/icons/fav/android-chrome-192x192.png\"");

WriteLiteral(" sizes=\"192x192\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"icon\"");

WriteLiteral(" type=\"image/png\"");

WriteLiteral(" href=\"/Content/images/icons/fav/favicon-96x96.png\"");

WriteLiteral(" sizes=\"96x96\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"icon\"");

WriteLiteral(" type=\"image/png\"");

WriteLiteral(" href=\"/Content/images/icons/fav/favicon-16x16.png\"");

WriteLiteral(" sizes=\"16x16\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"manifest\"");

WriteLiteral(" href=\"/Content/images/icons/fav/manifest.json\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" rel=\"shortcut icon\"");

WriteLiteral(" href=\"/Content/images/icons/fav/favicon.ico\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"msapplication-TileColor\"");

WriteLiteral(" content=\"#da532c\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"msapplication-TileImage\"");

WriteLiteral(" content=\"/Content/images/icons/fav/mstile-144x144.png\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"msapplication-config\"");

WriteLiteral(" content=\"/Content/images/icons/fav/browserconfig.xml\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"theme-color\"");

WriteLiteral(" content=\"#000000\"");

WriteLiteral(">\r\n\r\n\r\n    <!-- send jquery to the foot of the page -->\r\n    <script");

WriteLiteral(" type=\'text/javascript\'");

WriteLiteral(">window.q = []; window.$ = function (f) { q.push(f); }</script>\r\n\r\n</head>\r\n<body" +
">\r\n");

WriteLiteral("    ");

            
            #line 49 "..\..\Views\Shared\_Layout.cshtml"
Write(Html.Partial("Header"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n");

WriteLiteral("    ");

            
            #line 51 "..\..\Views\Shared\_Layout.cshtml"
Write(Html.Partial("Modals"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n");

            
            #line 53 "..\..\Views\Shared\_Layout.cshtml"
    
            
            #line default
            #line hidden
            
            #line 53 "..\..\Views\Shared\_Layout.cshtml"
     if (ViewBag.SuppressPageBd == true)
    {
        
            
            #line default
            #line hidden
            
            #line 55 "..\..\Views\Shared\_Layout.cshtml"
   Write(RenderBody());

            
            #line default
            #line hidden
            
            #line 55 "..\..\Views\Shared\_Layout.cshtml"
                     
    }
    else
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"page-bd\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"wrap\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 61 "..\..\Views\Shared\_Layout.cshtml"
           Write(RenderBody());

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n");

            
            #line 64 "..\..\Views\Shared\_Layout.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    \r\n");

WriteLiteral("    ");

            
            #line 68 "..\..\Views\Shared\_Layout.cshtml"
Write(Html.Partial("Footer"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n");

WriteLiteral("    ");

            
            #line 70 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/jquery"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 71 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/cookiedirective"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 72 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/bootstrap"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 73 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/homegrown"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n    \r\n");

WriteLiteral("    ");

            
            #line 76 "..\..\Views\Shared\_Layout.cshtml"
Write(RenderSection("scripts", required: false));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">$.each(q, function(i, f) { $(f); })</script>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">
    (function (i, s, o, g, r, a, m) {
        i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date(); a = s.createElement(o),
        m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
    })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

    ga('create', 'UA-63951801-1', 'auto');
    ga('send', 'pageview');
</script>
</body>
</html>
");

        }
    }
}
#pragma warning restore 1591
