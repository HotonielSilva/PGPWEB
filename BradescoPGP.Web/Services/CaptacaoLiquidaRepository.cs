using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace BradescoPGP.Web.Services
{
    public class CaptacaoLiquidaRepository
    {
        public List<CaptacaoLiquida> ObterCapLiq(string mesDataBase = null, string matriclaConsultor = null, string MatriculaCordenador = null)
        {
            var connStr = ConfigurationManager.ConnectionStrings["PGP"].ConnectionString;

            var retorno = new List<CaptacaoLiquida>();

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (var cmd = new SqlCommand("sp_ObterCapLiq", conn))
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@matriculaConsultor", matriclaConsultor ?? string.Empty);
                    cmd.Parameters.AddWithValue("@matriculaCord", MatriculaCordenador ?? string.Empty);
                    cmd.Parameters.AddWithValue("@mesDataBase", mesDataBase);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            var capLiq = new CaptacaoLiquida();

                            capLiq.Diretoria = rd["Diretoria"]?.ToString();
                            capLiq.GerenciaRegional = rd["GerenciaRegional"]?.ToString();
                            capLiq.Produto = rd["Produto"]?.ToString();
                            capLiq.MatriculaConsultor = rd["MatriculaConsultor"]?.ToString();
                            capLiq.Consultor = rd["Consultor"]?.ToString();
                            capLiq.MatriculaCordenador = rd["MatriculaCordenador"]?.ToString();
                            capLiq.CordenadorPGP = rd["CordenadorPGP"]?.ToString();
                            capLiq.ValorNET = decimal.Parse(rd["ValorNET"]?.ToString() ?? "0");

                            retorno.Add(capLiq);
                        }
                        return retorno;
                    }
                }
            }
        }

        public List<CaptacaoLiquida> ObterDetalheCapLiq(string dataBase, string matriculaConsultor = null, string matrculCordenador = null)
        {
            var connStr = ConfigurationManager.ConnectionStrings["PGP"].ConnectionString;

            var retorno = new List<CaptacaoLiquida>();

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (var cmd = new SqlCommand("sp_ObterDetalheCaptacaoLiquida", conn))
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@matriculaConsultor", matriculaConsultor);
                    cmd.Parameters.AddWithValue("@matriculaCordenador", matrculCordenador);
                    cmd.Parameters.AddWithValue("@mesDataBase", dataBase);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            var capLiq = new CaptacaoLiquida();
                            
                            capLiq.Diretoria = rd["Diretoria"]?.ToString();
                            capLiq.GerenciaRegional = rd["GerenciaRegional"]?.ToString();
                            capLiq.Produto = rd["Produto"]?.ToString();
                            capLiq.MatriculaConsultor = rd["MatriculaConsultor"]?.ToString();
                            capLiq.Consultor = rd["Consultor"]?.ToString();
                            capLiq.MatriculaCordenador = rd["MatriculaCordenador"]?.ToString();
                            capLiq.CordenadorPGP = rd["CordenadorPGP"]?.ToString();
                            capLiq.ValorNET = decimal.Parse(rd["ValorNET"]?.ToString() ?? "0");
                            capLiq.Agencia = rd["Agencia"]?.ToString();
                            capLiq.Conta = rd["Conta"]?.ToString();
                            capLiq.Ag_Conta = rd["Ag_Conta"]?.ToString();
                            capLiq.CodAgencia = rd["CodAgencia"]?.ToString();
                            capLiq.TipoPessoa = rd["TipoPessoa"]?.ToString();
                            if (DateTime.TryParse(rd["DataBase"]?.ToString(), out var dataBaseConvert)) capLiq.DataBase = dataBaseConvert;
                            capLiq.ValorAplicacao = decimal.Parse(rd["ValorAplicacao"]?.ToString() ?? "0");
                            capLiq.ValorResgate = decimal.Parse(rd["ValorResgate"]?.ToString() ?? "0");
                            capLiq.ValorNET = decimal.Parse(rd["ValorNET"]?.ToString() ?? "0");


                            retorno.Add(capLiq);
                        }
                        return retorno;
                    }
                }
            }
        }


        //TODO: Adicionar Mesdatabase para consulta
        public List<vw_CaminhoDinheiroAgrupado> ObterCaminhoDinheiroAgrupado(NivelAcesso nivelAcesso, string matricula, string anoMes)
        {
            using (var db = new PGPEntities())
            {
                var result = default(List<vw_CaminhoDinheiroAgrupado>);

                switch (nivelAcesso)
                {
                    case NivelAcesso.Master:
                        result = db.vw_CaminhoDinheiroAgrupado.Where(s => s.MesDataBase == anoMes).ToList();
                        break;
                    case NivelAcesso.Especialista:
                        result = db.vw_CaminhoDinheiroAgrupado.Where(s => s.MatriculaConsultor == matricula && s.MesDataBase == anoMes).ToList();
                        break;
                    case NivelAcesso.Gestor:
                        result = db.vw_CaminhoDinheiroAgrupado.Where(s => s.MatriculaCordenador == matricula && s.MesDataBase == anoMes).ToList();
                        break;
                }

                return result;
            }
        }


        public List<CaminhoDinheiro> ObterCaminhoDinheiroAnalitico(NivelAcesso nivelAcesso, string matricula, string anoMes)
        {
            using (var db = new PGPEntities())
            {
                var result = default(List<CaminhoDinheiro>);

                switch (nivelAcesso)
                {
                    case NivelAcesso.Master:
                        result = db.CaminhoDinheiro.Where(s => s.MesDataBase == anoMes).ToList();
                        break;
                    case NivelAcesso.Especialista:
                        result = db.CaminhoDinheiro.Where(s => s.MatriculaConsultor == matricula && s.MesDataBase == anoMes).ToList();
                        break;
                    case NivelAcesso.Gestor:
                        result = db.CaminhoDinheiro.Where(s => s.MatriculaCordenador == matricula && s.MesDataBase == anoMes).ToList();
                        break;
                }

                return result;
            }
        }
    }
}