using System.Web.Mvc;

namespace BradescoPGP.Web.Areas.Portabilidade
{
    public class PortabilidadeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Portabilidade";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {


            context.MapRoute(
                  "Portabilidade_Operacional",
                  "Portabilidade/Operacional/{action}/{id}",
                  new { action = "Index", controller = "Operacional", id = UrlParameter.Optional },
                  namespaces: new[] { "BradescoPGP.Web.Areas.Portabilidade.Controllers" }
              );

            context.MapRoute(
                  "Portabilidade_GerencialIndicadoresRanking",
                  "Portabilidade/GerencialIndicadoresRanking/{action}/{id}",
                  new { action = "Index", controller = "GerencialIndicadoresRanking", id = UrlParameter.Optional },
                  namespaces: new[] { "BradescoPGP.Web.Areas.Portabilidade.Controllers" }
              );
            context.MapRoute(
               "Portabilidade_GerencialMotivoSubmotivo",
               "Portabilidade/GerencialMotivoSubMotivo/{action}/{id}",
               new { action = "Index", controller = "GerencialMotivoSubMotivo", id = UrlParameter.Optional },
               namespaces: new[] { "BradescoPGP.Web.Areas.Portabilidade.Controllers" }
           );
            context.MapRoute(
                  "Portabilidade_GerencialIndicadoresEntidade",
                  "Portabilidade/GerencialIndicadoresEntidade/{action}/{id}",
                  new { action = "Index", controller = "GerencialIndicadoresEntidade", id = UrlParameter.Optional },
                  namespaces: new[] { "BradescoPGP.Web.Areas.Portabilidade.Controllers" }
              );
            context.MapRoute(
                  "Portabilidade_ParametroMotivoSubmotivo",
                  "Portabilidade/ParametroMotivoSubmotivo/{action}/{id}",
                  new { action = "Index", controller = "ParametroMotivoSubmotivo", id = UrlParameter.Optional },
                  namespaces: new[] { "BradescoPGP.Web.Areas.Portabilidade.Controllers" }
              );
            context.MapRoute(
               "Portabilidade_ParametroStatusSubStatus",
               "Portabilidade/ParametroStatusSubStatus/{action}/{id}",
               new { action = "Index", controller = "ParametroStatusSubStatus", id = UrlParameter.Optional },
               namespaces: new[] { "BradescoPGP.Web.Areas.Portabilidade.Controllers" }
           );
            context.MapRoute(
                 "Portabilidade_GerencialCliente",
                 "Portabilidade/GerencialCliente/{action}/{id}",
                 new { action = "Index", controller = "GerencialCliente", id = UrlParameter.Optional },
                 namespaces: new[] { "BradescoPGP.Web.Areas.Portabilidade.Controllers" }
             );
            context.MapRoute(
                "Portabilidade_GerencialEspecialista",
                "Portabilidade/GerencialEspecialista/{action}/{id}",
                new { action = "Index", controller = "GerencialEspecialista", id = UrlParameter.Optional },
                namespaces: new[] { "BradescoPGP.Web.Areas.Portabilidade.Controllers" }
            );

            if (context.Routes["Portabilidade_default"] == null)
            {
                context.MapRoute(
                        "Portabilidade_default",
                        "Portabilidade/{controller}/{action}/{id}",
                        new { action = "Index", id = UrlParameter.Optional },
                        namespaces: new[] { "BradescoPGP.Web.Areas.Portabilidade.Controllers" }
                    );
            }

        }
    }
}