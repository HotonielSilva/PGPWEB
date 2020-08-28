using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using BradescoPGP.Web.Areas.Portabilidade.Models;
using BradescoPGP.Web.Areas.Portabilidade.Servicos;
using BradescoPGP.Web.Controllers;
using BradescoPGP.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BradescoPGP.Web.Areas.Portabilidade.Controllers
{
    public class GerencialMotivoSubMotivoController : AbstractController
    {
        private readonly IMotivoService _motivoService;
        private readonly PGPEntities _context;
        private readonly IUsuarioService _usuarioService;
        private ISolicitacaoService _solicitacaoService;

        public GerencialMotivoSubMotivoController(DbContext context, 
            ISolicitacaoService solicitacaoService, 
            IUsuarioService usuarioService, IMotivoService motivoService) : base(context)
        {
            _motivoService = motivoService;

            _context = context as PGPEntities;

            _usuarioService = usuarioService;

            _solicitacaoService = solicitacaoService;
        }

        // GET: Portabilidade/GerencialMotivoSubMotivo
        public ActionResult Index(FiltrosPortabilidade filtros)
        {
            ViewBag.Titulo = "Motivo/Submotivo";
            var model = default(List<Solicitacao>);
            var motivos = new List<Motivo>();

            if (TemFiltro(filtros))
            {
                model = _solicitacaoService.Obter(filtros.De.Value, filtros.Ate.Value);
            }
            else
            {
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = new DateTime(dataAtual.Year, dataAtual.Month, dataAtual.Day - 1);

                model = _solicitacaoService.Obter(minDate, maxDate);
            }

            if (Cargo == NivelAcesso.Gestor.ToString())
            {
                model = model.Join(_context.Usuario, s => s.MatriculaConsultor, u => u.Matricula,
                    (d, u) => new { d, u.MatriculaSupervisor })
                    .Where(s => s.MatriculaSupervisor == MatriculaUsuario)
                    .Select(s => s.d).ToList();
            }


            var viewModel = model.ConvertAll(s => SolicitacaoViewModel.Mapear(s));

            ViewBag.TododsMotivos = _motivoService.ObterTodosMotivos(true);

            return View(viewModel);
        }
     

        public JsonResult PreencherTabela(int submotivo, FiltrosPortabilidade filtros = null)
        {
            var dados = default(List<Solicitacao>);

            if (TemFiltro(filtros))
            {
                dados = _solicitacaoService.Obter(filtros.De.Value, filtros.Ate.Value);
            }
            else
            {
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = new DateTime(dataAtual.Year, dataAtual.Month, dataAtual.Day - 1);

                dados = _solicitacaoService.Obter(minDate, maxDate);
            }

            dados = dados.Where(s => s.SubMotivoId == submotivo).ToList();


            return Json(new { data = dados.ConvertAll(s => TabelaMotivoSubmotivoViewModel.Mapear(s, _usuarioService.ObterNomeUsuario(s.MatriculaConsultor))) }, JsonRequestBehavior.AllowGet);
        }

        public bool TemFiltro(FiltrosPortabilidade filtros)
        {
            return filtros.De.HasValue || filtros.Ate.HasValue;
        }
    }

}