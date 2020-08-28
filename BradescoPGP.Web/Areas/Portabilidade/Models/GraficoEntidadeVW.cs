using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class GraficoEntidadeVW
    {
        public string NomeEntidade { get; set; }
        public decimal? ValorSolicitado { get; set; }
        public int Quantidade { get; set; }
        public double PorcentagemValor { get; set; }
        public string Fundo { get; set; }

        public static GraficoEntidadeVW Mapear(GraficoEntidadeVW dadoGrafico) {

            var entidades = new GraficoEntidadeVW();

            entidades.NomeEntidade = dadoGrafico.NomeEntidade;
            entidades.ValorSolicitado = dadoGrafico.ValorSolicitado;
            entidades.Quantidade = dadoGrafico.Quantidade;
            entidades.PorcentagemValor = dadoGrafico.PorcentagemValor;
            entidades.Fundo = dadoGrafico.Fundo;

            return entidades;


        }
    }
}