using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Graficos
{
    public class GraficoQuantidadeTed
    {
        public SerieGraficoBar Dados { get; set; }
    }
    public class GraficoValorTed
    {
        public SerieGraficoBar Dados { get; set; }
    }
    public class GraficoQuantidadeStatus
    {
        public SerieGraficoBar Dados { get; set; }
    }
    public class GraficoValorStatus
    {
        public SerieGraficoBar Dados { get; set; }
    }
    public class GraficoAplicacaoProduto
    {
        public SerieGraficoPie Dados { get; set; }
    }
    public class GraficoMotivoNaoAplicacao
    {
        public SerieGraficoPie Dados { get; set; }
    }
}