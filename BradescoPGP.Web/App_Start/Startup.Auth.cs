using System;
using System.Configuration;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace BradescoPGP.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var coockieOptions = new CookieAuthenticationOptions();
            coockieOptions.AuthenticationType = "ApplicationCookie";
            coockieOptions.AuthenticationMode = AuthenticationMode.Active;
            coockieOptions.LoginPath = new PathString("/Conta/Login");
            coockieOptions.CookieManager = new SystemWebCookieManager();
            coockieOptions.CookieName = "DashBoardApplicationCookie";
            coockieOptions.Provider = new CookieAuthenticationProvider
            {
                OnApplyRedirect = ApplyRedirect
            };
            //coockieOptions.LoginPath = new PathString(ConfigurationManager.AppSettings["portalUrl"]);
            coockieOptions.SlidingExpiration = true;
            coockieOptions.ExpireTimeSpan = TimeSpan.FromMinutes(60);


            app.UseCookieAuthentication(coockieOptions);
        }

        private static void ApplyRedirect(CookieApplyRedirectContext context)
        {
            if (Uri.TryCreate(context.RedirectUri, UriKind.Absolute, out Uri absoluteUri))
            {
                var path = PathString.FromUriComponent(absoluteUri);
                if (path == context.OwinContext.Request.PathBase + context.Options.LoginPath)
                {
                    context.RedirectUri = ConfigurationManager.AppSettings["portalUrl"] +
                        new QueryString(
                            context.Options.ReturnUrlParameter,
                            context.Request.Uri.AbsoluteUri);
                }
            }

            context.Response.Redirect(context.RedirectUri);
        }

        public class SystemWebCookieManager : ICookieManager
        {
            public string GetRequestCookie(IOwinContext context, string key)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                var webContext = context.Get<HttpContextBase>(typeof(HttpContextBase).FullName);
                var cookie = webContext.Request.Cookies[key];
                return cookie == null ? null : cookie.Value;
            }

            public void AppendResponseCookie(IOwinContext context, string key, string value, CookieOptions options)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (options == null)
                {
                    throw new ArgumentNullException("options");
                }

                var webContext = context.Get<HttpContextBase>(typeof(HttpContextBase).FullName);

                bool domainHasValue = !string.IsNullOrEmpty(options.Domain);
                bool pathHasValue = !string.IsNullOrEmpty(options.Path);
                bool expiresHasValue = options.Expires.HasValue;

                var cookie = new HttpCookie(key, value);
                if (domainHasValue)
                {
                    cookie.Domain = options.Domain;
                }
                if (pathHasValue)
                {
                    cookie.Path = options.Path;
                }
                if (expiresHasValue)
                {
                    cookie.Expires = options.Expires.Value;
                }
                if (options.Secure)
                {
                    cookie.Secure = true;
                }
                if (options.HttpOnly)
                {
                    cookie.HttpOnly = true;
                }

                webContext.Response.AppendCookie(cookie);
            }

            public void DeleteCookie(IOwinContext context, string key, CookieOptions options)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (options == null)
                {
                    throw new ArgumentNullException("options");
                }

                AppendResponseCookie(
                    context,
                    key,
                    string.Empty,
                    new CookieOptions
                    {
                        Path = options.Path,
                        Domain = options.Domain,
                        Expires = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    });
            }

            
        }
    }
}