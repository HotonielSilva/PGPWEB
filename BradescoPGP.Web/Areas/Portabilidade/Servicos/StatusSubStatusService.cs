using BradescoPGP.Common;
using BradescoPGP.Common.Logging;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Servicos
{
    public class StatusSubStatusService : IStatusSubStatusService
    {
        private readonly PGPEntities _context;


        public StatusSubStatusService(DbContext context)
        {
            _context = context as PGPEntities;
        }
        public List<Status> ObterStatus()
        {
            return _context.Status.Where(s => s.Evento == "Portabilidade").ToList();

        }
        public List<SubStatus> ObterSubStatus()
        {
            return _context.SubStatus.ToList();
        }
        public bool EditarStatus(int id, string descricao)
        {
            var entidade = _context.Status.FirstOrDefault(s => s.Id == id);

            entidade.Descricao = descricao;

            try
            {
                _context.Entry(entidade).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Não foi possível editar o Status", e);
                return false;
            }
        }
        public bool NovoStatus(string descricao)
        {
            var novoStatus = new Status
            {
                Descricao = descricao,
                Evento = Eventos.Portabilidade.ToString()
            };
            _context.Status.Add(novoStatus);
            try
            {
                _context.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
               Log.Error("Não foi possível Incluir um novo Status", e);
               return false;
            }
        }
        public bool NovoSubStatus (int idStatus, string descricao)
        {
            var novoSubStatus = new SubStatus {
                Descricao = descricao,
                StatusId = idStatus
            };
            try
            {
                _context.SubStatus.Add(novoSubStatus);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Erro ao criar novo substatus", e);
                return false;
            }
        }
        public bool EditarSubStatus(int idSubstatus, string descricao)
        {
            var substatus = _context.SubStatus.FirstOrDefault(s => s.Id == idSubstatus);

            if (substatus == null)
            {
                Log.Error($"Erro ao editar SubStatus, pois o mesmo não foi encontrado. Id pesquisado {idSubstatus}");
                return false;
            }
            substatus.Descricao = descricao;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao editar SubStatus", ex);
                return false;
            }
            return true;
           
        }

    }
}