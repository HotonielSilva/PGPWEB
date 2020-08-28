using System;
using System.Collections.Generic;
using BradescoPGP.Repositorio;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class CarteiraClienteViewModel
    {
        public int Id { get; set; }
        public string Especialista { get; set; }
        public string Conta { get; set; }
        public string Agencia { get; set; }
        public string CPF { get; set; }
        public string PerfilApi { get; set; }
        public string MES_VCTO_API { get; set; }
        public string NIVEL_DESENQ_FX_RISCO { get; set; }
        //public decimal SALDO_TOTAL_M3 { get; set; }
        public decimal SALDO_TOTAL { get; set; }
        //public decimal SALDO_CORRETORA_BRA { get; set; }
        //public decimal SALDO_CORRETORA_AGORA { get; set; }
        //public decimal SALDO_CORRETORA { get; set; }
        //public decimal SALDO_PREVIDENCIA { get; set; }
        //public decimal SALDO_POUPANCA { get; set; }
        //public decimal SALDO_INVESTS { get; set; }
        //public decimal SALDO_DAV_20K { get; set; }
        //public decimal SALDO_COMPROMISSADAS { get; set; }
        //public decimal SALDO_ISENTOS { get; set; }
        //public decimal SALDO_LF { get; set; }
        //public decimal SALDO_CDB { get; set; }
        //public decimal SALDO_FUNDOS { get; set; }
        public string NomeGerente { get; set; }
        public string NomeCliente { get; set; }
        public DateTime? UltimoContato { get; set; }
        public DateTime UltimaTentativa { get; set; }
        public int DiasCorridosÚltimoContato { get; set; }
        public string Situacao { get; set; }
        public string Matricula { get; set; }
        public string Equipe { get; set; }

        public static CarteiraClienteViewModel Mapear(Clusterizacoes clusterizacao, string especialista, string NomeGerente, DateTime dataContato, bool teveExito, string matricula)
        {
            return new CarteiraClienteViewModel
            {
                Especialista = especialista,
                Agencia = clusterizacao.AGENCIA.ToString(),
                Conta = clusterizacao.CONTA.ToString(),
                CPF = clusterizacao.CPF_CNPJ,
                PerfilApi = clusterizacao.PERFIL_API,
                NIVEL_DESENQ_FX_RISCO = clusterizacao.NIVEL_DESENQ_FX_RISCO,
                MES_VCTO_API = clusterizacao.MES_VCTO_API,
                //SALDO_TOTAL_M3 = clusterizacao.SALDO_TOTAL_M3,
                SALDO_TOTAL = clusterizacao.SALDO_TOTAL,
                //SALDO_CORRETORA_BRA = clusterizacao.SALDO_CORRETORA_BRA,
                //SALDO_CORRETORA_AGORA = clusterizacao.SALDO_CORRETORA_AGORA,
                //SALDO_CORRETORA = clusterizacao.SALDO_CORRETORA,
                //SALDO_PREVIDENCIA = clusterizacao.SALDO_PREVIDENCIA,
                //SALDO_POUPANCA = clusterizacao.SALDO_POUPANCA,
                //SALDO_INVESTS = clusterizacao.SALDO_INVESTS,
                //SALDO_DAV_20K = clusterizacao.SALDO_DAV_20K,
                //SALDO_COMPROMISSADAS = clusterizacao.SALDO_COMPROMISSADAS,
                //SALDO_ISENTOS = clusterizacao.SALDO_ISENTOS,
                //SALDO_LF = clusterizacao.SALDO_LF,
                //SALDO_CDB = clusterizacao.SALDO_CDB,
                //SALDO_FUNDOS = clusterizacao.SALDO_FUNDOS,
                NomeGerente = NomeGerente,
                UltimaTentativa = dataContato,
                UltimoContato = teveExito ? (DateTime?)dataContato : null,
                DiasCorridosÚltimoContato = DateTime.Now.Subtract(dataContato).Days,
                Situacao = clusterizacao.Situacao,
                Matricula = matricula
            };
        }

    }
}