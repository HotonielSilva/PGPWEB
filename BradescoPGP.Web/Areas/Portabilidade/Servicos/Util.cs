using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using BradescoPGP.Web.Areas.Portabilidade.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BradescoPGP.Web.Areas.Portabilidade.Servicos
{
    public class Util : IUtil
    {
        private readonly PGPEntities _context;

        public Util(DbContext context)
        {
            _context = context as PGPEntities;
        }
        public byte[] GerarExcelPortabilidade<T>(List<T> dados, string nomePlainlha)
        {
            var tipoDado = typeof(T);

            using (var excel = new ExcelPackage())
            {
                var plan = excel.Workbook.Worksheets.Add(nomePlainlha);

                var row = 1;

                var colunasExcluidas = new string[] {
                        "Id", "StatusId", "SubStatusId","MotivoId","SubMotivoId","SubmotivoId",
                        "PerfilId","DataInclusao"
                    };

                var properties = tipoDado.GetProperties().Where(p => !colunasExcluidas.Contains(p.Name)).Select(s => s.Name).ToArray();
                
                //Cabeçalho
                for (int i = 0; i < properties.Length; i++)
                {
                    plan.Cells[row, i + 1].Value = properties[i];
                }

                //Dados
                foreach (var d in dados)
                {
                    row++;
                    if (tipoDado == typeof(SolicitacaoViewModel))
                    {
                        var solicitacoes = d as SolicitacaoViewModel;

                        plan.Cells[row, 1].Value = solicitacoes.Segmento;
                        plan.Cells[row, 2].Value = solicitacoes.Lideranca;
                        plan.Cells[row, 3].Value = solicitacoes.ConsultorMatriz;
                        plan.Cells[row, 4].Value = solicitacoes.ConsultorPGP;
                        plan.Cells[row, 5].Value = solicitacoes.NomeParticipante;
                        plan.Cells[row, 6].Value = solicitacoes.CPF;
                        plan.Cells[row, 7].Value = solicitacoes.SaldoPrevidencia;
                        plan.Cells[row, 8].Value = solicitacoes.ValorPrevistoSaida;
                        plan.Cells[row, 9].Value = solicitacoes.NomeEntidade;
                        plan.Cells[row, 10].Value = solicitacoes.DataInicioProcesso.ToShortDateString();
                        plan.Cells[row, 11].Value = solicitacoes.PrazoAtendimento?.ToShortDateString();
                        plan.Cells[row, 12].Value = solicitacoes.DataRef?.ToShortDateString() ;
                        plan.Cells[row, 13].Value = solicitacoes.CodigoIdentificadorProcesso;
                        plan.Cells[row, 14].Value = solicitacoes.CodigoIdentificadorProposta;
                        plan.Cells[row, 15].Value = solicitacoes.SusepCedente;
                        plan.Cells[row, 16].Value = solicitacoes.SusepCessionaria;
                        plan.Cells[row, 17].Value = solicitacoes.CidtfdCnpjCdent;
                        plan.Cells[row, 18].Value = solicitacoes.CidtfdCnpjCessionaria;
                        var matricula = default(int?);

                        if (!string.IsNullOrEmpty(solicitacoes.MatriculaConsultor))
                            matricula =  int.Parse(solicitacoes.MatriculaConsultor);

                        plan.Cells[row, 19].Value = matricula;
                        plan.Cells[row, 20].Value = solicitacoes.ValorRetido;
                        plan.Cells[row, 21].Value = solicitacoes.Observacao;
                        plan.Cells[row, 22].Value = solicitacoes.Agencia;
                        plan.Cells[row, 23].Value = solicitacoes.Conta;
                        plan.Cells[row, 24].Value = solicitacoes.DataConclusao?.ToShortDateString();
                        plan.Cells[row, 25].Value = solicitacoes.PrazoFinal?.ToShortDateString();
                        plan.Cells[row, 26].Value = solicitacoes.ContatoAgencia;
                        plan.Cells[row, 27].Value = solicitacoes.DescricaoTipoSolicitacao;
                        plan.Cells[row, 28].Value = solicitacoes.CodigoIdentificadorAgenciaBRA;
                        plan.Cells[row, 29].Value = solicitacoes.Status;
                        plan.Cells[row, 30].Value = solicitacoes.SubStatus;
                        plan.Cells[row, 31].Value = solicitacoes.Motivo;
                        plan.Cells[row, 32].Value = solicitacoes.Submotivo;
                        plan.Cells[row, 33].Value = solicitacoes.Usuario?.Nome;

                    }
                    else if (tipoDado.Name == typeof(RankingViewModel).Name)
                    {
                        var ranking = d as RankingViewModel;

                        plan.Cells[row, 1].Value = ranking.Especialista;
                        plan.Cells[row, 2].Value = ranking.QuantidadeSolicitacoes;
                        plan.Cells[row, 3].Value = ranking.ValorSolicitacoes;
                        plan.Cells[row, 4].Value = ranking.Contatos;
                        plan.Cells[row, 5].Value = ranking.PorcentagemContatos;
                        plan.Cells[row, 6].Value = ranking.QuantidadeRetida;
                        plan.Cells[row, 7].Value = ranking.ValorRetido;
                        plan.Cells[row, 8].Value = Math.Round(ranking.PorcentgemRetida ?? 0, 2);
                    }
                }

                plan.Cells[plan.Dimension.Address].Style.Border.BorderFull();

                return excel.GetAsByteArray();
            }

        }
        public List<Produtividade> ObterProdutividade()
        {
            return _context.Produtividade.ToList();
        }
    }
}