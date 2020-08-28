using BradescoPGP.Common;
using BradescoPGP.Common.Logging;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BradescoPGP.Web.Services
{
    public class AgendaService
    {
        public AgendaViewModel ObterAgendaCompleta(string matricula, string perfil)
        {
            var vencimentos = ObterVencimentosAgenda(matricula);
            var pipes = ObterPipeLinesAgenda(matricula);
            var teds = ObterTedsAgenda(matricula);
            var niver = ObterAniversariantesAgenda(matricula, perfil);
            var eventos = ObterEventosAgenda(matricula);

            return AgendaViewModel.Mapear(vencimentos, pipes, teds, niver, eventos);
        }

        public List<Vencimento> ObterVencimentosAgenda(string matricula)
        {
            using (var context = new PGPEntities())
            {
                var minDate = DateTime.Now.AddDays(-90).Date;
                var dataAtual = DateTime.Now.Date;
                int[] statusValidos = new int[] { 5, 6, 7 };

                var vencimentosDia = context.Vencimento
                    .Join(context.Encarteiramento,
                    venc => new { agencia = venc.Cod_Agencia.ToString(), conta = venc.Cod_Conta_Corrente.ToString() },
                    enc => new { agencia = enc.Agencia, conta = enc.Conta },
                    (venc, enc) => new { venc, enc })
                    .Where(v => v.enc.Matricula == matricula && v.venc.StatusId.HasValue && statusValidos.Contains(v.venc.StatusId.Value) && 
                    v.venc.Dt_Vecto_Contratado >= minDate && v.venc.Dt_Vecto_Contratado <= dataAtual )
                    .Select(s => s.venc).ToList();

                return vencimentosDia;
            }
        }

        public List<Pipeline> ObterPipeLinesAgenda(string matricula)
        {
            var statusValidos = new int[] { 3, 4 };

            using (var context = new PGPEntities())
            {
                var minDate= DateTime.Now.Date.AddMonths(-12);
                
                var pipelines = context.Pipeline
                    .Where(p => p.MatriculaConsultor == matricula && statusValidos.Contains(p.StatusId) && 
                    (p.DataProrrogada >= minDate || p.DataPrevista >= minDate ))
                    .ToList();

                return pipelines;
            }
        }

        public List<TED> ObterTedsAgenda(string matricula)
        {
            using (var context = new PGPEntities())
            {
                var statusValidos = context.Status.Where(s => s.Evento.Contains("TED") &&
                    !s.Descricao.Contains("Aplicado") && !s.Descricao.Contains("Finalizado")).Select(s => s.Id).ToList();

                var dados = new List<TED>();
                var ted = context.TED.Include("Motivo").Include("Status").Where(t => t.Area == "PGP");
                var dataAtual = DateTime.Now.Date;
                var minData = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(-1);
                var maxData = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));


                dados = ted.Where(t => 
                    t.MatriculaConsultor == matricula &&  
                    statusValidos.Contains(t.StatusId) &&
                    DbFunctions.TruncateTime(t.Data) >= minData && DbFunctions.TruncateTime(t.Data) <= maxData).ToList();
                
                return dados;
            }
        }

        public List<AniversarianteViewModel> ObterAniversariantesAgenda(string matricula, string perfil)
        {
            if (perfil == NivelAcesso.Especialista.ToString())
            {
                using (var db = new PGPEntities())
                {
                    var data = DateTime.Now.Date;

                    var nivers =  db.Aniversarios.Join(db.Encarteiramento,
                        niver => new { agencia = niver.Agencia.ToString(), conta = niver.Conta.ToString() },
                        enc => new { agencia = enc.Agencia, conta = enc.Conta },
                        (niver, enc) => new { niver, enc.Matricula, enc.Agencia, enc.Conta })
                        .Join(db.Cockpit,
                        res => new { agencia = res.Agencia, conta = res.Conta },
                        cokc => new { agencia = cokc.CodigoAgencia.ToString(), conta = cokc.Conta.ToString() },
                        (resp, cock) => new { resp, cock.CodigoAgencia, cock.Conta, cock.NomeCliente })
                        .Where(r => r.resp.Matricula == matricula && 
                         r.resp.niver.DataNascimento.Day == data.Day && r.resp.niver.DataNascimento.Month == data.Month)
                        .GroupBy(g => new { agencia = g.CodigoAgencia, conta = g.Conta })
                        .Select(s => new AniversarianteViewModel
                        {
                            Agencia = s.FirstOrDefault().CodigoAgencia,
                            Conta = s.FirstOrDefault().Conta,
                            Nome = s.FirstOrDefault().NomeCliente,
                            DataAniversario = s.FirstOrDefault().resp.niver.DataNascimento
                        }).ToList();

                    return nivers; 
                }
            }

            return new List<AniversarianteViewModel>();
        }

        public List<Evento> ObterEventosAgenda(string matricula)
        {
            using (var db = new PGPEntities())
            {
                var dataAtual = DateTime.Now.Date;
                var minDate = DateTime.Now.Date.AddDays(-1);
                var maxDate = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));

                var eventos = db.Evento.Where(e => e.MatriculaConsultor == matricula && 
                //e.DataHoraInicio.Day == dateAtual.Day && e.DataHoraInicio.Month == dateAtual.Month && e.DataHoraInicio.Year == dateAtual.Year || 
                e.DataHoraInicio >= minDate && e.DataHoraInicio <= maxDate ).ToList();

                return eventos;
            }
        }

        public Evento NovoEvento(Evento evento)
        {
            using(var db = new PGPEntities())
            {
                if(evento != null)
                {
                    try
                    {
                        db.Evento.Add(evento);
                        db.SaveChanges();
                        return evento;
                    }
                    catch (Exception e)
                    {
                        Log.Error("Erro ao criar um novo evento", e);
                        return new Evento();
                    }
                }
                else
                {
                    return new Evento();
                }
            }
        }

        public bool ExcluirEvento(int id)
        {
            using (var db = new PGPEntities())
            {
                var evento = db.Evento.First(e => e.Id == id);
                db.Evento.Remove(evento);
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Log.Error("Erro ao deletar evento " + e);
                    return false;
                    throw;
                }
            }
        }
    }
}