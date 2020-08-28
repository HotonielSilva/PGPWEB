using BradescoPGP.Repositorio;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using BradescoPGP.Web.Models;

namespace BradescoPGP.Web.Controllers
{
    public class ContaController : Controller
    {
        private readonly PGPEntities _context;

        public ContaController(DbContext context)
        {
            _context = context as PGPEntities;
        }

        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(string nomeUsuario)
        {
            var user = _context.Usuario.FirstOrDefault(u => u.NomeUsuario.ToLower() == nomeUsuario.ToLower());

            if (user != null)
            {
                var claims = new[] {
                    new Claim("cargo", user.Perfil.Descricao),
                    new Claim("nomeUsuario", user.NomeUsuario),
                    new Claim("equipe", user.Equipe),
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim("matricula", user.Matricula),
                    new Claim(ClaimTypes.Role, user.Perfil.Descricao),
                };

                var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                var context = Request.GetOwinContext();

                var authManager = context.Authentication;

                authManager.SignIn(new AuthenticationProperties
                { IsPersistent = false}, identity);

                //return RedirectToAction("Secure");
                return RedirectToAction("Index", "Home");
            }

            return View("Error", new ErrorViewModel {
                Mensagem = "Você não tem autorização para acessar o PGP",
                Status = "401 - Não autorizado"
            });
        }

        public ActionResult Logoff()
        {
            var context = Request.GetOwinContext();
            
            var authManager = context.Authentication;

            authManager.SignOut("ApplicationCookie");

            Session.Clear();

            return Redirect(ConfigurationManager.AppSettings["portalUrl"]);
        }

        [Authorize]
        public ActionResult Secure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}