using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Services
{
    public class BackUpCapLiqService
    {
        //public class CaptacaoLiquidaExcel
        //{
        //    public string Matricula { get; set; }
        //    public string MatriculaSupervisor { get; set; }
        //    public string NomeEspecialista { get; set; }
        //    public decimal TotalCapLiquida { get; set; }
        //    public decimal CDB { get; set; }
        //    public decimal FUNDOS { get; set; }
        //    public decimal COMPROMISSADA { get; set; }
        //    public decimal LF { get; set; }
        //    public decimal ISENTOS { get; set; }
        //    public decimal CORRETORA { get; set; }
        //    public decimal PREVI { get; set; }
        //    public decimal INVEST { get; internal set; }
        //    public decimal OUTROS { get; internal set; }

        //    public Dictionary<string, CaminhoDinheiro> CaminhoDinheiro { get; set; }
        //}

        //public class CaptacaoLiquidaService
        //{
        //    public IEnumerable<CaptacaoLiquidaExcelModel> ObterCaptacao()
        //    {
        //        var conStr = ConfigurationManager.ConnectionStrings["PGP"].ConnectionString;

        //        var result = new List<CaptacaoLiquidaExcelModel>();

        //        using (var conn = new SqlConnection(conStr))
        //        {
        //            conn.Open();

        //            var command = new SqlCommand("dbo.sp_CapLiqCanDin", conn);

        //            command.CommandType = System.Data.CommandType.StoredProcedure;

        //            using (var rd = command.ExecuteReader())
        //            {
        //                while (rd.Read())
        //                {
        //                    var dado = new CaptacaoLiquidaExcelModel();

        //                    dado.Agencia = rd["Agencia"].ToString();
        //                    dado.Conta = rd["Conta"].ToString();
        //                    dado.Produto = rd["Produto"].ToString();
        //                    dado.Especialista = rd["Especialista"].ToString();
        //                    dado.MatriculaConsultor = rd["MatriculaConsultor"].ToString();
        //                    dado.MatriculaSupervisor = rd["MatriculaSupervisor"].ToString();
        //                    decimal.TryParse(rd["TotalAplicacao"]?.ToString(), out var totalAplicado);
        //                    dado.TotalAplicacao = totalAplicado;
        //                    decimal.TryParse(rd["TotalResgate"]?.ToString(), out var totalResgate);
        //                    dado.TotalResgate = totalResgate;
        //                    decimal.TryParse(rd["CapLiq"]?.ToString(), out var capLiq);
        //                    dado.CapLiq = capLiq;
        //                    decimal.TryParse(rd["VL_DINHEIRO_NOVO"]?.ToString(), out var dinheiroNovo);
        //                    dado.VL_DINHEIRO_NOVO = dinheiroNovo;
        //                    decimal.TryParse(rd["VL_RESG_CDB"]?.ToString(), out var resgCDB);
        //                    dado.VL_RESG_CDB = resgCDB;
        //                    decimal.TryParse(rd["VL_RESG_ISENTOS"]?.ToString(), out var resgIsentos);
        //                    dado.VL_RESG_ISENTOS = resgIsentos;
        //                    decimal.TryParse(rd["VL_RESG_COMPROMISSADAS"]?.ToString(), out var resgCompr);
        //                    dado.VL_RESG_COMPROMISSADAS = resgCompr;
        //                    decimal.TryParse(rd["VL_RESG_LF"]?.ToString(), out var resgLF);
        //                    dado.VL_RESG_LF = resgLF;
        //                    decimal.TryParse(rd["VL_RESG_FUNDOS"]?.ToString(), out var resgFundos);
        //                    dado.VL_RESG_FUNDOS = resgFundos;
        //                    decimal.TryParse(rd["VL_RESG_CORRET"]?.ToString(), out var resgCorret);
        //                    dado.VL_RESG_CORRET = resgCorret;
        //                    decimal.TryParse(rd["VL_RESG_PREVI"]?.ToString(), out var resgPrevi);
        //                    dado.VL_RESG_PREVI = resgPrevi;

        //                    result.Add(dado);
        //                }

        //                rd.Close();
        //            }

        //            return result;
        //        }
        //    }

        //    public Dictionary<string, CaptacaoLiquidaExcel> GerarCaptacaoLiquida()
        //    {
        //        //Filtros da capliq
        //        var captacaoLiquida = ObterCaptacao().GroupBy(g => g.MatriculaConsultor).ToList();

        //        var result = new Dictionary<string, CaptacaoLiquidaExcel>(); //key = martricula

        //        var produtosInvest = new string[] { "INVEST FÁCIL", "INVEST PLUS" };

        //        foreach (var capLiq in captacaoLiquida)
        //        {
        //            var capLiqExc = new CaptacaoLiquidaExcel();

        //            //var totalAplicacoes = capLiq.Sum(s => s.TotalAplicacao);

        //            //var totalResgates = capLiq.Sum(s => s.TotalResgate);

        //            capLiqExc.NomeEspecialista = capLiq.FirstOrDefault().Especialista;

        //            capLiqExc.Matricula = capLiq.Key;

        //            capLiqExc.MatriculaSupervisor = capLiq.FirstOrDefault().MatriculaSupervisor;

        //            capLiqExc.TotalCapLiquida = capLiq.Sum(s => s.CapLiq);

        //            capLiqExc.CDB = capLiq.Where(c => c.Produto.ToUpper() == "CDB").Sum(s => s.TotalAplicacao) - capLiq.Where(c => c.Produto.ToUpper() == "CDB").Sum(s => s.TotalResgate);

        //            capLiqExc.FUNDOS = capLiq.Where(c => c.Produto.ToUpper() == "FUNDOS").Sum(s => s.TotalAplicacao) - capLiq.Where(c => c.Produto.ToUpper() == "FUNDOS").Sum(s => s.TotalResgate);

        //            capLiqExc.COMPROMISSADA = capLiq.Where(c => c.Produto.ToUpper().StartsWith("COMPR")).Sum(s => s.TotalAplicacao) - capLiq.Where(c => c.Produto.ToUpper().StartsWith("COMPR")).Sum(s => s.TotalResgate);

        //            capLiqExc.LF = capLiq.Where(c => c.Produto.ToUpper().StartsWith("LF")).Sum(s => s.TotalAplicacao) - capLiq.Where(c => c.Produto.ToUpper().StartsWith("LF")).Sum(s => s.TotalResgate);

        //            capLiqExc.ISENTOS = capLiq.Where(c => c.Produto.ToUpper().StartsWith("ISENTOS")).Sum(s => s.TotalAplicacao) - capLiq.Where(c => c.Produto.ToUpper().StartsWith("ISENTOS")).Sum(s => s.TotalResgate);

        //            capLiqExc.CORRETORA = capLiq.Where(c => c.Produto.ToUpper().StartsWith("CORRET")).Sum(s => s.TotalAplicacao) - capLiq.Where(c => c.Produto.ToUpper().StartsWith("CORRET")).Sum(s => s.TotalResgate);

        //            capLiqExc.PREVI = capLiq.Where(c => c.Produto.ToUpper().StartsWith("PREVI")).Sum(s => s.TotalAplicacao) - capLiq.Where(c => c.Produto.ToUpper().StartsWith("PREVI")).Sum(s => s.TotalResgate);

        //            capLiqExc.INVEST = capLiq.Where(c => produtosInvest.Contains(c.Produto)).Sum(s => s.TotalAplicacao) - capLiq.Where(c => produtosInvest.Contains(c.Produto)).Sum(s => s.TotalResgate);

        //            var produtos = Enum.GetNames(typeof(ProutosCap));

        //            var capLiqProds = capLiqExc.CDB + capLiqExc.FUNDOS + capLiqExc.COMPROMISSADA + capLiqExc.LF + capLiqExc.ISENTOS + capLiqExc.CORRETORA + capLiqExc.PREVI + capLiqExc.INVEST;

        //            capLiqExc.OUTROS = capLiqExc.TotalCapLiquida - capLiqProds;

        //            capLiqExc.CaminhoDinheiro = ObterCaminhoDinheiro(capLiq);

        //            result.Add(capLiq.Key, capLiqExc);
        //        }

        //        return result;

        //    }

        //    private Dictionary<string, CaminhoDinheiro> ObterCaminhoDinheiro(IGrouping<string, CaptacaoLiquidaExcelModel> capLiq)
        //    {
        //        var result = new Dictionary<string, CaminhoDinheiro>(); //key = produto

        //        var produtos = Enum.GetNames(typeof(ProutosCap)).ToList();

        //        produtos.Add("OUTROS");

        //        var totalResgate = capLiq.Sum(s => s.TotalResgate);

        //        var totalAplicacoes = capLiq.Sum(s => s.TotalAplicacao);

        //        foreach (var produto in produtos)
        //        {
        //            var caminhoDinheiro = new CaminhoDinheiro();

        //            if (produto != "OUTROS")
        //            {
        //                var totalAplicacoesPorProduto = capLiq.Where(c => c.Produto.StartsWith(produto))
        //                    .Sum(s => s.TotalAplicacao);

        //                var totalResgatesPorProduto = capLiq.Where(c => c.Produto.StartsWith(produto))
        //                    .Sum(s => s.TotalResgate);

        //                //CapLiq
        //                caminhoDinheiro.VolumeCapLiq = totalAplicacoesPorProduto - totalResgatesPorProduto;

        //                //Aplicacaoes
        //                caminhoDinheiro.VolumeAplicacao = totalAplicacoesPorProduto;

        //                caminhoDinheiro.PercNewMoney = capLiq.Where(c => c.Produto.StartsWith(produto)).Sum(s => s.VL_DINHEIRO_NOVO);

        //                //RESGATE
        //                //var somaMSMTIT = capLiq.Select(s => s.MsmTitTotalEnv).Sum();
        //                //var somaDIFTIT = capLiq.Select(s => s.DifTitTotalEnv).Sum();

        //                caminhoDinheiro.VolumeResg = totalResgatesPorProduto;

        //                var capProd = capLiq.Where(s => s.Produto.ToUpper().StartsWith(produto));

        //                if (totalResgate > 0)
        //                {
        //                    caminhoDinheiro.PercFDOS = capProd.Sum(s => s.VL_RESG_FUNDOS);

        //                    caminhoDinheiro.PercCDB = capProd.Sum(s => s.VL_RESG_CDB);

        //                    caminhoDinheiro.PercLF = capProd.Sum(s => s.VL_RESG_LF);

        //                    caminhoDinheiro.PercISENTOS = capProd.Sum(s => s.VL_RESG_ISENTOS);

        //                    caminhoDinheiro.PercCORRET = capProd.Sum(s => s.VL_RESG_CORRET);

        //                    caminhoDinheiro.PercPREVI = capProd.Sum(s => s.VL_RESG_PREVI);

        //                    caminhoDinheiro.PercCOMPR = capProd.Sum(s => s.VL_RESG_COMPROMISSADAS);

        //                    //caminhoDinheiro.PercINVEST = ObterValorPorProduto(capLiq, "INVEST FACIL", "INVEST PLUS") / totalResgate;

        //                    //caminhoDinheiro.PercTEDSENV = (somaMSMTIT + somaDIFTIT) / totalResgate;

        //                }
        //            }
        //            else
        //            {
        //                var produtosExcluidos = produtos.Where(p => p != produto);

        //                var totalAplicacoesPorProduto = capLiq.Where(c => !produtosExcluidos.Contains(c.Produto))
        //                    .Sum(s => s.TotalAplicacao);

        //                var totalResgatesPorProduto = capLiq.Where(c => !produtosExcluidos.Contains(c.Produto))
        //                    .Sum(s => s.TotalResgate);

        //                //CapLiq
        //                caminhoDinheiro.VolumeCapLiq = totalAplicacoesPorProduto - totalResgatesPorProduto;

        //                //Aplicacaoes
        //                caminhoDinheiro.VolumeAplicacao = totalAplicacoesPorProduto;

        //                caminhoDinheiro.PercNewMoney = capLiq.Where(c => !produtosExcluidos.Contains(c.Produto)).Sum(s => s.VL_DINHEIRO_NOVO);

        //                //RESGATE
        //                //var somaMSMTIT = capLiq.Select(s => s.MsmTitTotalEnv).Sum();
        //                //var somaDIFTIT = capLiq.Select(s => s.DifTitTotalEnv).Sum();

        //                caminhoDinheiro.VolumeResg = totalResgatesPorProduto;

        //                var capProd = capLiq.Where(s => !produtosExcluidos.Contains(s.Produto));

        //                if (totalResgate > 0)
        //                {
        //                    caminhoDinheiro.PercFDOS = capProd.Sum(s => s.VL_RESG_FUNDOS);

        //                    caminhoDinheiro.PercCDB = capProd.Sum(s => s.VL_RESG_CDB);

        //                    caminhoDinheiro.PercLF = capProd.Sum(s => s.VL_RESG_LF);

        //                    caminhoDinheiro.PercISENTOS = capProd.Sum(s => s.VL_RESG_ISENTOS);

        //                    caminhoDinheiro.PercCORRET = capProd.Sum(s => s.VL_RESG_CORRET);

        //                    caminhoDinheiro.PercPREVI = capProd.Sum(s => s.VL_RESG_PREVI);

        //                    caminhoDinheiro.PercCOMPR = capProd.Sum(s => s.VL_RESG_COMPROMISSADAS);

        //                    //caminhoDinheiro.PercINVEST = ObterValorPorProduto(capLiq, "INVEST FACIL", "INVEST PLUS") / totalResgate;

        //                    //caminhoDinheiro.PercTEDSENV = (somaMSMTIT + somaDIFTIT) / totalResgate;

        //                }
        //            }


        //            //var calcOutrosPt1 = totalAplicacoes + somaDIFTIT + somaMSMTIT;

        //            //caminhoDinheiro.PercOUTROS =
        //            //    totalResgatesPorProduto > 0 ? (totalAplicacoesPorProduto - calcOutrosPt1) / totalResgatesPorProduto : totalResgatesPorProduto;

        //            result.Add(produto, caminhoDinheiro);
        //        }

        //        return result;
        //    }
        //}



        #region versaoAntiga
        // internal enum ProutosCap
        //    {
        //        CDB,
        //        FUNDOS,
        //        COMPROMISSADA,
        //        ISENTOS,
        //        CORRETORA,
        //        PREVI,
        //        INVEST
        //    }

        #endregion
    }
}