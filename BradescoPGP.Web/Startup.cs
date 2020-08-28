using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BradescoPGP.Web.Startup))]
namespace BradescoPGP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //app.MapSignalR();
        }
    }
}
