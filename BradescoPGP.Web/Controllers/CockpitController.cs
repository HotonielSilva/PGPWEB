using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using BradescoPGP.Web.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public class CockpitController : AbstractController
    {
        private readonly PGPEntities _context;
        private FiltroService filtroService;
        public CockpitController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
            filtroService = new FiltroService(_context);
        }

        public ViewResult Index(FiltrosTelas filtros = null)
        {
            var cockpits = default(List<Cockpit>);
            var viewModel = default(List<CockpitViewModel>);
            var minData = DateTime.Now.Date.AddDays(-90);
            var maxData = DateTime.Now.Date;

            if (!HasFilter(filtros))
            {
                
                // Retorna com resultados apenas para especialista, outros perfis apenas com filtragem
                if (Cargo == NivelAcesso.Especialista.ToString())
                {
                    cockpits = _context.Cockpit.Where(x => x.MatriculaConsultor == MatriculaUsuario && x.DataContato >=minData && x.DataContato <= maxData).ToList();
                }
                else if (Cargo == NivelAcesso.Gestor.ToString())
                {

                    cockpits = new List<Cockpit>(); 
                    //_context.Cockpit.Join(_context.Usuario,
                    //        c => c.MatriculaConsultor,
                    //        u => u.Matricula,
                    //        (c, u) => new { c, u }
                    //        ).Where(res => res.u.MatriculaSupervisor == MatriculaUsuario && res.c.DataContato >= minData && res.c.DataContato <= maxData).Select(result => result.c).ToList();
                }
                else
                {
                    cockpits = new List<Cockpit>(); /*_context.Cockpit.Where(x => x.DataContato >= minData && x.DataContato <= maxData).Take(500).ToList();*/
                }
            }
            else
            {
                cockpits = filtroService.FiltrarCockpit(filtros, MatriculaUsuario, Cargo);
            }

            //var linksCapInv = GetLinksCapInvest();
            //ViewBag.LinksCap = linksCapInv.FirstOrDefault(l => l.Titulo == "CapLiq").Url;
            //ViewBag.LinksInvest = linksCapInv.FirstOrDefault(l => l.Titulo == "Invest").Url;
            var dataUltimaImportacao = _context.WindowsServiceConfig.FirstOrDefault(c => c.Tarefa == Comando.ImportarCockpit.ToString())?.UltimaExecucao;
            ViewBag.UltimaImportacao = dataUltimaImportacao;
            ViewBag.Especialistas = Services.SelectListItemGenerator.Especialistas();

            ViewBag.Titulo = "Cockpit";

            viewModel = cockpits.ConvertAll(c => CockpitViewModel.Mapear(c));
           
            return View(viewModel);
        }

        public ActionResult ExportarExcel(FiltrosTelas filtros = null)
        {
            var cockpits = default(List<CockpitExportExcel>);
            var minData = DateTime.Now.Date.AddDays(-90);
            var maxData = DateTime.Now.Date;
            var excel = default(byte[]);

            if (!HasFilter(filtros))
            {
                if (Cargo != NivelAcesso.Especialista.ToString())
                    return View("Error", new ErrorViewModel
                    {
                        Mensagem = "Para poder exportar para excel é preciso ter realizado um filtro antes",
                        Status = "400"
                    });


                cockpits = _context.Cockpit
                    .Join(_context.Usuario,
                        cock => cock.MatriculaConsultor,
                        usu => usu.Matricula,
                        (cock, usu) => new { cock, usu.Equipe, usu.Nome })
                    .Where(x => x.cock.MatriculaConsultor == MatriculaUsuario && x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                    .Select(s => new CockpitExportExcel
                    {
                        CodFuncionalGerente = s.cock.CodFuncionalGerente,
                        NomeGerente = s.cock.NomeGerente,
                        CPF = s.cock.CPF,
                        NomeCliente = s.cock.NomeCliente,
                        Conta = s.cock.Conta,
                        DataEncarteiramento = s.cock.DataEncarteiramento,
                        DataContato = s.cock.DataContato,
                        DataRetorno = s.cock.DataRetorno,
                        Observacao = s.cock.Observacao,
                        ContatoTeveExito = s.cock.ContatoTeveExito,
                        DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                        MeioContato = s.cock.MeioContato,
                        ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                        TipoTransacao = s.cock.TipoTransacao,
                        Finalizado = s.cock.Finalizado,
                        GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                        MatriculaConsultor = s.cock.MatriculaConsultor,
                        Equipe = s.Equipe,
                        CodigoAgencia = s.cock.CodigoAgencia,
                        NomeAgencia = s.cock.NomeAgencia,
                        Especialista = s.Nome
                       
                    })
                    .ToList();

                excel = GerarExcel(cockpits, $"Cokpits_Pesquisa_{filtros.Comentario?.ToUpper()}");
            }
            else
            {
                cockpits = filtroService.FiltrarCockpitExportExcel(filtros, MatriculaUsuario, Cargo);

                excel = GerarExcel(cockpits, $"Cokpits_Pesquisa_{filtros.Comentario.ToUpper()}");
            }

            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Exportação_Cokpit_Pesquisa_{filtros.Comentario?.ToUpper()}.xlsx");
        }

        private bool HasFilter(FiltrosTelas filtros)
        {
            return !String.IsNullOrEmpty(filtros.Comentario) || !String.IsNullOrEmpty(filtros.Equipe) || !string.IsNullOrEmpty(filtros.Especialista)
                || filtros.Agencia != null || filtros.Conta != null;
        }
    }
}