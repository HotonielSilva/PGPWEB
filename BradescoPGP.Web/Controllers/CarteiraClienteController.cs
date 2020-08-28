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
    public class CarteiraClienteController : AbstractController
    {
        private readonly PGPEntities _context;

        public CarteiraClienteController(DbContext context): base(context)
        {
            _context = context as PGPEntities;
        }

        // GET: CarteiraCliente
        public ActionResult Index(FiltrosTelas filtros = null, int? page = null, int? pageSize = null)
        {
            var matricula = MatriculaUsuario;

            var perfil = User.IsInRole(NivelAcesso.Gestor.ToString()) ? NivelAcesso.Gestor : User.IsInRole(NivelAcesso.Master.ToString()) ? NivelAcesso.Master : NivelAcesso.Especialista;

            var carteiraService = new CarteiraClienteService();

            var query = carteiraService.ObterClusterizacoes(_context, matricula, perfil);

            if (HasFilter(filtros))
            {
                var modoFiltro = FiltroService.SetaModoFiltro(filtros);

                switch (modoFiltro)
                {
                    case ModoDeFiltro.EspecialistaAgenciaConta:
                        query = query.Where(c => c.Agencia == filtros.Agencia && c.Conta == filtros.Conta && c.Especialista == filtros.Especialista);

                        break;
                    case ModoDeFiltro.ApenasEspecialista:
                        if (perfil == NivelAcesso.Especialista)
                            query = carteiraService.ObterClusterizacoes(_context, matricula, NivelAcesso.Master);
                        
                        query = query.Where(c => c.Especialista == filtros.Especialista);

                        break;

                    case ModoDeFiltro.AgenciaConta:
                        query = query.Where(c => c.Conta == filtros.Conta && c.Agencia == filtros.Agencia);
                        break;

                    case ModoDeFiltro.Situacao:
                        break;

                    case ModoDeFiltro.EspecialistaAgencia:
                        if (perfil == NivelAcesso.Especialista)
                            query = carteiraService.ObterClusterizacoes(_context, matricula, NivelAcesso.Master);

                        query = query.Where(c => c.Especialista == filtros.Especialista && c.Agencia == filtros.Agencia);

                        break;

                    case ModoDeFiltro.EspecialistaAgenciaContaGerente:
                        if (perfil == NivelAcesso.Especialista)
                            query = carteiraService.ObterClusterizacoes(_context, matricula, NivelAcesso.Master);
                        
                        query = query.Where(c => c.Conta == filtros.Conta && c.Agencia == filtros.Agencia &&
                        c.Especialista == filtros.Especialista && c.NomeGerente == filtros.Gerente);
                        
                        break;

                    case ModoDeFiltro.EspecialistaGerente:
                        if (perfil == NivelAcesso.Especialista)
                            query = carteiraService.ObterClusterizacoes(_context, matricula, NivelAcesso.Master);

                        query = query.Where(c => c.Especialista == filtros.Especialista && c.NomeGerente == filtros.Gerente);

                        break;

                    case ModoDeFiltro.AgenciaContaGerente:
                        query = query.Where(c => c.Conta == filtros.Conta && c.Agencia == filtros.Agencia && c.NomeGerente == filtros.Gerente);
                        break;

                    case ModoDeFiltro.Gerente:
                        query = query.Where(c => c.NomeGerente == filtros.Gerente);
                        break;

                    case ModoDeFiltro.Agencia:
                        query = query.Where(c => c.Agencia == filtros.Agencia);
                        break;
                    case ModoDeFiltro.Equipe:
                        query = query.Where(c => c.Equipe == filtros.Equipe);
                        break;
                    //case ModoDeFiltro.EquipeSituacao:
                    //    query = query.Where(c => c.Equipe == c.Equipe &)
                    //    break;

                    default:
                        if (User.IsInRole(NivelAcesso.Especialista.ToString()))
                            query = query.Where(c => c.Matricula == matricula);
                        
                        break;
                }
            }

            //var linksCapInv = GetLinksCapInvest();
            //ViewBag.LinksCap = linksCapInv.FirstOrDefault(l => l.Titulo == "CapLiq").Url;
            //ViewBag.LinksInvest = linksCapInv.FirstOrDefault(l => l.Titulo == "Invest").Url;

            ViewBag.FiltroAtual = filtros;

            //var dicionarioEspecialsitaGerente = SelectListItemGenerator();

            ViewBag.Especialistas = SelectListItemGenerator.Especialistas();
            ViewBag.Gerentes = SelectListItemGenerator.Getentes();

            var dataUltimaImportacao = _context.WindowsServiceConfig.FirstOrDefault(c => c.Tarefa == Comando.ImportarClusHieEnc.ToString())?.UltimaExecucao;
            
            ViewBag.UltimaImportacao = dataUltimaImportacao;

            ViewBag.Titulo = "Clusterização";

            ViewBag.Count = query.Count();

            return View(query.OrderBy(c => c.Especialista).ToPagedList(page ?? 1, pageSize ?? 25));
        }

        public ActionResult ExportarExcel(FiltrosTelas filtros = null)
        {
            var matricula = MatriculaUsuario;
            var excel = default(byte[]);
            var carteira = default(List<CarteiraClienteExportExcel>);

            var perfil = User.IsInRole(NivelAcesso.Gestor.ToString()) ? NivelAcesso.Gestor : User.IsInRole(NivelAcesso.Master.ToString()) ? NivelAcesso.Master : NivelAcesso.Especialista;

            var carteiraService = new CarteiraClienteService();

            var query = carteiraService.ObterClusterizacoesExportacao(_context, matricula, perfil);

            if (HasFilter(filtros))
            {
                var modoFiltro = FiltroService.SetaModoFiltro(filtros);

                switch (modoFiltro)
                {
                    case ModoDeFiltro.EspecialistaAgenciaConta:
                        query = query.Where(c => c.Agencia == filtros.Agencia && c.Conta == filtros.Conta && c.Especialista == filtros.Especialista);

                        break;
                    case ModoDeFiltro.ApenasEspecialista:
                        if (perfil == NivelAcesso.Especialista)
                            query = carteiraService.ObterClusterizacoesExportacao(_context, matricula, NivelAcesso.Master);

                        query = query.Where(c => c.Especialista == filtros.Especialista);

                        break;

                    case ModoDeFiltro.AgenciaConta:
                        query = query.Where(c => c.Conta == filtros.Conta && c.Agencia == filtros.Agencia);
                        break;

                    case ModoDeFiltro.Situacao:
                        break;

                    case ModoDeFiltro.EspecialistaAgencia:
                        if (perfil == NivelAcesso.Especialista)
                            query = carteiraService.ObterClusterizacoesExportacao(_context, matricula, NivelAcesso.Master);

                        query = query.Where(c => c.Especialista == filtros.Especialista && c.Agencia == filtros.Agencia);

                        break;

                    case ModoDeFiltro.EspecialistaAgenciaContaGerente:
                        if (perfil == NivelAcesso.Especialista)
                            query = carteiraService.ObterClusterizacoesExportacao(_context, matricula, NivelAcesso.Master);

                        query = query.Where(c => c.Conta == filtros.Conta && c.Agencia == filtros.Agencia &&
                        c.Especialista == filtros.Especialista && c.NomeGerente == filtros.Gerente);

                        break;

                    case ModoDeFiltro.EspecialistaGerente:
                        if (perfil == NivelAcesso.Especialista)
                            query = carteiraService.ObterClusterizacoesExportacao(_context, matricula, NivelAcesso.Master);

                        query = query.Where(c => c.Especialista == filtros.Especialista && c.NomeGerente == filtros.Gerente);

                        break;

                    case ModoDeFiltro.AgenciaContaGerente:
                        query = query.Where(c => c.Conta == filtros.Conta && c.Agencia == filtros.Agencia && c.NomeGerente == filtros.Gerente);
                        break;

                    case ModoDeFiltro.Gerente:
                        query = query.Where(c => c.NomeGerente == filtros.Gerente);
                        break;

                    case ModoDeFiltro.Agencia:
                        query = query.Where(c => c.Agencia == filtros.Agencia);
                        break;

                    default:
                        if (User.IsInRole(NivelAcesso.Especialista.ToString()))
                            query = query.Where(c => c.Matricula == matricula);

                        break;
                }

                carteira = query.OrderBy(o => o.Especialista).ToList();
                excel = GerarExcel(carteira, "Cluesterização");
            }
            else
            {
                carteira = query.OrderBy(o => o.Especialista).ToList();
                excel = GerarExcel(carteira, "Clusterizações");
            }

            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Clusterizações_Export_PGPWEB.xlsx");
        }

        private bool HasFilter(FiltrosTelas filtros)
        {
            return !String.IsNullOrEmpty(filtros.Especialista) || !String.IsNullOrEmpty(filtros.Agencia) ||
                !String.IsNullOrEmpty(filtros.Conta) || !String.IsNullOrEmpty(filtros.Equipe) ||
                filtros.De != null || filtros.Ate != null || !string.IsNullOrEmpty(filtros.Gerente);
        }
    }
}