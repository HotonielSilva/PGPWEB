using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
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

namespace BradescoPGP.Web.Areas.Portabilidade.Controllers
{
    public class OperacionalController : AbstractController
    {
        private readonly PGPEntities _context;
        private readonly IUtil _util;

        private ISolicitacaoService _solicitacaoService;

        public OperacionalController(DbContext context, ISolicitacaoService solicitacaoService, IUtil util) : base(context)
        {
            _context = context as PGPEntities;

            _util = util;

            _solicitacaoService = solicitacaoService;
        }


        // GET: Portabilidade/Operacional
        public ActionResult Index(FiltrosPortabilidade filtros)
        {
            ViewBag.Titulo = "Operacional";

            var model = default(List<Solicitacao>);

            if (TemFiltro(filtros))
            {
                model = _solicitacaoService.Obter(filtros);
            }
            else
            {
                var dataAtual = DateTime.Now;

                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);

                var maxDate = dataAtual.Date;

                model = _solicitacaoService.Obter(minDate, maxDate);
            }

            //Verificar Perfil
            if (Cargo == NivelAcesso.Especialista.ToString())
            {
                model = model.Where(s => s.MatriculaConsultor == MatriculaUsuario).ToList();
            }
            else if (Cargo == NivelAcesso.Gestor.ToString())
            {
                model = model.Join(_context.Usuario, s => s.MatriculaConsultor, u => u.Matricula,
                    (d, u) => new { d, u.MatriculaSupervisor })
                    .Where(s => s.MatriculaSupervisor == MatriculaUsuario)
                    .Select(s => s.d).ToList();
            }

            ViewBag.Status = SelectListItemGenerator.Status(EventosStatusMotivosOrigens.Portabilidade.ToString());

            return View(model);
        }


        [HttpPost]
        public JsonResult AtualizarSituacao(OperacionalViewModel solicitacao)
        {
            if (solicitacao == null)
                return Json(new { status = false, mensagem = "Solicitacao não foi preenchido corretamente." });

            if (_solicitacaoService.AtualizarSolicitacao(solicitacao))
                return Json(new { status = true, mensagem = "Registro atualizado com sucesso" });
            else
                return Json(new { status = false, mensagem = "Erro ao salvar atualização." });
        }


        public string ObterSolicitacao(int idSolicitacao)
        {
            var soli = _solicitacaoService.Obter(idSolicitacao);

            if (soli == null)
                return "";

            var operacionalModel = OperacionalViewModel.Mapear(soli);

            operacionalModel.Especialista = _context.Solicitacao.Join(_context.Usuario, s => s.MatriculaConsultor, u => u.Matricula,
               (s, u) => new { u.Nome, s.Id }).FirstOrDefault(s => s.Id == soli.Id)?.Nome;


            var motivos = new List<Motivo>();

            motivos = _context.Motivo.Where(s => s.Evento == "Portabilidade" && s.EmUso.HasValue && s.EmUso.Value).ToList(); //obter apenas ativos
            
            operacionalModel.MotivosCombo = motivos.ToDictionary(k => k.Id, v => v.Descricao).ToList();

            var json = JsonConvert.SerializeObject(operacionalModel);

            return json;
        }

        public string ObterSubMotivos(int motivoId, bool inativos)
        {
            var subMotivos = _context.Motivo.FirstOrDefault(s => s.Id == motivoId)?.SubMotivo.ToList();

            if (!inativos)
                subMotivos = subMotivos.Where(s => s.EmUso == true).ToList();

            var submotivosResult = subMotivos.ConvertAll(s => SubmotivosViewModel.Mapear(s));

            var json = JsonConvert.SerializeObject(submotivosResult);

            return json;
        }

        public string ObterSubStatus(int id)
        {
            var subStatus = _context.SubStatus.Where(s => s.StatusId == id).ToList();

            if(subStatus == null)
            {
                return JsonConvert.SerializeObject(new List<SubStatus>());
            }

            return JsonConvert.SerializeObject(subStatus.ConvertAll(s => new { s.Id, s.Descricao}));
        }

        public ActionResult ExportarExcel(FiltrosPortabilidade filtros)
        {
            var model = default(List<Solicitacao>);

            if (Cargo == NivelAcesso.Especialista.ToString())
                filtros.Especialista = MatriculaUsuario;

            if (TemFiltro(filtros))
            {
                model = _solicitacaoService.Obter(filtros);
            }
            else
            {
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = dataAtual.Date;

                model = _solicitacaoService.Obter(minDate, maxDate);
            }

            var dadosExcel = model.ConvertAll(s => SolicitacaoViewModel.Mapear(s));

            var excel = _util.GerarExcelPortabilidade(dadosExcel, "Exportação Operacional");

            return File(excel, System.Net.Mime.MediaTypeNames.Application.Octet, "Exportação Operacional.xlsx");
        }

        public bool TemFiltro(FiltrosPortabilidade filtros)
        {
            return filtros.De.HasValue || filtros.Ate.HasValue || !string.IsNullOrEmpty(filtros.Especialista) || !string.IsNullOrEmpty(filtros.Nome);
        }
    }
}