using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;

namespace BradescoPGP.Web.Controllers
{
    [Autorizacao(Roles = "Master, Gestor")]
    public class ConfiguracaoController : AbstractController
    {
        private readonly PGPEntities _context;

        public ConfiguracaoController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
        }

        // GET: Configuracao
        public ActionResult Index()
        {
            var linksNaoInclusos = new string[] { "CapLiq", "Invest" };
            var importacoes = _context.WindowsServiceConfig.Where(c => !String.IsNullOrEmpty(c.CaminhoOrigem)).ToList();
            var links = _context.Links.Where(l => !l.Exibir && !linksNaoInclusos.Contains(l.Titulo)).ToList();
            var faixasTedPorEquipe = _context.TEDFaixaEquipe.ToList();

            var viewModel = new ConfiguracaoViewModel(importacoes, links, faixasTedPorEquipe);

            ViewBag.Titulo = "Configuração";

            return View(viewModel);
        }

        [HttpPost]
        public String AtualizarImportacao([Bind(Include = "Id, CaminhoOrigem, PadraoPesquisa")]WindowsServiceConfig configuracao)
        {
            var config = _context.WindowsServiceConfig.FirstOrDefault(c => c.Id == configuracao.Id);

            if(config == null)
                return JsonConvert.SerializeObject(new { success = false, atualizar = true, excluir = false });

            config.CaminhoOrigem = configuracao.CaminhoOrigem;

            config.PadraoPesquisa = configuracao.PadraoPesquisa;

            _context.Entry(config).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new { success = false, atualizar = true, excluir = false });
            }

            return JsonConvert.SerializeObject(new { success = true, atualizar = true, excluir = false });
        }

        [HttpPost]
        public String ExcluirImportacao(int id)
        {
            var config = _context.WindowsServiceConfig.FirstOrDefault(c => c.Id == id);

            if(config == null)
                return JsonConvert.SerializeObject(new { success = false, atualizar = false, excluir = true });

            try
            {
                _context.WindowsServiceConfig.Remove(config);
                
                _context.SaveChanges();

            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new { success = false, atualizar = false, excluir = true });
            }

            return JsonConvert.SerializeObject(new { success = true, atualizar = false, excluir = true });
        }

        [HttpPost]
        public String AtualizarLinkExterno([Bind(Include = "Id, Url")]Links link)
        {
            var entity = _context.Links.First(l => l.Id == link.Id);

            entity.Url = link.Url;

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new { success = false, atualizar = true, excluir = false });

            }

            return JsonConvert.SerializeObject(new { success = true, atualizar = true, excluir = false });

        }

        public String ExcluirLinkExterno(int id)
        {
            var link = _context.Links.First(c => c.Id == id);

            _context.Links.Remove(link);

            try
            {
                _context.SaveChanges();

            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new { success = false, atualizar = false, excluir = true });
            }

            return JsonConvert.SerializeObject(new { success = true, atualizar = false, excluir = true });
        }

        [HttpPost]
        public String AtualizarFaixaValorTed(TEDFaixaEquipe faixa)
        {
            var entity = _context.TEDFaixaEquipe.First(l => l.Id == faixa.Id);

            entity.De = faixa.De;
            entity.Ate = faixa.Ate;

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new { success = false, atualizar = true, excluir = false });

            }

            return JsonConvert.SerializeObject(new { success = true, atualizar = true, excluir = false });

        }

        public String ExcluirFaixaValorTed(int id)
        {
            var link = _context.TEDFaixaEquipe.First(c => c.Id == id);

            _context.TEDFaixaEquipe.Remove(link);

            try
            {
                _context.SaveChanges();

            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new { success = false, atualizar = false, excluir = true });
            }

            return JsonConvert.SerializeObject(new { success = true, atualizar = false, excluir = true });
        }

    }
}