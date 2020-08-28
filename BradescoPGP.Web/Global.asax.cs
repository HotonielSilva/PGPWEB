using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BradescoPGP.Repositorio;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using Newtonsoft.Json;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using BradescoPGP.Web.Areas.Portabilidade.Servicos;

namespace BradescoPGP.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //var name = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(5);
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<DbContext, PGPEntities>(Lifestyle.Scoped);

            container.Register<ISolicitacaoService, SolicitacaoService>(Lifestyle.Scoped);

            container.Register<IUsuarioService, UsuarioService>(Lifestyle.Scoped);

            container.Register<IMotivoService, MotivoService>(Lifestyle.Scoped);

            container.Register<IStatusSubStatusService, StatusSubStatusService>(Lifestyle.Scoped);

            container.Register<IUtil, Util>(Lifestyle.Scoped);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
    }
}
