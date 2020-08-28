using BradescoPGP.Web.App_Start;
using System.Web;
using System.Web.Optimization;

namespace BradescoPGP.Web
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            var bundle = new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/umd/popper.min.js",
                        "~/Scripts/moment/moment.min.js",
                        "~/Scripts/moment/locale/pt-br.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/adminlte.min.js",
                        "~/Scripts/jquery.slimscroll.min.js",
                        "~/Scripts/fullcalendar/fullcalendar.min.js",
                        "~/Scripts/fullcalendar/locale-all.js",
                        "~/Scripts/jquery-ui/jquery-ui.min.js",
                        "~/Scripts/DataTable/jquery.dataTables.min.js",
                        "~/Scripts/DataTable/dataTables.bootstrap.min.js",
                        "~/Scripts/DataTable/fixed_header/dataTables.fixedHeader.min.js",
                        "~/Scripts/DataTable/fixed_header/fixedHeader.bootstrap.js",
                        "~/Scripts/notify/notify.min.js",
                        "~/Scripts/notify/toastr.min.js",
                        "~/Scripts/notify/webkitNotification.js",
                        "~/Scripts/jquery.qtip.min.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js",
                        "~/Scripts/core.js",
                        "~/Scripts/Solicitacao.js",
                        "~/Scripts/Atendimento.js",
                        "~/Scripts/Chart.min.js",
                        "~/Scripts/notifyFunc.js",
                        "~/Scripts/modais.js",
                        "~/Scripts/apexcharts.min.js",
                        "~/Scripts/custom-notify.js",
                        "~/Scripts/jquery.signalR-2.4.1.min.js"
                        );


            bundle.Orderer = new NonOrderingBundleOrderer();

            bundles.Add(bundle);

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                        //~/Scripts/inputmask/dependencyLibs/inputmask.dependencyLib.js",  //if not using jquery
                        "~/Scripts/inputmask/inputmask.js",
                        "~/Scripts/inputmask/jquery.inputmask.js",
                        "~/Scripts/inputmask/inputmask.extensions.js",
                        "~/Scripts/inputmask/inputmask.date.extensions.js",
                        "~/Scripts/inputmask/inputmask.numeric.extensions.js"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").IncludeDirectory("~/Content/adminLte", "*.css").Include(
                      "~/Content/preloader.css",
                      "~/Content/modalMinimizer.css",
                      "~/Content/bootstrap.css",
                      "~/Content/fullcalendar.min.css",
                      "~/Content/adminLte/AdminLTE.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/DataTable/dataTables.bootstrap.min.css",
                      "~/Content/DataTable/select/select.bootstrap.min.css",
                      "~/Content/DataTable/fixed_header/fixedHeader.bootstrap.min.css",
                      "~/Content/DataTable/fixed_header/fixedHeader.dataTables.min.css",
                      "~/Content/DataTable/responsive/responsive.bootstrap.min.css",
                      "~/Content/toastr.min.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/jquery.qtip.min.css",
                      "~/Content/apexcharts.css",
                      //"~/Content/CustomSite.css",
                      "~/Content/Site.css"
                      ));

            bundles.IgnoreList.Clear();
        }
    }
}
