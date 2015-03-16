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
    
    #line 1 "..\..\Views\Shared\_Layout.cshtml"
    using System.Text.RegularExpressions;
    
    #line default
    #line hidden
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
    
    #line 2 "..\..\Views\Shared\_Layout.cshtml"
    using Dilemma.Business.Services;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Shared\_Layout.cshtml"
    using Dilemma.Business.ViewModels;
    
    #line default
    #line hidden
    using Dilemma.Common;
    using Dilemma.Web;
    using Disposable.Common.Extensions;
    
    #line 4 "..\..\Views\Shared\_Layout.cshtml"
    using Disposable.Common.ServiceLocator;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/_Layout.cshtml")]
    public partial class _Views_Shared__Layout_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Views_Shared__Layout_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Views\Shared\_Layout.cshtml"
  
    var tidyRegex = new Regex("[^a-zA-Z]");
    Func<string, string> formatCategoryUrl = (x) => tidyRegex.Replace(x, string.Empty).ToLower();
    var counter = 3;
    Func<int> getNextCounter = () => { return counter++; };
    Func<string, string> getMenuClass = (x) => x == ViewBag.Category ? "is-page-current" : string.Empty;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" http-equiv=\"X-UA-Compatible\"");

WriteLiteral(" content=\"IE=edge\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width, initial-scale=1.0\"");

WriteLiteral("/>\r\n    <meta");

WriteLiteral(" name=\"description\"");

WriteLiteral(" content=\"\"");

WriteLiteral(">\r\n\r\n    <title>");

            
            #line 21 "..\..\Views\Shared\_Layout.cshtml"
      Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral(" - Dilemma</title>\r\n");

WriteLiteral("    ");

            
            #line 22 "..\..\Views\Shared\_Layout.cshtml"
Write(Styles.Render("~/Content/css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 23 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/modernizr"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n    <!--icons-->\r\n    <link");

WriteLiteral(" rel=\"shortcut icon\"");

WriteLiteral(" href=\"/Content/images/icons/favicon.png\"");

WriteLiteral(" type=\"image/x-icon\"");

WriteLiteral("/>\r\n    <link");

WriteLiteral(" rel=\"apple-touch-icon-precomposed\"");

WriteLiteral(" href=\"/Content/images/icons/apple-touch-icon-precomposed.png\"");

WriteLiteral(" type=\"image/png\"");

WriteLiteral("/>\r\n\r\n    <!-- send jquery to the foot of the page -->\r\n    <script");

WriteLiteral(" type=\'text/javascript\'");

WriteLiteral(">window.q = []; window.$ = function (f) { q.push(f) }</script>\r\n\r\n</head>\r\n<body>" +
"\r\n    <a");

WriteLiteral(" class=\"skip-link screen-reader-text\"");

WriteLiteral(" href=\"#content\"");

WriteLiteral(">Skip to content</a>\r\n    \r\n    <header");

WriteLiteral(" role=\"banner\"");

WriteLiteral(" class=\"page-hd\"");

WriteLiteral(">\r\n        <nav");

WriteLiteral(" class=\"navbar navbar-default navbar-inverse\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"container-fluid\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"wrap\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"navbar-header\"");

WriteLiteral(">\r\n                        <a");

WriteLiteral(" href=\"/dilemmas?category=all\"");

WriteLiteral(" rel=\"home\"");

WriteLiteral(" class=\"page-logo\"");

WriteLiteral(">\r\n                            <img");

WriteLiteral(" src=\"/Content/images/device.png\"");

WriteLiteral(" alt=\"\"");

WriteLiteral(" width=\"60\"");

WriteLiteral(" style=\"position: absolute;top: -5px; margin-right: 5px;\"");

WriteLiteral(">\r\n                            <img");

WriteLiteral(" src=\"/Content/images/logo.png\"");

WriteLiteral(" alt=\"\"");

WriteLiteral(" width=\"200\"");

WriteLiteral(" style=\" margin: 10px 0 0 70px;\"");

WriteLiteral(">\r\n                        </a>\r\n                    </div>\r\n\r\n                  " +
"  <ul");

WriteLiteral(" class=\"nav navbar-nav right\"");

WriteLiteral(">\r\n                        <li");

WriteLiteral(" class=\"dropdown nav-level-1\"");

WriteLiteral(">\r\n                            <a");

WriteLiteral(" href=\"/dilemmas\"");

WriteLiteral(" class=\"dropdown-toggle nav-level-1-link js-nav-level-2-menu-trigger\"");

WriteLiteral(" role=\"button\"");

WriteLiteral(" aria-expanded=\"false\"");

WriteLiteral(">\r\n                                <img");

WriteLiteral(" src=\"/Content/images/icons/conversation.png\"");

WriteLiteral(" alt=\"\"");

WriteLiteral(" width=\"30\"");

WriteLiteral(">\r\n                                <span");

WriteLiteral(" class=\"nav-level-1-text\"");

WriteLiteral(">\r\n                                    Advise &amp; Vote \r\n                      " +
"              <span");

WriteLiteral(" class=\"caret\"");

WriteLiteral("></span>\r\n                                </span>\r\n                \r\n            " +
"                </a>\r\n                            <ul");

WriteLiteral(" class=\"nav-level-2-menu\"");

WriteLiteral(" role=\"menu\"");

WriteLiteral(">\r\n                                <li");

WriteAttribute("class", Tuple.Create(" class=\"", 2758), Tuple.Create("\"", 2830)
, Tuple.Create(Tuple.Create("", 2766), Tuple.Create("nav-level-2", 2766), true)
, Tuple.Create(Tuple.Create(" ", 2777), Tuple.Create("in-category-1", 2778), true)
, Tuple.Create(Tuple.Create(" ", 2791), Tuple.Create("js-categories-all", 2792), true)
            
            #line 58 "..\..\Views\Shared\_Layout.cshtml"
      , Tuple.Create(Tuple.Create(" ", 2809), Tuple.Create<System.Object, System.Int32>(getMenuClass("all")
            
            #line default
            #line hidden
, 2810), false)
);

WriteLiteral(">\r\n                                    <a");

WriteLiteral(" href=\"/dilemmas\"");

WriteLiteral(" class=\"nav-level-2-link\"");

WriteLiteral(">\r\n                                        <span");

WriteLiteral(" class=\"glyphicon glyphicon-refresh\"");

WriteLiteral(" aria-hidden=\"true\"");

WriteLiteral("></span>\r\n                                        All Categories\r\n               " +
"                     </a>\r\n                                </li>\r\n              " +
"                  <li");

WriteAttribute("class", Tuple.Create(" class=\"", 3199), Tuple.Create("\"", 3283)
, Tuple.Create(Tuple.Create("", 3207), Tuple.Create("nav-level-2", 3207), true)
, Tuple.Create(Tuple.Create(" ", 3218), Tuple.Create("in-category-2", 3219), true)
, Tuple.Create(Tuple.Create(" ", 3232), Tuple.Create("js-categories-bookmarks", 3233), true)
            
            #line 64 "..\..\Views\Shared\_Layout.cshtml"
            , Tuple.Create(Tuple.Create(" ", 3256), Tuple.Create<System.Object, System.Int32>(getMenuClass("bookmarks")
            
            #line default
            #line hidden
, 3257), false)
);

WriteLiteral(">\r\n                                    <a");

WriteLiteral(" href=\"/dilemmas/bookmarks\"");

WriteLiteral(" class=\"nav-level-2-link\"");

WriteLiteral(">\r\n                                        <span");

WriteLiteral(" class=\"glyphicon glyphicon-refresh\"");

WriteLiteral(" aria-hidden=\"true\"");

WriteLiteral("></span>\r\n                                        My Bookmarks\r\n                 " +
"                   </a>\r\n                                </li>\r\n");

            
            #line 70 "..\..\Views\Shared\_Layout.cshtml"
                                
            
            #line default
            #line hidden
            
            #line 70 "..\..\Views\Shared\_Layout.cshtml"
                                 foreach (CategoryViewModel category in ViewBag.Categories)
                                {

            
            #line default
            #line hidden
WriteLiteral("                                    <li");

WriteAttribute("class", Tuple.Create(" class=\"", 3792), Tuple.Create("\"", 3889)
, Tuple.Create(Tuple.Create("", 3800), Tuple.Create("nav-level-2", 3800), true)
, Tuple.Create(Tuple.Create(" ", 3811), Tuple.Create("in-category-", 3812), true)
            
            #line 72 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 3824), Tuple.Create<System.Object, System.Int32>(getNextCounter()
            
            #line default
            #line hidden
, 3824), false)
            
            #line 72 "..\..\Views\Shared\_Layout.cshtml"
        , Tuple.Create(Tuple.Create(" ", 3841), Tuple.Create<System.Object, System.Int32>(getMenuClass(formatCategoryUrl(category.Name))
            
            #line default
            #line hidden
, 3842), false)
);

WriteLiteral(">\r\n                                        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 3935), Tuple.Create("\"", 3985)
, Tuple.Create(Tuple.Create("", 3942), Tuple.Create("/dilemmas/", 3942), true)
            
            #line 73 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 3952), Tuple.Create<System.Object, System.Int32>(formatCategoryUrl(category.Name)
            
            #line default
            #line hidden
, 3952), false)
);

WriteLiteral(" class=\"nav-level-2-link\"");

WriteLiteral(">\r\n                                            <span");

WriteLiteral(" class=\"glyphicon glyphicon-refresh\"");

WriteLiteral(" aria-hidden=\"true\"");

WriteLiteral("></span>\r\n");

WriteLiteral("                                            ");

            
            #line 75 "..\..\Views\Shared\_Layout.cshtml"
                                       Write(category.Name);

            
            #line default
            #line hidden
WriteLiteral("\r\n                                        </a>\r\n                                 " +
"   </li>\r\n");

            
            #line 78 "..\..\Views\Shared\_Layout.cshtml"
                                }

            
            #line default
            #line hidden
WriteLiteral("                            </ul>\r\n                        </li>\r\n               " +
"         <li");

WriteLiteral(" class=\"nav-level-1\"");

WriteLiteral(">\r\n                            <!-- <a href=\"#\" data-toggle=\"modal\" data-target=\"" +
"#modal-ask\"> -->\r\n                            <a");

WriteLiteral(" href=\"/ask\"");

WriteLiteral(" class=\"nav-level-1-link\"");

WriteLiteral(">\r\n                                <img");

WriteLiteral(" src=\"/Content/images/icons/ask.png\"");

WriteLiteral(" alt=\"\"");

WriteLiteral(" width=\"30\"");

WriteLiteral(">\r\n                                <span");

WriteLiteral(" class=\"nav-level-1-text\"");

WriteLiteral(">Ask</span>\r\n                            </a>\r\n                        </li>\r\n   " +
"                     <li");

WriteLiteral(" class=\"nav-level-1\"");

WriteLiteral(">\r\n                            <a");

WriteLiteral(" href=\"/profile\"");

WriteLiteral(" class=\"nav-level-1-link\"");

WriteLiteral(">\r\n                                <img");

WriteLiteral(" src=\"/Content/images/icons/user.png\"");

WriteLiteral(" alt=\"\"");

WriteLiteral(" width=\"26\"");

WriteLiteral(">\r\n                                <span");

WriteLiteral(" class=\"nav-level-1-text\"");

WriteLiteral(">Profile</span>\r\n                                <!--?php $unread_notifications =" +
" 99; ?-->\r\n                                <!--?php if ($unread_notifications > " +
"0) { ?-->\r\n                                <span");

WriteLiteral(" class=\"nav-unread-notifications\"");

WriteLiteral(@">
                                    <!--?=$unread_notifications?-->
                                    $unread_notifications
                                </span>
                                <!--?php } ?-->
                            </a>
                        </li>
                    </ul>          
                </div><!-- wrap -->
            </div>
        </nav>

    </header>
    <!-- banner-->
    
    <!--?php require '_ask-modal.php'; ?-->
    
    <div");

WriteLiteral(" class=\"page-bd\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 112 "..\..\Views\Shared\_Layout.cshtml"
   Write(RenderBody());

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n    \r\n    <footer");

WriteLiteral(" role=\"contentinfo\"");

WriteLiteral(" class=\"page-ft\"");

WriteLiteral(">\r\n        <small>\r\n            <div");

WriteLiteral(" class=\"wrap\"");

WriteLiteral(">\r\n                <ul");

WriteLiteral(" class=\"unstyled inline with-separators\"");

WriteLiteral(">\r\n                    <li><a");

WriteLiteral(" href=\"#\"");

WriteLiteral(">Privacy Policy</a></li>\r\n                    <li><a");

WriteLiteral(" href=\"#\"");

WriteLiteral(">Terms &amp; Conditions</a></li>\r\n                    <li><a");

WriteLiteral(" href=\"#\"");

WriteLiteral(">Cookies Policy</a></li>\r\n                    <li><a");

WriteLiteral(" href=\"#\"");

WriteLiteral(">Contact Us</a></li>\r\n                </ul>\r\n                <div");

WriteLiteral(" class=\"copyright\"");

WriteLiteral(">&copy;&#160;Our Dilemmas&#160;2015&#160;All&#160;rights&#160;reserved</div>\r\n   " +
"         </div><!-- wrap-->\r\n        </small>\r\n    </footer><!-- contentinfo-->\r" +
"\n\r\n    \r\n");

WriteLiteral("    ");

            
            #line 130 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/jquery"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 131 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/cookiedirective"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 132 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/bootstrap"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 133 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/homegrown"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n    \r\n");

WriteLiteral("    ");

            
            #line 136 "..\..\Views\Shared\_Layout.cshtml"
Write(RenderSection("scripts", required: false));

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">$.each(q, function (i, f) { $(f); })</script>\r\n</body>\r\n</html>\r\n");

        }
    }
}
#pragma warning restore 1591
