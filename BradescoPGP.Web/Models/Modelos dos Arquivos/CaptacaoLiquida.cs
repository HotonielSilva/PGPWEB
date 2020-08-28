using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Modelos_dos_Arquivos
{
    [Table("CaptacaoLiquida")]
    public class CaptacaoLiquida
    {
        public int Id { get; set; }
        public string CONTA { get; set; }
        public string AGENCIA { get; set; }
        public string SEGMENTO_CLIENTE { get; set; }
        public string SEGM_AGRUPADO { get; set; }
        public string PRODUTO_MACRO { get; set; }
        public string ANOMES { get; set; }
        public decimal VL_APLIC  { get; set; }
        public decimal VL_RESG { get; set; }
        public decimal VL_NET { get; set; }
        public decimal N_MONEY { get; set; }
        public decimal TED_TOTAL { get; set; }
        public decimal MSM_TIT_ITAU { get; set; }
        public decimal MSM_TIT_BB { get; set; }
        public decimal MSM_TIT_SANTANDER { get; set; }
        public decimal MSM_TIT_CAIXA  { get; set; }
        public decimal MSM_TIT_SAFRA { get; set; }
        public decimal MSM_TIT_OUTROS  { get; set; }
        public decimal DIF_TIT_ITAU { get; set; }
        public decimal DIF_TIT_BB { get; set; }
        public decimal DIF_TIT_SANTANDER { get; set; }
        public decimal DIF_TIT_CAIXA { get; set; }
        public decimal DIF_TIT_SAFRA  { get; set; }
        public decimal DIF_TIT_OUTROS { get; set; }
        public decimal TED_ENV_MSM_TIT { get; set; }
        public decimal TED_ENV_TIT_DIF { get; set; }
    }
}