using System.Web;
using System.Web.Optimization;

namespace ServisTakip
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/yon/css").Include(
                      "~/Scripts/bootstrap/css/bootstrap.min.css",
                      "~/Scripts/plugins/font-awesome/css/font-awesome.css",
                      "~/assets/css/ionicons.min.css",
                      "~/Scripts/dist/css/AdminLTE.min.css",
                      "~/Scripts/dist/css/skins/_all-skins.min.css",
                      "~/Scripts/plugins/iCheck/flat/blue.css",
                      "~/Scripts/plugins/morris/morris.css",
                      "~/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.css",
                      "~/Scripts/plugins/datepicker/datepicker3.css",
                      "~/Scripts/plugins/daterangepicker/daterangepicker.css",
                      "~/Scripts/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                      "~/Scripts/plugins/select2/select2.min.css",
                      "~/assets/css/buttons.dataTables.min.css"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/yon/js").Include(
                      "~/Scripts/plugins/jQuery/jquery-2.2.3.min.js",
                      "~/Scripts/jquery-ui.min.js",
                      "~/Scripts/bootstrap/js/bootstrap.min.js",
                      "~/Scripts/plugins/raphael/raphael-min.js",
                      "~/Scripts/plugins/morris/morris.min.js",
                      "~/Scripts/plugins/sparkline/jquery.sparkline.min.js",
                      "~/Scripts/plugins/datatables/jquery.dataTables.min.js",
                      "~/Scripts/plugins/datatables/dataTables.bootstrap.min.js",
                      "~/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                      "~/Scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                      "~/Scripts/plugins/knob/jquery.knob.js",
                      "~/Scripts/plugins/moment/moment.min.js",
                      "~/Scripts/plugins/daterangepicker/daterangepicker.js",
                      "~/Scripts/plugins/datepicker/bootstrap-datepicker.js",
                      "~/Scripts/plugins/datepicker/locales/bootstrap-datepicker.tr.js",
                      "~/Scripts/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                      "~/Scripts/plugins/slimScroll/jquery.slimscroll.min.js",
                      "~/Scripts/plugins/fastclick/fastclick.min.js",
                      "~/Scripts/dist/js/pages/dashboard2.js",
                      "~/Scripts/dist/js/app.min.js",
                      "~/Scripts/dist/js/demo.js"
                      ));
        }
    }
}
