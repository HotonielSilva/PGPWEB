using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BradescoPGP.Repositorio;

namespace BradescoPGP.Web.Models
{
    public class InvestFacilExcel
    {
        public string SEGMENTO_CLIENTE { get; set; }
        public string AGENCIA { get; set; }
        public string CONTA { get; set; }
        public string NUM_CONTRATO { get; set; }
        public string MES_DT_BASE { get; set; }
        public DateTime? DT_EMISSAO { get; set; }
        public int? PRAZO_PERMAN { get; set; }
        public string FX_PERMANENCIA { get; set; }
        public string FX_VOLUME { get; set; }
        public decimal Vlr_Evento { get; set; }
        public string SEGM_AGRUPADO { get; set; }
        public string SEGMENTO_MACRO { get; set; }
        public string Especialista { get; set; }
        public string Equipe { get; set; }
    }
}