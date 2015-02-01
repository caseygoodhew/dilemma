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

namespace Dilemma.Web.Views.Shared
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
    
    #line 1 "..\..\Views\Shared\_Layout.cshtml"
    using Dilemma.Business.Services;
    
    #line default
    #line hidden
    using Dilemma.Common;
    using Dilemma.Web;
    using Disposable.Common.Extensions;
    
    #line 2 "..\..\Views\Shared\_Layout.cshtml"
    using Disposable.Common.ServiceLocator;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/_Layout.cshtml")]
    public partial class Layout : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Layout()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Shared\_Layout.cshtml"
  
    var systemServerConfiguration = Locator.Get<IAdministrationService>().GetSystemServerConfiguration();

    var isDevMode = systemServerConfiguration.SystemConfigurationViewModel.SystemEnvironment == SystemEnvironment.Development;
    var isQuestionSeeder = systemServerConfiguration.ServerConfigurationViewModel.ServerRole == ServerRole.QuestionSeeder;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width, initial-scale=1.0\"");

WriteLiteral(">\r\n    <title>");

            
            #line 15 "..\..\Views\Shared\_Layout.cshtml"
      Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral(" - Dilemma</title>\r\n");

WriteLiteral("    ");

            
            #line 16 "..\..\Views\Shared\_Layout.cshtml"
Write(Styles.Render("~/Content/css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 17 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/modernizr"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n</head>\r\n<body>\r\n    <header");

WriteLiteral(" role=\"banner\"");

WriteLiteral(" class=\"page-hd\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"wrap line\"");

WriteLiteral(">\r\n            <a");

WriteLiteral(" href=\"/\"");

WriteLiteral(" rel=\"home\"");

WriteLiteral(" class=\"page-logo\"");

WriteLiteral("><span>Our</span>Dilemmas</a>\r\n            <ul");

WriteLiteral(" class=\"nav-main\"");

WriteLiteral(">\r\n");

            
            #line 25 "..\..\Views\Shared\_Layout.cshtml"
                
            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\Shared\_Layout.cshtml"
                 if (!isQuestionSeeder)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <li>");

            
            #line 27 "..\..\Views\Shared\_Layout.cshtml"
                   Write(Html.ActionLink("QList", "List", "Question"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

WriteLiteral("                    <li>");

            
            #line 28 "..\..\Views\Shared\_Layout.cshtml"
                   Write(Html.ActionLink("Activity", "Index", "Activity"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

WriteLiteral("                    <li>");

            
            #line 29 "..\..\Views\Shared\_Layout.cshtml"
                   Write(Html.ActionLink("Ask", "Create", "Question"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

WriteLiteral("                    <li>");

            
            #line 30 "..\..\Views\Shared\_Layout.cshtml"
                   Write(Html.ActionLink("SysConfig", "Index", "SystemServerConfiguration"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

WriteLiteral("                    <li>");

            
            #line 31 "..\..\Views\Shared\_Layout.cshtml"
                   Write(Html.ActionLink("Notification", "Index", "Notification"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

WriteLiteral("                    <li>");

            
            #line 32 "..\..\Views\Shared\_Layout.cshtml"
                   Write(Html.ActionLink("Moderation", "Index", "Moderation"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

            
            #line 33 "..\..\Views\Shared\_Layout.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral(@"            </ul>
        </div>
    </header>
    
    
    
    
    <!--div class=""navbar navbar-inverse navbar-fixed-top"">
        <div class=""container"">
            <div class=""navbar-header"">
                <button type=""button"" class=""navbar-toggle"" data-toggle=""collapse"" data-target="".navbar-collapse"">
                    <span class=""icon-bar""></span>
                    <span class=""icon-bar""></span>
                    <span class=""icon-bar""></span>
                </button>
");

WriteLiteral("                ");

            
            #line 49 "..\..\Views\Shared\_Layout.cshtml"
           Write(Html.ActionLink("Dilemma", "Index", "Home", null, new { @class = "navbar-brand" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n");

            
            #line 51 "..\..\Views\Shared\_Layout.cshtml"
            
            
            #line default
            #line hidden
            
            #line 51 "..\..\Views\Shared\_Layout.cshtml"
             if (!isQuestionSeeder)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"navbar-collapse collapse\"");

WriteLiteral(">\r\n                    <ul");

WriteLiteral(" class=\"nav navbar-nav\"");

WriteLiteral(">\r\n                        <li>");

            
            #line 55 "..\..\Views\Shared\_Layout.cshtml"
                       Write(Html.ActionLink("QList", "List", "Question"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                        <li>");

            
            #line 56 "..\..\Views\Shared\_Layout.cshtml"
                       Write(Html.ActionLink("Activity", "Index", "Activity"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                        <li>");

            
            #line 57 "..\..\Views\Shared\_Layout.cshtml"
                       Write(Html.ActionLink("Ask", "Create", "Question"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                        <li>");

            
            #line 58 "..\..\Views\Shared\_Layout.cshtml"
                       Write(Html.ActionLink("SysConfig", "Index", "SystemConfiguration"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                        <li>");

            
            #line 59 "..\..\Views\Shared\_Layout.cshtml"
                       Write(Html.ActionLink("Notification", "Index", "Notification"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                        <li>");

            
            #line 60 "..\..\Views\Shared\_Layout.cshtml"
                       Write(Html.ActionLink("Moderation", "Index", "Moderation"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                    </ul>\r\n                    <ul");

WriteLiteral(" class=\"nav navbar-nav navbar-right\"");

WriteLiteral(">\r\n                        <li");

WriteLiteral(" class=\"\"");

WriteLiteral("><a");

WriteLiteral(" href=\"/glimpse.axd\"");

WriteLiteral(">Glimpse</a></li>\r\n");

            
            #line 64 "..\..\Views\Shared\_Layout.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 64 "..\..\Views\Shared\_Layout.cshtml"
                         if (isDevMode)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <li>");

            
            #line 66 "..\..\Views\Shared\_Layout.cshtml"
                           Write(Html.ActionLink("Auth", "Index", "QuickAuthentication"));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

            
            #line 67 "..\..\Views\Shared\_Layout.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </ul>\r\n                </div>\r\n");

            
            #line 70 "..\..\Views\Shared\_Layout.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div-->\r\n    <div");

WriteLiteral(" class=\"page-bd\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"wrap\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 76 "..\..\Views\Shared\_Layout.cshtml"
           Write(RenderBody());

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n    \r\n    <footer");

WriteLiteral(" role=\"contentinfo\"");

WriteLiteral(" class=\"page-ft\"");

WriteLiteral(">\r\n        <small>\r\n            <div");

WriteLiteral(" class=\"wrap\"");

WriteLiteral(@">
                <!--ul class=""unstyled inline with-separators"">
                    <li><a href=""#"">Privacy Policy</a></li>
                    <li><a href=""#"">Terms &amp; Conditions</a></li>
                    <li><a href=""#"">Cookies Policy</a></li>
                    <li><a href=""#"">Contact Us</a></li>
                </ul-->
                <div");

WriteLiteral(" class=\"copyright\"");

WriteLiteral(">&copy; ");

            
            #line 90 "..\..\Views\Shared\_Layout.cshtml"
                                         Write(DateTime.Now.Year);

            
            #line default
            #line hidden
WriteLiteral(" - Dilemma</div>\r\n            </div>\r\n        </small>\r\n    </footer>\r\n    \r\n");

WriteLiteral("    ");

            
            #line 95 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/jquery"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 96 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/cookiedirective"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 97 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/bootstrap"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 98 "..\..\Views\Shared\_Layout.cshtml"
Write(RenderSection("scripts", required: false));

            
            #line default
            #line hidden
WriteLiteral("\r\n</body>\r\n</html>\r\n");

        }
    }
}
#pragma warning restore 1591
