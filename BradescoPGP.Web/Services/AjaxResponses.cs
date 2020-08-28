using BradescoPGP.Common.Logging;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace BradescoPGP.Web.Services
{
    public class AjaxResponses
    {
        // Pipeline
        public List<Pipeline> AtualizarPipeline(Pipeline pipeline)
        {

            using (PGPEntities db = new PGPEntities())
            {
                var pipe = db.Pipeline.FirstOrDefault(p => p.Id == pipeline.Id);
                pipe.OrigemId = pipeline.OrigemId;
                pipe.StatusId = pipeline.StatusId;
                pipe.ValorDoPipe = pipeline.ValorDoPipe;
                pipe.ValorAplicado = pipeline.ValorAplicado;
                pipe.Observacoes = pipeline.Observacoes;
                pipe.DataPrevista = pipeline.DataPrevista;
                pipe.ValoresNoMercado = pipeline.ValoresNoMercado;
                pipe.DataDaConversao = pipeline.DataDaConversao ?? pipe.DataDaConversao;
                pipe.DataProrrogada = pipeline.DataProrrogada ?? null;
                pipe.BradescoPrincipalBanco = pipeline.BradescoPrincipalBanco;
                pipe.MotivoId = pipeline.MotivoId;

                db.Entry(pipe).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return new List<Pipeline>();
                }

                return db.Pipeline.Include(p => p.Status).Include(p => p.Origem).Include(p => p.Motivo).Where(p => p.Agencia == pipe.Agencia && p.Conta == pipe.Conta).ToList();
            }
        }

        public Pipeline ObterPipe(int? idPipe)
        {
            if (!idPipe.HasValue) throw new Exception("O parametro 'IdPipe' não pode ser nulo");
            var pipe = new Pipeline();
            using (PGPEntities db = new PGPEntities())
            {
                pipe = db.Pipeline.Include(p => p.Motivo).Include(p => p.Origem).Include(p => p.Status).FirstOrDefault(x => x.Id == idPipe);
                return pipe;
            }
        }
        public bool NovoPipeline(Pipeline pipeline, out List<Pipeline> pipelines)
        {
            using (PGPEntities db = new PGPEntities())
            { 
                db.Pipeline.Add(pipeline);
                
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) 
                {
                    Log.Error("Erro ao incluir Pipeline", e);
                
                    pipelines = null;
                    
                    return false;
                }
                pipelines = db.Pipeline
                    .Include(p => p.Status)
                    .Include(p => p.Origem)
                    .Include(p => p.Motivo)
                    .Where(p => p.Agencia == pipeline.Agencia && p.Conta == pipeline.Conta).ToList(); ;
                
                return true;
            }
        }

        public TEDViewModel ObterTed(int id)
        {
            //var ted = db.TEDs.First(x => x.Id == id);
            //return ted;            

            return default(TEDViewModel);
        }

        public List<TEDViewModel> ObeterTodasTeds()
        {
            //return db.TEDs.ToList();
            return default(List<TEDViewModel>);
        }





























        //private async Task<List<TED>> CriaListaAsync(SqlDataReader reader)
        //{
        //    var dataLista = new List<TED>();
        //    var situacao = new List<string> { "Pendente", "Concluído", "Em Trativa" };
        //    var length = situacao.Count;
        //    while (await reader.ReadAsync())
        //    {
        //        var ted = new TED();

        //        ted.Agencia = (string)reader["Agencia"];
        //        ted.Conta = (string)reader["Conta"];
        //        ted.CPFCPNJ = (string)reader["CPF"];
        //        //ted.Cliente = "Maicon Fagundes";
        //        ted.Data = Convert.ToDateTime(reader["Data"]);
        //        ted.Valor = (decimal)reader["Valor"];
        //        ted.Situacao = situacao[new Random().Next(0, length - 1)];
        //        ted.DataPrevista = DateTime.Now;
        //        //Precisa trazer o compo Id
        //        dataLista.Add(ted);
        //    }
        //    closeConnection();
        //    return dataLista;

        //}
    }
}