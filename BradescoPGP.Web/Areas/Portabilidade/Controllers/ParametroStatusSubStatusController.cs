using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using BradescoPGP.Web.Areas.Portabilidade.Models;
using BradescoPGP.Web.Controllers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using BradescoPGP.Web.Models;
using BradescoPGP.Common;

namespace BradescoPGP.Web.Areas.Portabilidade.Controllers
{
    public class ParametroStatusSubStatusController : AbstractController
    {
        private IStatusSubStatusService _statusService;

        private readonly PGPEntities _context;

        public ParametroStatusSubStatusController(DbContext context, IStatusSubStatusService statusService) : base(context)
        {
            _context = context as PGPEntities;
            _statusService = statusService;
        }

        // GET: Portabilidade/ParametroStatusSubStatus
        public ActionResult Index()
        {
            var status = _statusService.ObterStatus();
            var substatus = _statusService.ObterSubStatus();

            ViewBag.Titulo = "Parametro Status SubStatus";

            return View(status);
        }
        public ActionResult Editar(int id, string descricao)
        {
            if (_statusService.EditarStatus(id, descricao))
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error", new ErrorViewModel
            {
                Mensagem = "Erro ao editar este status"
            });
        }

        public ActionResult NovoStatus(string descricao)
        {
            if (string.IsNullOrEmpty(descricao))
            {
                return View("Error", new ErrorViewModel
                {
                    Mensagem = "Não foi possível criar novo status, pois o mesmo esta nulo ou não tem valor"
                });
            }


            if (_statusService.NovoStatus(descricao))
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error", new ErrorViewModel
            {
                Mensagem = "Erro ao cadastrar novo status"
            });
        }

        public ActionResult NovoSubStatus(int idStatus, string descricao)
        {
            if (_statusService.NovoSubStatus(idStatus, descricao))
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Error", new ErrorViewModel {

                Mensagem = "Erro ao criar um SubStatus"
            });
        }
        public ActionResult EditarSubStatus(int idSubstatus, string descricao)
        {
            if (_statusService.EditarSubStatus(idSubstatus, descricao))
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error", new ErrorViewModel
            {
                Mensagem = "Erro ao editar este status"
            });
        }



    }
}







