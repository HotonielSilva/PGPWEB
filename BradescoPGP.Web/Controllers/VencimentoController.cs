using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using BradescoPGP.Web.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public class VencimentoController : AbstractController
    {
        private readonly PGPEntities _context;

        private FiltroService filtroService;

        public VencimentoController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
            filtroService = new FiltroService(_context);
        }

        // GET: Vencimento
        public ActionResult Index(FiltrosTelas filtros = null)
        {
            var vencimentosViewModel = new List<VencimentoViewModel>();

            if (HasFilter(filtros))
            {
                vencimentosViewModel = filtroService.FiltraVencimentos(filtros, MatriculaUsuario, Cargo);
            }
            else
            {
                var minDate = DateTime.Now.Date.AddDays(-5);
                var maxDate = DateTime.Now.Date.AddDays(10);

                if (User.IsInRole(NivelAcesso.Especialista.ToString()))
                {
                    var vencimentos = _context.Vencimento.Include(i => i.Status).Join(_context.Encarteiramento,
                        ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                        enc => new { agencia = enc.Agencia, conta = enc.Conta },
                        (ven, enc) => new { ven, enc.Matricula, enc.CONSULTOR })
                        .Where(res => res.Matricula == MatriculaUsuario && res.ven.Dt_Vecto_Contratado >= minDate && res.ven.Dt_Vecto_Contratado <= maxDate).ToList();

                    vencimentosViewModel = vencimentos.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.CONSULTOR));

                }
                else if (User.IsInRole(NivelAcesso.Gestor.ToString()))
                {
                    var vencimentos = _context.Vencimento.Join(_context.Encarteiramento,
                        ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                        enc => new { agencia = enc.Agencia, conta = enc.Conta },
                        (ven, enc) => new { ven, enc.Matricula })
                        .Join(_context.Usuario,
                        res => res.Matricula,
                        usu => usu.Matricula,
                        (res, usu) => new { res, usu.MatriculaSupervisor, usu.Nome }
                        )
                        .Where(result => result.MatriculaSupervisor == MatriculaUsuario && result.res.ven.Dt_Vecto_Contratado >= minDate && result.res.ven.Dt_Vecto_Contratado <= maxDate).ToList();

                    vencimentosViewModel = vencimentos.ConvertAll(v => VencimentoViewModel.Mapear(v.res.ven, v.Nome));
                }
                else
                {
                    var vencimentos = _context.Vencimento.Include(i => i.Status).Join(_context.Encarteiramento,
                    ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                    enc => new { agencia = enc.Agencia, conta = enc.Conta },
                    (ven, enc) => new { ven, enc.CONSULTOR })
                    .Where(result => result.ven.Dt_Vecto_Contratado >= minDate && result.ven.Dt_Vecto_Contratado <= maxDate).ToList();

                    vencimentosViewModel = vencimentos.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.CONSULTOR));
                }

            }
            
            ViewBag.Especialistas = SelectListItemGenerator.Especialistas();
            ViewBag.Status = SelectListItemGenerator.Status(EventosStatusMotivosOrigens.Vencimentos.ToString());

            var dataUltimaImportacao = _context.WindowsServiceConfig.FirstOrDefault(c => c.Tarefa == Comando.ImportarVencimentos.ToString())?.UltimaExecucao;
            ViewBag.UltimaImportacao = dataUltimaImportacao;

            ViewBag.Titulo = "Vencimentos";

            return View(vencimentosViewModel);
        }

        public ActionResult AtualizarVencimento(int id, int? StatusId)
        {
            var venc = _context.Vencimento.FirstOrDefault(v => v.Id == id);
            venc.StatusId = StatusId.Value;

            _context.Entry(venc).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = "Erro ao atualizar Vencimento " + e }, JsonRequestBehavior.AllowGet);
            }

            var vencimentos = _context.Vencimento.Where(v => v.Cod_Agencia == venc.Cod_Agencia && v.Cod_Conta_Corrente == venc.Cod_Conta_Corrente).Join(
                _context.Encarteiramento,
                v => new { agencia = v.Cod_Agencia.ToString(), conta = v.Cod_Conta_Corrente.ToString() },
                enc => new { agencia = enc.Agencia, conta = enc.Conta },
                (v, enc) => new { v, enc }
                ).Join(_context.Usuario,
                res => res.enc.Matricula,
                usu => usu.Matricula,
                (res, usu) => new { res.v, usu }
                ).ToList();

            var vencimentosViewModel = vencimentos.ConvertAll(ven => VencimentoViewModel.Mapear(ven.v, ven.usu.Nome));
            return Json(vencimentosViewModel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AtualizarVencimentoNaTela(int id, int? StatusId, string redirectUrl)
        {
            var venc = _context.Vencimento.FirstOrDefault(v => v.Id == id);

            venc.StatusId = StatusId.Value;

            _context.Entry(venc).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = "Erro ao atualizar Vencimento " + e }, JsonRequestBehavior.AllowGet);
            }

            if (redirectUrl != null)
            {
                return Redirect(redirectUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult ExportarExcel(FiltrosTelas filtros)
        {
            var vencimentosViewModel = new List<VencimentoViewModel>();
            var excel = default(byte[]);

            var usuarios = _context.Usuario.GroupBy(s => s.Matricula)
                .ToDictionary(k => k.FirstOrDefault().Matricula, v => v.FirstOrDefault().Equipe);

            if (HasFilter(filtros))
            {
                vencimentosViewModel = filtroService.FiltraVencimentos(filtros, MatriculaUsuario, Cargo);
            }

            else
            {
                var minDate = DateTime.Now.Date.AddDays(-5);
                var maxDate = DateTime.Now.Date.AddDays(10);

                if (User.IsInRole(NivelAcesso.Especialista.ToString()))
                {
                    var vencimentos = _context.Vencimento.Include(i => i.Status).Join(_context.Encarteiramento,
                        ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                        enc => new { agencia = enc.Agencia, conta = enc.Conta },
                        (ven, enc) => new { ven, enc.Matricula, enc.CONSULTOR })
                        .Where(res => res.Matricula == MatriculaUsuario && res.ven.Dt_Vecto_Contratado >= minDate && res.ven.Dt_Vecto_Contratado <= maxDate).ToList();

                    vencimentosViewModel = vencimentos.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.CONSULTOR, v.Matricula));

                }
                else if (User.IsInRole(NivelAcesso.Gestor.ToString()))
                {
                    var vencimentos = _context.Vencimento.Join(_context.Encarteiramento,
                        ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                        enc => new { agencia = enc.Agencia, conta = enc.Conta },
                        (ven, enc) => new { ven, enc.Matricula })
                        .Join(_context.Usuario,
                        res => res.Matricula,
                        usu => usu.Matricula,
                        (res, usu) => new { res, usu.MatriculaSupervisor, usu.Nome, usu.Matricula }
                        )
                        .Where(result => result.MatriculaSupervisor == MatriculaUsuario && result.res.ven.Dt_Vecto_Contratado >= minDate && result.res.ven.Dt_Vecto_Contratado <= maxDate).ToList();

                    vencimentosViewModel = vencimentos.ConvertAll(v => VencimentoViewModel.Mapear(v.res.ven, v.Nome, v.Matricula));
                }
                else
                {
                    var vencimentos = _context.Vencimento.Include(i => i.Status).Join(_context.Encarteiramento,
                    ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                    enc => new { agencia = enc.Agencia, conta = enc.Conta },
                    (ven, enc) => new { ven, enc.CONSULTOR, enc.Matricula })
                    .Where(result => result.ven.Dt_Vecto_Contratado >= minDate && result.ven.Dt_Vecto_Contratado <= maxDate).ToList();

                    vencimentosViewModel = vencimentos.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.CONSULTOR, v.Matricula));
                }
            }

            vencimentosViewModel.ForEach(v =>
            {
                v.Equipe = usuarios[v.Matriucla];
            });

            excel = GerarExcel(vencimentosViewModel, "Vencimentos");

            return File(excel, Application.Octet, "Vencimentos_Export_PGPWEB.xlsx");
        }

        [HttpPost]
        public JsonResult ResetarStatus(int id)
        {
            try
            {
                var vencimento = _context.Vencimento.First(t => t.Id == id);

                vencimento.StatusId = 11;

                _context.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { status = false, evento = "Vencimento" });
            }

            return Json(new { status = true, evento = "Vencimento" });

        }

        private bool HasFilter (FiltrosTelas filtros)
        {
            return !String.IsNullOrEmpty(filtros.Especialista) || !String.IsNullOrEmpty(filtros.Agencia) || !String.IsNullOrEmpty(filtros.Conta)
                || filtros.Situacao.HasValue || filtros.De != null || filtros.Ate != null || !String.IsNullOrEmpty(filtros.Equipe);
        }
    }
}