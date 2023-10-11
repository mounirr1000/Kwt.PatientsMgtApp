using System.Web;
using System.Web.Optimization;

namespace Kwt.PatientsMgtApp.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        //Included for kendoui to migrate with the new Jquery V3
                        "~/Scripts/jquery-migrate-3.0.0.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        //new for printing specific div and conserving styles
                        "~/Scripts/printThis.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));



            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            ////Included for kendoui to migrate with the new Jquery V3
            //bundles.Add(new ScriptBundle("~/bundles/jquerymigrate").Include(
            //            "~/Scripts/jquery-migrate-{version}.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
            // "~/Scripts/Kendo/kendo.all.min.js",
            //// "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
            //"~/Scripts/Kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendoui").Include(
                      "~/Scripts/kendo.ui.core.min.js",
                      "~/Scripts/kendo.all.min.js",
                     // "~/Scripts/Kendo/kendo.all.min.js",// new with kendo ui for mvc
                      "~/Scripts/kendo.aspnetmvc.min.js"// new with kendo ui for mvc
                      ));

            //bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
            //"~/Content/Kendo/kendo.common.min.css",
            //"~/Content/Kendo/kendo.default.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      
                     // "~/Content/kendo.common.min.css",
                      "~/Content/kendo.metro.min.css",
                      
                      "~/Content/Kendo/kendo.common.min.css",// new with kendo ui for mvc
                      "~/Content/Kendo/kendo.default.min.css",// new with kendo ui for mvc
                      //"~/Content/themes/variables.scss",
                      //"~/Content/themes/kendo_theme.css",


                      "~/Content/themes/base/jquery.ui.css",
                       //"~/Content/fontawesome"));
                       "~/Content/font-awesome.css"));
            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //  "~/Content/themes/base/jquery.ui.css"));

            bundles.IgnoreList.Clear();
        }
    }
}
