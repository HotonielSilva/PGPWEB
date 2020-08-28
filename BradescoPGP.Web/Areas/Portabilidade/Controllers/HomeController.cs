using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.DTO;
using BradescoPGP.Web.Areas.Portabilidade.Enums;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using BradescoPGP.Web.Areas.Portabilidade.Models;
using BradescoPGP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BradescoPGP.Web.Areas.Portabilidade.Controllers
{
    public class HomeController : AbstractController
    {
        private readonly ISolicitacaoService _solicitacaoService;

        private readonly PGPEntities _context;

        private readonly IUtil _util;

        public HomeController(DbContext context, ISolicitacaoService solicitacaoService, IUtil util) : base(context) {
            _solicitacaoService = solicitacaoService;
            _context = context as PGPEntities;
            _util = util;

        }
       
        #region INDEX

        // GET: Portabilidade/Home
        public ActionResult Index(FiltrosPortabilidade filtros)
        {
            ViewBag.Titulo = "Consolidado";

            #region Filtro 
           
            var dados = default(List<Solicitacao>);
            var dadosAVencer = default(List<Solicitacao>);

            if (filtros.TemFiltro())
            {
                dados = _solicitacaoService.Obter(filtros);
                dadosAVencer = _solicitacaoService.ObterAVencer(filtros);
            }   
            else
            {
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = dataAtual.AddDays(-1).Date;

                dados = _solicitacaoService.Obter(minDate, maxDate);
                dadosAVencer = _solicitacaoService.ObterAVencer(minDate, maxDate);
            }

            if (Cargo == NivelAcesso.Especialista.ToString())
            {
                dados = dados.Where(s => s.MatriculaConsultor == MatriculaUsuario).ToList();
                dadosAVencer = dadosAVencer.Where(s => s.MatriculaConsultor == MatriculaUsuario).ToList();
            }
            else if(Cargo == NivelAcesso.Gestor.ToString())
            {
                dados = dados.Join(_context.Usuario, s => s.MatriculaConsultor, u => u.Matricula,
                    (d,u) => new { d, u.MatriculaSupervisor})
                    .Where(s => s.MatriculaSupervisor == MatriculaUsuario)
                    .Select(s => s.d).ToList();

                dadosAVencer = dadosAVencer.Join(_context.Usuario, s => s.MatriculaConsultor, u => u.Matricula,
                    (d, u) => new { d, u.MatriculaSupervisor })
                    .Where(s => s.MatriculaSupervisor == MatriculaUsuario)
                    .Select(s => s.d).ToList();

            }

            #endregion

            #region GRAFICO SOLICITAÇÃO


            var statusLista = _context.Status.Where(s => s.Evento == Eventos.Portabilidade.ToString() && s.Descricao != "Tratado").ToList();

            var substatus = _context.SubStatus.ToList();

            var valoresStatusSolicitacao = new Dictionary<string, int>();

            //var dadosSemNaoElegiveis = RemoverNaoElegivel(dados);

            valoresStatusSolicitacao["Total Solicitações"] = dados.Count();

            foreach (var status in statusLista)
            {
                valoresStatusSolicitacao[status.Descricao] = dados.Where(d => d.StatusId == status.Id).Count();
            }

            foreach (var SubStatus in substatus)
            {
                valoresStatusSolicitacao[SubStatus.Descricao] = dados.Where(d => d.SubStatusId == SubStatus.Id).Count();
            }

            var graficoStatusSolicitacao = new GraficoStatusSolicitacaoViewModel
            {
                Labels = valoresStatusSolicitacao.Keys.ToList(),
                Data = valoresStatusSolicitacao.Values.ToList()
            };
            #endregion

            #region GRAFICO MOTIVOS
            var motivos = _context.Motivo.Where(s => s.Evento == Eventos.Portabilidade.ToString() && s.EmUso.HasValue && s.EmUso.Value).ToList();

            var valorRetido = new List<decimal>();

            var valorNaoRetido = new List<decimal>();

            var motivoRetido = dados.Where(s => s.Motivo.Evento == Eventos.Portabilidade.ToString() && s.Motivo != null);

            foreach (var motivo in motivos) {
                var valoresMotivoRetencao = dados.Where(s => s.MotivoId == motivo.Id);

                var idSubStatus = substatus.Where(s => s.Descricao.StartsWith("Retido") || s.Descricao.StartsWith("Não Retido"))
                    .ToDictionary(k => k.Descricao, v => v.Id);

                valorRetido.Add(valoresMotivoRetencao.Where(p => p.SubStatusId == idSubStatus["Retido"]).Sum(v => { return v.ValorRetido ?? 0; }));
                valorNaoRetido.Add(valoresMotivoRetencao.Where(p => p.SubStatusId == idSubStatus["Não Retido"]).Sum(v => { return v.ValorPrevistoSaida; }));
            }

            var graficoMotivos = new GraficoMotivoViewModel {
                Labels = motivos.Select(s => s.Descricao).ToList(),
                Retido = new DataSet { Data = valorRetido },
                NaoRetido = new DataSet { Data = valorNaoRetido }
            };
            #endregion

            #region GRAFICO OPERACOES
            var umDia = 0;
            var doisDias = 0;
            var tresDias = 0;

            foreach (var dado in dadosAVencer)
            {
                if (dado.PrazoAtendimento.HasValue)
                {
                    var filtroUmDia = 0;
                    var filtroDoisDias = 1;
                    var filtroTresDias = 2;

                    var dataAtual = DateTime.Now.Date;

                    var diaSemana = dataAtual.DayOfWeek;

                    switch (diaSemana)
                    {
                        case DayOfWeek.Thursday:
                            filtroTresDias += 2;
                            break;

                        case DayOfWeek.Friday:
                            filtroDoisDias += 2;
                            filtroTresDias += 2;
                            break;
                    }


                    if ((dado.PrazoAtendimento.Value.Date - dataAtual).TotalDays == filtroUmDia)
                    {
                        umDia++;
                    }
                    else if ((dado.PrazoAtendimento.Value.Date - dataAtual).TotalDays == filtroDoisDias)
                    {
                        doisDias++;
                    }
                    else if ((dado.PrazoAtendimento.Value.Date - dataAtual).TotalDays == filtroTresDias)
                    {
                        tresDias++;
                    }
                }
            }

            var graficoOperacoesVencer = new GraficoOperacoesVencerViewModel
            {
                UmDia = umDia,
                DoisDias = doisDias,
                TresDias = tresDias
            };

            #endregion

            #region GRAFICO EQUIPES

            var equipesDict = _context.Usuario.GroupBy(g => g.Matricula)
                .ToDictionary(k => k.Key, v => v.FirstOrDefault().Equipe);

            var equipes = _context.Usuario.Where(s => s.Perfil.Descricao == NivelAcesso.Especialista.ToString()).Select(s => s.Equipe)
                .Distinct().ToList();

            var valorRetidoEquipe = new List<decimal>();

            var valorNaoRetidoEquipe = new List<decimal>();

            foreach (var equipe in equipes) {
                var result = dados.Where(s => equipesDict.ContainsKey(s.MatriculaConsultor) && equipesDict[s.MatriculaConsultor] == equipe);
                
                valorRetidoEquipe.Add(result.Where(p => p.SubStatusId == substatus.FirstOrDefault(s => s.Descricao.StartsWith("Ret")).Id)
                    .Sum(v => { return v.ValorRetido ?? 0; }));

                valorNaoRetidoEquipe.Add(result.Where(p => p.SubStatusId == substatus.FirstOrDefault(s => s.Descricao.StartsWith("Não Ret")).Id)
                    .Sum(v => { return v.ValorPrevistoSaida; }));
            }

            var graficoEquipe = new GraficoEquipeViewModel {
                Labels = equipes.ToList(),
                Retido = new DataSet { Data = valorRetidoEquipe },
                NaoRetido = new DataSet { Data = valorNaoRetidoEquipe }
            };

            #endregion

            var viewModel = new ConsolidadaViewModel {
                StatusSolicitacao = graficoStatusSolicitacao,
                MotivosRetencao = graficoMotivos,
                OperacoesVencer = graficoOperacoesVencer,
                Equipe = graficoEquipe
            };

            return View(viewModel);
        }
        #endregion

        #region Frilto
        private bool TemFiltro(FiltrosPortabilidade filtros) {
            return filtros.De.HasValue && filtros.Ate.HasValue;
        }
        #endregion

        #region METODOS DE PREENCHER TABELAS
        public JsonResult TabelaStatus(string status, string substatus, FiltrosPortabilidade filtros = null) {
            var solicitacoes = default(List<Solicitacao>);
            solicitacoes = ObterPorStatus(status, filtros);
            var model = solicitacoes.ConvertAll(s => TabelaStatusViewModel.Mapear(s));
            var dados = new { data = model };
            return Json(dados, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TabelaAVencer(int periodo, FiltrosPortabilidade filtros = null) {
            var model = ObterAVencer(periodo, filtros);
            var result = model.ConvertAll(s => TabelaStatusViewModel.Mapear(s));
            var dados = new { data = result };
            return Json(dados, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TabelaMotivo(string serie, string motivo, FiltrosPortabilidade filtros = null) {
            var solicitacoes = default(List<Solicitacao>);
            solicitacoes = ObterPorMotivo(serie, motivo, filtros);
            var model = solicitacoes.ConvertAll(s => TabelaStatusViewModel.Mapear(s));
            var dados = new { data = model };
            return Json(dados, JsonRequestBehavior.AllowGet);

        }
        public JsonResult TabelaEquipe(string serie, string equipe, FiltrosPortabilidade filtros = null) {
            var solicitacoes = default(List<Solicitacao>);
            solicitacoes = ObterEquipe(serie, equipe, filtros);
            var model = solicitacoes.ConvertAll(s => TabelaStatusViewModel.Mapear(s));
            var dados = new { data = model};
            return Json(dados, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region MÉTODOS DE OBTER SOLICITACOES       
        private List<Solicitacao> ObterPorStatus(string status, FiltrosPortabilidade filtros = null) {

            var dados = default(List<Solicitacao>);

            if (filtros.TemFiltro())
            {
                dados = _solicitacaoService.Obter(filtros);
            }
            else
            {
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = dataAtual.AddDays(-1).Date;

                dados = _solicitacaoService.Obter(minDate, maxDate);
            }

            if (Cargo == NivelAcesso.Especialista.ToString())
            {
                dados = dados.Where(s => s.MatriculaConsultor == MatriculaUsuario).ToList();
            }

            //var dadosSemNaoElegiveis = RemoverNaoElegivel(dados);

            if (status != "Total Solicitações")
            {
                dados = dados.Where(s => s.Status.Descricao == status || s.SubStatus?.Descricao == status).ToList();
            }

            return dados;
        }

        private List<Solicitacao> ObterAVencer(int periodo, FiltrosPortabilidade filtros = null) {
            var dados = new List<Solicitacao>();

            if (TemFiltro(filtros))
            {
                dados = _solicitacaoService.ObterAVencer(filtros);
            }
            else
            {
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = dataAtual.AddDays(-1);

                dados = _solicitacaoService.ObterAVencer(minDate, maxDate);
            }

            if (Cargo == NivelAcesso.Especialista.ToString())
            {
                dados = dados.Where(s => s.MatriculaConsultor == MatriculaUsuario).ToList();
            }

            var model = new List<Solicitacao>();



            dados.ForEach(s => {
                if (s.PrazoAtendimento.HasValue)
                {
                    var filtroUmDia = 0;
                    var filtroDoisDias = 1;
                    var filtroTresDias = 2;

                    var dataAtual = DateTime.Now.Date;

                    var diaSemana = dataAtual.DayOfWeek;

                    switch (diaSemana)
                    {
                        case DayOfWeek.Thursday:
                            filtroTresDias += 2;
                            break;

                        case DayOfWeek.Friday:
                            filtroDoisDias += 2;
                            filtroTresDias += 2;
                            break;
                    }

                    if (periodo == 1)
                    {
                        if ((s.PrazoAtendimento.Value.Date - dataAtual).TotalDays == filtroUmDia)
                        {
                            model.Add(s);
                        }
                    }
                    else if (periodo == 2)
                    {
                        if ((s.PrazoAtendimento.Value.Date - dataAtual).TotalDays == filtroDoisDias)
                        {
                            model.Add(s);
                        }
                    }
                    else if (periodo == 3)
                    {
                        if ((s.PrazoAtendimento.Value.Date - dataAtual).TotalDays == filtroTresDias)
                        {
                            model.Add(s);
                        }
                    }
                }
            });

            return model;
        }

        private List<Solicitacao> ObterPorMotivo(string serie, string motivo, FiltrosPortabilidade filtros = null){
            var dados = new List<Solicitacao>();

            if (filtros.TemFiltro()) {
                dados = _solicitacaoService.Obter(filtros);
            }
            else {
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = dataAtual.AddDays(-1).Date;

                dados = _solicitacaoService.Obter(minDate, maxDate);
            }

            if (Cargo == NivelAcesso.Especialista.ToString()) {
                dados = dados.Where(s => s.MatriculaConsultor == MatriculaUsuario).ToList();
            }

            var substatus = _context.SubStatus.ToList();

            if (serie.ToLower().StartsWith("retido"))
            {
                dados = dados.Where(s => s.Motivo?.Descricao.ToUpper().Trim() == motivo.ToUpper().Trim() &&
                s.SubStatusId == substatus.FirstOrDefault(x => x.Descricao.StartsWith("Ret")).Id).ToList();
            }
            else if (serie.ToLower().StartsWith("não ret"))
            {
                dados = dados.Where(s => s.Motivo?.Descricao.ToUpper().Trim() == motivo.ToUpper().Trim() &&
                s.SubStatusId == substatus.FirstOrDefault(x => x.Descricao.StartsWith("Não Ret")).Id).ToList();
            }
            return dados;
        }

        private List<Solicitacao> ObterEquipe(string serie, string equipe, FiltrosPortabilidade filtros = null) {
            var dados = new List<Solicitacao>();
            if (filtros.TemFiltro()){
                dados = _solicitacaoService.Obter(filtros);
            }
            else {
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = dataAtual.AddDays(-1).Date;

                dados = _solicitacaoService.Obter(minDate, maxDate);
            }

            if (Cargo == NivelAcesso.Especialista.ToString())
            {
                dados = dados.Where(s => s.MatriculaConsultor == MatriculaUsuario).ToList();
            }

            var equipesDict = _context.Usuario.GroupBy(g => g.Matricula)
                .ToDictionary(k => k.Key, v => v.FirstOrDefault().Equipe);

            dados = dados.Where(s => equipesDict.ContainsKey(s.MatriculaConsultor) && equipesDict[s.MatriculaConsultor] == equipe).ToList();

            if (serie.ToLower().StartsWith("retido")) {

                dados = dados.Where(s => s.SubStatusId == 1).ToList();

            }
            else if (serie.ToLower().StartsWith("não")) {
                dados = dados.Where(s => s.SubStatusId == 2).ToList();
            }
            return dados;
        }

        #endregion

        #region ExpertarExcel
        public ActionResult ExpertarExcel(TipoExportacaoExcelGrafico tipoPesquisa, 
           ParamentrosExportacaoExcelGraficos parametro, 
            FiltrosPortabilidade filtros = null)
        {
            var dados = new List<Solicitacao>();

            var nomePlanilha = string.Empty;

            var excel = default(byte[]);

            switch (tipoPesquisa)
            {
                case TipoExportacaoExcelGrafico.Equipe:

                    dados = ObterEquipe(parametro.serie, parametro.equipe, filtros);

                    nomePlanilha = "Dados por diretoria regional";

                    break;
                case TipoExportacaoExcelGrafico.Status:
                    dados = ObterPorStatus(parametro.status, filtros);

                    nomePlanilha = "Dados por Status";
                    break;
                case TipoExportacaoExcelGrafico.Motivo:

                    dados = ObterPorMotivo(parametro.serie, parametro.motivo,filtros);
                    nomePlanilha = "Dados por Motivo";

                    break;
                case TipoExportacaoExcelGrafico.OperacaoAVencer:

                    dados = ObterAVencer(int.Parse(parametro.periodo), filtros);
                    nomePlanilha = "Dados por Operacões a vencer";

                    break;
            }

            var model = dados.ConvertAll(s => SolicitacaoViewModel.Mapear(s));

            excel = _util.GerarExcelPortabilidade(model, nomePlanilha);

            return File(excel, System.Net.Mime.MediaTypeNames.Application.Octet, $"Exportação {nomePlanilha}.xlsx");
        }
        #endregion
    }
}