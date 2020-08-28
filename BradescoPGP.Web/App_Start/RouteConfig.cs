using System.Web.Mvc;
using System.Web.Routing;

namespace BradescoPGP.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Ranking",
                url: "Gerencial/Indicadores/Ranking/{action}/{id}",
                defaults: new { controller = "GerencialIndicadoresRanking", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BradescoPGP.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {"BradescoPGP.Web.Controllers"}
            );
        }
    }
}
