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
    public class MotivoService : IMotivoService
    {
        private readonly PGPEntities _context;

        public MotivoService(DbContext context)
        {
            _context = context as PGPEntities;
        }

        public Motivo ObterMotivo(int id)
        {
            return _context.Motivo.FirstOrDefault(m => m.Id == id);
        }

        public List<Motivo> ObterTodosMotivos(bool inativos = false)
        {
            if (inativos)
                return _context.Motivo.ToList();
            else
                return _context.Motivo.Where(m => m.EmUso.HasValue && m.EmUso.Value).ToList();
        }

        public bool EditarMotivo(int idMotivo, string motivo)
        {
            var motivoBd = _context.Motivo.FirstOrDefault(m => m.Id == idMotivo);

            if (motivoBd == null)
            {
                Log.Error($"Erro ao editar motivo, pois o mesmo não foi encontrado. Id pesquisado {idMotivo}");
                return false;
            }

            motivoBd.Descricao = motivo;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao editar motivo", ex);
                return false;
            }

            return true;
        }

        public bool EditarSubmotivo(int idSubmotivo, string submotivo)
        {
            var submotivoDb = _context.SubMotivo.FirstOrDefault(s => s.Id == idSubmotivo);

            if (submotivoDb == null)
            {
                Log.Error($"Erro ao editar submotivo, pois o mesmo não foi encontrado. Id pesquisado {idSubmotivo}");
                return false;
            }
            submotivoDb.Descricao = submotivo;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao editar submotivo", ex);
                return false;
            }
            return true;
        }

        public bool ExcluirMotivo(int idMotivo)
        {
            var motivo = _context.Motivo.FirstOrDefault(m => m.Id == idMotivo);

            if (motivo == null)
                return false;

            motivo.EmUso = false;

            foreach (var sub in motivo.SubMotivo)
            {
                sub.EmUso = false;
            }

            try
            {
                _context.Entry(motivo).State = EntityState.Modified;

                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExcluirSubmotivo(int idSubMotivo)
        {
            var submotivo = _context.SubMotivo.FirstOrDefault(s => s.Id == idSubMotivo);

            if (submotivo == null)
                return false;

            submotivo.EmUso = false;

            try
            {
                _context.Entry(submotivo).State = EntityState.Modified;

                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool NovoMotivo(Motivo motivo)
        {
            if (motivo == null)
                return false;
            try
            {
                _context.Motivo.Add(motivo);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Erro ao cadatrar motivo", e);
                return false;
            }
               
        }

        public bool NovoSubMotivo(SubMotivo subMotivo)
        {
            try
            {
                _context.SubMotivo.Add(subMotivo);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Erro ao criar novo submotivo", e);
                return false;
            }
        }
    }
}