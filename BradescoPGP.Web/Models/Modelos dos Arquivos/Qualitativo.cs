using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Modelos_dos_Arquivos
{
    public class Qualitativo
    {
        public int Id { get; set; }

        public string NomeConsultor { get; set; }

        public string OBJETIVOTOTAL { get; set; }

        public string DENTRODACARTEIRA { get; set; }

        public string FORADACARTEIRA { get; set; }

        public string TOTALCONTATOS { get; set; }

        public string PORCENTAGEMATINGIMENTO { get; set; }

        public string GIRODECARTEIRAOBJETIVO { get; set; }

        public string GIRODECARTEIRAOREALIZADO { get; set; }

        public string PORCENTAGEMATINGIMENTOGIRO { get; set; }

        public string REVISAOFINANCEIRAOBJETIVO { get; set; }

        public string REVISAOFINANCEIRAREALIZADO { get; set; }

        public string PORCENTAGEMATINGIMENTOREVISAO { get; set; }

        public string CADASTROAPIOBJETIVO { get; set; }

        public string CADASTROAPIREALIZADO { get; set; }

        public string PORCENTAGEMATINGIMENTOCADASTROAPI { get; set; }
    }
}