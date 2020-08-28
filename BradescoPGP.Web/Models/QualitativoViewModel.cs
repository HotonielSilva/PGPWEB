using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class QualitativoViewModel
    {
        public string NomeConsultor { get; set; }
        public int OBJETIVOTOTAL { get; set; }
        public int DENTRODACARTEIRA { get; set; }
        public int FORADACARTEIRA { get; set; }
        public int TOTALCONTATOS { get; set; }
        public string PORCENTAGEMATINGIMENTO { get; set; }
        public int GIRODECARTEIRAOBJETIVO { get; set; }
        public int GIRODECARTEIRAOREALIZADO { get; set; }
        public string PORCENTAGEMATINGIMENTOGIRO { get; set; }
        public string REVISAOFINANCEIRAOBJETIVO { get; set; }
        public int REVISAOFINANCEIRAREALIZADO { get; set; }
        public string PORCENTAGEMATINGIMENTOREVISAO { get; set; }
        public string CADASTROAPIOBJETIVO { get; set; }
        public double CADASTROAPIREALIZADO { get; set; }
        public string PORCENTAGEMATINGIMENTOCADASTROAPI { get; set; }

        public static QualitativoViewModel Mapear(Qualitativo qualitativo)
        {
            return new QualitativoViewModel
            {
                NomeConsultor = qualitativo.NomeConsultor,
                OBJETIVOTOTAL = qualitativo.OBJETIVOTOTAL,
                DENTRODACARTEIRA = qualitativo.DENTRODACARTEIRA,
                FORADACARTEIRA = qualitativo.FORADACARTEIRA,
                TOTALCONTATOS = qualitativo.TOTALCONTATOS,
                PORCENTAGEMATINGIMENTO = qualitativo.PORCENTAGEMATINGIMENTO,
                GIRODECARTEIRAOBJETIVO = qualitativo.GIRODECARTEIRAOBJETIVO,
                GIRODECARTEIRAOREALIZADO = qualitativo.GIRODECARTEIRAOREALIZADO,
                PORCENTAGEMATINGIMENTOGIRO = qualitativo.PORCENTAGEMATINGIMENTOGIRO,
                REVISAOFINANCEIRAOBJETIVO = qualitativo.REVISAOFINANCEIRAOBJETIVO,
                REVISAOFINANCEIRAREALIZADO = qualitativo.REVISAOFINANCEIRAREALIZADO,
                PORCENTAGEMATINGIMENTOREVISAO = qualitativo.PORCENTAGEMATINGIMENTOREVISAO,
                CADASTROAPIOBJETIVO = qualitativo.CADASTROAPIOBJETIVO,
                CADASTROAPIREALIZADO = qualitativo.CADASTROAPIREALIZADO,
                PORCENTAGEMATINGIMENTOCADASTROAPI = qualitativo.PORCENTAGEMATINGIMENTOCADASTROAPI
            };
        }

        public static List<QualitativoViewModel> Mapear(List<Qualitativo> qualitativo)
        {
            var listaQualitativoViewModel = new List<QualitativoViewModel>();

            qualitativo.ForEach(q =>
            {
                listaQualitativoViewModel.Add(
                    new QualitativoViewModel
                    {
                        NomeConsultor = q.NomeConsultor,
                        OBJETIVOTOTAL = q.OBJETIVOTOTAL,
                        DENTRODACARTEIRA = q.DENTRODACARTEIRA,
                        FORADACARTEIRA = q.FORADACARTEIRA,
                        TOTALCONTATOS = q.TOTALCONTATOS,
                        PORCENTAGEMATINGIMENTO = q.PORCENTAGEMATINGIMENTO,
                        GIRODECARTEIRAOBJETIVO = q.GIRODECARTEIRAOBJETIVO,
                        GIRODECARTEIRAOREALIZADO = q.GIRODECARTEIRAOREALIZADO,
                        PORCENTAGEMATINGIMENTOGIRO = q.PORCENTAGEMATINGIMENTOGIRO,
                        REVISAOFINANCEIRAOBJETIVO = q.REVISAOFINANCEIRAOBJETIVO,
                        REVISAOFINANCEIRAREALIZADO = q.REVISAOFINANCEIRAREALIZADO,
                        PORCENTAGEMATINGIMENTOREVISAO = q.PORCENTAGEMATINGIMENTOREVISAO,
                        CADASTROAPIOBJETIVO = q.CADASTROAPIOBJETIVO,
                        CADASTROAPIREALIZADO = q.CADASTROAPIREALIZADO,
                        PORCENTAGEMATINGIMENTOCADASTROAPI = q.PORCENTAGEMATINGIMENTOCADASTROAPI
                    }
                );
            });

            return listaQualitativoViewModel;
        }

    }

   
}