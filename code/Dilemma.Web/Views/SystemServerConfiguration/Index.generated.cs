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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/SystemServerConfiguration/Index.cshtml")]
    public partial class _Views_SystemServerConfiguration_Index_cshtml : System.Web.Mvc.WebViewPage<Dilemma.Business.ViewModels.SystemServerConfigurationViewModel>
    {
        public _Views_SystemServerConfiguration_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\SystemServerConfiguration\Index.cshtml"
  
    ViewBag.Title = "System/Server Configuration";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 7 "..\..\Views\SystemServerConfiguration\Index.cshtml"
 using (Html.BeginForm("Index", "SystemServerConfiguration", FormMethod.Post, new { @class = "form-horizontal", role = "form" }))
{
    
            
            #line default
            #line hidden
            
            #line 9 "..\..\Views\SystemServerConfiguration\Index.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 9 "..\..\Views\SystemServerConfiguration\Index.cshtml"
                            
    

            
            #line default
            #line hidden
WriteLiteral("    <h2>System Configuration</h2>\r\n");

            
            #line 12 "..\..\Views\SystemServerConfiguration\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 14 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.SystemConfigurationViewModel.MaxAnswers, new { @class = "col-md-2 col-sm-3 control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 16 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.TextBoxFor(x => x.SystemConfigurationViewModel.MaxAnswers, new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 17 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.SystemConfigurationViewModel.MaxAnswers));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 20 "..\..\Views\SystemServerConfiguration\Index.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 22 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.SystemConfigurationViewModel.QuestionLifetime, new { @class = "col-md-2 col-sm-3 control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 24 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.EnumDropDownListFor(x => x.SystemConfigurationViewModel.QuestionLifetime, new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 25 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.SystemConfigurationViewModel.QuestionLifetime));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 28 "..\..\Views\SystemServerConfiguration\Index.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 30 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.SystemConfigurationViewModel.RetireQuestionAfterDays, new { @class = "col-md-2 col-sm-3 control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 32 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.TextBoxFor(x => x.SystemConfigurationViewModel.RetireQuestionAfterDays, new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 33 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.SystemConfigurationViewModel.RetireQuestionAfterDays));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 36 "..\..\Views\SystemServerConfiguration\Index.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 38 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.SystemConfigurationViewModel.ExpireAnswerSlotsAfterMinutes, new { @class = "col-md-2 col-sm-3 control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 40 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.TextBoxFor(x => x.SystemConfigurationViewModel.ExpireAnswerSlotsAfterMinutes, new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 41 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.SystemConfigurationViewModel.ExpireAnswerSlotsAfterMinutes));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 44 "..\..\Views\SystemServerConfiguration\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 46 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.SystemConfigurationViewModel.SystemEnvironment, new {@class = "col-md-2 col-sm-3 control-label"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 48 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.EnumDropDownListFor(x => x.SystemConfigurationViewModel.SystemEnvironment, new {@class = "form-control"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 49 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.SystemConfigurationViewModel.SystemEnvironment));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 52 "..\..\Views\SystemServerConfiguration\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 54 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.SystemConfigurationViewModel.EnableWebPurify, new { @class = "col-md-2 col-sm-3 control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 56 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.CheckBoxFor(x => x.SystemConfigurationViewModel.EnableWebPurify, new {@class = "form-control"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 57 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.SystemConfigurationViewModel.EnableWebPurify));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 60 "..\..\Views\SystemServerConfiguration\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 62 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.SystemConfigurationViewModel.EmailErrors, new {@class = "col-md-2 col-sm-3 control-label"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 64 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.CheckBoxFor(x => x.SystemConfigurationViewModel.EmailErrors, new {@class = "form-control"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 65 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.SystemConfigurationViewModel.EmailErrors));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 68 "..\..\Views\SystemServerConfiguration\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 70 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.SystemConfigurationViewModel.EmailErrorsTo, new { @class = "col-md-2 col-sm-3 control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 72 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.TextBoxFor(x => x.SystemConfigurationViewModel.EmailErrorsTo, new {@class = "form-control"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 73 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.SystemConfigurationViewModel.EmailErrorsTo));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 76 "..\..\Views\SystemServerConfiguration\Index.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <h2>Server Configuration</h2>\r\n");

            
            #line 78 "..\..\Views\SystemServerConfiguration\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 80 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.ServerConfigurationViewModel.ServerRole, new { @class = "col-md-2 col-sm-3 control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 82 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.EnumDropDownListFor(x => x.ServerConfigurationViewModel.ServerRole, new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 83 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.ServerConfigurationViewModel.ServerRole));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 86 "..\..\Views\SystemServerConfiguration\Index.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 88 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.ServerConfigurationViewModel.Name, new { @class = "col-md-2 col-sm-3 control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 90 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.TextBoxFor(x => x.ServerConfigurationViewModel.Name, new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 91 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.ServerConfigurationViewModel.Name));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 94 "..\..\Views\SystemServerConfiguration\Index.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 96 "..\..\Views\SystemServerConfiguration\Index.cshtml"
   Write(Html.LabelFor(x => x.AccessKey, new { @class = "col-md-2 col-sm-3 control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"col-md-3 col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 98 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.TextBoxFor(x => x.AccessKey, new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 99 "..\..\Views\SystemServerConfiguration\Index.cshtml"
       Write(Html.ValidationMessageFor(x => x.AccessKey));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 102 "..\..\Views\SystemServerConfiguration\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-md-offset-2 col-md-10\"");

WriteLiteral(">\r\n            <input");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" value=\"Submit\"");

WriteLiteral("/>\r\n            <a");

WriteLiteral(" class=\"btn btn-default\"");

WriteAttribute("href", Tuple.Create(" href=\"", 5329), Tuple.Create("\"", 5363)
            
            #line 106 "..\..\Views\SystemServerConfiguration\Index.cshtml"
, Tuple.Create(Tuple.Create("", 5336), Tuple.Create<System.Object, System.Int32>(Url.Action("RefreshCache")
            
            #line default
            #line hidden
, 5336), false)
);

WriteLiteral(" role=\"button\"");

WriteLiteral(">Refresh Cached Entries</a>\r\n        </div>\r\n    </div>\r\n");

            
            #line 109 "..\..\Views\SystemServerConfiguration\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" style=\"margin-top: 50px\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-md-offset-2 col-md-10\"");

WriteLiteral(">\r\n            <a");

WriteLiteral(" class=\"btn btn-default btn-danger\"");

WriteAttribute("href", Tuple.Create(" href=\"", 5590), Tuple.Create("\"", 5629)
            
            #line 112 "..\..\Views\SystemServerConfiguration\Index.cshtml"
, Tuple.Create(Tuple.Create("", 5597), Tuple.Create<System.Object, System.Int32>(Url.Action("TestErrorHandling")
            
            #line default
            #line hidden
, 5597), false)
);

WriteLiteral(" role=\"button\"");

WriteLiteral(">Test Error Handling</a>\r\n        </div>\r\n    </div>    \r\n");

            
            #line 115 "..\..\Views\SystemServerConfiguration\Index.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

DefineSection("Scripts", () => {

WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 118 "..\..\Views\SystemServerConfiguration\Index.cshtml"
Write(Scripts.Render("~/bundles/jqueryval"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

});

        }
    }
}
#pragma warning restore 1591
