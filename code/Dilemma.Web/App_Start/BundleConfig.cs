﻿using System.Web.Optimization;

namespace Dilemma.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/cookiedirective").Include(
                        "~/Scripts/jquery.cookiesdirective.js",
                        "~/Scripts/jquery.cookesdirective.configuration.js"));
            
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/homegrown").Include(
                        "~/Scripts/homegrown/plugins/waypoints.js",
                        "~/Scripts/homegrown/plugins/waypoints-sticky.js",
                        "~/Scripts/homegrown/plugins/jquery.cycle2.min.js",
                        "~/Scripts/homegrown/plugins/jquery.cycle2.swipe.min.js",
                        "~/Scripts/homegrown/main.js"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/homegrown/main.css"));
                      //"~/Content/alpha-fixes.css"));
                        //"~/Content/bootstrap/bootstrap.css",
                      //"~/Content/site.css"));
        }
    }
}
