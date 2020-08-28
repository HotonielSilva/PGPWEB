using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BradescoPGP.Web.Services
{
    public class CaptacaoLiquidaExcelService
    {
        public byte[] GerarExcelCaptacaoLiquida(ref List<IGrouping<string, CaptacaoLiquida>> dados, ref List<CaminhoDinheiroModel> dadosCaminhoDinheiro, ref List<CaminhoDinheiro> dadosCaminhoDinheiroAnalitico ,string nomePlanilha, string cargo)
        {
            using (var file = new ExcelPackage())
            {
                var wk = file.Workbook;

                var mesDataBase = DateTime.Now.ToString("MMMM yyyy");

                wk.Worksheets.Add("Captação Liquida");

                var planCap = wk.Worksheets["Captação Liquida"];

                if(dados.Count == 0)
                {
                    planCap.Cells["G1"].Value = "Nenhum resultado encontrado";
                    planCap.Cells["G1"].Style.Font.Color.SetColor(Color.Red);
                    planCap.Cells["G1"].Style.Font.Bold =true;
                    planCap.Cells["G1:N1"].Merge = true;

                    return file.GetAsByteArray();
                }

                var produtos = ObterProdutos();

                GerarCabecalhoCap(planCap, produtos);

                CarregarDados(dados, planCap, cargo);

                if (cargo == NivelAcesso.Especialista.ToString())
                {
                    wk.Worksheets.Add("Dados CaptaçãoLiquida");

                    var planCapDados = wk.Worksheets["Dados CaptaçãoLiquida"];

                    GerarCabecalhoCapDados(planCapDados);

                    CarregarDadosGeralCap(planCapDados, ref dados);
                }

                /***************** Cainho Dinheiro *************************/
                wk.Worksheets.Add("Caminho Dinheiro");

                var planCamDin = wk.Worksheets["Caminho Dinheiro"];

                GerarCabecalhoCamDin(planCamDin);

                CarregarDadosCaminhoDinheiro(dadosCaminhoDinheiro, planCamDin);

                wk.Worksheets.Add("Caminho Dinheiro Analitico");

                var planCamDinAnalitico = wk.Worksheets["Caminho Dinheiro Analitico"];

                GerarCabecalhoCamDinAnalitico(planCamDinAnalitico);

                CarregarDadosCamDinAnalitico(dadosCaminhoDinheiroAnalitico, planCamDinAnalitico);

                Formatar(planCap ,planCamDin, produtos, dados.Count);

                

                return file.GetAsByteArray();
            }
        }

        private void GerarCabecalhoCamDinAnalitico(ExcelWorksheet planCamDinAnalitico)
        {
            var propeiedades = typeof(CaminhoDinheiro).GetProperties().Where(s => !s.Name.Contains("Id")).Select(s => s.Name).ToArray();

            var idx = 1;

            foreach (var col in propeiedades)
            {
                planCamDinAnalitico.Cells[1, idx].Value = col;
                idx++;
            }
        }

        private void CarregarDadosCamDinAnalitico(List<CaminhoDinheiro> dadosCaminhoDinheiroAnalitico, ExcelWorksheet planCamDinAnalitico)
        {
            var linha = 2;

            foreach (var dado in dadosCaminhoDinheiroAnalitico)
            {
                planCamDinAnalitico.Cells[linha, 1].Value = dado.MesDataBase;
                planCamDinAnalitico.Cells[linha, 2].Value = dado.Agencia;
                planCamDinAnalitico.Cells[linha, 3].Value = dado.Ag_Conta;
                planCamDinAnalitico.Cells[linha, 4].Value = dado.Segmento;
                planCamDinAnalitico.Cells[linha, 5].Value = dado.Produto;
                planCamDinAnalitico.Cells[linha, 6].Value = dado.VL_APLIC;
                planCamDinAnalitico.Cells[linha, 7].Value = dado.VL_RESG;
                planCamDinAnalitico.Cells[linha, 8].Value = dado.PERC_DINHEIRO_NOVO;
                planCamDinAnalitico.Cells[linha, 9].Value = dado.PERC_CDB;
                planCamDinAnalitico.Cells[linha, 10].Value = dado.PERC_ISENTOS;
                planCamDinAnalitico.Cells[linha, 11].Value = dado.PERC_COMPROMISSADAS;
                planCamDinAnalitico.Cells[linha, 12].Value = dado.PERC_LF;
                planCamDinAnalitico.Cells[linha, 13].Value = dado.PERC_FUNDOS;
                planCamDinAnalitico.Cells[linha, 14].Value = dado.PERC_CORRET;
                planCamDinAnalitico.Cells[linha, 15].Value = dado.PERC_PREVI;
                planCamDinAnalitico.Cells[linha, 16].Value = dado.PERC_MSM_TIT_ENV_TOTAL;
                planCamDinAnalitico.Cells[linha, 17].Value = dado.PERC_DIF_TIT_ENV_TOTAL;
                planCamDinAnalitico.Cells[linha, 18].Value = dado.PERC_OUTROS;
                planCamDinAnalitico.Cells[linha, 19].Value = int.Parse(dado.MatriculaConsultor);
                planCamDinAnalitico.Cells[linha, 20].Value = dado.Consultor;
                planCamDinAnalitico.Cells[linha, 21].Value = int.Parse(dado.MatriculaCordenador);
                planCamDinAnalitico.Cells[linha, 22].Value = dado.Cordenador;
                planCamDinAnalitico.Cells[linha, 8, linha, 18].Style.Numberformat.Format = "0%";
                planCamDinAnalitico.Cells[linha, 6].Style.Numberformat.Format = "#.##0,00";
                planCamDinAnalitico.Cells[linha, 7].Style.Numberformat.Format = "#.##0,00";

                linha++;

            }
        }

        

        private string[] ObterProdutos()
        {
            return new string[] { "CDB", "CORRETORA", "FUNDOS", "ISENTOS", "LF", "PREVIDENCIA", "INVESTS" };
        }

        private void Formatar(ExcelWorksheet planCap, ExcelWorksheet planCamDin, string[] props, int toRow)
        {
            var addrs = planCap.Dimension.Address;

            planCap.Cells[addrs].Style.Border.BorderFull();
            planCap.Cells[addrs].AutoFitColumns();
            planCap.Cells[2, 1, 2, planCap.Dimension.Columns].AutoFilter = true;
            planCap.Cells[2, 1, 2, planCap.Dimension.Columns].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            planCap.View.FreezePanes(3, 1);
            planCap.Cells[2, 1, planCap.Dimension.Rows, planCap.Dimension.Columns].Style.Numberformat.Format = "#,##0.00";

            planCap.Cells[planCap.Dimension.Rows, 1, planCap.Dimension.Rows, planCap.Dimension.Columns].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            planCap.Cells[planCap.Dimension.Rows, 1, planCap.Dimension.Rows, planCap.Dimension.Columns].Style.Fill.BackgroundColor.SetColor(10, 189, 215, 238);
            planCap.Cells[planCap.Dimension.Rows, planCap.Dimension.Columns].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            planCap.Cells[planCap.Dimension.Rows, planCap.Dimension.Columns].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 0);
            planCap.Cells[planCap.Dimension.Rows, planCap.Dimension.Columns - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            planCap.Cells[planCap.Dimension.Rows, planCap.Dimension.Columns - 1].Style.Fill.BackgroundColor.SetColor(0, 255, 255, 0);


            addrs = planCamDin.Dimension.Address;
            planCamDin.Cells[addrs].Style.Border.BorderFull();
            planCamDin.Cells[addrs].AutoFitColumns();

            if (planCamDin.Dimension.Rows > 3)
            {
                planCamDin.Cells[planCamDin.Cells[4, 4, planCamDin.Dimension.Rows, 4].Address].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planCamDin.Cells[planCamDin.Cells[4, 4, planCamDin.Dimension.Rows, 4].Address].Style.Fill.BackgroundColor.SetColor(10, 180, 198, 231);

                planCamDin.Cells[planCamDin.Cells[4, 5, planCamDin.Dimension.Rows, 6].Address].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planCamDin.Cells[planCamDin.Cells[4, 5, planCamDin.Dimension.Rows, 6].Address].Style.Fill.BackgroundColor.SetColor(10, 198, 224, 180);

                planCamDin.Cells[planCamDin.Cells[4, 7, planCamDin.Dimension.Rows, planCamDin.Dimension.Columns].Address].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planCamDin.Cells[planCamDin.Cells[4, 7, planCamDin.Dimension.Rows, planCamDin.Dimension.Columns].Address].Style.Fill.BackgroundColor.SetColor(10, 248, 203, 173);
                planCamDin.Cells[4, 1, planCamDin.Dimension.Rows, planCamDin.Dimension.Columns].Style.Numberformat.Format = "#,##0.00";

            }

            planCamDin.Cells[3, 1, 3, planCamDin.Dimension.Columns].AutoFilter = true;
            planCamDin.Cells[2, 1, 3, planCamDin.Dimension.Columns].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            planCamDin.View.FreezePanes(4, 1);

        }

        private void CarregarDados(List<IGrouping<string, CaptacaoLiquida>> dados, ExcelWorksheet planCap /*ExcelWorksheet planCamDin*/, string cargo)
        {
            var linhaCap = 3;
            //var linhaCamDin = 4;
            var produtosInvest = new string[] { "INVEST FÁCIL", "INVEST PLUS" };
            var produtos = ObterProdutos();
            var col = 1;
            var diretoriaProcessada = 0;
            var volumeGeral = new Dictionary<string, decimal>();
            var volumeGeralMaster = new Dictionary<string, decimal>();
            var key = "TotalProd";

            /************************ CAPTACAO LIQUIDA ***********************************/
            if (cargo == NivelAcesso.Master.ToString())
            {

                foreach (var cordenador in dados)
                {
                    volumeGeral.Clear();

                    col = 1;

                    diretoriaProcessada++;

                    planCap.Cells[linhaCap, col].Value = cordenador.Key;

                    for (int i = 0; i < produtos.Length; i++)
                    {
                        col++;
                        //Soma por produto
                        var valorProd = cordenador.Where(s => s.Produto.StartsWith(produtos[i])).Sum(s => s.ValorNET);
                        planCap.Cells[linhaCap, col].Value = valorProd;

                        //Guarda montante dos produtos
                        if (!volumeGeralMaster.ContainsKey(produtos[i]))
                            volumeGeralMaster.Add(produtos[i], valorProd);
                        else
                            volumeGeralMaster[produtos[i]] += valorProd;
                    }

                    //Soma Outros
                    var valorOutros = cordenador.Where(s => !produtos
                    .Contains(s.Produto.Substring(0, s.Produto.IndexOf(" ") > 0 ? s.Produto.IndexOf(" ") : s.Produto.Length)))
                    .Sum(s => s.ValorNET);

                    planCap.Cells[linhaCap, produtos.Length + 2].Value = valorOutros;

                    if (!volumeGeralMaster.ContainsKey("OUTROS"))
                        volumeGeralMaster.Add("OUTROS", valorOutros);
                    else
                        volumeGeralMaster["OUTROS"] += valorOutros;

                    //Soma Total 
                    planCap.Cells[linhaCap, produtos.Length + 3].Value = cordenador.Sum(s => s.ValorNET);
                    planCap.Cells[linhaCap, produtos.Length + 4].Value = cordenador.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);

                    planCap.Row(linhaCap).Style.Font.Bold = true;

                    //Guarda montante geral diretoria
                    if (!volumeGeralMaster.ContainsKey(key))
                        volumeGeralMaster.Add(key, cordenador.Sum(s => s.ValorNET));
                    else
                        volumeGeralMaster[key] += cordenador.Sum(s => s.ValorNET);


                    if (!volumeGeralMaster.ContainsKey("TOTAL_SEM_INVEST"))
                        volumeGeralMaster.Add("TOTAL_SEM_INVEST", cordenador.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET));
                    else
                        volumeGeralMaster["TOTAL_SEM_INVEST"] += cordenador.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);


                    planCap.Cells[linhaCap, 1, linhaCap, planCap.Dimension.Columns].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    planCap.Cells[linhaCap, 1, linhaCap, planCap.Dimension.Columns].Style.Fill.BackgroundColor.SetColor(5, 255, 242, 204);

                    linhaCap++;

                    /************************ DIRETORIA *****************************/

                    var dadosProDiretoria = cordenador.GroupBy(s => s.Diretoria).ToList();

                    foreach (var dado in dadosProDiretoria)
                    {
                        col = 1;

                        var dadosGer = dado.GroupBy(s => s.GerenciaRegional).ToList();

                        //Nome Diretoria
                        planCap.Cells[linhaCap, col].Value = dado.Key;

                        for (int i = 0; i < produtos.Length; i++)
                        {
                            col++;
                            //Soma por produto
                            var valorProd = dado.Where(s => s.Produto.StartsWith(produtos[i])).Sum(s => s.ValorNET);
                            planCap.Cells[linhaCap, col].Value = valorProd;

                            //Guarda montante dos produtos
                            if (!volumeGeral.ContainsKey(produtos[i]))
                                volumeGeral.Add(produtos[i], valorProd);
                            else
                                volumeGeral[produtos[i]] += valorProd;
                        }

                        //Soma Outros
                        var valorOutrosmaster = dado.Where(s => !produtos
                        .Contains(s.Produto.Substring(0, s.Produto.IndexOf(" ") > 0 ? s.Produto.IndexOf(" ") : s.Produto.Length)))
                        .Sum(s => s.ValorNET);


                        planCap.Cells[linhaCap, produtos.Length + 2].Value = valorOutrosmaster;

                        if (!volumeGeral.ContainsKey("OUTROS"))
                            volumeGeral.Add("OUTROS", valorOutrosmaster);
                        else
                            volumeGeral["OUTROS"] += valorOutrosmaster;

                        //Soma Total da Diretoria
                        planCap.Cells[linhaCap, produtos.Length + 3].Value = dado.Sum(s => s.ValorNET);
                        planCap.Cells[linhaCap, produtos.Length + 4].Value = dado.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);

                        planCap.Row(linhaCap).Style.Font.Bold = true;

                        //Guarda montante geral diretoria
                        if (!volumeGeral.ContainsKey(key))
                            volumeGeral.Add(key, dado.Sum(s => s.ValorNET));
                        else
                            volumeGeral[key] += dado.Sum(s => s.ValorNET);


                        if (!volumeGeral.ContainsKey("TOTAL_SEM_INVEST"))
                            volumeGeral.Add("TOTAL_SEM_INVEST", dado.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET));
                        else
                            volumeGeral["TOTAL_SEM_INVEST"] += dado.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);

                        planCap.Row(linhaCap).OutlineLevel = diretoriaProcessada;
                        planCap.Row(linhaCap).Collapsed = true;

                        /************************* Gerencias ************************************************/
                        linhaCap++;

                        foreach (var regi in dadosGer)
                        {
                            col = 1;

                            var regiEspecialista = regi.GroupBy(s => s.Consultor).ToList();

                            planCap.Cells[linhaCap, col].Value = regi.Key.PadLeft(regi.Key.Length + 4, ' ');

                            for (int i = 0; i < produtos.Length; i++)
                            {
                                col++;
                                //Soma por produto
                                planCap.Cells[linhaCap, col].Value = regi.Where(s => s.Produto.StartsWith(produtos[i])).Sum(s => s.ValorNET);
                            }

                            planCap.Cells[linhaCap, produtos.Length + 2].Value =
                                regi
                                .Where(s => !produtos
                                .Contains(s.Produto.Substring(0, s.Produto.IndexOf(" ") > 0 ? s.Produto.IndexOf(" ") : s.Produto.Length)))
                                .Sum(s => s.ValorNET);

                            planCap.Cells[linhaCap, produtos.Length + 3].Value = regi.Sum(s => s.ValorNET);
                            planCap.Cells[linhaCap, produtos.Length + 4].Value = regi.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);

                            planCap.Row(linhaCap).OutlineLevel = diretoriaProcessada;
                            planCap.Row(linhaCap).Collapsed = true;

                            linhaCap++;

                            /************************ ESPECIALISTA *************************************************/

                            foreach (var espec in regiEspecialista)
                            {
                                col = 1;

                                planCap.Cells[linhaCap, col].Value = espec.Key.PadLeft(espec.Key.Length + 8, ' ');

                                for (int i = 0; i < produtos.Length; i++)
                                {
                                    col++;
                                    //Soma por produto
                                    planCap.Cells[linhaCap, col].Value = espec.Where(s => s.Produto.StartsWith(produtos[i])).Sum(s => s.ValorNET);
                                }

                                //Soma outros
                                planCap.Cells[linhaCap, produtos.Length + 2].Value =
                                espec
                                    .Where(s => !produtos
                                    .Contains(s.Produto.Substring(0, s.Produto.IndexOf(" ") > 0 ? s.Produto.IndexOf(" ") : s.Produto.Length)))
                                    .Sum(s => s.ValorNET);

                                planCap.Cells[linhaCap, produtos.Length + 3].Value = espec.Sum(s => s.ValorNET);
                                planCap.Cells[linhaCap, produtos.Length + 4].Value = espec.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);

                                planCap.Row(linhaCap).OutlineLevel = diretoriaProcessada;
                                planCap.Row(linhaCap).Collapsed = true;

                                linhaCap++;
                            }

                        }
                    }
                }

                col = 1;

                planCap.Cells[linhaCap, col].Value = "TOTAL";

                for (int i = 0; i < produtos.Length; i++)
                {
                    col++;

                    //Soma por produto
                    planCap.Cells[linhaCap, col].Value = volumeGeralMaster[produtos[i]];
                }

                planCap.Cells[linhaCap, produtos.Length + 2].Value = volumeGeralMaster["OUTROS"];
                planCap.Cells[linhaCap, produtos.Length + 3].Value = volumeGeralMaster[key];
                planCap.Cells[linhaCap, produtos.Length + 4].Value = volumeGeralMaster["TOTAL_SEM_INVEST"];

            }
            else
            {
                foreach (var dado in dados)
                {
                    col = 1;

                    diretoriaProcessada++;

                    var dadosGer = dado.GroupBy(s => s.GerenciaRegional).ToList();

                    //Nome Diretoria
                    planCap.Cells[linhaCap, col].Value = dado.Key;

                    for (int i = 0; i < produtos.Length; i++)
                    {
                        col++;
                        //Soma por produto
                        var valorProd = dado.Where(s => s.Produto.StartsWith(produtos[i])).Sum(s => s.ValorNET);
                        planCap.Cells[linhaCap, col].Value = valorProd;

                        //Guarda montante dos produtos
                        if (!volumeGeral.ContainsKey(produtos[i]))
                            volumeGeral.Add(produtos[i], valorProd);
                        else
                            volumeGeral[produtos[i]] += valorProd;
                    }

                    //Soma Outros

                    var valorOutros = dado.Where(s => !produtos
                    .Contains(s.Produto.Substring(0, s.Produto.IndexOf(" ") > 0 ? s.Produto.IndexOf(" ") : s.Produto.Length)))
                    .Sum(s => s.ValorNET);


                    planCap.Cells[linhaCap, produtos.Length + 2].Value = valorOutros;

                    if (!volumeGeral.ContainsKey("OUTROS"))
                        volumeGeral.Add("OUTROS", valorOutros);
                    else
                        volumeGeral["OUTROS"] += valorOutros;

                    //Soma Total da Diretoria
                    planCap.Cells[linhaCap, produtos.Length + 3].Value = dado.Sum(s => s.ValorNET);
                    planCap.Cells[linhaCap, produtos.Length + 4].Value = dado.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);

                    planCap.Row(linhaCap).Style.Font.Bold = true;

                    //Guarda montante geral diretoria
                    if (!volumeGeral.ContainsKey(key))
                        volumeGeral.Add(key, dado.Sum(s => s.ValorNET));
                    else
                        volumeGeral[key] += dado.Sum(s => s.ValorNET);


                    if (!volumeGeral.ContainsKey("TOTAL_SEM_INVEST"))
                        volumeGeral.Add("TOTAL_SEM_INVEST", dado.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET));
                    else
                        volumeGeral["TOTAL_SEM_INVEST"] += dado.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);

                    /************************* Gerencias ************************************************/
                    linhaCap++;

                    foreach (var regi in dadosGer)
                    {
                        col = 1;

                        var regiEspecialista = regi.GroupBy(s => s.Consultor).ToList();

                        planCap.Cells[linhaCap, col].Value = regi.Key.PadLeft(regi.Key.Length + 4, ' ');

                        for (int i = 0; i < produtos.Length; i++)
                        {
                            col++;
                            //Soma por produto
                            planCap.Cells[linhaCap, col].Value = regi.Where(s => s.Produto.StartsWith(produtos[i])).Sum(s => s.ValorNET);
                        }

                        planCap.Cells[linhaCap, produtos.Length + 2].Value =
                            regi
                            .Where(s => !produtos
                            .Contains(s.Produto.Substring(0, s.Produto.IndexOf(" ") > 0 ? s.Produto.IndexOf(" ") : s.Produto.Length)))
                            .Sum(s => s.ValorNET);

                        planCap.Cells[linhaCap, produtos.Length + 3].Value = regi.Sum(s => s.ValorNET);
                        planCap.Cells[linhaCap, produtos.Length + 4].Value = regi.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);

                        planCap.Row(linhaCap).OutlineLevel = diretoriaProcessada;
                        planCap.Row(linhaCap).Collapsed = true;

                        linhaCap++;

                        /************************ ESPECIALISTA *************************************************/

                        foreach (var espec in regiEspecialista)
                        {
                            col = 1;

                            planCap.Cells[linhaCap, col].Value = espec.Key.PadLeft(espec.Key.Length + 8, ' ');

                            for (int i = 0; i < produtos.Length; i++)
                            {
                                col++;
                                //Soma por produto
                                planCap.Cells[linhaCap, col].Value = espec.Where(s => s.Produto.StartsWith(produtos[i])).Sum(s => s.ValorNET);
                            }

                            //Soma outros
                            planCap.Cells[linhaCap, produtos.Length + 2].Value =
                            espec
                                .Where(s => !produtos
                                .Contains(s.Produto.Substring(0, s.Produto.IndexOf(" ") > 0 ? s.Produto.IndexOf(" ") : s.Produto.Length)))
                                .Sum(s => s.ValorNET);

                            planCap.Cells[linhaCap, produtos.Length + 3].Value = espec.Sum(s => s.ValorNET);
                            planCap.Cells[linhaCap, produtos.Length + 4].Value = espec.Where(s => !s.Produto.StartsWith("INVEST")).Sum(s => s.ValorNET);

                            planCap.Row(linhaCap).OutlineLevel = diretoriaProcessada;
                            planCap.Row(linhaCap).Collapsed = true;

                            linhaCap++;
                        }

                    }
                }

                col = 1;

                planCap.Cells[linhaCap, col].Value = "TOTAL";

                for (int i = 0; i < produtos.Length; i++)
                {
                    col++;

                    //Soma por produto
                    planCap.Cells[linhaCap, col].Value = volumeGeral[produtos[i]];
                }

                planCap.Cells[linhaCap, produtos.Length + 2].Value = volumeGeral["OUTROS"];
                planCap.Cells[linhaCap, produtos.Length + 3].Value = volumeGeral[key];
                planCap.Cells[linhaCap, produtos.Length + 4].Value = volumeGeral["TOTAL_SEM_INVEST"];
                planCap.Row(linhaCap).Style.Font.Bold = true;
            }
        }

        private void GerarCabecalhoCap(ExcelWorksheet plan, string[] produtos)
        {
            plan.Cells[1, 1, 1, produtos.Length + 4].Merge = true;
            plan.Cells[1, 1, 1, produtos.Length + 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            plan.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            plan.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(2, 0, 0, 0);
            plan.Cells[1, 1].Style.Font.Color.SetColor(0, 255, 255, 255);
            plan.Cells[1, 1].Value = "Captação Liquida";

            plan.Cells[2, 1].Value = "Rotulo";

            var col = 2;

            foreach (var prod in produtos)
            {
                plan.Cells[2, col].Value = prod;
                col++;
            }

            plan.Cells[2, produtos.Length + 2].Value = "OUTROS";
            plan.Cells[2, produtos.Length + 3].Value = "Total";
            plan.Cells[2, produtos.Length + 4].Value = "Total Sem Invest";
        }

        private void GerarCabecalhoCamDin(ExcelWorksheet plan)
        {
            var props = typeof(CaminhoDinheiroModel).GetProperties().Select(s => s.Name).ToArray();

            plan.Cells[1, 1, 1, props.Count()].Merge = true;
            plan.Cells[1, 1, 1, props.Count()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            plan.Cells[1, 1, 1, props.Count()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            plan.Cells[1, 1, 1, props.Count()].Style.Fill.BackgroundColor.SetColor(2, 0, 0, 0);
            plan.Cells[1, 1, 1, props.Count()].Style.Font.Color.SetColor(0, 255, 255, 255);
            plan.Cells[1, 1].Value = "Caminho do Dinheiro";

            plan.Cells[2, 1, 2, 3].Merge = true;
            plan.Cells[2, 1, 2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            plan.Cells[2, 4].Value = "Cap. Liq";
            plan.Cells[2, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            plan.Cells[2, 4].Style.Fill.BackgroundColor.SetColor(10, 180, 198, 231);

            plan.Cells[2, 5, 2, 6].Merge = true;
            plan.Cells[2, 5, 2, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            plan.Cells[2, 5].Value = "Aplicações";
            plan.Cells[2, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            plan.Cells[2, 5].Style.Fill.BackgroundColor.SetColor(10, 198, 224, 180);

            plan.Cells[2, 7, 2, props.Count()].Merge = true;
            plan.Cells[2, 7, 2, props.Count()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            plan.Cells[2, 7].Value = "Resgates";
            plan.Cells[2, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            plan.Cells[2, 7].Style.Fill.BackgroundColor.SetColor(10, 248, 203, 173);


            plan.Cells[3, 1].Value = "Especialista";
            plan.Cells[3, 2].Value = "Produto";

            var col = 1;

            foreach (var property in props)
            {
                var color = Color.FromArgb(10, 180, 198, 231);

                if (col >= 5 && col <= 6)
                {
                    color = Color.FromArgb(10, 198, 224, 180);
                }
                else if (col >= 7)
                {
                    color = Color.FromArgb(10, 248, 203, 173);
                }

                var proprie = property.StartsWith("Volu") ? "Volume" : property;

                if (property.StartsWith("Perc"))
                    proprie = property.Replace("Perc", "%");

                plan.Cells[3, col].Value = proprie;

                if(col >= 4)
                {
                    plan.Cells[3, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    plan.Cells[3, col].Style.Fill.BackgroundColor.SetColor(color);
                }

                col++;
            }

        }

        private void CarregarDadosGeralCap(ExcelWorksheet planCapDados, ref List<IGrouping<string, CaptacaoLiquida>> dados)
        {
            var linha = 2;

            var caps = new List<CaptacaoLiquida>();

            foreach (var group in dados)
            {
                caps.AddRange(group);
            }

            var totalRegistros = caps.Count;

            for (int i = 0; i < totalRegistros; i++)
            {
                //if (linha == 1048576 && caps.Count + 1 >= 1048576)
                //{
                //    caps.RemoveRange(0, i - 1);

                //    break;
                //}

                var dado = caps[i];

                planCapDados.Cells[linha, 1].Value = int.Parse(dado.Agencia);
                planCapDados.Cells[linha, 2].Value = long.Parse(dado.Conta);
                planCapDados.Cells[linha, 3].Value = dado.Ag_Conta;
                planCapDados.Cells[linha, 4].Value = dado.CodAgencia;
                planCapDados.Cells[linha, 5].Value = dado.MesDataBase;
                planCapDados.Cells[linha, 6].Value = dado.Diretoria;
                planCapDados.Cells[linha, 7].Value = dado.TipoPessoa;
                planCapDados.Cells[linha, 8].Value = dado.GerenciaRegional;
                planCapDados.Cells[linha, 9].Value = dado.CordenadorPGP;
                planCapDados.Cells[linha, 10].Value = dado.Consultor;
                planCapDados.Cells[linha, 11].Value = dado.DataBase;
                planCapDados.Cells[linha, 11].Style.Numberformat.Format = "dd/mm/yyyy";
                planCapDados.Cells[linha, 12].Value = dado.Produto;
                planCapDados.Cells[linha, 13].Value = dado.ValorAplicacao;
                planCapDados.Cells[linha, 13].Style.Numberformat.Format = "#,##0.00";
                planCapDados.Cells[linha, 14].Value = dado.ValorResgate;
                planCapDados.Cells[linha, 14].Style.Numberformat.Format = "#,##0.00";
                planCapDados.Cells[linha, 15].Value = dado.ValorNET;
                planCapDados.Cells[linha, 15].Style.Numberformat.Format = "#,##0.00";

                linha++;
            }

            var adds = planCapDados.Dimension.Address;
            planCapDados.Cells[adds].Style.Border.BorderFull();
            planCapDados.Cells[adds].AutoFitColumns();

            //if (linha == 1048576 && caps.Count > 0)
            //{
            //    var planCapDadosContinuação = planCapDados.Workbook.Worksheets.Add(planCapDados.Name + "- Continuação");

            //    totalRegistros = caps.Count;

            //    planCapDados.Cells[1, 1, 1, planCapDados.Dimension.Columns].Copy(planCapDadosContinuação.Cells["A1"]);

            //    linha = 2;

            //    for (int i = 0; i < totalRegistros; i++)
            //    {
            //        if (linha == 1048576 && caps.Count + 1 >= 1048576)
            //        {
            //            break;
            //        }

            //        var dado = caps[i];

            //        planCapDados.Cells[linha, 1].Value = int.Parse(dado.Agencia);
            //        planCapDados.Cells[linha, 2].Value = long.Parse(dado.Conta);
            //        planCapDados.Cells[linha, 3].Value = dado.Ag_Conta;
            //        planCapDados.Cells[linha, 4].Value = dado.CodAgencia;
            //        planCapDados.Cells[linha, 5].Value = dado.MesDataBase;
            //        planCapDados.Cells[linha, 6].Value = dado.Diretoria;
            //        planCapDados.Cells[linha, 7].Value = dado.TipoPessoa;
            //        planCapDados.Cells[linha, 8].Value = dado.GerenciaRegional;
            //        planCapDados.Cells[linha, 9].Value = dado.CordenadorPGP;
            //        planCapDados.Cells[linha, 10].Value = dado.Consultor;
            //        planCapDados.Cells[linha, 11].Value = dado.DataBase;
            //        planCapDados.Cells[linha, 11].Style.Numberformat.Format = "mm/d/yyyy";
            //        planCapDados.Cells[linha, 12].Value = dado.Produto;
            //        planCapDados.Cells[linha, 13].Value = dado.ValorAplicacao;
            //        planCapDados.Cells[linha, 13].Style.Numberformat.Format = "#,##0.00";
            //        planCapDados.Cells[linha, 14].Value = dado.ValorResgate;
            //        planCapDados.Cells[linha, 14].Style.Numberformat.Format = "#,##0.00";
            //        planCapDados.Cells[linha, 15].Value = dado.ValorNET;
            //        planCapDados.Cells[linha, 15].Style.Numberformat.Format = "#,##0.00";

            //        linha++;
            //    }
            //}

        }

        private void GerarCabecalhoCapDados(ExcelWorksheet planCapDados)
        {

            var columns = typeof(CaptacaoLiquida).GetProperties()
                .Select(s => s.Name).Where(s => !s.Contains("Id") && !s.StartsWith("Matricula")).ToArray();

            var idx = 1;

            foreach (var col in columns)
            {
                planCapDados.Cells[1, idx].Value = col;
                idx++;
            }
        }

        private void CarregarDadosCaminhoDinheiro(List<CaminhoDinheiroModel> dados, ExcelWorksheet planCamDin)
        {
            var produtos = Enum.GetNames(typeof(ProutosCap));

            var linhaCamDin = 4;

            var result = CaminhoDinheiroModel.Mapear(dados);

            foreach (var espe in result)
            {
                ////Caminho Dinheiro
                foreach (var camDinSub in espe.Value)
                {
                    var camDin = camDinSub.Value;

                    planCamDin.Cells[linhaCamDin, 1].Value = camDin.Cordenador;
                    planCamDin.Cells[linhaCamDin, 2].Value = camDin.Especialista;
                    planCamDin.Cells[linhaCamDin, 3].Value = camDin.Produto;
                    planCamDin.Cells[linhaCamDin, 4].Value = camDin.VolumeCapLiq;
                    planCamDin.Cells[linhaCamDin, 5].Value = camDin.VolumeAplicacao;
                    planCamDin.Cells[linhaCamDin, 6].Value = camDin.PercNewMoney;
                    planCamDin.Cells[linhaCamDin, 7].Value = camDin.VolumeResg;
                    planCamDin.Cells[linhaCamDin, 8].Value = camDin.PercFDOS;
                    planCamDin.Cells[linhaCamDin, 9].Value = camDin.PercCDB;
                    planCamDin.Cells[linhaCamDin, 10].Value = camDin.PercLF;
                    planCamDin.Cells[linhaCamDin, 11].Value = camDin.PercISENTOS;
                    planCamDin.Cells[linhaCamDin, 12].Value = camDin.PercCORRET;
                    planCamDin.Cells[linhaCamDin, 13].Value = camDin.PercPREVI;
                    planCamDin.Cells[linhaCamDin, 14].Value = camDin.PercCOMPR;
                    planCamDin.Cells[linhaCamDin, 15].Value = camDin.PercTEDSENV;
                    planCamDin.Cells[linhaCamDin, 16].Value = camDin.PercOUTROS;

                    linhaCamDin++;
                }
            }
            
        }

    }
}