﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class CarteiraClienteExportExcel
    {
        public string Especialista { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string CPF { get; set; }
        public string PerfilApi { get; set; }
        public string MES_VCTO_API { get; set; }
        public string NIVEL_DESENQ_FX_RISCO { get; set; }
        public string NomeCliente { get; set; }
        public string NomeGerente { get; set; }
        public DateTime? UltimoContato { get; set; }
        public DateTime UltimaTentativa { get; set; }
        public int DiasCorridosÚltimoContato { get; set; }
        public string Situacao { get; set; }

        public string Matricula { get; set; }
        public string Equipe { get; set; }
        public decimal SALDO_TOTAL_M3 { get; set; }
        public decimal SALDO_TOTAL { get; set; }
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
    }
}