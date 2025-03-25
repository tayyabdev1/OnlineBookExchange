using System.Web;
using System.Web.Optimization;

namespace OnlineBookExchange
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/argon").Include(
                      "~/Content/css/nucleo-icons.css",
                      "~/Content/css/nucleo-svg.css",
                      "~/Content/css/argon-dashboard.css"));

            bundles.Add(new ScriptBundle("~/bundles/argon").Include(
                      "~/Scripts/js/argon-dashboard.js",
                      "~/Scripts/js/core/popper.min.js",
                      "~/Scripts/js/core/bootstrap.min.js",
                      "~/Scripts/js/plugins/perfect-scrollbar.min.js",
                      "~/Scripts/js/plugins/smooth-scrollbar.min.js"));

        }
    }
}
