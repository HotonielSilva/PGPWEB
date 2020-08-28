using BradescoPGP.Common;
using BradescoPGP.Common.Logging;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using BradescoPGP.Web.Areas.Portabilidade.Models;
using BradescoPGP.Web.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
namespace BradescoPGP.Web.Areas.Portabilidade.Servicos
{
    public class SolicitacaoService : ISolicitacaoService
    {
        private readonly PGPEntities _context;
        public SolicitacaoService(DbContext context)
        {
            _context = context as PGPEntities;
        }

        public List<Solicitacao> ObterTodas()
        {
            return _context.Solicitacao.Include(s => s.Status)
                .Include(s => s.Motivo).ToList();
        }

        public List<Motivo> ObterMotivos(bool inativos = false)
        {
            if (inativos)
                return _context.Motivo.Include(s => s.SubMotivo).Where(s => s.Evento == "Portabilidade").ToList();

            return _context.Motivo.Include(s => s.SubMotivo).Where(s => s.EmUso.HasValue && s.EmUso.Value && s.Evento == "Portabilidade").ToList();

        }

        public List<Solicitacao> Obter(FiltrosPortabilidade filtros)
        {
            var solicitacoes = default(IQueryable<Solicitacao>);

            var modoFiltro = SetaModoFiltro.ObterModoFiltro(filtros);

            solicitacoes = _context.Solicitacao.Include(s => s.Status)
                .Include(s => s.Motivo);
            switch (modoFiltro)
            {
                case ModoFiltro.CPF:
                    solicitacoes = solicitacoes.Where(s => s.CPF == filtros.CPF);
                    break;
                case ModoFiltro.Nome:
                    solicitacoes = solicitacoes.Where(s => s.NomeParticipante.ToLower().Contains(filtros.Nome.ToLower()));
                    break;
                case ModoFiltro.Especialista:
                    solicitacoes = solicitacoes.Where(s => s.MatriculaConsultor == filtros.Especialista);
                    break;
                case ModoFiltro.Status:
                    solicitacoes = solicitacoes.Where(s => s.StatusId == filtros.Status);
                    break;
                case ModoFiltro.NomeStatus:
                    solicitacoes = solicitacoes.Where(s => s.NomeParticipante.ToLower() == filtros.Nome && s.StatusId == filtros.Status);
                    break;
                case ModoFiltro.CPFStatus:
                    solicitacoes = solicitacoes.Where(s => s.CPF == filtros.CPF && s.StatusId == filtros.Status);
                    break;

            }

           
            //Verifica datas
            if (filtros.De.HasValue && filtros.Ate.HasValue)
            {
                solicitacoes = solicitacoes.Where(s => s.DataInicioProcesso >= filtros.De && s.DataInicioProcesso <= filtros.Ate);
            }
            else
            {
                var dataAtual = DateTime.Now.Date;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = dataAtual.Date;

                solicitacoes = solicitacoes.Where(s => s.DataInicioProcesso >= minDate && s.DataInicioProcesso <= maxDate);
            }

            return solicitacoes.ToList();

        }

        public List<Solicitacao> ObterAVencer(FiltrosPortabilidade filtros)
        {
            var solicitacoes = default(IQueryable<Solicitacao>);

            var modoFiltro = SetaModoFiltro.ObterModoFiltro(filtros);

            solicitacoes = _context.Solicitacao.Include(s => s.Status)
                .Include(s => s.Motivo);
            switch (modoFiltro)
            {
                case ModoFiltro.CPF:
                    solicitacoes = solicitacoes.Where(s => s.CPF == filtros.CPF);
                    break;
                case ModoFiltro.Nome:
                    solicitacoes = solicitacoes.Where(s => s.NomeParticipante.ToLower().Contains(filtros.Nome.ToLower()));
                    break;
                case ModoFiltro.Especialista:
                    solicitacoes = solicitacoes.Where(s => s.MatriculaConsultor == filtros.Especialista);
                    break;
                case ModoFiltro.Status:
                    solicitacoes = solicitacoes.Where(s => s.StatusId == filtros.Status);
                    break;
                case ModoFiltro.NomeStatus:
                    solicitacoes = solicitacoes.Where(s => s.NomeParticipante.ToLower() == filtros.Nome && s.StatusId == filtros.Status);
                    break;
                case ModoFiltro.CPFStatus:
                    solicitacoes = solicitacoes.Where(s => s.CPF == filtros.CPF && s.StatusId == filtros.Status);
                    break;

            }



            //Verifica datas
            if (filtros.De.HasValue && filtros.Ate.HasValue)
            {
                solicitacoes = solicitacoes.Where(s => s.PrazoAtendimento >= filtros.De);
            }
            else
            {
                var dataAtual = DateTime.Now.Date;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var maxDate = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));

                solicitacoes = solicitacoes.Where(s => s.PrazoAtendimento >= minDate && s.PrazoAtendimento <= maxDate);
            }

            return solicitacoes.ToList();

        }
        public List<Solicitacao> ObterAVencer(DateTime inicio, DateTime fim)
        {
            fim = new DateTime(fim.Year,fim.Month, DateTime.DaysInMonth(fim.Year, fim.Month));
            return _context.Solicitacao.Include(s => s.Status)
                .Include(s => s.Motivo)
                .Where(s => s.PrazoAtendimento >= inicio && s.PrazoAtendimento <= fim).ToList();
        }
        public List<Solicitacao> ObterAVencer(DateTime inicio, DateTime fim, string matricula)
        {
            fim = new DateTime(fim.Year,fim.Month, DateTime.DaysInMonth(fim.Year, fim.Month));
            return _context.Solicitacao.Where(s => s.PrazoAtendimento >= inicio && s.PrazoAtendimento <= fim && s.MatriculaConsultor == matricula).ToList();
        }
        public bool AtualizarSolicitacao(OperacionalViewModel solicitacao)
        {

            var soli = _context.Solicitacao.FirstOrDefault(s => s.Id == solicitacao.Id);

            if (soli == null)
            {
                return false;
            }
            else
            {
                soli.MatriculaConsultor =  solicitacao.MatriculaConsultor ?? string.Empty;
                soli.Agencia = solicitacao.Agencia;
                soli.Conta = solicitacao.Conta;
                soli.ValorRetido = solicitacao.ValorRetido;
                soli.StatusId = solicitacao.StatusId;
                soli.MotivoId = solicitacao.MotivoId;
                soli.SubMotivoId = solicitacao.SubMotivoId;
                soli.SubStatusId = solicitacao.SubStatusId;
                soli.Observacao = solicitacao.Observacao;

                try
                {

                    _context.Entry(soli).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Log.Error("Não foi possivel atualizar solicitação", e);
                    return false;
                }
            }
        }
        public List<Solicitacao> Obter(DateTime inicio, DateTime fim)
        {
            return _context.Solicitacao.Include(s => s.Status)
                .Include(s => s.Motivo)
                .Where(s => s.DataInicioProcesso >= inicio && s.DataInicioProcesso <= fim).ToList();
        }
        public List<Solicitacao> Obter(DateTime inicio, DateTime fim, string matricula)
        {
            return _context.Solicitacao.Where(s => s.DataInicioProcesso >= inicio && s.DataInicioProcesso <= fim && s.MatriculaConsultor == matricula).ToList();
        }
        public Solicitacao Obter(int id)
        {
            return _context.Solicitacao.Include(s => s.Status).Include(s => s.Motivo).Include(s => s.SubStatus).Include(s => s.SubMotivo).FirstOrDefault(s => s.Id == id);
        }
        public List<string> ObterEntidades()
        {
            return _context.Solicitacao.Select(s => s.NomeEntidade).Distinct().ToList();
        }
        public List<Solicitacao> ObterSolicitacaoEntidade(string entidade, FiltrosPortabilidade filtros)
        {
            if (entidade != null && entidade.Contains("..."))
            {
                entidade = entidade.Substring(0, entidade.Length - 3);
            }
            return _context.Solicitacao.Where(s => s.NomeEntidade.StartsWith(entidade) && s.DataInicioProcesso >= filtros.De && s.DataInicioProcesso <= filtros.Ate).ToList();
        }
        public string ObterFundo(string CnpjCessionaria)
        {
            return _context.Fundos.FirstOrDefault(s => s.Cnpj == CnpjCessionaria)?.Nome;
        }

    }
}