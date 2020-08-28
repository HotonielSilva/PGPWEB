using BradescoPGP.Common;
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
    public class PipelineController : AbstractController
    {
        private readonly PGPEntities _context;
        private readonly FiltroService filtroService;
        private AjaxResponses ajaxResponses = new AjaxResponses();
        
        public PipelineController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
            filtroService = new FiltroService(_context);
        }

        // GET: Pipeline
        public ActionResult Index(FiltrosTelas filtros = null)
        {
            var pipelines = default(List<PipelineViewModel>);
            var pipes = default(List<Pipeline>);

            if (HasFilterPipeline(filtros))
            {
                pipelines = filtroService.FiltraPipeline(filtros, MatriculaUsuario, Cargo);
            }
            else
            {
                var minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-12);

                if (User.IsInRole("Especialista"))
                {
                    pipes = _context.Pipeline.Where(p => p.MatriculaConsultor == MatriculaUsuario &&
                    (p.DataProrrogada.HasValue ? p.DataProrrogada >= minDate : false || p.DataPrevista >= minDate)).ToList();
                    pipelines = pipes.ConvertAll(p => PipelineViewModel.Mapear(p));
                }
                else if (User.IsInRole("Gestor"))
                {
                    pipes = _context.Pipeline.Join(_context.Usuario,
                        pipe => pipe.MatriculaConsultor,
                        usu => usu.Matricula,
                        (pipe, usu) => new { pipe, usu }
                        ).Where(result => result.usu.MatriculaSupervisor == MatriculaUsuario &&
                        (result.pipe.DataPrevista >= minDate && result.pipe.DataProrrogada != null || result.pipe.DataPrevista >= minDate)).ToList()
                        .Select(r => r.pipe).ToList();

                    pipelines = pipes.ConvertAll(p => PipelineViewModel.Mapear(p));
                }
                else
                {
                    var result = _context.Pipeline
                        .Where(p => p.DataPrevista >= minDate && p.DataProrrogada != null || p.DataPrevista >= minDate).ToList();
                    
                    pipelines = result.ConvertAll(p => PipelineViewModel.Mapear(p));
                }
            }

            ViewBag.Especialistas = SelectListItemGenerator.Especialistas();

            ViewBag.Status = SelectListItemGenerator.Status(EventosStatusMotivosOrigens.Pipeline.ToString());

            ViewBag.Motivos = SelectListItemGenerator.Motivos(EventosStatusMotivosOrigens.Pipeline.ToString())/* selectList["Motivos"]*/;

            ViewBag.Origens = SelectListItemGenerator.Origens(EventosStatusMotivosOrigens.Pipeline.ToString()) /*selectList["Origens"]*/;

            ViewBag.Titulo = "Pipelines";

            return View(pipelines);
        }

        public ActionResult ExportarExcel(FiltrosTelas filtros = null)
        {
            var pipelines = default(List<PipelineViewModel>);
            var pipes = default(List<Pipeline>);

            var excel = default(byte[]);
            var usuarios = _context.Usuario.ToDictionary(k => k.Matricula, v => v.Equipe);

            if (HasFilterPipeline(filtros))
            {
                pipelines = filtroService.FiltraPipeline(filtros, MatriculaUsuario, Cargo);
            }
            else
            {
                var minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-12);
                var maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                if (User.IsInRole("Especialista"))
                {
                    pipes = _context.Pipeline.Where(p => p.MatriculaConsultor == MatriculaUsuario &&
                    (p.DataProrrogada.HasValue ? p.DataProrrogada >= minDate : false || p.DataPrevista >= minDate)).ToList();
                    pipelines = pipes.ConvertAll(p => PipelineViewModel.Mapear(p));
                }
                else if (User.IsInRole("Gestor"))
                {
                    pipes = _context.Pipeline.Join(_context.Usuario,
                        pipe => pipe.MatriculaConsultor,
                        usu => usu.Matricula,
                        (pipe, usu) => new { pipe, usu }
                        ).Where(result => result.usu.MatriculaSupervisor == MatriculaUsuario &&
                        (result.pipe.DataPrevista >= minDate && result.pipe.DataProrrogada != null || result.pipe.DataPrevista >= minDate)).ToList()
                        .Select(r => r.pipe).ToList();

                    pipelines = pipes.ConvertAll(p => PipelineViewModel.Mapear(p));
                }
                else
                {
                    var result = _context.Pipeline
                        .Where(p => p.DataPrevista >= minDate && p.DataProrrogada != null || p.DataPrevista >= minDate).ToList();
                    
                    pipelines = result.ConvertAll(p => PipelineViewModel.Mapear(p));
                }
            }

            pipelines.ForEach(p =>
            {
                p.Equipe = usuarios[p.Matricula];
            });

            excel = GerarExcel(pipelines, "Pipelines");

            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pipelines_Export_PGPWEB.xlsx");
        }
        
        [HttpPost]
        public ActionResult AtualizarPipeline(Pipeline pipe, string urlRedirect)
        {
            var pipelineViewModel = new List<PipelineViewModel>();

            var pipes = ajaxResponses.AtualizarPipeline(pipe);

            if (pipes.Count > 0)
            {
                pipelineViewModel = pipes.ConvertAll(p => PipelineViewModel.Mapear(p));
            }

            return Redirect(urlRedirect);

            //return JsonConvert.SerializeObject(pipelineViewModel, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public string ObterPipe(int idPipe)
        {
            var pipeline = _context.Pipeline.Include(p => p.Status).Include(p => p.Origem).Include(p => p.Motivo).FirstOrDefault(p => p.Id == idPipe);
            var pipeViewMoel = PipelineViewModel.Mapear(pipeline);

            var origens = _context.Origem.Where(o => o.Evento == "Pipeline").ToList();
            var selectListItens = new List<SelectListItem>();

            origens.ForEach(o =>
            {
                if (o.Id != pipeViewMoel.OrigemId)
                {
                    selectListItens.Add(new SelectListItem { Text = o.Descricao, Value = o.Id.ToString() });
                }
                else
                {
                    selectListItens.Add(new SelectListItem { Text = o.Descricao, Value = o.Id.ToString(), Selected = true });
                }
            });


            ViewBag.Origens = selectListItens;

            return JsonConvert.SerializeObject(pipeViewMoel, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public String NovoPipeline(Pipeline pipeline)
        {
            if (pipeline == null)
            {
                return string.Empty;
            }

            if (ajaxResponses.NovoPipeline(pipeline, out List<Pipeline> pipes))
            {
                var pipeViewModel = pipes.ConvertAll(p => PipelineViewModel.Mapear(p));
                
                return JsonConvert.SerializeObject(new { status = true, dados = pipeViewModel }, 
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }

            return JsonConvert.SerializeObject(new { status = false });

        }

        [HttpPost]
        public JsonResult ResetarStatus(int id)
        {
            try
            {
                var pipeline = _context.Pipeline.First(t => t.Id == id);

                pipeline.StatusId = 4;
                pipeline.MotivoId = null;
                pipeline.ValorAplicado = null;
                pipeline.DataProrrogada = null;

                _context.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { status = false, evento = "Pipeline" });
            }

            return Json(new { status = true, evento = "Pipeline" });

        }

        private bool HasFilterPipeline(FiltrosTelas filtros)
        {
            return !String.IsNullOrEmpty(filtros.Especialista) || !String.IsNullOrEmpty(filtros.Agencia) ||
                !String.IsNullOrEmpty(filtros.Conta) || !String.IsNullOrEmpty(filtros.Situacao.ToString()) ||
                filtros.De != null || filtros.Ate != null || !String.IsNullOrEmpty(filtros.Equipe);
        }
    }
}