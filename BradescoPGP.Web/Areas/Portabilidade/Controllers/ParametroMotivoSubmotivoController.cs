using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using BradescoPGP.Web.Controllers;
using BradescoPGP.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BradescoPGP.Web.Areas.Portabilidade.Controllers
{
    public class ParametroMotivoSubmotivoController : AbstractController
    {
        private readonly IMotivoService _motivoService;

        public ParametroMotivoSubmotivoController(DbContext context, IMotivoService motivoService) : base(context)
        {
            _motivoService = motivoService;
        }

        // GET: Portabilidade/ParametroMotivoSubmotivo
        public ActionResult Index()
        {
            ViewBag.Titulo = "Parametro Motivo SubMotivo";

            var model = _motivoService.ObterTodosMotivos();

            return View(model);
        }


        public ActionResult NovoMotivo(string motivo)
        {
            if (string.IsNullOrEmpty(motivo))
            {
                return View("Error", new ErrorViewModel { Mensagem = "Motivo não pode estar vazio" });
            }

            var novoMotivo = new Motivo
            {
                Descricao = motivo,
                Evento = Eventos.Portabilidade.ToString(),
                EmUso = true
            };

            if (_motivoService.NovoMotivo(novoMotivo))
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Error", new ErrorViewModel { Mensagem = "Erro ao criar novo motivo" });
        }

        public ActionResult NovoSubmotivo(int idMotivo, string submotivo)
        {
            var novoSubmotivo = new SubMotivo
            {
                Descricao = submotivo,
                MotivoId = idMotivo,
                EmUso = true
            };

            if (_motivoService.NovoSubMotivo(novoSubmotivo))
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Error", new ErrorViewModel { Mensagem = "Erro ao criar novo submotivo" });
        }

        public ActionResult EditarMotivo(int idMotivo, string motivo)
        {
            if (_motivoService.EditarMotivo(idMotivo, motivo))
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error", new ErrorViewModel
            {
                Mensagem = "Erro ao atualizar motivo"
            });
        }

        public ActionResult EditarSubmotivo(int idSubmotivo, string submotivo)
        {
            if (_motivoService.EditarSubmotivo(idSubmotivo, submotivo))
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error", new ErrorViewModel
            {
                Mensagem = "Erro ao atualizar submotivo"
            });
        }

        [HttpPost]
        public ActionResult Excluir(int idMotivo)
        {
            if (_motivoService.ExcluirMotivo(idMotivo))
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error", new ErrorViewModel { Mensagem = "Não foi possivel excluir este Motivo" });
        }

        [HttpPost]
        public ActionResult ExcluirSubmotivo(int idSubMotivo)
        {
            if (_motivoService.ExcluirSubmotivo(idSubMotivo))
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error", new ErrorViewModel { Mensagem = "Não foi possivel excluir este Motivo" });
        }
    }
}