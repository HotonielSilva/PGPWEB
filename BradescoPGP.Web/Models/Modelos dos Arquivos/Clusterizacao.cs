using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Modelos_dos_Arquivos
{
    [Table("Clusterizacoes")]
    public class Clusterizacao
    {
        public int Id { get; set; }
        public string CONTA { get; set; }
        public string AGENCIA { get; set; }
        public string CPF_CNPJ { get; set; }
        public string PERFIL_API  { get; set; }
        public string MES_VCTO_API  { get; set; }
        public string NIVEL_DESENQ_FX_RISCO { get; set; }
        public decimal SALDO_CORRETORA_BRA { get; set; }
        public decimal SALDO_CORRETORA_AGORA { get; set; }
        public decimal SALDO_CORRETORA { get; set; }
        public decimal SALDO_PREVIDENCIA { get; set; }
        public decimal SALDO_POUPANCA { get; set; }
        public decimal SALDO_INVESTS { get; set; }
        public decimal SALDO_DAV_20K { get; set; }
        public decimal SALDO_COMPROMISSADAS { get; set; }
        public decimal SALDO_ISENTOS { get; set; }
        public decimal SALDO_LF { get; set; }
        public decimal SALDO_CDB { get; set; }
        public decimal SALDO_FUNDOS { get; set; }
        public decimal SALDO_TOTAL { get; set; }
        public decimal SALDO_TOTAL_M3 { get; set; }

    }
}