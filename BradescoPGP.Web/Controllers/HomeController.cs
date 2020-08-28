using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BradescoPGP.Web.Models;
using BradescoPGP.Web.Services;
using BradescoPGP.Repositorio;
using System.Data.Entity;
using System.Security.Claims;
using System.IO;
using BradescoPGP.Common.Logging;
using BradescoPGP.Common;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public class HomeController : AbstractController
    {
        private AjaxResponses AjaxResponses = new AjaxResponses();
        //private FiltroService filtroService ;
        private BuscaService BuscaService = new BuscaService();
        private readonly PGPEntities _context;

        public HomeController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
            
        }

        public ActionResult Index()
        {
            var agenda = default(AgendaViewModel);

            if (User.IsInRole(NivelAcesso.Especialista.ToString()))
            {
                var agendaService = new AgendaService();
                agenda = agendaService.ObterAgendaCompleta(MatriculaUsuario, Cargo);
            }

            ViewBag.Titulo = "Home";

            ViewBag.Redirect = false;

            var especialistas = SelectListItemGenerator.Especialistas();

            if (Cargo == NivelAcesso.Especialista.ToString())
            {
                var indice = especialistas.FindIndex(p => p.Text.ToUpper() == ((ClaimsIdentity)User.Identity).Name.ToUpper());

                especialistas[indice].Selected = true;
            }

            ViewBag.Especialistas = especialistas;

            var viewModel = new ClienteViewModel() { Agenda = agenda };

            ViewBag.Error = false;

            return View(viewModel);
        }

        public ViewResult RedirecionarHome(int? agencia, int? conta, string cpfCnpj = null, bool carrgarPopUp = true)
        {
            var resultado = BuscaService.Buscar(agencia, conta, cpfCnpj, MatriculaUsuario, Cargo);

            var viewModel = default(ClienteViewModel);

            if (resultado.NomeCliente != null)
            {
               
                ViewBag.Redirect = true;
                ViewBag.Error = false;
            }
            else
            {
                ViewBag.Redirect = true;
                ViewBag.Error = true;
                ViewBag.ErrorMessage = "Cliente não encontrado no Cockpit";
                carrgarPopUp = false;
            }

            if(resultado.Agenda == null)
            {
                var agendaService = new AgendaService();

                resultado.Agenda = agendaService.ObterAgendaCompleta(MatriculaUsuario,Cargo);
            }

            viewModel = resultado;

            ViewBag.CarregarPopUps = carrgarPopUp;

            ViewBag.StatusVencimento = Services.SelectListItemGenerator.Status(EventosStatusMotivosOrigens.Vencimentos.ToString());

            ViewBag.Status = Services.SelectListItemGenerator.Status(EventosStatusMotivosOrigens.Pipeline.ToString());

            ViewBag.Motivos = Services.SelectListItemGenerator.Motivos(EventosStatusMotivosOrigens.Pipeline.ToString())/* selectList["Motivos"]*/;

            ViewBag.Origens = Services.SelectListItemGenerator.Origens(EventosStatusMotivosOrigens.Pipeline.ToString()) /*selectList["Origens"]*/;

            var especialistas = Services.SelectListItemGenerator.Especialistas();

            if (Cargo == NivelAcesso.Especialista.ToString())
            {
                var indice = especialistas.FindIndex(p => p.Text.ToUpper() == ((ClaimsIdentity)User.Identity).Name.ToUpper());

                especialistas[indice].Selected = true;
            }

            ViewBag.Especialistas = especialistas;
            ViewBag.Titulo = "Home";

            return View("Index", viewModel);
        }

        public ActionResult Buscar(int? agencia, int? conta, string cpfCnpj, string nome, string especialista)
        {

            if (String.IsNullOrEmpty(nome))
            {
                return RedirectToAction("RedirecionarHome", new { agencia, conta, cpfCnpj, carrgarPopUp = true });
            }

            var listaViewModel = new List<ClienteViewModel>();
            var matruculaUsuario = ((ClaimsIdentity)User.Identity).FindFirst("matricula").Value;
            
             listaViewModel = BuscaService.Buscar(nome, especialista);
            

            return Json(listaViewModel, JsonRequestBehavior.AllowGet);
            //return JsonConvert.SerializeObject(listaViewModel, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
        
        [HttpPost]
        public void SetaSession(string sapa)
        {
            System.Web.HttpContext.Current.Session["password"] = sapa;
        }

        
    }
}
