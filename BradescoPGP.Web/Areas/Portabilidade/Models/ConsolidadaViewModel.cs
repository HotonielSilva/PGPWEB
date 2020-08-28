using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class ConsolidadaViewModel
    {
        public GraficoStatusSolicitacaoViewModel StatusSolicitacao { get; set; }
        public GraficoMotivoViewModel MotivosRetencao { get; set; }
        public GraficoOperacoesVencerViewModel OperacoesVencer { get; set; }
        public GraficoEquipeViewModel Equipe { get; set; }
    }
}