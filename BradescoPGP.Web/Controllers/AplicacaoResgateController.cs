using BradescoPGP.Common;
using BradescoPGP.Common.Logging;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using BradescoPGP.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public class AplicacaoResgateController : AbstractController
    {
        private readonly PGPEntities _context;
        public AplicacaoResgateController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
        }

        // GET: AplicacaoResgate
        public ActionResult Index(FiltrosTelas filtros = null)
        {
            var dataUltimaImportacao = _context.WindowsServiceConfig
                .FirstOrDefault(c => c.Tarefa == Comando.ImportarAplicacaoResgate.ToString())?.UltimaExecucao;

            ViewBag.UltimaImportacao = dataUltimaImportacao;

            ViewBag.Especialistas = SelectListItemGenerator.Especialistas();

            ViewBag.AplicacaoResgateCount = 0;

            ViewBag.FiltroAtual = filtros;

            ViewBag.Titulo = "Aplicação - Resgate";

            return View();
        }

        [HttpPost]
        public ActionResult CarregarDados(FiltrosTelas filtros = null)
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

            var filtroService = new FiltroService(_context);

            var aplicacaoResgates = filtroService.FiltrarAplicacaoResgate(filtros, MatriculaUsuario, Cargo).AsQueryable();

            //Order
            if (!(string.IsNullOrEmpty(order) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                switch (order)
                {
                    case "0":

                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.Especialista) : aplicacaoResgates.OrderBy(p => p.Especialista);

                        break;

                    case "1":

                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.agencia) : aplicacaoResgates.OrderBy(p => p.agencia);

                        break;

                    case "2":

                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.conta) : aplicacaoResgates.OrderBy(p => p.conta);

                        break;

                    case "3":

                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.data) : aplicacaoResgates.OrderBy(p => p.data);

                        break;

                    case "4":
                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.hora) : aplicacaoResgates.OrderBy(p => p.hora);
                        break;

                    case "5":
                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.operacao) : aplicacaoResgates.OrderBy(p => p.operacao);
                        break;
                    case "6":
                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.perif) : aplicacaoResgates.OrderBy(p => p.perif);
                        break;
                    case "7":
                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.produto) : aplicacaoResgates.OrderBy(p => p.produto);
                        break;
                    case "8":
                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.terminal) : aplicacaoResgates.OrderBy(p => p.terminal);
                        break;

                    case "9":
                        aplicacaoResgates = sortColumnDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            aplicacaoResgates.OrderByDescending(p => p.valor) : aplicacaoResgates.OrderBy(p => p.valor);
                        break;
                }
            }

            recordsTotal = aplicacaoResgates.Count();

            var data = aplicacaoResgates.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }

        public JsonResult NotificacaoAplicacaoResgate()
        {
            var result = _context.AplicacaoResgate.Where(ar => ar.MatriculaConsultor == MatriculaUsuario && !ar.Notificado).ToList();

            result.ForEach(a =>
            {
                a.Notificado = true;
            });

            _context.SaveChanges();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult ExportarExcel(FiltrosTelas filtros = null)
        {

            var minData = DateTime.Now.Date.AddDays(-90);
            var maxData = DateTime.Now.Date;
            var excel = default(byte[]);
            var result = default(List<AplicacaoResgateViewModel>);

            var filtroService = new FiltroService(_context);

            result = filtroService.FiltrarAplicacaoResgate(filtros, MatriculaUsuario, Cargo);

            excel = GerarExcel(result, $"AplicacaoResgate_{filtros.De?.ToString("yyyyMM")}");

            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"AplicacaoResgate_{filtros.De?.ToString("yyyyMM")}.xlsx");

        }

        [HttpPost]
        public ActionResult AtualizarAplic(ContatoAplicacaoResgateModel model, FiltrosTelas filtros)
        {
            if (model.IdAplicResgate == default(int))
            {
                return View("Error", new ErrorViewModel { Mensagem = "Não foi possível atualizar este registro.", Status = "Erro" });
            }
            try
            {
                var dbAplic = _context.AplicacaoResgate.Include(a => a.AplicResgateContatos).FirstOrDefault(t => t.Id == model.IdAplicResgate);

                if (dbAplic is null)
                {
                    return new HttpStatusCodeResult(404);
                }


                if (dbAplic.AplicResgateContatos == null)
                {
                    var contatos = new AplicResgateContatos
                    {
                        ContatouCliente = model.ContatouCliente,
                        VaiAnalisarOferta = model.VaiAnalisarOferta,
                        AplicouEmOutroBanco = model.AplicouEmOutroBanco,
                        Realocou = model.Realocou,
                        ProblemasDeRelacionamento = model.ProblemasDeRelacionamento,
                        PagamentosUsoDoRecurso = model.PagamentosUsoDoRecurso,
                        IdAplicResgate = model.IdAplicResgate
                    };

                    dbAplic.AplicResgateContatos = contatos;
                }
                else
                {
                    dbAplic.AplicResgateContatos.ContatouCliente = model.ContatouCliente;
                    dbAplic.AplicResgateContatos.PagamentosUsoDoRecurso = model.PagamentosUsoDoRecurso;
                    dbAplic.AplicResgateContatos.ProblemasDeRelacionamento = model.ProblemasDeRelacionamento;
                    dbAplic.AplicResgateContatos.Realocou = model.Realocou;
                    dbAplic.AplicResgateContatos.VaiAnalisarOferta = model.VaiAnalisarOferta;
                    dbAplic.AplicResgateContatos.AplicouEmOutroBanco = model.AplicouEmOutroBanco;
                    dbAplic.AplicResgateContatos.IdAplicResgate = model.IdAplicResgate;

                }

                _context.SaveChanges();

                return RedirectToAction("Index", filtros);
            }
            catch (Exception ex)
            {
                Log.Error("Erro ao atualizar Aplicação Resgate.", ex);

                return View("Error", new ErrorViewModel { Status = "Erro", Mensagem = "Não foi possível atualizar este Registro." });
            }
        }

        public string ObterAplic(int IdAplicacaoResgate)
        {
            var aplic = _context.AplicacaoResgate.FirstOrDefault(s => s.Id == IdAplicacaoResgate);

            if (aplic == null)
            {
                return JsonConvert.SerializeObject(new { status = false, mensagem = "Não foi possível obter este registro" });
            }
            
            var viewModel = AplicacaoResgateViewModel.Mapear(aplic, string.Empty);

            return JsonConvert.SerializeObject(new { status = true, aplic = viewModel });
        }

        private bool HasFilter(FiltrosTelas filtros)
        {
            return !string.IsNullOrEmpty(filtros.Especialista) ||
                !string.IsNullOrEmpty(filtros.Agencia) ||
                !string.IsNullOrEmpty(filtros.Conta) ||
                !string.IsNullOrEmpty(filtros.Equipe) ||
                filtros.De.HasValue || filtros.Ate.HasValue;
        }
    }
}