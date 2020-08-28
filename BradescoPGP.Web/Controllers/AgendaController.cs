using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using BradescoPGP.Web.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public class AgendaController : AbstractController
    {
        public AgendaService serivicoAgenda = new AgendaService();
        private readonly PGPEntities _context;

        public AgendaController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
        }

        public ActionResult Index()
        {
            var vencimentos = new List<Vencimento>();
            var pipelines = new List<Pipeline>();
            var teds = new List<TED>();
            var eventos = new List<Evento>();
            var agendaViewModel = new AgendaViewModel();

            if (User.IsInRole(NivelAcesso.Especialista.ToString()))
            {
                agendaViewModel = serivicoAgenda.ObterAgendaCompleta(MatriculaUsuario, Cargo);
            }
            ViewBag.Titulo = "Agenda";
            return View(agendaViewModel);
        }

        [HttpPost]
        public ActionResult NovoEvento([Bind(Exclude ="Finalizado")]Evento evento)
        {
            var eventoResposta = serivicoAgenda.NovoEvento(evento);
            if(!string.IsNullOrEmpty(eventoResposta.Titulo))
            {
                return Json(new { success = true, evento = eventoResposta });
            }
            return Json(new { success = false, evento = eventoResposta });
        }

        [HttpPost]
        public JsonResult DeletarEvento(int id)
        {
            if (serivicoAgenda.ExcluirEvento(id))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }


        public JsonResult NotificacaoEvento()
        {
            var dataPesquisa = DateTime.Now.AddHours(1);

            var eventos = _context.Evento
                .Join(_context.Usuario, e => e.MatriculaConsultor, u => u.Matricula, (ev,u) => new { ev })
                
                .Where(e => e.ev.MatriculaConsultor == MatriculaUsuario && e.ev.DataHoraInicio >= DateTime.Now && 
                    e.ev.DataHoraInicio <= dataPesquisa && !e.ev.Notificado)
                .Select(s => s.ev)
                    .ToList();

            eventos.ForEach(e =>
            {
                e.Notificado = true;
            });


            _context.SaveChanges();

            return Json(eventos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NotificacaoPipeline()
        {
            var maxDate = DateTime.Now.AddHours(1);

            var pipes = _context.Pipeline
                .Where(p =>
                    p.MatriculaConsultor == MatriculaUsuario &&
                    (p.DataProrrogada.HasValue ? p.DataProrrogada >= DateTime.Now && p.DataProrrogada <= maxDate :
                    p.DataPrevista >= DateTime.Now && p.DataPrevista <= maxDate) && 
                    !p.Notificado
                    ).ToList();

            pipes.ForEach(p =>
            {
                p.Notificado = true;
            });

            _context.SaveChanges();

            var result = pipes.ConvertAll(p => PipelineViewModel.Mapear(p));

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}