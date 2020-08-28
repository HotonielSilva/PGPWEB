using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BradescoPGP.Web.Models
{
    public class CaminhoDinheiroModel
    {
        public string Cordenador { get; set; }
        public string Especialista { get; set; }
        public string Produto { get; set; }
        public decimal VolumeCapLiq { get; set; }
        public decimal VolumeAplicacao { get; set; }
        public decimal PercNewMoney { get; set; }
        public decimal VolumeResg { get; set; }
        public decimal PercFDOS { get; set; }
        public decimal PercCDB { get; set; }
        public decimal PercLF { get; set; }
        public decimal PercISENTOS { get; set; }
        public decimal PercCORRET { get; set; }
        public decimal PercPREVI { get; set; }
        public decimal PercCOMPR { get; set; }
        //public decimal PercINVEST { get; set; }
        public decimal PercTEDSENV { get; set; }
        public decimal PercOUTROS { get; set; }

        public static CaminhoDinheiroModel Mapear(vw_CaminhoDinheiroAgrupado vw_CamDin)
        {
            return new CaminhoDinheiroModel
            {
                Especialista = vw_CamDin.Consultor,
                Cordenador = vw_CamDin.Cordenador,
                Produto = vw_CamDin.Produto,
                VolumeCapLiq = vw_CamDin.CAPLIQ.Value,
                VolumeAplicacao = vw_CamDin.VL_APLIC.Value,
                PercNewMoney = vw_CamDin.PERC_NEWMONEY.Value,
                VolumeResg = vw_CamDin.VL_RESG.Value,
                PercFDOS = vw_CamDin.PERC_FDOS.Value,
                PercCDB = vw_CamDin.PERC_CDB.Value,
                PercLF = vw_CamDin.PERC_LF.Value,
                PercISENTOS = vw_CamDin.PERC_ISENTOS.Value,
                PercCORRET = vw_CamDin.PERC_CORRET.Value,
                PercTEDSENV = vw_CamDin.PERC_MSM_TIT_ENV_TOTAL.Value + vw_CamDin.PERC_DIF_TIT_ENV_TOTAL.Value,
                PercOUTROS = vw_CamDin.PERC_OUTROS.Value,
                PercCOMPR = vw_CamDin.PERC_CPMSS.Value,
                PercPREVI = vw_CamDin.PERC_PREVI.Value
            };
        }

        public static Dictionary<string, Dictionary<string, CaminhoDinheiroModel>> Mapear(List<CaminhoDinheiroModel> dados)
        {
            var agroupamento = dados.GroupBy(g => g.Especialista);

            var result = new Dictionary<string, Dictionary<string, CaminhoDinheiroModel>>();

            foreach (var group in agroupamento)
            {
               result.Add(group.Key,  CaminhoDinheiroModel.ObterCaminhoDinheiro(group));
            }

            return result;
        }

        private static Dictionary<string, CaminhoDinheiroModel> ObterCaminhoDinheiro(IGrouping<string, CaminhoDinheiroModel> capLiq)
        {
            var result = new Dictionary<string, CaminhoDinheiroModel>(); //key = produto

            var produtos = Enum.GetNames(typeof(ProutosCap)).ToList();

            produtos.Add("OUTROS");

            foreach (var produto in produtos)
            {
                var caminhoDinheiro = new CaminhoDinheiroModel();

                caminhoDinheiro.Cordenador = capLiq.FirstOrDefault().Cordenador;

                caminhoDinheiro.Especialista = capLiq.Key;

                caminhoDinheiro.Produto = produto;

                if (produto != "OUTROS")
                {
                    var totalAplicacoesPorProduto = capLiq.Where(c => c.Produto.StartsWith(produto))
                        .Sum(s => s.VolumeAplicacao);

                    var totalResgatesPorProduto = capLiq.Where(c => c.Produto.StartsWith(produto))
                        .Sum(s => s.VolumeResg);

                    //CapLiq
                    caminhoDinheiro.VolumeCapLiq = totalAplicacoesPorProduto - totalResgatesPorProduto;

                    //Aplicacaoes
                    caminhoDinheiro.VolumeAplicacao = totalAplicacoesPorProduto;

                    caminhoDinheiro.PercNewMoney = capLiq.Where(c => c.Produto.StartsWith(produto)).Sum(s => s.PercNewMoney);

                    //RESGATE
                    //var somaMSMTIT = capLiq.Select(s => s.MsmTitTotalEnv).Sum();
                    //var somaDIFTIT = capLiq.Select(s => s.DifTitTotalEnv).Sum();

                    caminhoDinheiro.VolumeResg = totalResgatesPorProduto;

                    var capProd = capLiq.Where(s => s.Produto.ToUpper().StartsWith(produto));


                    caminhoDinheiro.PercFDOS = capProd.Sum(s => s.PercFDOS);

                    caminhoDinheiro.PercCDB = capProd.Sum(s => s.PercCDB);

                    caminhoDinheiro.PercLF = capProd.Sum(s => s.PercLF);

                    caminhoDinheiro.PercISENTOS = capProd.Sum(s => s.PercISENTOS);

                    caminhoDinheiro.PercCORRET = capProd.Sum(s => s.PercCORRET);

                    caminhoDinheiro.PercPREVI = capProd.Sum(s => s.PercPREVI);

                    caminhoDinheiro.PercCOMPR = capProd.Sum(s => s.PercCOMPR);

                    //caminhoDinheiro.PercINVEST = ObterValorPorProduto(capLiq, "INVEST FACIL", "INVEST PLUS") / totalResgate;

                    caminhoDinheiro.PercTEDSENV = capProd.Sum(s => s.PercTEDSENV);

                    caminhoDinheiro.PercOUTROS = capProd.Sum(s => s.PercOUTROS);

                }
                else
                {
                    var produtosExcluidos = produtos.Where(p => p != produto);

                    //CapLiq
                    caminhoDinheiro.VolumeCapLiq = capLiq.Where(c => !produtosExcluidos.Contains(c.Produto)).Sum(s => s.VolumeCapLiq);

                    //Aplicacaoes
                    caminhoDinheiro.VolumeAplicacao = capLiq.Where(c => !produtosExcluidos.Contains(c.Produto))
                        .Sum(s => s.VolumeAplicacao); ;

                    caminhoDinheiro.PercNewMoney = capLiq.Where(c => !produtosExcluidos.Contains(c.Produto)).Sum(s => s.PercNewMoney);

                    //RESGATE

                    caminhoDinheiro.VolumeResg = capLiq.Where(c => !produtosExcluidos.Contains(c.Produto))
                        .Sum(s => s.VolumeResg); ;

                    var capProd = capLiq.Where(s => !produtosExcluidos.Contains(s.Produto));

                    
                        caminhoDinheiro.PercFDOS = capProd.Sum(s => s.PercFDOS);

                        caminhoDinheiro.PercCDB = capProd.Sum(s => s.PercCDB);

                        caminhoDinheiro.PercLF = capProd.Sum(s => s.PercLF);

                        caminhoDinheiro.PercISENTOS = capProd.Sum(s => s.PercISENTOS);

                        caminhoDinheiro.PercCORRET = capProd.Sum(s => s.PercCORRET);

                        caminhoDinheiro.PercPREVI = capProd.Sum(s => s.PercPREVI);

                        caminhoDinheiro.PercCOMPR = capProd.Sum(s => s.PercCOMPR);

                        //caminhoDinheiro.PercINVEST = ObterValorPorProduto(capLiq, "INVEST FACIL", "INVEST PLUS") / totalResgate;

                        caminhoDinheiro.PercTEDSENV = capProd.Sum(s => s.PercTEDSENV);

                         caminhoDinheiro.PercOUTROS = capProd.Sum(s => s.PercOUTROS);
                }

                result.Add(produto, caminhoDinheiro);
            }

            return result;
        }

    }

    public enum ProutosCap
    {
        CDB,
        FUNDOS,
        COMPROMISSADA,
        ISENTOS,
        CORRETORA,
        PREVIDENCIA,
        INVEST
    }
}
