using BradescoPGP.Common;
using BradescoPGP.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public class QualitativoController : AbstractController
    {
        private readonly Repositorio.PGPEntities _context;

        public QualitativoController(DbContext context) : base(context)
        {
            _context = context as Repositorio.PGPEntities;
        }

        // GET: Qualitativo
        public ActionResult Index(FiltrosTelas filtros = null)
        {
            var matricula = MatriculaUsuario;
            var qualitativos = new List<QualitativoViewModel>();

            if (!HasFilter(filtros))
            {
                if (User.IsInRole(NivelAcesso.Especialista.ToString()))
                {
                    var resultado = _context.Qualitativo.Where(q => q.MatriculaConsultor == matricula).ToList();
                    qualitativos = QualitativoViewModel.Mapear(resultado);

                }
                else if (User.IsInRole(NivelAcesso.Master.ToString()))
                {
                    var resultado = _context.Qualitativo.ToList();
                    qualitativos = QualitativoViewModel.Mapear(resultado);
                }
                else
                {
                    var resultado = _context.Qualitativo.Join(_context.Usuario,
                        qua => qua.MatriculaConsultor,
                        usu => usu.Matricula,
                        (qua, usu) => new { qua, usu }
                        ).Where(result => result.usu.MatriculaSupervisor == matricula).Select(s => s.qua).ToList();

                    qualitativos = resultado.ConvertAll(q => QualitativoViewModel.Mapear(q));
                }
            }
            else
            {
                var lista = _context.Qualitativo.Join(_context.Usuario,
                    qua => qua.MatriculaConsultor,
                    usu => usu.Matricula,
                    (qua, usu) => new { qua, usu.Equipe }
                    ).Where(result => result.Equipe == filtros.Equipe).Select(s => s.qua).ToList();

                qualitativos = lista.ConvertAll(q => QualitativoViewModel.Mapear(q));
            }

            var equipes = _context.Usuario.Select(u => u.Equipe).Distinct().ToList();

            var dataUltimaImportacao = _context.WindowsServiceConfig.FirstOrDefault(c => c.Tarefa == Comando.ImportarQualitativo.ToString())?.UltimaExecucao;

            ViewBag.UltimaImportacao = dataUltimaImportacao;
            
            ViewBag.Titulo = "Qualitativo";

            return View(qualitativos);
        }

        private bool HasFilter(FiltrosTelas filtros)
        {
            return !String.IsNullOrEmpty(filtros.Equipe);
        }
    }
}