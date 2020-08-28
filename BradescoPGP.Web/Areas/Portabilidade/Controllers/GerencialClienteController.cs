using BradescoPGP.Common;
using System.Configuration;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Models;
using BradescoPGP.Web.Areas.Portabilidade.Servicos;
using BradescoPGP.Web.Controllers;
using BradescoPGP.Web.Models;
using BradescoPGP.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;

namespace BradescoPGP.Web.Areas.Portabilidade.Controllers
{
    public class GerencialClienteController : AbstractController
    {
        private readonly PGPEntities _context;

        private ISolicitacaoService _soliticacao;
        private IUtil _util;

        public GerencialClienteController(DbContext context, ISolicitacaoService solicitacaoService, IUtil util) : base(context)
        {
            _context = context as PGPEntities;

            _soliticacao = solicitacaoService;

            _util = util;
        }

        // GET: Portabilidade/GerencialCliente
        public ActionResult Index(FiltrosPortabilidade filtros)
        {
            ViewBag.Titulo = "Portabilidade Cliente";
            var model = default(List<Solicitacao>);

            if (TemFiltro(filtros))
            {
                model = _soliticacao.Obter(filtros);
            }
            else
            {
                var dataAtual = DateTime.Now;

                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);

                var maxDate = dataAtual.Date;

                model = _soliticacao.Obter(minDate, maxDate);
            }

            if (Cargo == NivelAcesso.Gestor.ToString())
            {
                model = model.Join(_context.Usuario, s => s.MatriculaConsultor, u => u.Matricula,
                    (d, u) => new { d, u.MatriculaSupervisor })
                    .Where(s => s.MatriculaSupervisor == MatriculaUsuario)
                    .Select(s => s.d).ToList();
            }


            ViewBag.Status = SelectListItemGenerator.Status(EventosStatusMotivosOrigens.Portabilidade.ToString());

            return View(model);
        }

        public string ObterSolicitacao(int idSolicitacao)
        {
            var soli = _soliticacao.Obter(idSolicitacao);

            if (soli == null)
                return "";

            var clienteModel = OperacionalViewModel.Mapear(soli);

            var motivos = new List<Motivo>();

            if (soli.DataInicioProcesso.Year >= 2020)
            {
                motivos = _context.Motivo.Where(s => s.Evento == "Portabilidade").ToList(); //obter apenas ativos
            }
            else
            {
                motivos = _context.Motivo.Where(s => s.Evento == "Portabilidade").ToList(); // Obter com os inativos
            }

            clienteModel.MotivosCombo = motivos.ToDictionary(k => k.Id, v => v.Descricao).ToList();

            var json = JsonConvert.SerializeObject(clienteModel);

            return json;
        }

        public ActionResult ExportarExcel(FiltrosPortabilidade filtros)
        {
            var model = default(List<Solicitacao>);

            if (TemFiltro(filtros))
            {
                model = _soliticacao.Obter(filtros);
            }
            else
            {
                //Padrão
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = dataAtual.Date;

                model = _soliticacao.Obter(minDate, maxDate);
            }

            var dadosExcel = model.ConvertAll(s => SolicitacaoViewModel.Mapear(s));

            var excel = _util.GerarExcelPortabilidade(dadosExcel, "Exportação Cliente");

            return File(excel, System.Net.Mime.MediaTypeNames.Application.Octet, "Exportação Cliente.xlsx");
        }

        public bool TemFiltro(FiltrosPortabilidade filtros)
        {
            return filtros.De.HasValue || filtros.Ate.HasValue || !string.IsNullOrEmpty(filtros.Especialista) || !string.IsNullOrEmpty(filtros.Nome) || !string.IsNullOrEmpty(filtros.CPF) || filtros.Status.HasValue;
        }
    }
    
}