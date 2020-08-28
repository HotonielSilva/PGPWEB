using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class RankingViewModel
    {
        public RankingViewModel()
        {
            PorcentgemRetida = default(decimal);
            Contatos = default(int);
            QuantidadeSolicitacoes = default(int);
            ValorSolicitacoes = default(decimal);
            PorcentagemContatos = default(decimal);
            QuantidadeRetida = default(int);
            ValorRetido = default(decimal);
        }
        public string Especialista { get; set; }
        public int QuantidadeSolicitacoes { get; set; }
        public decimal ValorSolicitacoes { get; set; }
        public int? Contatos { get; set; }
        public decimal? PorcentagemContatos { get; set; }
        public int? QuantidadeRetida { get; set; }
        public decimal ValorRetido { get; set; }
        public decimal? PorcentgemRetida { get; set; }
    }

}