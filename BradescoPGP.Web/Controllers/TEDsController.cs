using BradescoPGP.Common;
using BradescoPGP.Common.Logging;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using BradescoPGP.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BradescoPGP.Web.Models.Graficos;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public class TEDsController : AbstractController
    {
        private readonly PGPEntities _context;

        public TEDsController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
        }

        public ActionResult Index(FiltrosTelas filtros = null)
        {
            try
            {
                ViewBag.filtroAtual = filtros;

                var linksCapInv = GetLinksCapInvest();

                ViewBag.LinksCap = linksCapInv.FirstOrDefault(l => l.Titulo == "CapLiq")?.Url;

                ViewBag.LinksInvest = linksCapInv.FirstOrDefault(l => l.Titulo == "Invest")?.Url;

                

                ViewBag.Especialistas = Services.SelectListItemGenerator.EspecialistasTeds();

                var situacoes = _context.Status.Where(s => s.Evento.StartsWith("TED")).ToList().ConvertAll(s => new SelectListItem { Text = s.Descricao, Value = s.Id.ToString() });

                situacoes.Insert(0, new SelectListItem { Text = "Selecione um Status", Selected = true, Value = "" });

                ViewBag.Situacao = situacoes;

                ViewBag.Titulo = "TEDs";

                var dataUltimaImportacao = _context.WindowsServiceConfig.FirstOrDefault(c => c.Tarefa == Comando.ImportarTEDs.ToString())?.UltimaExecucao;

                ViewBag.UltimaImportacao = dataUltimaImportacao;

                //dados = faixaValor.Equipe.ToUpper() != "PGP" && !User.IsInRole(NivelAcesso.Master.ToString()) ? dados.Where(t => t.Valor >= faixaValor.De && t.Valor <= faixaValor.Ate).ToList(): dados;

                //viewModel = dados.ConvertAll(t => TEDViewModel.Mapear(t));

                return View();
            }
            catch (Exception ex)
            {
                Log.Error("Error ao carregar TEDs.", ex);

                return View("Error", new ErrorViewModel { Mensagem = "Não foi possível carregar as TEDs.", Status = "Erro" });
            }
        }

        [HttpPost]
        public ActionResult CarregarDados(FiltrosTelas filtros)
        {
            var draw = Request.Form["draw"];

            var start = Request.Form["start"];

            var length = Request.Form["length"];

            var order = Request.Form["order[0][column]"];

            var sortColumnDirection = Request.Form["order[0][dir]"];

            var searchValue = Request.Form["search[value]"];

            int pageSize = int.TryParse(length, out int size) ? size : 0;

            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = 0;

            var teds = ObterTeds(filtros).AsQueryable();
            
            //Ordenar
            if (!(string.IsNullOrEmpty(order) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                switch(order)
                {

                    case "0":

                        teds = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? 
                            teds.OrderByDescending(p => p.MatriculaConsultor) : teds.OrderBy(p => p.MatriculaConsultor);

                        break;

                    case "1":

                        teds = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? 
                            teds.OrderByDescending(p => p.Agencia) : teds.OrderBy(p => p.Agencia);

                        break;

                    case "2":

                        teds = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? 
                            teds.OrderByDescending(p => p.Conta) : teds.OrderBy(p => p.Conta);

                        break;

                    case "3":

                        teds = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? 
                            teds.OrderByDescending(p => p.NomeCliente) : teds.OrderBy(p => p.NomeCliente);

                        break;

                    case "4":
                        teds = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            teds.OrderByDescending(p => p.Data) : teds.OrderBy(p => p.Data);
                        break;

                    case "5":
                        teds = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            teds.OrderByDescending(p => p.Valor) : teds.OrderBy(p => p.Valor);
                        break;
                    case "6":
                        teds = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            teds.OrderByDescending(p => p.StatusId) : teds.OrderBy(p => p.StatusId);
                        break;
                    case "7":
                        teds = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            teds.OrderByDescending(p => p.MotivoId) : teds.OrderBy(p => p.MotivoId);
                        break;
                    case "8":
                        teds = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            teds.OrderByDescending(p => p.ValorAplicado) : teds.OrderBy(p => p.ValorAplicado);
                        break;
                }
            }

            recordsTotal = teds.Count();

            var result = teds.Skip(skip).Take(pageSize).ToList();

            var data = result.ConvertAll(t => TEDViewModel.Mapear(t));

            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal , data });

        }

        private List<TED> ObterTeds(FiltrosTelas filtros)
        {
            var teds = _context.TED.Include(t => t.Motivo).Include(t => t.Status);
            var tedsUsuario = default(List<TedUsuarioModel>);
            var dados = default(List<TED>);
            var query = default(IQueryable<TED>);

            //var faixaValor = _context.TEDFaixaEquipe.First(f => f.Equipe.ToLower() == EquipeUsuario.ToLower());
            var dataAtual = DateTime.Now.Date;
            var minDate = dataAtual.AddDays(-1);
            var maxDataTed = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));

            if (HasFilter(filtros))
            {
                //Teds com filtros
                query = new FiltroService(_context).FiltrarTeds(filtros, MatriculaUsuario, Cargo);

                //Relaciona Teds com encarteiramento
                dados = query.Join(_context.Encarteiramento, t => new { agencia = t.Agencia, conta = t.Conta },
                    e => new { agencia = e.Agencia, conta = e.Conta },
                    (ted, e) => new { ted }).Select(s => s.ted).ToList();

                dados.RemoveAll(t => string.IsNullOrEmpty(t.MatriculaConsultor));

                var especialistas = dados.Join(_context.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u })
                   .Select(j => new { j.u.Nome, j.u.Matricula }).Distinct().ToList();

                dados.ForEach(ted =>
                {
                    ted.NomeConsultor = especialistas.FirstOrDefault(e => e.Matricula == ted.MatriculaConsultor)?.Nome;
                });
            }
            else
            {
                //Teds sem filtro
                #region Obtem TEDs por perfil
                if (User.IsInRole(NivelAcesso.Especialista.ToString()))
                {
                    if (filtros == null || FiltrosTelas.SemFiltros(filtros))
                    {
                        query = teds.Where(t => t.MatriculaConsultor == MatriculaUsuario);
                    }
                    else
                    {
                        query = teds.Where(t => t.Area.ToUpper().Contains("PGP"));
                    }

                    dados = query.Join(_context.Encarteiramento, t => new { agencia = t.Agencia, conta = t.Conta },
                           e => new { agencia = e.Agencia, conta = e.Conta },
                           (ted, e) => new { ted })
                           .Select(s => s.ted)
                           .Where(t => t.Data >= minDate && t.Data <= maxDataTed).ToList();

                    var especialistas = teds
                            .Join(_context.Usuario.Include(u => u.Perfil),
                            r => r.MatriculaConsultor,
                            u => u.Matricula,
                            (r, Usuario) => new { Usuario })

                            .Where(j => j.Usuario.Perfil.Descricao == NivelAcesso.Especialista.ToString())
                            .OrderBy(u => u.Usuario.Nome)
                            .Select(j => new { j.Usuario.Nome, j.Usuario.Matricula }).Distinct().ToList();

                    dados.ForEach(ted =>
                    {
                        if (!string.IsNullOrEmpty(ted.MatriculaConsultor))
                            ted.NomeConsultor = especialistas.FirstOrDefault(u => u.Matricula == ted.MatriculaConsultor)?.Nome;
                    });

                }
                else if (User.IsInRole(NivelAcesso.Gestor.ToString()))
                {
                    tedsUsuario = teds
                    .Join(_context.Encarteiramento, t => new { agencia = t.Agencia, conta = t.Conta },
                       e => new { agencia = e.Agencia, conta = e.Conta },
                       (ted, e) => new { ted })
                       .Select(s => s.ted)
                    .Join(_context.Usuario.Include(u => u.Perfil),
                        t => new { matricula = t.MatriculaConsultor },
                        u => new { matricula = u.Matricula },
                         (t, u) => new TedUsuarioModel { Usuario = u, TED = t })
                         .Where(j =>
                            j.Usuario.Equipe == EquipeUsuario &&
                            j.TED.Data >= minDate && j.TED.Data <= maxDataTed)
                        .OrderBy(u => u.Usuario.Nome).ToList();

                    var especialistas = tedsUsuario.Select(j => new { j.Usuario.Nome, j.Usuario.Matricula }).Distinct().ToList();

                    dados = tedsUsuario.Select(j => j.TED).ToList();

                    dados.ForEach(ted =>
                    {
                        if (!string.IsNullOrEmpty(ted.MatriculaConsultor))
                            ted.NomeConsultor = especialistas.FirstOrDefault(u => u.Matricula == ted.MatriculaConsultor).Nome;
                    });
                }
                else
                {

                    tedsUsuario = teds
                        .Join(_context.Encarteiramento, t => new { agencia = t.Agencia, conta = t.Conta },
                           e => new { agencia = e.Agencia, conta = e.Conta },
                           (ted, e) => new { ted })
                           .Select(s => s.ted)
                        .Join(_context.Usuario.Include(u => u.Perfil),
                            t => new { matricula = t.MatriculaConsultor },
                            u => new { matricula = u.Matricula },
                            (t, u) => new TedUsuarioModel { Usuario = u, TED = t })
                        .Where(j => j.TED.Area.Contains("PGP") &&
                            j.TED.Data >= minDate && j.TED.Data <= maxDataTed)
                        .OrderBy(u => u.Usuario.Nome).ToList();

                    var especialistas = tedsUsuario.Select(j => j.Usuario).Distinct().ToList();

                    dados = tedsUsuario.Select(j => j.TED).ToList();

                    dados.ForEach(ted =>
                    {
                        ted.NomeConsultor = tedsUsuario.FirstOrDefault(u => u.TED.Agencia == ted.Agencia && u.TED.Conta == ted.Conta).Usuario.Nome;
                    });

                }
                #endregion
            }

            return dados;

        }

        public ActionResult ExportarExcel(FiltrosTelas filtros)
        {
            var excel = default(byte[]);
            var teds = _context.TED.Include(t => t.Motivo).Include(t => t.Status).OrderByDescending(t => t.Data);
            var tedsUsuario = default(List<TedUsuarioModel>);
            var dados = default(List<TED>);
            var query = default(IQueryable<TED>);
            var viewModel = default(List<TEDViewModel>);
            var dataAtual = DateTime.Now.Date;
            var minDate = dataAtual.AddDays(-15);
            var maxDataTed = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));

            if (HasFilter(filtros))
            {
                //Teds com filtros
                query = new FiltroService(_context).FiltrarTeds(filtros, MatriculaUsuario, Cargo);

                //Relaciona Teds com encarteiramento
                dados = query.Join(_context.Encarteiramento, t => new { agencia = t.Agencia, conta = t.Conta },
                    e => new { agencia = e.Agencia, conta = e.Conta },
                    (ted, e) => new { ted }).Select(s => s.ted).ToList();

                dados.RemoveAll(t => string.IsNullOrEmpty(t.MatriculaConsultor));

                var especialistas = dados.Join(_context.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u })
                   .Select(j => new { j.u.Nome, j.u.Matricula }).Distinct().ToList();

                dados.ForEach(ted =>
                {
                    ted.NomeConsultor = especialistas.FirstOrDefault(e => e.Matricula == ted.MatriculaConsultor)?.Nome;
                });

                #region Filtros TED
                //    var agenciaConta = !string.IsNullOrWhiteSpace(filtros.Agencia) && !string.IsNullOrWhiteSpace(filtros.Conta);

                //    var deAte = filtros.De.HasValue && filtros.Ate.HasValue;

                //    var especialista = !string.IsNullOrWhiteSpace(filtros.Especialista);

                //    var agencia = !String.IsNullOrEmpty(filtros.Agencia);

                //    var situacao = filtros.Situacao.HasValue;

                //    var equipe = !String.IsNullOrEmpty(filtros.Equipe);

                //    var equipeSituacao = !String.IsNullOrEmpty(filtros.Equipe) && filtros.Situacao.HasValue;

                //    var tedsFiltradas = new List<TED>();

                //    if (especialista && deAte)
                //    {
                //        dados = teds.Where(t => t.MatriculaConsultor == filtros.Especialista && t.Data >= filtros.De && t.Data <= filtros.Ate).ToList();
                //    }
                //    else if(especialista)
                //    {
                //        dados = teds.Where(t => t.MatriculaConsultor == filtros.Especialista).ToList();

                //    }
                //    else if (agenciaConta)
                //    {
                //        dados = teds.Where(t => t.Agencia == filtros.Agencia && t.Conta == filtros.Conta).ToList();
                //    }
                //    else if (agencia)
                //    {
                //        dados = teds.Where(t => t.Agencia == filtros.Agencia).ToList();
                //    }
                //    else if(deAte)
                //    {
                //        dados = teds.Where(t => t.Data >= filtros.De && t.Data <= filtros.Ate).ToList();
                //    }
                //    else
                //    {
                //        if (User.IsInRole(NivelAcesso.Especialista.ToString()))
                //        {
                //            dados = teds.Where(t => t.MatriculaConsultor == MatriculaUsuario).ToList();
                //        }
                //        else if (User.IsInRole(NivelAcesso.Gestor.ToString()))
                //        {
                //            dados = teds.Join(_context.Usuario.Include(u => u.Perfil),
                //                t => new { matricula = t.MatriculaConsultor },
                //                u => new { matricula = u.Matricula },
                //                 (t, u) => new { Usuario = u, TED = t }).Where(j => j.Usuario.MatriculaSupervisor == MatriculaUsuario && j.TED.Data.Year == DateTime.Now.Year && (j.TED.Data.Month == DateTime.Now.Month - 1 || j.TED.Data.Month == DateTime.Now.Month))
                //                .OrderBy(u => u.Usuario.Nome).Select(s => s.TED).ToList();
                //        }
                //        else
                //        {
                //            dados =  teds.Join(_context.Usuario.Include(u => u.Perfil),
                //                t => new { matricula = t.MatriculaConsultor },
                //                u => new { matricula = u.Matricula },
                //                 (t, u) => new { u, t })
                //                 .Where(j => j.t.Area == "PGP" &&
                //                 j.t.Data >= minDate && j.t.Data <= dataAtual)
                //                .OrderBy(u => u.u.Nome).Select(s => s.t).ToList();
                //        }

                //    }

                //    foreach (var ted in dados)
                //    {
                //        if (agenciaConta && !MatchAgenciaConta(ted, filtros.Agencia, filtros.Conta))
                //        {
                //            continue;
                //        }
                //        if (deAte && !MatchPorDeAte(ted, filtros.De.Value, filtros.Ate.Value))
                //        {
                //            continue;
                //        }
                //        if (especialista && !MatchPorEspecialista(ted, filtros.Especialista))
                //        {
                //            continue;
                //        }
                //        if (situacao && !MatchPorSituacao(ted, filtros.Situacao.Value))
                //        {
                //            continue;
                //        }

                //        if (agencia && !MatchAgencia(ted, filtros.Agencia.ToString()))
                //            continue;

                //        tedsFiltradas.Add(ted);
                //    }
                //    dados = tedsFiltradas;

                #endregion
            }
            else
            {
                //Teds sem filtro
                #region Obtem TEDs por perfil
                if (User.IsInRole(NivelAcesso.Especialista.ToString()))
                {
                    if (filtros == null || FiltrosTelas.SemFiltros(filtros))
                    {
                        query = teds.Where(t => t.MatriculaConsultor == MatriculaUsuario);
                    }
                    else
                    {
                        query = teds.Where(t => t.Area.ToUpper().Contains("PGP"));
                    }

                    dados = query.Join(_context.Encarteiramento, t => new { agencia = t.Agencia, conta = t.Conta },
                           e => new { agencia = e.Agencia, conta = e.Conta },
                           (ted, e) => new { ted })
                           .Select(s => s.ted)
                           .Where(t => t.Data >= minDate && t.Data <= maxDataTed).ToList();

                    var especialistas = teds
                            .Join(_context.Usuario.Include(u => u.Perfil),
                            r => r.MatriculaConsultor,
                            u => u.Matricula,
                            (r, Usuario) => new { Usuario })

                            .Where(j => j.Usuario.Perfil.Descricao == NivelAcesso.Especialista.ToString())
                            .OrderBy(u => u.Usuario.Nome)
                            .Select(j => new { j.Usuario.Nome, j.Usuario.Matricula }).Distinct().ToList();

                    dados.ForEach(ted =>
                    {
                        if (!string.IsNullOrEmpty(ted.MatriculaConsultor))
                            ted.NomeConsultor = especialistas.FirstOrDefault(u => u.Matricula == ted.MatriculaConsultor).Nome;
                    });

                    var selectList = new List<SelectListItem>();

                    especialistas.ForEach(e =>
                    {
                        selectList.Add(new SelectListItem { Text = e.Nome, Value = e.Matricula, Selected = e.Matricula == MatriculaUsuario });
                    });

                    ViewBag.Especialistas = selectList;

                }
                else if (User.IsInRole(NivelAcesso.Gestor.ToString()))
                {
                    //if (DateTime.Now.Day <= 10)
                    //{
                    tedsUsuario = teds
                    .Join(_context.Encarteiramento, t => new { agencia = t.Agencia, conta = t.Conta },
                       e => new { agencia = e.Agencia, conta = e.Conta },
                       (ted, e) => new { ted })
                       .Select(s => s.ted)
                    .Join(_context.Usuario.Include(u => u.Perfil),
                        t => new { matricula = t.MatriculaConsultor },
                        u => new { matricula = u.Matricula },
                         (t, u) => new TedUsuarioModel { Usuario = u, TED = t })
                         .Where(j =>
                            j.Usuario.Equipe == EquipeUsuario &&
                            j.TED.Data.Year == DateTime.Now.Year &&
                            j.TED.Data >= minDate && j.TED.Data <= maxDataTed)
                        .OrderBy(u => u.Usuario.Nome).ToList();
                    //}
                    //else
                    //{
                    //    tedsUsuario = teds.Join(_context.Usuario.Include(u => u.Perfil),
                    //       t => new { matricula = t.MatriculaConsultor },
                    //       u => new { matricula = u.Matricula },
                    //        (t, u) => new TedUsuarioModel { Usuario = u, TED = t }).Where(j => j.Usuario.Equipe == EquipeUsuario && j.TED.Data.Year == DateTime.Now.Year && j.TED.Data.Month == DateTime.Now.Month)
                    //       .OrderBy(u => u.Usuario.Nome).ToList();
                    //}

                    var especialistas = tedsUsuario.Select(j => new { j.Usuario.Nome, j.Usuario.Matricula }).Distinct().ToList();

                    dados = tedsUsuario.Select(j => j.TED).ToList();

                    dados.ForEach(ted =>
                    {
                        if (!string.IsNullOrEmpty(ted.MatriculaConsultor))
                            ted.NomeConsultor = especialistas.FirstOrDefault(u => u.Matricula == ted.MatriculaConsultor).Nome;
                    });

                    var selectList = new List<SelectListItem>();

                    especialistas.ForEach(e =>
                    {
                        selectList.Add(new SelectListItem { Text = e.Nome, Value = e.Matricula });
                    });

                    ViewBag.Especialistas = selectList;
                }
                else
                {
                    //if (DateTime.Now.Day <= 10)
                    //{
                    tedsUsuario = teds.Join(_context.Usuario.Include(u => u.Perfil),
                        t => new { matricula = t.MatriculaConsultor },
                        u => new { matricula = u.Matricula },
                         (t, u) => new TedUsuarioModel { Usuario = u, TED = t })
                         .Where(j => j.TED.Area.Contains("PGP") &&
                         j.TED.Data >= minDate && j.TED.Data <= maxDataTed)
                        .OrderBy(u => u.Usuario.Nome).ToList();
                    //}
                    //else
                    //{
                    //    tedsUsuario = teds
                    //        .Join(_context.Encarteiramento,
                    //        ted => new { agencia = ted.Agencia, conta = ted.Conta },
                    //        enc => new { agencia = enc.Agencia, conta = enc.Conta },
                    //        (ted, enc) => new { ted, enc.Matricula })

                    //        .Join(_context.Usuario.Include(u => u.Perfil),
                    //        t => t.Matricula,
                    //        u => u.Matricula,
                    //        (t, u) => new TedUsuarioModel { Usuario = u, TED = t.ted })

                    //        .Where(j => j.TED.Area.ToUpper() == "PGP" &&
                    //        j.TED.Data.Year == DateTime.Now.Year && j.TED.Data.Month == DateTime.Now.Month)
                    //       .OrderBy(u => u.Usuario.Nome).ToList();
                    //}

                    var especialistas = tedsUsuario.Select(j => j.Usuario).Distinct().ToList();

                    dados = tedsUsuario.Select(j => j.TED).ToList();

                    dados.ForEach(ted =>
                    {
                        ted.NomeConsultor = tedsUsuario.FirstOrDefault(u => u.TED.Agencia == ted.Agencia && u.TED.Conta == ted.Conta).Usuario.Nome;
                    });

                    //var selectListEspecialista = new List<SelectListItem>();

                    //var especialistasCombo = _context.Usuario.Where(u => u.PerfilId == 3).Select(s => new { s.Nome, s.Matricula }).Distinct().ToList();

                    //especialistasCombo.ForEach(e =>
                    //{
                    //    selectListEspecialista.Add(new SelectListItem { Text = e.Nome, Value = e.Matricula });
                    //});

                }
                #endregion
            }

            var usuarios = _context.Usuario.Distinct().ToDictionary(k => k.Matricula, v => v.Equipe);

            viewModel = dados.ConvertAll(t => TEDViewModel.Mapear(t));

            viewModel.ForEach(t =>
            {
                t.Equipe = usuarios[t.MatriculaConsultor];
            });

            excel = GerarExcel(viewModel, "TEDs");

            return File(excel, MediaTypeNames.Application.Octet, "TEDs_Export_PGPWEB.xlsx");

        }

        [HttpPost]
        public ActionResult AtualizarTED(TEDViewModel model, string RedirectUrl)
        {
            if (model.Id == default(int))
            {
                return View("Error", new ErrorViewModel { Mensagem = "Não foi possível atualizar esta TED.", Status = "Erro" });
            }

            try
            {
                var dbTed = _context.TED.Include(t => t.TedsContatos).FirstOrDefault(t => t.Id == model.Id);

                if (dbTed is null)
                {
                    return new HttpStatusCodeResult(404);
                }

                dbTed.ValorAplicado = model.ValorAplicado;
                dbTed.StatusId = model.StatusId;
                dbTed.MotivoTedId = model.MotivoTedId;

                if (dbTed.TedsContatos == null)
                {
                    var contatos = new TedsContatos
                    {
                        ContatouCliente = model.ContatouCliente,
                        ContatouGerente = model.ContatouGerente,
                        GerenteSolicitouNaoAtuacao = model.GerenteSolicitouNaoAtuacao,
                        EspecialistaAtuou = model.EspecialistaAtuou,
                        GerenteInvestimentoAtuou = model.GerenteInvestimentoAtuou,
                        ClienteLocalizado = model.ClienteLocalizado,
                        ClienteAceitaConsultoria = model.ClienteAceitaConsultoria,
                        IdTed = model.Id
                    };

                    dbTed.TedsContatos = contatos;
                }
                else
                {
                    dbTed.TedsContatos.ContatouCliente = model.ContatouCliente;
                    dbTed.TedsContatos.ContatouGerente = model.ContatouGerente;
                    dbTed.TedsContatos.GerenteSolicitouNaoAtuacao = model.GerenteSolicitouNaoAtuacao;
                    dbTed.TedsContatos.EspecialistaAtuou = model.EspecialistaAtuou;
                    dbTed.TedsContatos.GerenteInvestimentoAtuou = model.GerenteInvestimentoAtuou;
                    dbTed.TedsContatos.ClienteLocalizado = model.ClienteLocalizado;
                    dbTed.TedsContatos.ClienteAceitaConsultoria = model.ClienteAceitaConsultoria;
                    dbTed.TedsContatos.IdTed = model.Id;
                }

                dbTed.Observacao = model.Observacao;
                dbTed.OutrasInstId = model.OutrasInstId;

                _context.SaveChanges();

                if (RedirectUrl != null)
                    return Redirect(RedirectUrl);
                
                return RedirectToAction("RedirecionarHome", "Home", new { agencia = dbTed.Agencia, conta = dbTed.Conta, carrgarPopUp = false });
            }
            catch (Exception ex)
            {
                Log.Error("Erro ao atualizar TED.", ex);

                return View("Error", new ErrorViewModel { Status = "Erro", Mensagem = "Não foi possível atualizar esta TED." });
            }
        }

        private bool MatchAgenciaConta(TED ted, string agencia, string conta)
        {
            return ted.Agencia == agencia && ted.Conta == conta;
        }

        private bool MatchAgencia(TED ted, string agencia)
        {
            return ted.Agencia == agencia;
        }

        private bool MatchPorDeAte(TED ted, DateTime de, DateTime ate)
        {
            return de <= ted.Data.Date && ted.Data.Date <= ate;
        }

        private bool MatchPorEspecialista(TED ted, string matricula)
        {
            return ted.MatriculaConsultor == matricula;
        }

        private bool MatchPorSituacao(TED ted, int situacaoId)
        {
            return ted.StatusId == situacaoId;
        }

        public string ExcluirAplicacao(int id)
        {
            var aplic = _context.TedsAplicacoes.FirstOrDefault(s => s.Id == id);

            if (aplic == null)
            {
                return JsonConvert.SerializeObject(new { mensagem = "Não foi possível excluir esta aplicação", status = false });
            }

            try
            {
                _context.TedsAplicacoes.Remove(aplic);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error("Erro ao excluir aplicação ted ", ex);

                return JsonConvert.SerializeObject(new { mensagem = "Não foi possível excluir esta aplicação", status = false });
            }

            return JsonConvert.SerializeObject(new { mensagem = "Aplicação excluida com sucesso!!", status = true, valorExcluido = aplic.ValorAplicado });


        }

        public string ObterTED(int id)
        {
            var ted = TEDViewModel.Mapear(_context.TED.Include("Motivo")
                    .Include(t => t.TedsContatos).Include(t => t.TedsAplicacoes).Include(s => s.TedsMotivos).Include("Status").FirstOrDefault(t => t.Id == id));

            var motivos = _context.Motivo.Where(t => t.Evento == "TED").ToList();

            ted.Motivos = motivos.ConvertAll(m => OptionsParaTela.Mapear(m));

            ted.TedsMotivos = _context.TedsMotivos.ToList().ConvertAll(m => OptionsParaTela.Mapear(m));

            ted.TedsMotivosOutrasInst = _context.TedsMotivoOutrasInst.ToList().ConvertAll(m => OptionsParaTela.Mapear(m));

            ted.Situacoes = _context.Status.Where(t => t.Evento == "TEDNovo").ToList().ConvertAll(s => OptionsParaTela.Mapear(s));

            ted.Produtos = _context.TedsProdutos.ToList().ConvertAll(p => OptionsParaTela.Mapear(p)).OrderBy(o => o.Descricao).ToList();

            return JsonConvert.SerializeObject(ted, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [HttpPost]
        public JsonResult ResetarStatus(int id)
        {
            try
            {
                var ted = _context.TED.First(t => t.Id == id);

                var statusInicialTed = _context.Status.FirstOrDefault(s => s.Evento == Eventos.TED.ToString() && s.Descricao == "Em Branco");
                
                ted.StatusId = statusInicialTed?.Id ?? 11;
                ted.MotivoId = null;
                ted.MotivoTedId = null;
                ted.ValorAplicado = null;
                ted.OutrasInstId = null;

                _context.TedsContatos.Remove(ted.TedsContatos);

                _context.TedsAplicacoes.RemoveRange(ted.TedsAplicacoes);
                    
                _context.Entry(ted).State = EntityState.Modified;


                _context.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { status = false, evento = "TED" });
            }

            return Json(new { status = true, evento = "TED" });

        }
        public string VerificarNovosTEDs()
        {
            var novasTEDs = new List<TED>();

            string[] statusNaoValidos = new string[] { "aplicado", "em negociação", "não aplicado" };

            var dataAtual = DateTime.Now.Date;

            if (User.IsInRole(NivelAcesso.Especialista.ToString()))
            {
                novasTEDs = _context.TED
                    .Include(t => t.Motivo)
                    .Include(t => t.Status).Where(t => t.MatriculaConsultor == MatriculaUsuario && t.Status.Evento == Eventos.TED.ToString() && DbFunctions.TruncateTime(t.Data) == dataAtual &&
                    !statusNaoValidos.Contains(t.Status.Descricao.ToLower()) && !t.Notificado)
                    .ToList();
            }
            //else if (User.IsInRole(NivelAcesso.Gestor.ToString()))
            //{
            //    novasTEDs = _context.TED
            //        .Include(t => t.Motivo)
            //        .Include(t => t.Status)
            //        .Join(_context.Usuario.Include(u => u.Perfil),
            //            t => new { matricula = t.MatriculaConsultor },
            //            u => new { matricula = u.Matricula },
            //            (t, u) => new { Usuario = u, TED = t })
            //        .Where(t => t.Usuario.Equipe == EquipeUsuario && t.TED.Status.Evento == Eventos.TED.ToString() && DbFunctions.TruncateTime(t.TED.Data) == DbFunctions.TruncateTime(DateTime.Now) && !statusNaoValidos.Contains(t.TED.Status.Descricao.ToLower()) && !t.TED.Notificado)
            //        .Select(j => j.TED).ToList();
            //}
            //else
            //{
            //    novasTEDs = _context.TED
            //       .Include(t => t.Motivo)
            //       .Include(t => t.Status).Where(t =>
            //            t.Area.ToUpper() == "PGP" &&
            //            t.Status.Evento == Eventos.TED.ToString() &&
            //            DbFunctions.TruncateTime(t.Data) == DbFunctions.TruncateTime(DateTime.Now) &&
            //            !statusNaoValidos.Contains(t.Status.Descricao.ToLower()) &&
            //            !t.Notificado)
            //       .ToList();
            //}

            if (novasTEDs.Any())
            {

                foreach (var ted in novasTEDs)
                {
                    ted.Notificado = true;
                }

                _context.SaveChanges();

                var viewModel = novasTEDs.ConvertAll(t => new TEDViewModel { Agencia = t.Agencia, Conta = t.Conta, NomeCliente = t.NomeCliente, Valor = t.Valor });

                return JsonConvert.SerializeObject(viewModel);
            }

            //var model = _context.TED.OrderByDescending(t => t.Data).Where(t => !string.IsNullOrEmpty(t.NomeCliente)).Take(5).ToList().ConvertAll(t => new TEDViewModel { Agencia = t.Agencia, Conta = t.Conta, NomeCliente = t.NomeCliente, Valor = t.Valor });
            //return JsonConvert.SerializeObject(model);

            return "{}";
        }

        public string AdicionarProduto(TedsAplicacoes aplicacao)
        {
            if (aplicacao == null)
                return "Os dados não podem ser nulos";

            if (aplicacao.ValorAplicado == 0)
                return JsonConvert.SerializeObject(new { mensagem = "Valor aplicado não pode ser 0", status = false });


            var id = 0;

            try
            {
                _context.TedsAplicacoes.Add(aplicacao);
                _context.SaveChanges();
                id = _context.TedsAplicacoes.ToList().Last().Id;
            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new { mensagem = "Erro ao adicionar esta aplicação", status = false });
            }

            return JsonConvert.SerializeObject(new { mensagem = "Aplicação adicionado com sucesso!!", status = true, Id = id });

        }

        private bool HasFilter(FiltrosTelas filtros)
        {
            return !String.IsNullOrEmpty(filtros.Especialista) || !String.IsNullOrEmpty(filtros.Equipe) || !String.IsNullOrEmpty(filtros.Agencia) || !String.IsNullOrEmpty(filtros.Conta)
                || filtros.Situacao.HasValue || filtros.De != null || filtros.Ate != null;
        }

        public ActionResult Graficos(FiltrosTelas filtros)
        {
            var dados = ObterTeds(filtros);

            #region Calculos

            //QuantidadeTEDs
            var quantidadeTed = new List<decimal>();

            var statusVAlidos = _context.Status.Where(s => s.Evento == Eventos.TEDNovo.ToString()).Select(s => s.Id).ToList();

            quantidadeTed.Add(dados.Where(s => statusVAlidos.Contains(s.StatusId)).Count()); //Total

            for (int i = 0; i < statusVAlidos.Count; i++)
            {
                quantidadeTed.Add(dados.Where(ted => ted.StatusId == statusVAlidos[i]).Count());

            }

            var graficoQuantidadeTed = new GraficoQuantidadeTed
            {
                Dados = new SerieGraficoBar
                {
                    data = quantidadeTed,
                    name = "Quantidade"
                }
            };

            //Valor Teds
            var valorTed = new List<decimal>();
            valorTed.Add(dados.Where(s => statusVAlidos.Contains(s.StatusId)).Sum(t => t.Valor)); //Total
            valorTed.Add(dados.Where(ted => ted.StatusId == 16).Sum(t => t.Valor));
            valorTed.Add(dados.Where(ted => ted.StatusId == 17).Sum(t => t.Valor));
            valorTed.Add(dados.Where(ted => ted.StatusId == 18).Sum(t => t.Valor));

            var graficoValorTed = new GraficoValorTed
            {
                Dados = new SerieGraficoBar { name = "Valor", data = valorTed }
            };


            //Quantidade Status
            var quantidadeStatus = new List<decimal>();

            var idStatusFinalizado = _context.Status.FirstOrDefault(s => s.Evento == Eventos.TEDNovo.ToString() && s.Descricao.StartsWith("Finalizado")).Id;

            quantidadeStatus.Add(dados.Where(t => t.StatusId == idStatusFinalizado).Count());

            quantidadeStatus.Add(dados.Where(t => t.StatusId == idStatusFinalizado && t.TedsAplicacoes.Any()).Count());

            quantidadeStatus.Add(dados.Where(t => 
                t.StatusId == idStatusFinalizado && !t.TedsAplicacoes.Any()).Count());
            
            var graficaQuantidadeStatus = new GraficoQuantidadeStatus
            {
                Dados = new SerieGraficoBar
                {
                    data = quantidadeStatus,
                    name = "Quantidade"
                }
            };


            //Valor status
            var valorStatus = new List<decimal>();

            var valorTotalAplicado = dados.Where(t => 
                t.StatusId == idStatusFinalizado && t.TedsAplicacoes.Any())
                .Sum(s => s.TedsAplicacoes.Sum(a => a.ValorAplicado));

            var valorTotalTed = dados.Where(t => t.StatusId == idStatusFinalizado).Sum(s => s.Valor);

            var valorTotalNaoAplicado = valorTotalTed - valorTotalAplicado;

            valorStatus.Add(valorTotalTed);

            valorStatus.Add(valorTotalAplicado);

            valorStatus.Add(valorTotalNaoAplicado);

            var graficaValorStatus = new GraficoValorStatus
            {
                Dados = new SerieGraficoBar
                {
                    data = valorStatus,
                    name = "Quantidade"
                }
            };


            //Aplicacao por produtos
            var produtos =  _context.TedsProdutos.ToDictionary(k => k.Produto, v => v.Id);

            var aplicacoes = new List<TedsAplicacoes>();

            var quantidadesProdutos = new List<decimal>();

            dados.ForEach(t =>
            {
                aplicacoes.AddRange(t.TedsAplicacoes.ToList());
            });

            var produtosObjeto = aplicacoes.Select(s => new { s.TedsProdutos.Produto, s.TedsProdutos.Id }).Distinct().ToList();

            var produtosAplicados = produtosObjeto.ToDictionary(s => s.Produto, k => k.Id);


            foreach (var produto in produtosAplicados.Values)
            {
                quantidadesProdutos.Add(aplicacoes.Where(p => p.IdProduto == produto).Count());
            }

            var labels = produtosAplicados.Keys.ToList();

            var graficoAplicacaoProduto = new GraficoAplicacaoProduto
            {
                Dados = new SerieGraficoPie { labels = labels, series = quantidadesProdutos }
            };


            //Motivos não aplicação

            var motivos = new HashSet<TedsMotivos>();

            dados.ForEach(t =>
            {
                if (t.TedsMotivos != null)
                    motivos.Add(t.TedsMotivos);
            });

            var quantidadeMotivos = new List<decimal>();

            motivos.ToList().ForEach(m =>
            {
                quantidadeMotivos.Add(dados.Where(t => t.MotivoTedId.HasValue && t.MotivoTedId == m.Id).Count());
            });

            labels = motivos.Count > 0 ? motivos.Select(s => s.Motivo).Distinct().ToList() : new List<string>();

            var graficoMotivoNaoAplicacao = new GraficoMotivoNaoAplicacao
            {
                Dados = new SerieGraficoPie
                {
                    labels = labels,
                    series = quantidadeMotivos
                }
            };

            var model = new TedGraficos
            {
                GraficoQuantidadeTed = graficoQuantidadeTed,
                GraficoValorTed = graficoValorTed,
                GraficoAplicacaoProduto = graficoAplicacaoProduto,
                GraficoMotivoNaoAplicacao = graficoMotivoNaoAplicacao,
                GraficoQuantidadeStatus = graficaQuantidadeStatus,
                GraficoValorStatus = graficaValorStatus

            };
            #endregion

            ViewBag.Especialistas = SelectListItemGenerator.Especialistas();

            ViewBag.Equipes = SelectListItemGenerator.Equipes();

            return View(model);
        }

        #region Filtros TED
        //    var agenciaConta = !string.IsNullOrWhiteSpace(filtros.Agencia) && !string.IsNullOrWhiteSpace(filtros.Conta);

        //    var deAte = filtros.De.HasValue && filtros.Ate.HasValue;

        //    var especialista = !string.IsNullOrWhiteSpace(filtros.Especialista);

        //    var agencia = !String.IsNullOrEmpty(filtros.Agencia);

        //    var situacao = filtros.Situacao.HasValue;

        //    var equipe = !String.IsNullOrEmpty(filtros.Equipe);

        //    var equipeSituacao = !String.IsNullOrEmpty(filtros.Equipe) && filtros.Situacao.HasValue;

        //    var tedsFiltradas = new List<TED>();

        //    if (especialista && deAte)
        //    {
        //        dados = teds.Where(t => t.MatriculaConsultor == filtros.Especialista && t.Data >= filtros.De && t.Data <= filtros.Ate).ToList();
        //    }
        //    else if(especialista)
        //    {
        //        dados = teds.Where(t => t.MatriculaConsultor == filtros.Especialista).ToList();

        //    }
        //    else if (agenciaConta)
        //    {
        //        dados = teds.Where(t => t.Agencia == filtros.Agencia && t.Conta == filtros.Conta).ToList();
        //    }
        //    else if (agencia)
        //    {
        //        dados = teds.Where(t => t.Agencia == filtros.Agencia).ToList();
        //    }
        //    else if(deAte)
        //    {
        //        dados = teds.Where(t => t.Data >= filtros.De && t.Data <= filtros.Ate).ToList();
        //    }
        //    else
        //    {
        //        if (User.IsInRole(NivelAcesso.Especialista.ToString()))
        //        {
        //            dados = teds.Where(t => t.MatriculaConsultor == MatriculaUsuario).ToList();
        //        }
        //        else if (User.IsInRole(NivelAcesso.Gestor.ToString()))
        //        {
        //            dados = teds.Join(_context.Usuario.Include(u => u.Perfil),
        //                t => new { matricula = t.MatriculaConsultor },
        //                u => new { matricula = u.Matricula },
        //                 (t, u) => new { Usuario = u, TED = t }).Where(j => j.Usuario.MatriculaSupervisor == MatriculaUsuario && j.TED.Data.Year == DateTime.Now.Year && (j.TED.Data.Month == DateTime.Now.Month - 1 || j.TED.Data.Month == DateTime.Now.Month))
        //                .OrderBy(u => u.Usuario.Nome).Select(s => s.TED).ToList();
        //        }
        //        else
        //        {
        //            dados =  teds.Join(_context.Usuario.Include(u => u.Perfil),
        //                t => new { matricula = t.MatriculaConsultor },
        //                u => new { matricula = u.Matricula },
        //                 (t, u) => new { u, t })
        //                 .Where(j => j.t.Area == "PGP" &&
        //                 j.t.Data >= minDate && j.t.Data <= dataAtual)
        //                .OrderBy(u => u.u.Nome).Select(s => s.t).ToList();
        //        }

        //    }

        //    foreach (var ted in dados)
        //    {
        //        if (agenciaConta && !MatchAgenciaConta(ted, filtros.Agencia, filtros.Conta))
        //        {
        //            continue;
        //        }
        //        if (deAte && !MatchPorDeAte(ted, filtros.De.Value, filtros.Ate.Value))
        //        {
        //            continue;
        //        }
        //        if (especialista && !MatchPorEspecialista(ted, filtros.Especialista))
        //        {
        //            continue;
        //        }
        //        if (situacao && !MatchPorSituacao(ted, filtros.Situacao.Value))
        //        {
        //            continue;
        //        }

        //        if (agencia && !MatchAgencia(ted, filtros.Agencia.ToString()))
        //            continue;

        //        tedsFiltradas.Add(ted);
        //    }
        //    dados = tedsFiltradas;

        #endregion
    }
}