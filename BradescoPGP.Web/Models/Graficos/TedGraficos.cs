using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Graficos
{
    public class TedGraficos
    {
        public GraficoQuantidadeTed GraficoQuantidadeTed { get; set; }
        public GraficoValorTed GraficoValorTed { get; set; }
        public GraficoQuantidadeStatus GraficoQuantidadeStatus { get; set; }
        public GraficoValorStatus GraficoValorStatus { get; set; }
        public GraficoAplicacaoProduto GraficoAplicacaoProduto { get; set; }
        public GraficoMotivoNaoAplicacao GraficoMotivoNaoAplicacao { get; set; }
    }
}