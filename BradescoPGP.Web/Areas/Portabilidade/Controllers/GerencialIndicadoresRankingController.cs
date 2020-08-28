using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Enums;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using BradescoPGP.Web.Areas.Portabilidade.Models;
using BradescoPGP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BradescoPGP.Web.Areas.Portabilidade.Controllers
{
    public class GerencialIndicadoresRankingController : AbstractController
    {
        private readonly ISolicitacaoService _solicitacaoService;

        private readonly PGPEntities _context;

        private readonly IUsuarioService _usuario;

        private readonly IUtil _util;

        public GerencialIndicadoresRankingController(DbContext context, ISolicitacaoService solicitacaoService, IUtil util, IUsuarioService usuario) : base(context)
        {
            _solicitacaoService = solicitacaoService;
            _context = context as PGPEntities;
            _util = util;
            _usuario = usuario as IUsuarioService;

        }
        // GET: Portabilidade/GerencialIndicadoresRanking
        public ActionResult Index(FiltrosPortabilidade filtros = null)
        {
            var produtividade = _util.ObterProdutividade();

            var modelRankin = GerarRanking(filtros);

            var RetencaoAtual = produtividade
                .Where(s => s.De <= DateTime.Now.Date && s.Parametro == OpcoesProdutividade.MinimoRetencao.ToString()).FirstOrDefault()?.ValorPercerntualMinino ?? 0;

            var minimoContato = produtividade
                .Where(s => s.De <= DateTime.Now.Date && s.Parametro == OpcoesProdutividade.MinimoContato.ToString()).FirstOrDefault()?.ValorPercerntualMinino ?? 0;

            ViewBag.PercMinimoContato = minimoContato;

            ViewBag.PercMinimoRetencao = RetencaoAtual;

            ViewBag.Titulo = "Ranking de Especialistas";

            return View(modelRankin.OrderByDescending(s => s.PorcentgemRetida).ToList());
        }

        private List<RankingViewModel> GerarRanking(FiltrosPortabilidade filtros)
        {

            var dados = _usuario.ObterTodosUsuarios();

            //Traz apenas usuarios de um gestor
            if (Cargo == NivelAcesso.Gestor.ToString())
            {
                dados = dados.Where(s => s.MatriculaSupervisor == MatriculaUsuario).ToList();
            }

            dados = dados.Where(u => u.PerfilId == 3).ToList();

            var dataAtual = DateTime.Now;
            var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
            var maxDate = new DateTime(dataAtual.Year, dataAtual.Month, dataAtual.Day - 1);
            var modelRankin = new List<RankingViewModel>();

            dados.ForEach(m =>
            {
                var viewModel = new RankingViewModel();

                var solicitacao = new List<Solicitacao>();

                if (filtros.TemFiltro())
                {
                    solicitacao = _solicitacaoService.Obter(filtros.De.Value, filtros.Ate.Value, m.Matricula);
                }
                else
                {
                    solicitacao = _solicitacaoService.Obter(minDate, maxDate, m.Matricula);
                }

                //Remove não elegíveis
                
                var idNaoElegivel = _context.Motivo.FirstOrDefault(s => s.Descricao.Contains("Não Elegível"))?.Id;

                solicitacao = solicitacao.Where(s => s.MotivoId != idNaoElegivel).ToList();


                viewModel.Especialista = m.Nome;

                viewModel.QuantidadeSolicitacoes = solicitacao.Count();

                viewModel.ValorSolicitacoes = solicitacao.Sum(s => s.ValorPrevistoSaida);

                //Remove solicitacoes com motivo especifico para não ser incluido na contagem de valor de solicitações
                var idMotivoCliente = _context.Motivo.FirstOrDefault(s => s.Descricao.Contains("Não Conseguiu Falar com o Cliente"))?.Id;

                var solicitacoesNaoInclusas = solicitacao.Where(s =>
                    s.Motivo != null && s.MotivoId == idMotivoCliente)
                    .Sum(s => s.ValorPrevistoSaida);


                //Porcentagem de contatos
                var qtdContatado = solicitacao.Count(s => s.MotivoId.HasValue);

                viewModel.Contatos = qtdContatado;

                var qtdSolicitacaoes = viewModel.QuantidadeSolicitacoes == 0 ? 1 : viewModel.QuantidadeSolicitacoes;

                viewModel.PorcentagemContatos = qtdContatado != 0 ? Math.Round(Convert.ToDecimal(qtdContatado) / qtdSolicitacaoes * 100, 2) : 0;

                viewModel.QuantidadeRetida = solicitacao.Where(s => s.ValorRetido != null && s.SubStatusId == 1).Count();

                viewModel.ValorRetido = solicitacao.Where(s => s.ValorRetido != null && s.SubStatusId == 1).Sum(s => s.ValorRetido ?? 0);

                var totalSolicitacoes = viewModel.ValorSolicitacoes == 0 ? 1 : viewModel.ValorSolicitacoes;

                var total = (totalSolicitacoes - solicitacoesNaoInclusas);
                
                viewModel.PorcentgemRetida = total != 0  && viewModel.ValorRetido != 0 ? Math.Round(viewModel.ValorRetido / total * 100, 2) : 0;

                modelRankin.Add(viewModel);
            });

            return modelRankin;
        }
    }
}