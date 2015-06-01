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
    ViewBag.DontPanic = true;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n<h1");

WriteLiteral(" class=\"mb0 mh15 pb10\"");

WriteLiteral(">\r\n    <span");

WriteLiteral(" data-icon=\"user\"");

WriteLiteral("></span> \r\n    Your Profile\r\n</h1>\r\n\r\n<div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n\r\n    <aside");

WriteLiteral(" class=\"sidebar\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 17 "..\..\Views\Profile\Index.cshtml"
   Write(Html.DisplayFor(x => x.Sidebar, "Sidebar", new { HideNotifications = true }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </aside><!-- sidebar -->\r\n        \r\n    <main");

WriteLiteral(" role=\"main\"");

WriteLiteral(" class=\"main\"");

WriteLiteral(">\r\n            \r\n        <div");

WriteLiteral(" class=\"Dilemmas\"");

WriteLiteral(">\r\n\r\n        <div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n\r\n            <div");

WriteLiteral(" class=\"gu1of3\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"#notifications\"");

WriteLiteral(" class=\"Card Notification Notification--mini\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"Notification-score\"");

WriteLiteral(">\r\n                        <div>\r\n                            <span");

WriteLiteral(" data-icon=\"notification\"");

WriteLiteral("></span>\r\n                        </div>\r\n                    </div>\r\n           " +
"         <div");

WriteLiteral(" class=\"Notification-summary\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"Notification--mini-number\"");

WriteLiteral(">4</div>\r\n                        <div");

WriteLiteral(" class=\"Notification--mini-type\"");

WriteLiteral(">NOTIFICATIONS</div>\r\n                    </div><!-- Notification-summary -->\r\n  " +
"              </a>\r\n            </div><!-- gu1of3 -->\r\n\r\n            <div");

WriteLiteral(" class=\"gu1of3\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"#dilemmas\"");

WriteLiteral(" class=\"Card Notification Notification--ask Notification--mini\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"Notification-score\"");

WriteLiteral(">\r\n                        <div>\r\n                            <span");

WriteLiteral(" data-icon=\"ask\"");

WriteLiteral("></span>\r\n                        </div>\r\n                    </div>\r\n           " +
"         <div");

WriteLiteral(" class=\"Notification-summary\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"Notification--mini-number\"");

WriteLiteral(">3</div>\r\n                        <div");

WriteLiteral(" class=\"Notification--mini-type\"");

WriteLiteral(">DILEMMAS</div>\r\n                    </div><!-- Notification-summary -->\r\n       " +
"         </a>\r\n            </div><!-- gu1of3 -->\r\n\r\n            <div");

WriteLiteral(" class=\"gu1of3\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"#answers\"");

WriteLiteral(" class=\"Card Notification Notification--advise Notification--mini\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"Notification-score\"");

WriteLiteral(">\r\n                        <div>\r\n                            <span");

WriteLiteral(" data-icon=\"advise\"");

WriteLiteral("></span>\r\n                        </div>\r\n                    </div>\r\n           " +
"         <div");

WriteLiteral(" class=\"Notification-summary\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"Notification--mini-number\"");

WriteLiteral(">1</div>\r\n                        <div");

WriteLiteral(" class=\"Notification--mini-type\"");

WriteLiteral(">ANSWERS</div>\r\n                    </div><!-- Notification-summary -->\r\n        " +
"        </a>\r\n            </div><!-- gu1of3 -->\r\n\r\n        </div><!-- line -->\r\n" +
"\r\n            <h2");

WriteLiteral(" class=\"js-sticky h3\"");

WriteLiteral(" id=\"notifications\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                    Notifications\r\n                </span>\r\n            </h2>\r" +
"\n\r\n            <div");

WriteLiteral(" class=\"Card Notification Card-go--container\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"/answer.php\"");

WriteLiteral(" class=\"Card-go--trigger\"");

WriteLiteral("></a>\r\n                <div");

WriteLiteral(" class=\"Notification-score\"");

WriteLiteral(">\r\n                    <div>\r\n                        <span");

WriteLiteral(" data-icon=\"notification\"");

WriteLiteral("></span>\r\n                    </div>\r\n                </div>\r\n                <di" +
"v");

WriteLiteral(" class=\"Notification-summary\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"Card-label--small\"");

WriteLiteral(">2 hours ago</div>\r\n                    <div");

WriteLiteral(" class=\"Notification-summary-text\"");

WriteLiteral(">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Blanditiis praesentium" +
", officiis omnis voluptatibus!</div>\r\n                    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"Notification-summary-link\"");

WriteLiteral(">View Details</a>\r\n                </div><!-- Notification-summary -->\r\n         " +
"   </div><!-- Card -->\r\n\r\n            <div");

WriteLiteral(" class=\"Card Notification Card-go--container\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"/answer.php\"");

WriteLiteral(" class=\"Card-go--trigger\"");

WriteLiteral("></a>\r\n                <div");

WriteLiteral(" class=\"Notification-score\"");

WriteLiteral(">\r\n                    <div>+6\r\n                        <span");

WriteLiteral(" class=\"Notification-score-label\"");

WriteLiteral(">Points</span>\r\n                    </div>\r\n                </div>\r\n             " +
"   <div");

WriteLiteral(" class=\"Notification-summary\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"Card-label--small\"");

WriteLiteral(">2 hours ago</div>\r\n                    <div");

WriteLiteral(" class=\"Notification-summary-text\"");

WriteLiteral(">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Blanditiis praesentium" +
", officiis omnis voluptatibus!</div>\r\n                    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"Notification-summary-link\"");

WriteLiteral(">View Details</a>\r\n                </div><!-- Notification-summary -->\r\n         " +
"   </div><!-- Card -->\r\n\r\n");

WriteLiteral("            ");

            
            #line 104 "..\..\Views\Profile\Index.cshtml"
       Write(Html.DisplayForList(x => x.Notifications, "Notifications"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n\r\n            <h2");

WriteLiteral(" class=\"js-sticky h3\"");

WriteLiteral(" id=\"dilemmas\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                    Your Current Dilemmas\r\n                </span>\r\n          " +
"  </h2>\r\n\r\n            <div");

WriteLiteral(" class=\"Card Notification Notification--ask Card-go--container\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"/answer.php\"");

WriteLiteral(" class=\"Card-go--trigger\"");

WriteLiteral("></a>\r\n                <div");

WriteLiteral(" class=\"Notification-score\"");

WriteLiteral(">\r\n                    <div>\r\n                        <span");

WriteLiteral(" data-icon=\"ask\"");

WriteLiteral("></span>\r\n                    </div>\r\n                </div>\r\n                <di" +
"v");

WriteLiteral(" class=\"Notification-summary\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"Card-label--small\"");

WriteLiteral(">2 hours ago</div>\r\n                    <div");

WriteLiteral(" class=\"Notification-summary-text\"");

WriteLiteral(">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Blanditiis praesentium" +
", officiis omnis voluptatibus!</div>\r\n                    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"Notification-summary-link\"");

WriteLiteral(">View Details</a>\r\n                </div><!-- Notification-summary -->\r\n         " +
"   </div><!-- Card -->\r\n\r\n\r\n");

            
            #line 129 "..\..\Views\Profile\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 129 "..\..\Views\Profile\Index.cshtml"
             if (Model.Dilemmas.Any())
            {
                
            
            #line default
            #line hidden
            
            #line 131 "..\..\Views\Profile\Index.cshtml"
           Write(Html.DisplayForList(x => x.Dilemmas, "QuestionNotification"));

            
            #line default
            #line hidden
            
            #line 131 "..\..\Views\Profile\Index.cshtml"
                                                                             
            }
            else
            {
                
            
            #line default
            #line hidden
            
            #line 135 "..\..\Views\Profile\Index.cshtml"
           Write(Html.Partial("DisplayTemplates/NoDilemmas"));

            
            #line default
            #line hidden
            
            #line 135 "..\..\Views\Profile\Index.cshtml"
                                                            
            }

            
            #line default
            #line hidden
WriteLiteral("\r\n            <h2");

WriteLiteral(" class=\"js-sticky h3\"");

WriteLiteral(" id=\"answers\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"js-sticky--offset\"");

WriteLiteral(">\r\n                    Advice You\'ve Given\r\n                </span>\r\n            " +
"</h2>\r\n\r\n            <div");

WriteLiteral(" class=\"Card Notification Notification--advise Card-go--container\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"/answer.php\"");

WriteLiteral(" class=\"Card-go--trigger\"");

WriteLiteral("></a>\r\n                <div");

WriteLiteral(" class=\"Notification-score\"");

WriteLiteral(">\r\n                    <div>\r\n                        <span");

WriteLiteral(" data-icon=\"advise\"");

WriteLiteral("></span>\r\n                    </div>\r\n                </div>\r\n                <di" +
"v");

WriteLiteral(" class=\"Notification-summary\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"Card-label--small\"");

WriteLiteral(">2 hours ago</div>\r\n                    <div");

WriteLiteral(" class=\"Notification-summary-text\"");

WriteLiteral(">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Blanditiis praesentium" +
", officiis omnis voluptatibus!</div>\r\n                    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"Notification-summary-link\"");

WriteLiteral(">View Details</a>\r\n                </div><!-- Notification-summary -->\r\n         " +
"   </div><!-- Card -->\r\n\r\n");

            
            #line 158 "..\..\Views\Profile\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 158 "..\..\Views\Profile\Index.cshtml"
             if (Model.Answers.Any())
            {
                
            
            #line default
            #line hidden
            
            #line 160 "..\..\Views\Profile\Index.cshtml"
           Write(Html.DisplayForList(x => x.Answers, "QuestionNotification"));

            
            #line default
            #line hidden
            
            #line 160 "..\..\Views\Profile\Index.cshtml"
                                                                            
            }
            else
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"Card Dilemmas-empty\"");

WriteLiteral(">\r\n                    <p>Looks like it\'s time to give some advice!</p>\r\n        " +
"            <div");

WriteLiteral(" class=\"line\"");

WriteLiteral(">\r\n                        <span>&nbsp;&nbsp;&nbsp;</span>\r\n                     " +
"   <a");

WriteAttribute("href", Tuple.Create(" href=\"", 7091), Tuple.Create("\"", 7130)
            
            #line 168 "..\..\Views\Profile\Index.cshtml"
, Tuple.Create(Tuple.Create("", 7098), Tuple.Create<System.Object, System.Int32>(Url.Action("Index", "Dilemmas")
            
            #line default
            #line hidden
, 7098), false)
);

WriteLiteral(" class=\"Button--ask\"");

WriteLiteral(">\r\n                            <span");

WriteLiteral(" data-icon=\"discuss-2\"");

WriteLiteral("></span> \r\n                            Start Advising Now\r\n                      " +
"  </a>\r\n                    </div>\r\n                </div>\r\n");

            
            #line 174 "..\..\Views\Profile\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("                \r\n\r\n                \r\n        </div>\r\n\r\n    </main><!-- main-->  " +
" \r\n\r\n</div>\r\n    \r\n\r\n");

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
