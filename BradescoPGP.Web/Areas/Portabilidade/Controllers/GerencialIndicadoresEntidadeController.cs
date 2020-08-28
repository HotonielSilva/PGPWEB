using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using BradescoPGP.Web.Areas.Portabilidade.Models;
using BradescoPGP.Web.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BradescoPGP.Web.Areas.Portabilidade.Controllers
{
    public class GerencialIndicadoresEntidadeController : AbstractController
    {
        private ISolicitacaoService _solicitacaoService;
        private readonly PGPEntities _context;
        public GerencialIndicadoresEntidadeController(DbContext context, ISolicitacaoService solicitacaoService) : base(context)
        {
            _context = context as PGPEntities;
            _solicitacaoService = solicitacaoService;
        }
        // GET: Portabilidade/GerencialIndicadoresEntidade
        public ActionResult Index(FiltrosPortabilidade filtros = null)
        {
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

            if (Cargo == NivelAcesso.Gestor.ToString())
            {
                model = model.Join(_context.Usuario, s => s.MatriculaConsultor, u => u.Matricula,
                    (d, u) => new { d, u.MatriculaSupervisor })
                    .Where(s => s.MatriculaSupervisor == MatriculaUsuario)
                    .Select(s => s.d).ToList();
            }

            var entidades = model.Select(s => s.NomeEntidade).Distinct().ToList();

            var quantidade = new List<int>();

            var valoresEntidade = new Dictionary<string, decimal>();

            entidades.ForEach(e =>
            {
                quantidade.Add(model.Where(s => s.NomeEntidade == e).Count());

                if (!valoresEntidade.ContainsKey(e))
                {
                    valoresEntidade[e.Length > 30 ? e.Substring(0, 27) + "..." : e] = model.Where(s => s.NomeEntidade == e).Sum(s => s.ValorPrevistoSaida);
                }
            });

            ViewBag.TooltipValores = valoresEntidade;

            var labels = new List<string>();
            entidades.ForEach(l =>
            {
                if (l.Length > 30)
                    labels.Add(l.Substring(0, 27) + "...");
                else
                    labels.Add(l);
            });

            ViewBag.Entidades = JsonConvert.SerializeObject(labels);

            ViewBag.Quantidades = JsonConvert.SerializeObject(quantidade);

            ViewBag.QuantidadeTotalRetidos = model.Count();
            ViewBag.ValorTotalNaoRetidos = model.Sum(s => s.ValorPrevistoSaida).ToString("N2", new CultureInfo("pt-br"));

            ViewBag.Titulo = "Entidade Solicitante";
            return View();
        }
       
        public string ObterSolicitcaoEntidade(string entidade, FiltrosPortabilidade filtros)
        {
            var solicitacao = default(List<Solicitacao>);

            if (filtros.TemFiltro())
            {
                solicitacao = _solicitacaoService.ObterSolicitacaoEntidade(entidade, filtros);
            }
            else
            {
                var dataAtual = DateTime.Now;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = dataAtual.Date;
                filtros.De = minDate;
                filtros.Ate = maxDate;
                solicitacao = _solicitacaoService.ObterSolicitacaoEntidade(entidade, filtros);
            }
            var listEntidades = new List<GraficoEntidadeVW>();

            var totalValorSolicitado = solicitacao.Sum(s => s.ValorPrevistoSaida);

            solicitacao.ForEach(s =>
            {
                var entidades = new GraficoEntidadeVW();
                entidades.NomeEntidade = s.NomeEntidade;
                entidades.ValorSolicitado = s.ValorPrevistoSaida;
                entidades.Fundo = _solicitacaoService.ObterFundo(Regex.Replace(s.CIDTFDCNPJCessionaria, @"\D", ""));
                entidades.Quantidade = solicitacao.Count();
                //var totalValorSolicitado = solicitacao.Sum(s => s.ValorPrevistoSaida);
                entidades.PorcentagemValor = s.ValorPrevistoSaida > 0 ? (double)(entidades.ValorSolicitado / totalValorSolicitado * 100) : 0;

                listEntidades.Add(entidades);
            });

            var listEntidadesResumo = new List<GraficoEntidadeVW>();

            var entidadesPorFundos = listEntidades.GroupBy(s => s.Fundo).ToList();

            entidadesPorFundos.ForEach(e =>
            {
                var entidadeResumo = new GraficoEntidadeVW();

                entidadeResumo.Fundo = e.FirstOrDefault().Fundo;
                entidadeResumo.ValorSolicitado = e.Sum(s => s.ValorSolicitado);
                entidadeResumo.PorcentagemValor = (double)(entidadeResumo.ValorSolicitado / totalValorSolicitado * 100);

                listEntidadesResumo.Add(entidadeResumo);
            });

            return JsonConvert.SerializeObject(listEntidadesResumo.OrderByDescending(o => o.PorcentagemValor));
        }
        private bool TemFiltro(FiltrosPortabilidade filtros)
        {
            return filtros.De.HasValue || filtros.Ate.HasValue;
        }

    }
}