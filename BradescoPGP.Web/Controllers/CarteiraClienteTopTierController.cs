using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using BradescoPGP.Web.Services;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public class CarteiraClienteTopTierController : AbstractController
    {
        private readonly PGPEntities _context;

        public CarteiraClienteTopTierController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
        }

        // GET: CarteiraCliente
        public ActionResult Index(FiltrosTelas filtros = null, int? page = null, int? pageSize = null)
        {
            var matricula = MatriculaUsuario;

            var perfil = User.IsInRole(NivelAcesso.Gestor.ToString()) ? NivelAcesso.Gestor : User.IsInRole(NivelAcesso.Master.ToString()) ? NivelAcesso.Master : NivelAcesso.Especialista;

            var carteiraService = new CarteiraClienteService();

            var query = carteiraService.ObterClusterizacoesTopTier(_context);

            var nome = User.Identity.Name;

            if (perfil == NivelAcesso.Especialista)
                query = query.Where(c => c.Especialista == nome);

            if (HasFilter(filtros))
            {
                if (!string.IsNullOrEmpty(filtros.Especialista))
                    query = query.Where(c => c.Especialista == filtros.Especialista);

                if (!string.IsNullOrEmpty(filtros.Agencia))
                    query = query.Where(c => c.Agencia == filtros.Agencia);

                if (!string.IsNullOrEmpty(filtros.Conta))
                    query = query.Where(c => c.Conta == filtros.Conta);

                if (!string.IsNullOrEmpty(filtros.Acao))
                    query = query.Where(c => c.ACAO == filtros.Acao);
            }

            ViewBag.FiltroAtual = filtros;
            ViewBag.Especialistas = SelectListItemGenerator.Especialistas();
            ViewBag.Acoes = SelectListItemGenerator.Acoes();

            var dataUltimaImportacao = _context.WindowsServiceConfig.FirstOrDefault(c => c.Tarefa == Comando.ImportarClusHieEnc.ToString())?.UltimaExecucao;

            ViewBag.UltimaImportacao = dataUltimaImportacao;

            ViewBag.Titulo = "Clusterização";
            ViewBag.Count = query.Count();
            return View(query.OrderByDescending(c => c.SALDO_TOTAL).ToPagedList(page ?? 1, pageSize ?? 25));
        }

        public ActionResult ExportarExcel(FiltrosTelas filtros = null)
        {
            var matricula = MatriculaUsuario;
            var excel = default(byte[]);
            var carteira = default(List<CarteiraClienteTopTierViewModel>);

            var perfil = User.IsInRole(NivelAcesso.Gestor.ToString()) ? NivelAcesso.Gestor : User.IsInRole(NivelAcesso.Master.ToString()) ? NivelAcesso.Master : NivelAcesso.Especialista;

            var carteiraService = new CarteiraClienteService();

            var query = carteiraService.ObterClusterizacoesTopTier(_context);

            var nome = User.Identity.Name;

            if (perfil == NivelAcesso.Especialista)
                query = query.Where(c => c.Especialista == nome);

            if (HasFilter(filtros))
            {
                if (!string.IsNullOrEmpty(filtros.Especialista))
                    query = query.Where(c => c.Especialista == filtros.Especialista);

                if (!string.IsNullOrEmpty(filtros.Agencia))
                    query = query.Where(c => c.Agencia == filtros.Agencia);

                if (!string.IsNullOrEmpty(filtros.Conta))
                    query = query.Where(c => c.Conta == filtros.Conta);

                if (!string.IsNullOrEmpty(filtros.Acao))
                    query = query.Where(c => c.ACAO == filtros.Acao);

                carteira = query.ToList();
                excel = GerarExcel(carteira, "Cluesterização");
            }
            else
            {
                carteira = query.ToList();
                excel = GerarExcel(carteira, "Clusterizações");
            }

            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Clusterizações_Export_PGPWEB.xlsx");
        }

        private bool HasFilter(FiltrosTelas filtros)
        {
            return !String.IsNullOrEmpty(filtros.Especialista) || !String.IsNullOrEmpty(filtros.Agencia) ||
                !String.IsNullOrEmpty(filtros.Conta) || !String.IsNullOrEmpty(filtros.Equipe) ||
                filtros.De != null || filtros.Ate != null || !string.IsNullOrEmpty(filtros.Gerente) || !string.IsNullOrEmpty(filtros.Acao);
        }
    }
}