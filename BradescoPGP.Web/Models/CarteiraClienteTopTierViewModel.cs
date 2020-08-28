using BradescoPGP.Repositorio;
using System;

namespace BradescoPGP.Web.Models
{
    public class CarteiraClienteTopTierViewModel
    {
        public string Especialista { get; set; }
        public string Conta { get; set; }
        public string Agencia { get; set; }
        public string CPF { get; set; }
        public string GER_RELC { get; set; }
        public string ACAO { get; set; }
        public string NIVEL_DESENQ_FX_RISCO { get; set; }
        public string PERFIL_API { get; set; }
        public decimal SALDO_TOTAL { get; set; }
        public decimal SALDO_PREVIDENCIA { get; set; }
        public string ACAO_PRINCIPAL { get; set; }



        public static CarteiraClienteTopTierViewModel Mapear(vwClusterTopTier clusterizacao)
        {
            return new CarteiraClienteTopTierViewModel
            {
                Especialista = clusterizacao.CONSULTOR,
                Agencia = clusterizacao.AGENCIA.ToString(),
                Conta = clusterizacao.CONTA.ToString(),
                CPF = clusterizacao.CPF_CNPJ,
                GER_RELC = clusterizacao.GER_RELC,
                ACAO = clusterizacao.ACAO,
                NIVEL_DESENQ_FX_RISCO = clusterizacao.NIVEL_DESENQ_FX_RISCO,
                PERFIL_API = clusterizacao.PERFIL_API,
                SALDO_TOTAL = clusterizacao.SALDO_TOTAL,
                SALDO_PREVIDENCIA = clusterizacao.SALDO_PREVIDENCIA,
                ACAO_PRINCIPAL = clusterizacao.ACAO_PRINCIPAL
            };
        }

    }
}