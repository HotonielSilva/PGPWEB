using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BradescoPGP.Web.Services
{
    public static class SelectListItemGenerator
    {
        public static List<SelectListItem> Status(string evento = null)
        {
            using (var db = new PGPEntities())
            {
                var selectListItensStatus = new List<SelectListItem>();

                var situacoes = default(List<Status>);

                if (String.IsNullOrEmpty(evento))
                    situacoes = db.Status.ToList();
                else
                    situacoes = db.Status.Where(s => s.Evento == evento).ToList();

                selectListItensStatus = situacoes.ConvertAll(s => new SelectListItem { Text = s.Descricao, Value = s.Id.ToString() });
                
                selectListItensStatus.Insert(0, new SelectListItem { Text = "Selecione um Status", Selected = true, Value = "" });

                return selectListItensStatus;
            }
        }

        public static List<SelectListItem> Motivos(string evento = null)
        {
            using (var db = new PGPEntities())
            {
                var motivos = default(List<Motivo>);
                var selectListItensMotivos = new List<SelectListItem>();

                if (String.IsNullOrEmpty(evento))
                    motivos = db.Motivo.ToList();
                else
                    motivos = db.Motivo.Where(m => m.Evento == evento).ToList();

                selectListItensMotivos = motivos.ConvertAll(m => new SelectListItem { Text = m.Descricao, Value = m.Id.ToString() });

                return selectListItensMotivos;
            }
        }

        public static List<SelectListItem> Getentes()
        {
            using (var db = new PGPEntities())
            {
                var gerentes = db.Clusterizacoes.Select(s => s.GER_RELC).Distinct().ToList();

                var selectListItemGerente = new List<SelectListItem>();

                selectListItemGerente = gerentes.ConvertAll(g => new SelectListItem { Text = g, Value = g });
                
                selectListItemGerente.Insert(0, new SelectListItem { Text = "Selecione um Gerente", Selected = true, Value = "" });

                return selectListItemGerente;
            }
        }

        public static List<SelectListItem> Acoes()
        {
            using (var db = new PGPEntities())
            {
                var acoes = db.vwClusterTopTier.Select(s => s.ACAO).Distinct().ToList();

                var selectListItemAcoes = new List<SelectListItem>();

                selectListItemAcoes = acoes.ConvertAll(g => new SelectListItem { Text = g, Value = g });

                selectListItemAcoes.Insert(0, new SelectListItem { Text = "Selecione uma Ação", Selected = true, Value = "" });

                return selectListItemAcoes;
            }
        }

        public static List<SelectListItem> Especialistas()
        {
            using (var db = new PGPEntities())
            {
                var especialista = db.Usuario.Where(u => u.PerfilId == 3).Select(s => s.Nome).Distinct().ToList();

                var selectListEspecialistas = new List<SelectListItem>();

                selectListEspecialistas = especialista.ConvertAll(e => new SelectListItem { Text = e, Value = e });
                
                selectListEspecialistas.Insert(0, new SelectListItem { Text = "Especialista", Selected = true, Value = "" });

                return selectListEspecialistas;
            }
        }

        public static List<SelectListItem> EspecialistasPortabilidade()
        {
            using (var db = new PGPEntities())
            {
                var especialista = db.Usuario.Where(u => u.PerfilId == 3).Distinct().OrderBy(o => o.Nome).ToList();

                var selectListEspecialistas = new List<SelectListItem>();

                selectListEspecialistas = especialista.ConvertAll(e => new SelectListItem { Text = e.Nome, Value = e.Matricula });

                selectListEspecialistas.Insert(0, new SelectListItem { Text = "Especialista", Selected = true, Value = "" });

                return selectListEspecialistas;
            }
        }
        public static List<SelectListItem> EspecialistasTeds()
        {
            using (var db = new PGPEntities())
            {
                var especialista = db.Usuario.Where(u => u.PerfilId == 3).Select(e => new { e.Nome, e.Matricula}).Distinct().ToList();

                var selectListEspecialistasTeds = new List<SelectListItem>();

                selectListEspecialistasTeds = especialista.ConvertAll(e => new SelectListItem { Text = e.Nome.ToUpper(), Value = e.Matricula });

                selectListEspecialistasTeds.Insert(0, new SelectListItem { Text = "Especialista", Selected = true, Value = "" });

                return selectListEspecialistasTeds;
            }
        }

        public static List<SelectListItem> Origens(string evento)
        {
            using (var db = new PGPEntities())
            {
                var origens = default(List<Origem>);
                var selectListOrigens = default(List<SelectListItem>);

                if (string.IsNullOrEmpty(evento))
                    origens = db.Origem.ToList();
                else
                    origens = db.Origem.Where(o => o.Evento == evento).ToList();

                selectListOrigens = origens.ConvertAll(o => new SelectListItem { Text = o.Descricao, Value = o.Id.ToString() });

                return selectListOrigens;
            }
        }

        public static List<SelectListItem> Equipes()
        {
            using (var db = new PGPEntities())
            {
                var equipes = db.Usuario.Where(u => u.Equipe != "PGP").Select(u => u.Equipe.ToUpper()).Distinct().ToList();

                var selectListEquipes = equipes.ConvertAll(e => new SelectListItem { Text = e.ToUpper(), Value = e });
                selectListEquipes.Insert(0, new SelectListItem { Text = "", Value = "" });
                return selectListEquipes;
            }
        }
    }
}