using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Services
{
    public enum ModoDeFiltro
    {
        EspecialistaAgenciaContaSituacao,
        EspecialistaAgenciaConta,
        EspecialistaAgencia,
        EspecialistaSituacao,
        EspecialistaAgenciaContaGerente,
        EspecialistaGerente,
        EspecialistaComentario,
        EspecialistaAgenciaContaComentario,
        EspecialistaAgenciaComentario,
        ApenasEspecialista,
        AgenciaContaSituacao,
        AgenciaConta,
        AgenciaContaGerente,
        AgenciaSituacao,
        Agencia,
        AgenciaGerente,
        Gerente,
        Situacao,
        ApenasData,
        Null,
        EquipeSituacao,
        EquipeComentario,
        Comentario,
        Equipe,
        EspecialistaAgenciaSituacao,
        AgenciaContaComentario,
        AgenciaComentario
    }

    public class FiltroService
    {
        private readonly PGPEntities db;

        public FiltroService(PGPEntities context)
        {
            db = context;
        }

        public static ModoDeFiltro SetaModoFiltro(FiltrosTelas filtros)
        {
            ModoDeFiltro ModoFiltro = ModoDeFiltro.Null;

            if (filtros.Especialista != null)
            {
                ModoFiltro = !String.IsNullOrEmpty(filtros.Agencia) && !String.IsNullOrEmpty(filtros.Conta) &&
                    filtros.Situacao.HasValue ? ModoDeFiltro.EspecialistaAgenciaContaSituacao : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;


                ModoFiltro = !String.IsNullOrEmpty(filtros.Agencia) && filtros.Situacao.HasValue ?
                    ModoDeFiltro.EspecialistaAgenciaSituacao : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;


                ModoFiltro = filtros.Situacao.HasValue
                    ? ModoDeFiltro.EspecialistaSituacao : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Agencia) && !String.IsNullOrEmpty(filtros.Conta) &&
                  !String.IsNullOrEmpty(filtros.Gerente) ? ModoDeFiltro.EspecialistaAgenciaContaGerente : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Gerente) ? ModoDeFiltro.EspecialistaGerente : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;



                //Cokpit

                ModoFiltro = !String.IsNullOrEmpty(filtros.Agencia) && !String.IsNullOrEmpty(filtros.Conta) && !String.IsNullOrEmpty(filtros.Comentario) ? ModoDeFiltro.EspecialistaAgenciaContaComentario : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Agencia) && !String.IsNullOrEmpty(filtros.Comentario) ? ModoDeFiltro.EspecialistaAgenciaComentario : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Comentario) ? ModoDeFiltro.EspecialistaComentario : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;


                ModoFiltro = !String.IsNullOrEmpty(filtros.Agencia) && !String.IsNullOrEmpty(filtros.Conta)
                  ? ModoDeFiltro.EspecialistaAgenciaConta : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Agencia) ? ModoDeFiltro.EspecialistaAgencia : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                return ModoDeFiltro.ApenasEspecialista;
            }


            if (!String.IsNullOrEmpty(filtros.Agencia) && !String.IsNullOrEmpty(filtros.Conta))
            {
                ModoFiltro = filtros.Situacao.HasValue ? ModoDeFiltro.AgenciaContaSituacao : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Gerente) ? ModoDeFiltro.AgenciaContaGerente : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Comentario) ? ModoDeFiltro.AgenciaContaComentario : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = ModoDeFiltro.AgenciaConta;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;


            }

            if (!String.IsNullOrEmpty(filtros.Equipe))
            {
                ModoFiltro = filtros.Situacao.HasValue ? ModoDeFiltro.EquipeSituacao : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Comentario) ? ModoDeFiltro.EquipeComentario : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = ModoDeFiltro.Equipe;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;
            }

            if (!string.IsNullOrEmpty(filtros.Agencia))
            {
                ModoFiltro = filtros.Situacao.HasValue ? ModoDeFiltro.AgenciaSituacao : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Gerente) ? ModoDeFiltro.AgenciaGerente : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = !String.IsNullOrEmpty(filtros.Comentario) ? ModoDeFiltro.AgenciaComentario : ModoDeFiltro.Null;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

                ModoFiltro = ModoDeFiltro.Agencia;
                if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;
            }

            ModoFiltro = !String.IsNullOrEmpty(filtros.Comentario) ? ModoDeFiltro.Comentario : ModoDeFiltro.Null;
            if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

            ModoFiltro = filtros.Situacao.HasValue ? ModoDeFiltro.Situacao : ModoDeFiltro.Null;
            if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

            ModoFiltro = !String.IsNullOrEmpty(filtros.Gerente) ? ModoDeFiltro.Gerente : ModoDeFiltro.Null;
            if (ModoFiltro != ModoDeFiltro.Null) return ModoFiltro;

            return ModoDeFiltro.ApenasData;

        }

        public List<PipelineViewModel> FiltraPipeline(FiltrosTelas filtros, string matricula, string perfil)
        {
            var modoFiltro = SetaModoFiltro(filtros);
            var pipeComFiltro = default(List<PipelineViewModel>);
            var minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-12);
            var maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var resultadoPesquisa = default(List<Pipeline>);

            switch (modoFiltro)
            {
                case ModoDeFiltro.EspecialistaAgenciaContaSituacao:

                    resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo")
                        .Where(x =>
                            x.Consultor == filtros.Especialista && x.Agencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta && x.Status.Id == filtros.Situacao &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false ||
                            x.DataPrevista >= minDate))
                        .ToList() :

                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo")
                        .Where(x => x.Consultor == filtros.Especialista &&
                            x.Agencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta && x.Status.Id == filtros.Situacao &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate))
                        .ToList();

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.EspecialistaAgenciaSituacao:
                    resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo")
                        .Where(x =>
                            x.Consultor == filtros.Especialista && x.Agencia.ToString() == filtros.Agencia &&
                            x.Status.Id == filtros.Situacao &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false ||
                            x.DataPrevista >= minDate))
                        .ToList() :

                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo")
                        .Where(x => x.Consultor == filtros.Especialista &&
                            x.Agencia.ToString() == filtros.Agencia &&
                            x.Status.Id == filtros.Situacao &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate))
                        .ToList();

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));
                    break;

                case ModoDeFiltro.EspecialistaAgenciaConta:

                    resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                         db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Consultor == filtros.Especialista && x.Agencia.ToString() == filtros.Agencia &&
                       x.Conta.ToString() == filtros.Conta &&
                       (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate && x.DataPrevista <= maxDate))
                        .ToList() :

                       db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Consultor == filtros.Especialista && x.Agencia.ToString() == filtros.Agencia &&
                        x.Conta.ToString() == filtros.Conta &&
                        (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate))
                        .ToList();

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.EspecialistaSituacao:

                    resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                         db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Consultor == filtros.Especialista && x.Status.Id == filtros.Situacao &&
                         (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate))
                        .ToList() :

                       db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Consultor == filtros.Especialista && x.Status.Id == filtros.Situacao &&
                        (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate))
                        .ToList();
                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.ApenasEspecialista:
                    resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Consultor.Trim() == filtros.Especialista.Trim() &&
                        (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate))
                       .ToList() :

                      db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Consultor == filtros.Especialista &&
                      (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate))
                       .ToList();

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.EquipeSituacao:

                    resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo")
                        .Join(db.Usuario, pipe => pipe.MatriculaConsultor, usu => usu.Matricula, (pipe, usu) => new { pipe, usu.Equipe })
                        .Where(x => x.Equipe == filtros.Equipe && x.pipe.Status.Id == filtros.Situacao &&
                        (x.pipe.DataProrrogada.HasValue ? x.pipe.DataProrrogada >= minDate : false || x.pipe.DataPrevista >= minDate))
                        .Select(p => p.pipe)
                        .ToList() :

                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo")
                        .Join(db.Usuario, pipe => pipe.MatriculaConsultor, usu => usu.Matricula, (pipe, usu) => new { pipe, usu.Equipe })
                        .Where(x => x.Equipe == filtros.Equipe && x.pipe.Status.Id == filtros.Situacao &&
                        (x.pipe.DataProrrogada.HasValue ? x.pipe.DataProrrogada >= filtros.De && x.pipe.DataProrrogada <= filtros.Ate : false || x.pipe.DataPrevista >= filtros.De && x.pipe.DataPrevista <= filtros.Ate))
                        .Select(p => p.pipe)
                        .ToList();

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.Equipe:
                    resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo")
                        .Join(db.Usuario, pipe => pipe.MatriculaConsultor, usu => usu.Matricula, (pipe, usu) => new { pipe, usu.Equipe })
                        .Where(x => x.Equipe == filtros.Equipe &&
                        (x.pipe.DataProrrogada.HasValue ? x.pipe.DataProrrogada >= minDate : false || x.pipe.DataPrevista >= minDate))
                        .Select(p => p.pipe)
                        .ToList() :

                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo")
                        .Join(db.Usuario, pipe => pipe.MatriculaConsultor, usu => usu.Matricula, (pipe, usu) => new { pipe, usu.Equipe })
                        .Where(x => x.Equipe == filtros.Equipe &&
                        (x.pipe.DataProrrogada.HasValue ? x.pipe.DataProrrogada >= filtros.De && x.pipe.DataProrrogada <= filtros.Ate : false || x.pipe.DataPrevista >= filtros.De && x.pipe.DataPrevista <= filtros.Ate))
                        .Select(p => p.pipe)
                        .ToList();

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.AgenciaContaSituacao:
                    if (perfil == "Especialista")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                         db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia &&
                        x.Conta.ToString() == filtros.Conta && x.StatusId == filtros.Situacao && x.MatriculaConsultor == matricula &&
                        (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :

                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia && x.MatriculaConsultor == matricula &&
                        x.Conta.ToString() == filtros.Conta && x.StatusId == filtros.Situacao &&
                        (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();

                    }
                    else if (perfil == "Gestor")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu }
                            ).Where(res => res.usu.MatriculaSupervisor == matricula).Select(s => s.pipe)
                            .Where(x => x.Agencia.ToString() == filtros.Agencia && x.Conta.ToString() == filtros.Conta &&
                            x.Status.Id == filtros.Situacao &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :

                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu }
                            ).Where(res => res.usu.MatriculaSupervisor == matricula).Select(s => s.pipe)
                            .Where(x => x.Agencia.ToString() == filtros.Agencia && x.Conta.ToString() == filtros.Conta &&
                            x.Status.Id == filtros.Situacao &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();
                    }
                    else
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                         db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia &&
                        x.Conta.ToString() == filtros.Conta && x.Status.Id == filtros.Situacao &&
                        (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :



                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia &&
                        x.Conta.ToString() == filtros.Conta && x.Status.Id == filtros.Situacao &&
                        (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();

                    }

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.AgenciaConta:
                    if (perfil == "Especialista")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta && x.MatriculaConsultor == matricula &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :



                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia && x.MatriculaConsultor == matricula &&
                            x.Conta.ToString() == filtros.Conta &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();

                    }
                    else if (perfil == "Gestor")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu }
                            ).Where(res => res.usu.MatriculaSupervisor == matricula).Select(s => s.pipe)
                             .Where(x => x.Agencia.ToString() == filtros.Agencia &&
                               x.Conta.ToString() == filtros.Conta &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :



                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu }
                            ).Where(res => res.usu.MatriculaSupervisor == matricula).Select(s => s.pipe)
                            .Where(x => x.Agencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();


                    }
                    else
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :



                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();

                    }

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.Agencia:
                    if (perfil == "Especialista")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                           db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia &&
                            x.MatriculaConsultor == matricula &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :

                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia && x.MatriculaConsultor == matricula &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();

                    }
                    else if (perfil == "Gestor")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu })
                            .Where(res =>
                                res.usu.MatriculaSupervisor == matricula &&
                                res.pipe.Agencia.ToString() == filtros.Agencia &&
                                (res.pipe.DataProrrogada.HasValue ? res.pipe.DataProrrogada >= minDate : false ||
                                res.pipe.DataPrevista >= minDate)).Select(s => s.pipe).ToList() :

                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu })
                            .Where(res =>
                                res.usu.MatriculaSupervisor == matricula &&
                                res.pipe.Agencia.ToString() == filtros.Agencia &&
                                (res.pipe.DataProrrogada.HasValue ? res.pipe.DataProrrogada >= filtros.De && res.pipe.DataProrrogada <= filtros.Ate : false ||
                                res.pipe.DataPrevista >= filtros.De && res.pipe.DataPrevista <= filtros.Ate))
                            .Select(s => s.pipe)
                            .ToList();
                    }
                    else
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :

                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Agencia.ToString() == filtros.Agencia &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();

                    }

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.Situacao:

                    if (perfil == "Especialista")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?

                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.StatusId == filtros.Situacao && x.MatriculaConsultor == matricula &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :



                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.StatusId == filtros.Situacao && x.MatriculaConsultor == matricula &&
                          (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();

                    }
                    else if (perfil == "Gestor")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu }
                            ).Where(res => res.usu.MatriculaSupervisor == matricula).Select(s => s.pipe)
                             .Where(x => x.Status.Id == filtros.Situacao &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :



                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu }
                            ).Where(res => res.usu.MatriculaSupervisor == matricula).Select(s => s.pipe)
                            .Where(x => x.Status.Id == filtros.Situacao &&
                              (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();

                    }
                    else
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?

                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Status.Id == filtros.Situacao &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :



                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.Status.Id == filtros.Situacao &&
                              (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();

                    }

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;

                case ModoDeFiltro.ApenasData:
                    if (perfil == "Especialista")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.MatriculaConsultor == matricula &&
                            (x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate)).ToList() :



                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x => x.MatriculaConsultor == matricula &&
                              (x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate)).ToList();


                    }
                    else if (perfil == "Gestor")
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu }
                            ).Where(res => res.usu.MatriculaSupervisor == matricula).Select(s => s.pipe)
                             .Where(x => x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate).ToList() :

                            db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Join(db.Usuario,
                            pipe => pipe.MatriculaConsultor,
                            usu => usu.Matricula,
                            (pipe, usu) => new { pipe, usu }
                            ).Where(res => res.usu.MatriculaSupervisor == matricula).Select(s => s.pipe)
                            .Where(x => x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate).ToList();
                    }
                    else
                    {
                        resultadoPesquisa = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x =>
                        x.DataProrrogada.HasValue ? x.DataProrrogada >= minDate : false || x.DataPrevista >= minDate).ToList() :

                        db.Pipeline.Include("Status").Include("Origem").Include("Motivo").Where(x =>
                        x.DataProrrogada.HasValue ? x.DataProrrogada >= filtros.De && x.DataProrrogada <= filtros.Ate : false || x.DataPrevista >= filtros.De && x.DataPrevista <= filtros.Ate).ToList();
                    }

                    pipeComFiltro = resultadoPesquisa.ConvertAll(p => PipelineViewModel.Mapear(p));

                    break;
            }

            return pipeComFiltro;
        }

        public List<VencimentoViewModel> FiltraVencimentos(FiltrosTelas filtros, string matricula, string perfil)
        {
            var modoFiltro = SetaModoFiltro(filtros);

            var minDate = DateTime.Now.Date.AddDays(-5);
            var maxDate = DateTime.Now.Date.AddDays(10);

            var vencimentoViewModelFiltro = new List<VencimentoViewModel>();

            switch (modoFiltro)
            {
                case ModoDeFiltro.EspecialistaAgenciaContaSituacao:
                    var vencimentoFitroEspecialista = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento
                            .Join(db.Encarteiramento,
                                ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                                enc => new { agencia = enc.Agencia, conta = enc.Conta },
                                (ven, enc) => new { ven, enc })
                            .Join(db.Usuario,
                                res => res.enc.Matricula,
                                usu => usu.Matricula,
                                (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                    vencimentoViewModelFiltro = vencimentoFitroEspecialista.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    break;

                case ModoDeFiltro.EspecialistaAgenciaSituacao:

                    var vencimentoFitroEspecialistaAgenciaSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento
                            .Join(db.Encarteiramento,
                                ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                                enc => new { agencia = enc.Agencia, conta = enc.Conta },
                                (ven, enc) => new { ven, enc })
                            .Join(db.Usuario,
                                res => res.enc.Matricula,
                                usu => usu.Matricula,
                                (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                    vencimentoViewModelFiltro = vencimentoFitroEspecialistaAgenciaSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));

                    break;
                case ModoDeFiltro.Agencia:

                    var agencia = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                       db.Vencimento.Join(db.Encarteiramento,
                           ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                           enc => new { agencia = enc.Agencia, conta = enc.Conta },
                           (ven, enc) => new { ven, enc }).Join(db.Usuario,
                           res => res.enc.Matricula,
                           usu => usu.Matricula,
                           (res, usu) => new { res.ven, usu })

                           .Where(resp => resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                               resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                       db.Vencimento.Join(db.Encarteiramento,
                           ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                           enc => new { agencia = enc.Agencia, conta = enc.Conta },
                           (ven, enc) => new { ven, enc }).Join(db.Usuario,
                           res => res.enc.Matricula,
                           usu => usu.Matricula,
                           (res, usu) => new { res.ven, usu })

                           .Where(resp => resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                               resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                    vencimentoViewModelFiltro = agencia.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    break;

                case ModoDeFiltro.AgenciaSituacao:
                    var agenciaSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                       db.Vencimento.Join(db.Encarteiramento,
                           ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                           enc => new { agencia = enc.Agencia, conta = enc.Conta },
                           (ven, enc) => new { ven, enc }).Join(db.Usuario,
                           res => res.enc.Matricula,
                           usu => usu.Matricula,
                           (res, usu) => new { res.ven, usu })

                           .Where(resp => resp.ven.Cod_Agencia.ToString() == filtros.Agencia && resp.ven.StatusId == filtros.Situacao &&
                               resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                       db.Vencimento.Join(db.Encarteiramento,
                           ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                           enc => new { agencia = enc.Agencia, conta = enc.Conta },
                           (ven, enc) => new { ven, enc }).Join(db.Usuario,
                           res => res.enc.Matricula,
                           usu => usu.Matricula,
                           (res, usu) => new { res.ven, usu })

                           .Where(resp => resp.ven.Cod_Agencia.ToString() == filtros.Agencia && resp.ven.StatusId == filtros.Situacao &&
                               resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                    vencimentoViewModelFiltro = agenciaSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    break;

                case ModoDeFiltro.EspecialistaAgenciaConta:

                    var vencimentoFitroEspecialistaAgenciaConta = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate)
                                .ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate)
                                .ToList();

                    vencimentoViewModelFiltro = vencimentoFitroEspecialistaAgenciaConta.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));

                    break;

                case ModoDeFiltro.EspecialistaAgencia:

                    var vencimentoFitroEspecialistaAgencia = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                       db.Vencimento.Join(db.Encarteiramento,
                           ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                           enc => new { agencia = enc.Agencia, conta = enc.Conta },
                           (ven, enc) => new { ven, enc }).Join(db.Usuario,
                           res => res.enc.Matricula,
                           usu => usu.Matricula,
                           (res, usu) => new { res.ven, usu })

                           .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                               resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate)
                               .ToList() :

                       db.Vencimento.Join(db.Encarteiramento,
                           ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                           enc => new { agencia = enc.Agencia, conta = enc.Conta },
                           (ven, enc) => new { ven, enc }).Join(db.Usuario,
                           res => res.enc.Matricula,
                           usu => usu.Matricula,
                           (res, usu) => new { res.ven, usu })

                           .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                               resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate)
                               .ToList();
                    vencimentoViewModelFiltro = vencimentoFitroEspecialistaAgencia.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));

                    break;

                case ModoDeFiltro.EspecialistaSituacao:

                    var vencimentoFitroEspecialistaSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate)
                                .ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate)
                                .ToList();
                    vencimentoViewModelFiltro = vencimentoFitroEspecialistaSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));

                    break;

                case ModoDeFiltro.ApenasEspecialista:

                    var vencimentoFitroApenasEspecialista = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Include("Status").Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate)
                                .ToList() :

                        db.Vencimento.Include("Status").Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Nome == filtros.Especialista &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate)
                                .ToList();

                    vencimentoViewModelFiltro = vencimentoFitroApenasEspecialista.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    break;

                case ModoDeFiltro.Equipe:

                    var vencimentoFitroEquipe = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Include("Status").Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu.Equipe, usu.Nome, usu.Matricula })

                            .Where(resp => resp.Equipe == filtros.Equipe &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Include("Status").Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc })
                            .Join(db.Usuario,
                                res => res.enc.Matricula,
                                usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu.Equipe, usu.Nome, usu.Matricula })

                            .Where(resp => resp.Equipe == filtros.Equipe &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate)
                            .ToList();

                    vencimentoViewModelFiltro = vencimentoFitroEquipe.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.Nome, v.Matricula));

                    break;

                case ModoDeFiltro.EquipeSituacao:

                    var vencimentoFitroEquipeSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Include("Status").Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu.Equipe, usu.Nome, usu.Matricula })

                            .Where(resp => resp.Equipe == filtros.Equipe && resp.ven.StatusId == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Include("Status").Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu.Equipe, usu.Nome, usu.Matricula })

                            .Where(resp => resp.Equipe == filtros.Equipe && resp.ven.StatusId == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate)
                            .ToList();

                    vencimentoViewModelFiltro = vencimentoFitroEquipeSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.Nome, v.Matricula));

                    break;
                case ModoDeFiltro.AgenciaContaSituacao:
                    if (perfil == "Especialista")
                    {
                        var AgenciaContaSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Matricula == matricula && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Matricula == matricula && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                        vencimentoViewModelFiltro = AgenciaContaSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));

                    }
                    else if (perfil == "Gestor")
                    {
                        var AgenciaContaSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.MatriculaSupervisor == matricula && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.MatriculaSupervisor == matricula && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                        vencimentoViewModelFiltro = AgenciaContaSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }
                    else
                    {
                        var AgenciaContaSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                        vencimentoViewModelFiltro = AgenciaContaSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }

                    break;

                case ModoDeFiltro.AgenciaConta:
                    if (perfil == "Especialista")
                    {
                        var vencimentoFitroAgenciaConta = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Matricula == matricula && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Matricula == matricula && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();


                        vencimentoViewModelFiltro = vencimentoFitroAgenciaConta.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }
                    else if (perfil == "Gestor")
                    {
                        var vencimentoFitroAgenciaConta = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                         db.Vencimento.Join(db.Encarteiramento,
                             ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                             enc => new { agencia = enc.Agencia, conta = enc.Conta },
                             (ven, enc) => new { ven, enc }).Join(db.Usuario,
                             res => res.enc.Matricula,
                             usu => usu.Matricula,
                             (res, usu) => new { res.ven, usu })

                             .Where(resp => resp.usu.MatriculaSupervisor == matricula && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                 resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta &&
                                 resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                         db.Vencimento.Join(db.Encarteiramento,
                             ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                             enc => new { agencia = enc.Agencia, conta = enc.Conta },
                             (ven, enc) => new { ven, enc }).Join(db.Usuario,
                             res => res.enc.Matricula,
                             usu => usu.Matricula,
                             (res, usu) => new { res.ven, usu })

                             .Where(resp => resp.usu.MatriculaSupervisor == matricula && resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                 resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta &&
                                 resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                        vencimentoViewModelFiltro = vencimentoFitroAgenciaConta.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }
                    else
                    {
                        var vencimentoFitroAgenciaConta = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.ven.Cod_Agencia.ToString() == filtros.Agencia &&
                                resp.ven.Cod_Conta_Corrente.ToString() == filtros.Conta &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                        vencimentoViewModelFiltro = vencimentoFitroAgenciaConta.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }
                    break;
                case ModoDeFiltro.Situacao:

                    if (perfil == "Especialista")
                    {
                        var vencimentoFitroSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Matricula == matricula && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Matricula == matricula && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                        vencimentoViewModelFiltro = vencimentoFitroSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }
                    else if (perfil == "Gestor")
                    {
                        var vencimentoFitroSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.MatriculaSupervisor == matricula && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.MatriculaSupervisor == matricula && resp.ven.Status.Id == filtros.Situacao &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                        vencimentoViewModelFiltro = vencimentoFitroSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }
                    else
                    {
                        var vencimentoFitroSituacao = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                         db.Vencimento.Join(db.Encarteiramento,
                             ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                             enc => new { agencia = enc.Agencia, conta = enc.Conta },
                             (ven, enc) => new { ven, enc }).Join(db.Usuario,
                             res => res.enc.Matricula,
                             usu => usu.Matricula,
                             (res, usu) => new { res.ven, usu })

                             .Where(resp => resp.ven.Status.Id == filtros.Situacao &&
                                 resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                         db.Vencimento.Join(db.Encarteiramento,
                             ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                             enc => new { agencia = enc.Agencia, conta = enc.Conta },
                             (ven, enc) => new { ven, enc }).Join(db.Usuario,
                             res => res.enc.Matricula,
                             usu => usu.Matricula,
                             (res, usu) => new { res.ven, usu })

                             .Where(resp => resp.ven.Status.Id == filtros.Situacao &&
                                 resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                        vencimentoViewModelFiltro = vencimentoFitroSituacao.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }
                    break;

                case ModoDeFiltro.ApenasData:
                    if (perfil == "Especialista")
                    {
                        var vencimentoFitroApenasData = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Matricula == matricula &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.Matricula == matricula &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();
                        vencimentoViewModelFiltro = vencimentoFitroApenasData.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));

                    }
                    else if (perfil == "Gestor")
                    {
                        var vencimentoFitroApenasData = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.MatriculaSupervisor == matricula &&
                                resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate).ToList() :

                        db.Vencimento.Join(db.Encarteiramento,
                            ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ven, enc) => new { ven, enc }).Join(db.Usuario,
                            res => res.enc.Matricula,
                            usu => usu.Matricula,
                            (res, usu) => new { res.ven, usu })

                            .Where(resp => resp.usu.MatriculaSupervisor == matricula &&
                                resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate).ToList();

                        vencimentoViewModelFiltro = vencimentoFitroApenasData.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }
                    else
                    {
                        var vencimentoFitroApenasData = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                         db.Vencimento.Join(db.Encarteiramento,
                             ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                             enc => new { agencia = enc.Agencia, conta = enc.Conta },
                             (ven, enc) => new { ven, enc }).Join(db.Usuario,
                             res => res.enc.Matricula,
                             usu => usu.Matricula,
                             (res, usu) => new { res.ven, usu })

                             .Where(resp => resp.ven.Dt_Vecto_Contratado >= minDate && resp.ven.Dt_Vecto_Contratado <= maxDate)
                             .ToList() :

                         db.Vencimento.Join(db.Encarteiramento,
                             ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                             enc => new { agencia = enc.Agencia, conta = enc.Conta },
                             (ven, enc) => new { ven, enc }).Join(db.Usuario,
                             res => res.enc.Matricula,
                             usu => usu.Matricula,
                             (res, usu) => new { res.ven, usu })

                             .Where(resp => resp.ven.Dt_Vecto_Contratado >= filtros.De && resp.ven.Dt_Vecto_Contratado <= filtros.Ate)
                             .ToList();
                        vencimentoViewModelFiltro = vencimentoFitroApenasData.ConvertAll(v => VencimentoViewModel.Mapear(v.ven, v.usu.Nome, v.usu.Matricula));
                    }

                    break;
            }

            return vencimentoViewModelFiltro;
        }

        public List<Cockpit> FiltrarCockpit(FiltrosTelas filtros, string matricula, string perfil)
        {
            var cockpits = default(List<Cockpit>);
            var minData = DateTime.Now.Date.AddDays(-90);
            var maxData = DateTime.Now.Date;
            var modoFiltro = SetaModoFiltro(filtros);


            switch (modoFiltro)
            {
                case ModoDeFiltro.EspecialistaAgenciaContaComentario:
                    cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?

                        db.Cockpit
                            .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Nome })
                            .Where(x =>
                                x.cock.CodigoAgencia.ToString() == filtros.Agencia && x.cock.Conta.ToString() == filtros.Conta &&
                                x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                                x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                                .Select(s => s.cock)
                                .ToList() :

                        db.Cockpit
                            .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Nome })
                            .Where(x =>
                                x.cock.CodigoAgencia.ToString() == filtros.Agencia && x.cock.Conta.ToString() == filtros.Conta &&
                                x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                                x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                                .Select(s => s.cock)
                                .ToList();
                    break;

                case ModoDeFiltro.EspecialistaAgenciaComentario:
                    cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?

                        db.Cockpit
                            .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Nome })
                            .Where(x =>
                                x.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                                x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                                x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                                .Select(s => s.cock)
                                .ToList() :

                        db.Cockpit
                            .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Nome })
                            .Where(x =>
                                x.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                                x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                                x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                                .Select(s => s.cock)
                                .ToList();
                    break;

                case ModoDeFiltro.EspecialistaAgenciaConta:
                    cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?

                       db.Cockpit
                           .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                           (cock, usu) => new { cock, usu.Nome })
                           .Where(x =>
                               x.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                               x.cock.Conta.ToString() == filtros.Conta &&
                               x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                               x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                               .Select(s => s.cock)
                               .ToList() :

                       db.Cockpit
                           .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                           (cock, usu) => new { cock, usu.Nome })
                           .Where(x =>
                               x.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                               x.cock.Conta.ToString() == filtros.Conta &&
                               x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                               x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                               .Select(s => s.cock)
                               .ToList();
                    break;
                case ModoDeFiltro.AgenciaConta:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {

                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Where(x =>
                            x.CodigoAgencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta &&
                            x.MatriculaConsultor == matricula &&
                            x.DataContato >= filtros.De && x.DataContato <= filtros.Ate).ToList() :

                        cockpits = db.Cockpit.Where(x =>
                            x.CodigoAgencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta &&
                            x.MatriculaConsultor == matricula &&
                            x.DataContato >= minData && x.DataContato <= maxData).ToList();
                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu })
                            .Where(r =>
                                r.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                                r.cock.Conta.ToString() == filtros.Conta &&
                                r.usu.MatriculaSupervisor == matricula &&
                                r.cock.DataContato >= filtros.De.Value &&
                                r.cock.DataContato <= filtros.Ate.Value)
                            .Select(s => s.cock).ToList() :

                        db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu })
                            .Where(r =>
                                r.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                                r.cock.Conta.ToString() == filtros.Conta &&
                                r.usu.MatriculaSupervisor == matricula &&
                                r.cock.DataContato >= minData && r.cock.DataContato <= maxData)
                            .Select(s => s.cock).ToList();
                    }
                    else
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Where(x =>
                                x.CodigoAgencia.ToString() == filtros.Agencia &&
                                x.Conta.ToString() == filtros.Conta &&
                                x.DataContato >= filtros.De &&
                                x.DataContato <= filtros.Ate
                               ).ToList() :

                            db.Cockpit
                                .Where(x =>
                                    x.CodigoAgencia.ToString() == filtros.Agencia &&
                                    x.Conta.ToString() == filtros.Conta &&
                                    x.DataContato >= minData && x.DataContato <= maxData).ToList();
                    }


                    break;

                case ModoDeFiltro.AgenciaContaComentario:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {

                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Where(x =>
                            x.CodigoAgencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta &&
                            x.MatriculaConsultor == matricula &&
                            x.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                            x.DataContato >= filtros.De && x.DataContato <= filtros.Ate).ToList() :

                        cockpits = db.Cockpit.Where(x =>
                            x.CodigoAgencia.ToString() == filtros.Agencia &&
                            x.Conta.ToString() == filtros.Conta &&
                            x.MatriculaConsultor == matricula &&
                            x.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) && x.DataContato >= minData && x.DataContato <= maxData).ToList();
                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu })
                            .Where(r =>
                                r.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                                r.cock.Conta.ToString() == filtros.Conta &&
                                r.usu.MatriculaSupervisor == matricula &&
                                r.cock.DataContato >= filtros.De.Value &&
                                r.cock.DataContato <= filtros.Ate.Value &&
                                r.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()))
                            .Select(s => s.cock).ToList() :

                        db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu })
                            .Where(r =>
                                r.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                                r.cock.Conta.ToString() == filtros.Conta &&
                                r.usu.MatriculaSupervisor == matricula && r.cock.DataContato >= minData && r.cock.DataContato <= maxData &&
                                r.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()))
                            .Select(s => s.cock).ToList();
                    }
                    else
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Where(x =>
                                x.CodigoAgencia.ToString() == filtros.Agencia &&
                                x.Conta.ToString() == filtros.Conta &&
                                x.DataContato >= filtros.De &&
                                x.DataContato <= filtros.Ate &&
                                x.Observacao.ToLower().Contains(filtros.Comentario.ToLower())).ToList() :

                            db.Cockpit
                                .Where(x =>
                                    x.CodigoAgencia.ToString() == filtros.Agencia &&
                                    x.Conta.ToString() == filtros.Conta &&
                                    x.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                    x.DataContato >= minData && x.DataContato <= maxData).ToList();
                    }

                    break;

                case ModoDeFiltro.AgenciaComentario:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Where(x =>
                            x.CodigoAgencia.ToString() == filtros.Agencia &&
                            x.MatriculaConsultor == matricula &&
                            x.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                            x.DataContato >= filtros.De && x.DataContato <= filtros.Ate).ToList() :

                        cockpits = db.Cockpit.Where(x =>
                            x.CodigoAgencia.ToString() == filtros.Agencia &&
                            x.MatriculaConsultor == matricula &&
                            x.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) && x.DataContato >= minData && x.DataContato <= maxData).ToList();
                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu })
                            .Where(r =>
                                r.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                                r.usu.MatriculaSupervisor == matricula &&
                                r.cock.DataContato >= filtros.De.Value &&
                                r.cock.DataContato <= filtros.Ate.Value &&
                                r.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()))
                            .Select(s => s.cock).ToList() :

                        db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu })
                            .Where(r =>
                                r.cock.CodigoAgencia.ToString() == filtros.Agencia &&
                                r.usu.MatriculaSupervisor == matricula && r.cock.DataContato >= minData && r.cock.DataContato <= maxData &&
                                r.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()))
                            .Select(s => s.cock).ToList();
                    }
                    else
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Where(x =>
                                x.CodigoAgencia.ToString() == filtros.Agencia &&
                                x.DataContato >= filtros.De &&
                                x.DataContato <= filtros.Ate &&
                                x.Observacao.ToLower().Contains(filtros.Comentario.ToLower())).ToList() :

                            db.Cockpit
                                .Where(x =>
                                    x.CodigoAgencia.ToString() == filtros.Agencia &&
                                    x.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                    x.DataContato >= minData && x.DataContato <= maxData).ToList();
                    }
                    break;

                case ModoDeFiltro.EspecialistaComentario:
                    cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?

                        db.Cockpit
                            .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Nome })
                            .Where(x => x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                                x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                                .Select(s => s.cock)
                                .ToList() :

                        db.Cockpit
                            .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Nome })
                            .Where(x => x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                                x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                                .Select(s => s.cock)
                                .ToList();
                    break;

                case ModoDeFiltro.ApenasEspecialista:
                    cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?

                        db.Cockpit
                            .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Nome })
                            .Where(x => x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                                x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                                .Select(s => s.cock)
                                .ToList() :

                        db.Cockpit
                            .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Nome })
                            .Where(x => x.Nome.ToLower() == filtros.Especialista.ToLower() &&
                                x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                                .Select(s => s.cock)
                                .ToList();
                    break;

                case ModoDeFiltro.Equipe:
                    cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                        db.Cockpit
                        .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula, (cock, usu) => new { cock, usu.Equipe })
                        .Where(x => x.Equipe == filtros.Equipe &&
                            x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                            .Select(s => s.cock)
                            .ToList() :

                        db.Cockpit
                        .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula, (cock, usu) => new { cock, usu.Equipe })
                        .Where(x => x.Equipe == filtros.Equipe &&
                        x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                        .Select(s => s.cock).ToList();

                    break;

                case ModoDeFiltro.EquipeComentario:
                    cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                        db.Cockpit
                        .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula, (cock, usu) => new { cock, usu.Equipe })
                        .Where(x => x.Equipe == filtros.Equipe &&
                            x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                            x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                            .Select(s => s.cock)
                            .ToList() :

                        db.Cockpit
                        .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula, (cock, usu) => new { cock, usu.Equipe })
                        .Where(x => x.Equipe == filtros.Equipe &&
                        x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                        x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                        .Select(s => s.cock).ToList();
                    break;

                case ModoDeFiltro.Comentario:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Where(x =>
                            x.MatriculaConsultor == matricula &&
                            x.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                            x.DataContato >= filtros.De && x.DataContato <= filtros.Ate).ToList() :

                        cockpits = db.Cockpit.Where(x => x.MatriculaConsultor == matricula &&
                            x.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) && x.DataContato >= minData && x.DataContato <= maxData).ToList();
                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu })
                            .Where(r => r.usu.MatriculaSupervisor == matricula && r.cock.DataContato >= filtros.De.Value && r.cock.DataContato <= filtros.Ate.Value &&
                            r.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()))
                            .Select(s => s.cock).ToList() :

                        db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu })
                            .Where(r => r.usu.MatriculaSupervisor == matricula && r.cock.DataContato >= minData && r.cock.DataContato <= maxData &&
                            r.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()))
                            .Select(s => s.cock).ToList();
                    }
                    else
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Where(x => x.DataContato >= filtros.De && x.DataContato <= filtros.Ate &&
                            x.Observacao.ToLower().Contains(filtros.Comentario.ToLower())).ToList() :

                            db.Cockpit
                                .Where(x => x.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                x.DataContato >= minData && x.DataContato <= maxData).ToList();
                    }

                    break;
            }

            return cockpits;

        }

        public List<CockpitExportExcel> FiltrarCockpitExportExcel(FiltrosTelas filtros, string matricula, string perfil)
        {
            var cockpits = default(List<CockpitExportExcel>);
            var minData = DateTime.Now.Date.AddDays(-90);
            var maxData = DateTime.Now.Date;
            var modoFiltro = SetaModoFiltro(filtros);

            switch (modoFiltro)
            {
                case ModoDeFiltro.Equipe:
                    cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                        db.Cockpit
                        .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula, (cock, usu) => new { cock, usu.Equipe })
                        .Where(x => x.Equipe == filtros.Equipe &&
                            x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                            .Select(s => new CockpitExportExcel
                            {
                                CodFuncionalGerente = s.cock.CodFuncionalGerente,
                                NomeGerente = s.cock.NomeGerente,
                                CPF = s.cock.CPF,
                                NomeCliente = s.cock.NomeCliente,
                                Conta = s.cock.Conta,
                                DataEncarteiramento = s.cock.DataEncarteiramento,
                                DataContato = s.cock.DataContato,
                                DataRetorno = s.cock.DataRetorno,
                                Observacao = s.cock.Observacao,
                                ContatoTeveExito = s.cock.ContatoTeveExito,
                                DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                                MeioContato = s.cock.MeioContato,
                                ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                                TipoTransacao = s.cock.TipoTransacao,
                                Finalizado = s.cock.Finalizado,
                                GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                                MatriculaConsultor = s.cock.MatriculaConsultor,
                                Equipe = s.Equipe,
                                CodigoAgencia = s.cock.CodigoAgencia,
                                NomeAgencia = s.cock.NomeAgencia
                            })
                            .ToList() :

                        db.Cockpit
                        .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                        (cock, usu) => new { cock, usu.Equipe, usu.Nome })
                        .Where(x => x.Equipe == filtros.Equipe &&
                        x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                        .Select(s => new CockpitExportExcel
                        {
                            CodFuncionalGerente = s.cock.CodFuncionalGerente,
                            NomeGerente = s.cock.NomeGerente,
                            CPF = s.cock.CPF,
                            NomeCliente = s.cock.NomeCliente,
                            Conta = s.cock.Conta,
                            DataEncarteiramento = s.cock.DataEncarteiramento,
                            DataContato = s.cock.DataContato,
                            DataRetorno = s.cock.DataRetorno,
                            Observacao = s.cock.Observacao,
                            ContatoTeveExito = s.cock.ContatoTeveExito,
                            DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                            MeioContato = s.cock.MeioContato,
                            ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                            TipoTransacao = s.cock.TipoTransacao,
                            Finalizado = s.cock.Finalizado,
                            GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                            MatriculaConsultor = s.cock.MatriculaConsultor,
                            Equipe = s.Equipe,
                            CodigoAgencia = s.cock.CodigoAgencia,
                            NomeAgencia = s.cock.NomeAgencia,
                            Especialista = s.Nome
                        }).ToList();

                    break;

                case ModoDeFiltro.EquipeComentario:
                    cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                        db.Cockpit
                        .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                        (cock, usu) => new { cock, usu.Equipe, usu.Nome })
                        .Where(x => x.Equipe == filtros.Equipe &&
                            x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                            x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                            .Select(s => new CockpitExportExcel
                            {
                                CodFuncionalGerente = s.cock.CodFuncionalGerente,
                                NomeGerente = s.cock.NomeGerente,
                                CPF = s.cock.CPF,
                                NomeCliente = s.cock.NomeCliente,
                                Conta = s.cock.Conta,
                                DataEncarteiramento = s.cock.DataEncarteiramento,
                                DataContato = s.cock.DataContato,
                                DataRetorno = s.cock.DataRetorno,
                                Observacao = s.cock.Observacao,
                                ContatoTeveExito = s.cock.ContatoTeveExito,
                                DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                                MeioContato = s.cock.MeioContato,
                                ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                                TipoTransacao = s.cock.TipoTransacao,
                                Finalizado = s.cock.Finalizado,
                                GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                                MatriculaConsultor = s.cock.MatriculaConsultor,
                                Equipe = s.Equipe,
                                CodigoAgencia = s.cock.CodigoAgencia,
                                NomeAgencia = s.cock.NomeAgencia,
                                Especialista = s.Nome
                            })
                            .ToList() :

                        db.Cockpit
                        .Join(db.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula,
                        (cock, usu) => new { cock, usu.Equipe, usu.Nome })
                        .Where(x => x.Equipe == filtros.Equipe &&
                        x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                        x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                        .Select(s => new CockpitExportExcel
                        {
                            CodFuncionalGerente = s.cock.CodFuncionalGerente,
                            NomeGerente = s.cock.NomeGerente,
                            CPF = s.cock.CPF,
                            NomeCliente = s.cock.NomeCliente,
                            Conta = s.cock.Conta,
                            DataEncarteiramento = s.cock.DataEncarteiramento,
                            DataContato = s.cock.DataContato,
                            DataRetorno = s.cock.DataRetorno,
                            Observacao = s.cock.Observacao,
                            ContatoTeveExito = s.cock.ContatoTeveExito,
                            DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                            MeioContato = s.cock.MeioContato,
                            ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                            TipoTransacao = s.cock.TipoTransacao,
                            Finalizado = s.cock.Finalizado,
                            GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                            MatriculaConsultor = s.cock.MatriculaConsultor,
                            Equipe = s.Equipe,
                            CodigoAgencia = s.cock.CodigoAgencia,
                            NomeAgencia = s.cock.NomeAgencia,
                            Especialista = s.Nome
                        }).ToList();
                    break;

                case ModoDeFiltro.Comentario:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit
                            .Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Equipe, usu.Nome })
                            .Where(x =>
                            x.cock.MatriculaConsultor == matricula &&
                            x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                            x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate)
                            .Select(s => new CockpitExportExcel
                            {
                                CodFuncionalGerente = s.cock.CodFuncionalGerente,
                                NomeGerente = s.cock.NomeGerente,
                                CPF = s.cock.CPF,
                                NomeCliente = s.cock.NomeCliente,
                                Conta = s.cock.Conta,
                                DataEncarteiramento = s.cock.DataEncarteiramento,
                                DataContato = s.cock.DataContato,
                                DataRetorno = s.cock.DataRetorno,
                                Observacao = s.cock.Observacao,
                                ContatoTeveExito = s.cock.ContatoTeveExito,
                                DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                                MeioContato = s.cock.MeioContato,
                                ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                                TipoTransacao = s.cock.TipoTransacao,
                                Finalizado = s.cock.Finalizado,
                                GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                                MatriculaConsultor = s.cock.MatriculaConsultor,
                                Equipe = s.Equipe,
                                CodigoAgencia = s.cock.CodigoAgencia,
                                NomeAgencia = s.cock.NomeAgencia,
                                Especialista = s.Nome
                            })
                            .ToList() :

                            cockpits = db.Cockpit
                            .Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Equipe, usu.Nome })
                            .Where(x => x.cock.MatriculaConsultor == matricula &&
                            x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower())
                            && x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                            .Select(s => new CockpitExportExcel
                            {
                                CodFuncionalGerente = s.cock.CodFuncionalGerente,
                                NomeGerente = s.cock.NomeGerente,
                                CPF = s.cock.CPF,
                                NomeCliente = s.cock.NomeCliente,
                                Conta = s.cock.Conta,
                                DataEncarteiramento = s.cock.DataEncarteiramento,
                                DataContato = s.cock.DataContato,
                                DataRetorno = s.cock.DataRetorno,
                                Observacao = s.cock.Observacao,
                                ContatoTeveExito = s.cock.ContatoTeveExito,
                                DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                                MeioContato = s.cock.MeioContato,
                                ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                                TipoTransacao = s.cock.TipoTransacao,
                                Finalizado = s.cock.Finalizado,
                                GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                                MatriculaConsultor = s.cock.MatriculaConsultor,
                                Equipe = s.Equipe,
                                CodigoAgencia = s.cock.CodigoAgencia,
                                NomeAgencia = s.cock.NomeAgencia,
                                Especialista = s.Nome
                            })
                            .ToList();
                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.MatriculaSupervisor, usu.Equipe, usu.Nome })
                            .Where(r => r.MatriculaSupervisor == matricula && r.cock.DataContato >= filtros.De.Value && r.cock.DataContato <= filtros.Ate.Value &&
                            r.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()))
                            .Select(s => new CockpitExportExcel
                            {
                                CodFuncionalGerente = s.cock.CodFuncionalGerente,
                                NomeGerente = s.cock.NomeGerente,
                                CPF = s.cock.CPF,
                                NomeCliente = s.cock.NomeCliente,
                                Conta = s.cock.Conta,
                                DataEncarteiramento = s.cock.DataEncarteiramento,
                                DataContato = s.cock.DataContato,
                                DataRetorno = s.cock.DataRetorno,
                                Observacao = s.cock.Observacao,
                                ContatoTeveExito = s.cock.ContatoTeveExito,
                                DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                                MeioContato = s.cock.MeioContato,
                                ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                                TipoTransacao = s.cock.TipoTransacao,
                                Finalizado = s.cock.Finalizado,
                                GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                                MatriculaConsultor = s.cock.MatriculaConsultor,
                                Equipe = s.Equipe,
                                CodigoAgencia = s.cock.CodigoAgencia,
                                NomeAgencia = s.cock.NomeAgencia,
                                Especialista = s.Nome
                            })
                            .ToList() :

                        db.Cockpit.Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.MatriculaSupervisor, usu.Equipe, usu.Nome })
                            .Where(r => r.MatriculaSupervisor == matricula && r.cock.DataContato >= minData && r.cock.DataContato <= maxData &&
                            r.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()))
                            .Select(s => new CockpitExportExcel
                            {
                                CodFuncionalGerente = s.cock.CodFuncionalGerente,
                                NomeGerente = s.cock.NomeGerente,
                                CPF = s.cock.CPF,
                                NomeCliente = s.cock.NomeCliente,
                                Conta = s.cock.Conta,
                                DataEncarteiramento = s.cock.DataEncarteiramento,
                                DataContato = s.cock.DataContato,
                                DataRetorno = s.cock.DataRetorno,
                                Observacao = s.cock.Observacao,
                                ContatoTeveExito = s.cock.ContatoTeveExito,
                                DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                                MeioContato = s.cock.MeioContato,
                                ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                                TipoTransacao = s.cock.TipoTransacao,
                                Finalizado = s.cock.Finalizado,
                                GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                                MatriculaConsultor = s.cock.MatriculaConsultor,
                                Equipe = s.Equipe,
                                CodigoAgencia = s.cock.CodigoAgencia,
                                NomeAgencia = s.cock.NomeAgencia,
                                Especialista = s.Nome
                            })
                            .ToList();
                    }
                    else
                    {
                        cockpits = filtros.De.HasValue && filtros.Ate.HasValue ?
                            db.Cockpit
                            .Join(db.Usuario,
                            cock => cock.MatriculaConsultor,
                            usu => usu.Matricula,
                            (cock, usu) => new { cock, usu.Equipe, usu.Nome })
                            .Where(x => x.cock.DataContato >= filtros.De && x.cock.DataContato <= filtros.Ate &&
                            x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()))
                            .Select(s => new CockpitExportExcel
                            {
                                CodFuncionalGerente = s.cock.CodFuncionalGerente,
                                NomeGerente = s.cock.NomeGerente,
                                CPF = s.cock.CPF,
                                NomeCliente = s.cock.NomeCliente,
                                Conta = s.cock.Conta,
                                DataEncarteiramento = s.cock.DataEncarteiramento,
                                DataContato = s.cock.DataContato,
                                DataRetorno = s.cock.DataRetorno,
                                Observacao = s.cock.Observacao,
                                ContatoTeveExito = s.cock.ContatoTeveExito,
                                DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                                MeioContato = s.cock.MeioContato,
                                ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                                TipoTransacao = s.cock.TipoTransacao,
                                Finalizado = s.cock.Finalizado,
                                GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                                MatriculaConsultor = s.cock.MatriculaConsultor,
                                Equipe = s.Equipe,
                                CodigoAgencia = s.cock.CodigoAgencia,
                                NomeAgencia = s.cock.NomeAgencia,
                                Especialista = s.Nome
                            })
                            .ToList() :

                            db.Cockpit
                                .Join(db.Usuario,
                                cock => cock.MatriculaConsultor,
                                usu => usu.Matricula,
                                (cock, usu) => new { cock, usu.Equipe, usu.Nome })
                                .Where(x => x.cock.Observacao.ToLower().Contains(filtros.Comentario.ToLower()) &&
                                x.cock.DataContato >= minData && x.cock.DataContato <= maxData)
                                .Select(s => new CockpitExportExcel
                                {
                                    CodFuncionalGerente = s.cock.CodFuncionalGerente,
                                    NomeGerente = s.cock.NomeGerente,
                                    CPF = s.cock.CPF,
                                    NomeCliente = s.cock.NomeCliente,
                                    Conta = s.cock.Conta,
                                    DataEncarteiramento = s.cock.DataEncarteiramento,
                                    DataContato = s.cock.DataContato,
                                    DataRetorno = s.cock.DataRetorno,
                                    Observacao = s.cock.Observacao,
                                    ContatoTeveExito = s.cock.ContatoTeveExito,
                                    DataHoraEdicaoContato = s.cock.DataHoraEdicaoContato,
                                    MeioContato = s.cock.MeioContato,
                                    ClienteNaoLocalizado = s.cock.ClienteNaoLocalizado,
                                    TipoTransacao = s.cock.TipoTransacao,
                                    Finalizado = s.cock.Finalizado,
                                    GerenteRegistrouContato = s.cock.GerenteRegistrouContato,
                                    MatriculaConsultor = s.cock.MatriculaConsultor,
                                    Equipe = s.Equipe,
                                    CodigoAgencia = s.cock.CodigoAgencia,
                                    NomeAgencia = s.cock.NomeAgencia,
                                    Especialista = s.Nome
                                })
                                .ToList();
                    }

                    break;
            }

            return cockpits;
        }

        public IQueryable<TED> FiltrarTeds(FiltrosTelas filtros, string matricula, string perfil)
        {
            var teds = default(IQueryable<TED>);
            var dataAtual = DateTime.Now.Date;
            var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(-1);
            var maxData = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));
            var modoFiltro = SetaModoFiltro(filtros);

            teds = db.TED.Include("Status").Include("Motivo").Where(t => t.Area.Contains("PGP"));

            //Filtros
            switch (modoFiltro)
            {
                case ModoDeFiltro.EspecialistaAgenciaContaSituacao:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.Agencia == filtros.Agencia &&
                        t.Conta == filtros.Conta &&
                        t.StatusId == filtros.Situacao &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.Agencia == filtros.Agencia &&
                        t.Conta == filtros.Conta &&
                        t.StatusId == filtros.Situacao &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);

                    break;

                case ModoDeFiltro.EspecialistaAgenciaSituacao:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                                teds
                                .Where(t =>
                                t.MatriculaConsultor == filtros.Especialista &&
                                t.Agencia == filtros.Agencia &&
                                t.StatusId == filtros.Situacao &&
                                t.Data >= minDate && t.Data <= maxData) :

                                teds
                                .Where(t =>
                                t.MatriculaConsultor == filtros.Especialista &&
                                t.Agencia == filtros.Agencia &&
                                t.StatusId == filtros.Situacao &&
                                t.Data >= filtros.De && t.Data <= filtros.Ate);

                    break;

                case ModoDeFiltro.EspecialistaAgenciaConta:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.Agencia == filtros.Agencia &&
                        t.Conta == filtros.Conta &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.Agencia == filtros.Agencia &&
                        t.Conta == filtros.Conta &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);

                    break;

                case ModoDeFiltro.EspecialistaAgencia:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.Agencia == filtros.Agencia &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.Agencia == filtros.Agencia &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);


                    break;

                case ModoDeFiltro.EspecialistaSituacao:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.StatusId == filtros.Situacao &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.StatusId == filtros.Situacao &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);


                    break;

                case ModoDeFiltro.ApenasEspecialista:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.MatriculaConsultor == filtros.Especialista &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);


                    break;
                case ModoDeFiltro.AgenciaContaSituacao:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.Agencia == filtros.Agencia &&
                        t.Conta == filtros.Conta &&
                        t.StatusId == filtros.Situacao &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.Agencia == filtros.Agencia &&
                        t.Conta == filtros.Conta &&
                        t.StatusId == filtros.Situacao &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);

                    break;

                case ModoDeFiltro.AgenciaConta:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.Agencia == filtros.Agencia &&
                        t.Conta == filtros.Conta &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.Agencia == filtros.Agencia &&
                        t.Conta == filtros.Conta &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);

                    break;

                case ModoDeFiltro.AgenciaSituacao:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.Agencia == filtros.Agencia &&
                        t.StatusId == filtros.Situacao &&
                        !string.IsNullOrEmpty(t.MatriculaConsultor) &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.Agencia == filtros.Agencia &&
                        t.StatusId == filtros.Situacao &&
                        !string.IsNullOrEmpty(t.MatriculaConsultor) &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);

                    break;

                case ModoDeFiltro.Agencia:

                    teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.Agencia == filtros.Agencia && !string.IsNullOrEmpty(t.MatriculaConsultor) &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.Agencia == filtros.Agencia && !string.IsNullOrEmpty(t.MatriculaConsultor) &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);

                    break;

                case ModoDeFiltro.Situacao:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {

                        teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.StatusId == filtros.Situacao &&
                        t.MatriculaConsultor == matricula &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.StatusId == filtros.Situacao &&
                        t.MatriculaConsultor == matricula &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);

                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {

                        teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Join(db.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u.MatriculaSupervisor })
                        .Where(r =>
                        r.t.StatusId == filtros.Situacao &&
                        r.MatriculaSupervisor == matricula &&
                        r.t.Data >= minDate && r.t.Data <= maxData).Select(s => s.t) :

                        teds
                        .Join(db.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u.MatriculaSupervisor })
                        .Where(r =>
                        r.t.StatusId == filtros.Situacao &&
                        r.MatriculaSupervisor == matricula &&
                        r.t.Data >= filtros.De && r.t.Data <= filtros.Ate).Select(s => s.t);


                    }
                    else
                    {
                        teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                        teds
                        .Where(t =>
                        t.StatusId == filtros.Situacao &&
                        t.Data >= minDate && t.Data <= maxData) :

                        teds
                        .Where(t =>
                        t.StatusId == filtros.Situacao &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);

                    }

                    break;
                case ModoDeFiltro.Equipe:

                    if (filtros.De.HasValue && filtros.Ate.HasValue)
                    {
                        teds = teds
                                .Join(db.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u.Equipe })
                                .Where(t => t.Equipe == filtros.Equipe &&
                                 t.t.Data >= filtros.De && t.t.Data <= filtros.Ate
                                ).Select(s => s.t);
                    }
                    else
                    {

                        teds = teds
                            .Join(db.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u.Equipe })
                            .Where(t => t.Equipe == filtros.Equipe &&
                             t.t.Data >= minDate && t.t.Data <= maxData
                            ).Select(s => s.t);

                    }

                    break;
                case ModoDeFiltro.EquipeSituacao:

                    if (filtros.De.HasValue && filtros.Ate.HasValue)
                    {
                        teds = teds
                                .Join(db.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u.Equipe })
                                .Where(t => t.Equipe == filtros.Equipe &&
                                 t.t.Data >= filtros.De && t.t.Data <= filtros.Ate
                                ).Select(s => s.t);
                    }
                    else
                    {
                        teds = !filtros.De.HasValue && !filtros.Ate.HasValue ?
                            teds
                            .Join(db.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u.Equipe })
                            .Where(t => t.Equipe == filtros.Equipe &&
                            t.t.Data >= minDate && t.t.Data <= maxData &&
                            t.t.StatusId == filtros.Situacao).Select(s => s.t) :

                            teds
                            .Join(db.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u.Equipe })
                            .Where(t => t.Equipe == filtros.Equipe &&
                            t.t.Data >= filtros.De && t.t.Data <= filtros.Ate &&
                            t.t.StatusId == filtros.Situacao).Select(s => s.t);
                    }

                    break;

                case ModoDeFiltro.ApenasData:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {
                        teds = teds
                        .Where(t =>
                        t.MatriculaConsultor == matricula &&
                        t.Data >= filtros.De && t.Data <= filtros.Ate);
                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {
                        teds = teds
                        .Join(db.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u.MatriculaSupervisor })
                        .Where(r =>
                        r.MatriculaSupervisor == matricula &&
                        r.t.Data >= filtros.De && r.t.Data <= filtros.Ate).Select(s => s.t);
                    }
                    else
                    {
                        teds = teds
                        .Join(db.Usuario, t => t.MatriculaConsultor, u => u.Matricula, (t, u) => new { t, u })
                        .Where(ted =>
                        ted.t.Data >= filtros.De && ted.t.Data <= filtros.Ate).Select(s => s.t);
                    }

                    break;
            }

            return teds;
        }

        public List<AplicacaoResgateViewModel> FiltrarAplicacaoResgate(FiltrosTelas filtros, string matricula, string perfil)
        {
            var modoFiltro = SetaModoFiltro(filtros);

            var query = default(IQueryable<AplicacaoResgateViewModel>);

            var dataAtual = DateTime.Now.Date;

            switch (modoFiltro)
            {
                case ModoDeFiltro.EspecialistaAgenciaConta:
                    query =
                        db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                        .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Nome })
                        .Where(r => r.Nome.ToUpper() == filtros.Especialista.ToUpper() &&
                            r.ar.agencia.ToString() == filtros.Agencia &&
                            r.ar.conta.ToString() == filtros.Conta)
                        .Select(s => new AplicacaoResgateViewModel
                        {
                            advisor = s.ar.advisor,
                            agencia = s.ar.agencia,
                            conta = s.ar.conta,
                            data = s.ar.data,
                            enviado = s.ar.enviado,
                            Especialista = s.Nome,
                            gerente = s.ar.gerente,
                            hora = s.ar.hora,
                            Id = s.ar.Id,
                            operacao = s.ar.operacao,
                            perif = s.ar.perif,
                            produto = s.ar.produto,
                            segmento = s.ar.segmento,
                            terminal = s.ar.terminal,
                            valor = s.ar.valor,
                            AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                            ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                            PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                            ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                            Realocou = s.ar.AplicResgateContatos.Realocou,
                            VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                            Matricula = s.ar.MatriculaConsultor
                        });

                    break;
                case ModoDeFiltro.EspecialistaAgencia:
                    query =

                        db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                        .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Nome })
                        .Where(r => r.Nome.ToUpper() == filtros.Especialista.ToUpper() &&
                            r.ar.agencia.ToString() == filtros.Agencia)
                        .Select(s => new AplicacaoResgateViewModel
                        {
                            advisor = s.ar.advisor,
                            agencia = s.ar.agencia,
                            conta = s.ar.conta,
                            data = s.ar.data,
                            enviado = s.ar.enviado,
                            Especialista = s.Nome,
                            gerente = s.ar.gerente,
                            hora = s.ar.hora,
                            Id = s.ar.Id,
                            operacao = s.ar.operacao,
                            perif = s.ar.perif,
                            produto = s.ar.produto,
                            segmento = s.ar.segmento,
                            terminal = s.ar.terminal,
                            valor = s.ar.valor,
                            AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                            ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                            PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                            ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                            Realocou = s.ar.AplicResgateContatos.Realocou,
                            VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                            Matricula = s.ar.MatriculaConsultor
                        });

                    break;
                case ModoDeFiltro.ApenasEspecialista:
                    query =
                        db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                        .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Nome })
                        .Where(r => r.Nome.ToUpper() == filtros.Especialista.ToUpper())
                        .Select(s => new AplicacaoResgateViewModel
                        {
                            advisor = s.ar.advisor,
                            agencia = s.ar.agencia,
                            conta = s.ar.conta,
                            data = s.ar.data,
                            enviado = s.ar.enviado,
                            Especialista = s.Nome,
                            gerente = s.ar.gerente,
                            hora = s.ar.hora,
                            Id = s.ar.Id,
                            operacao = s.ar.operacao,
                            perif = s.ar.perif,
                            produto = s.ar.produto,
                            segmento = s.ar.segmento,
                            terminal = s.ar.terminal,
                            valor = s.ar.valor,
                            AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                            ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                            PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                            ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                            Realocou = s.ar.AplicResgateContatos.Realocou,
                            VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                            Matricula = s.ar.MatriculaConsultor
                        });

                    break;

                case ModoDeFiltro.AgenciaConta:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {
                        query =

                           db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                            .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Nome })
                           .Where(r => r.ar.agencia.ToString() == filtros.Agencia &&
                               r.ar.conta.ToString() == filtros.Conta &&
                               r.ar.MatriculaConsultor == matricula)
                            .Select(s => new AplicacaoResgateViewModel
                            {
                                advisor = s.ar.advisor,
                                agencia = s.ar.agencia,
                                conta = s.ar.conta,
                                data = s.ar.data,
                                enviado = s.ar.enviado,
                                Especialista = s.Nome,
                                gerente = s.ar.gerente,
                                hora = s.ar.hora,
                                Id = s.ar.Id,
                                operacao = s.ar.operacao,
                                perif = s.ar.perif,
                                produto = s.ar.produto,
                                segmento = s.ar.segmento,
                                terminal = s.ar.terminal,
                                valor = s.ar.valor,
                                AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                                ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                                PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                                ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                                Realocou = s.ar.AplicResgateContatos.Realocou,
                                VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                                Matricula = s.ar.MatriculaConsultor
                            });

                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {
                        query =

                        db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                        .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.MatriculaSupervisor, u.Nome })
                        .Where(r => r.ar.agencia.ToString() == filtros.Agencia &&
                            r.ar.conta.ToString() == filtros.Conta &&
                            r.MatriculaSupervisor == matricula)
                        .Select(s => new AplicacaoResgateViewModel
                        {
                            advisor = s.ar.advisor,
                            agencia = s.ar.agencia,
                            conta = s.ar.conta,
                            data = s.ar.data,
                            enviado = s.ar.enviado,
                            Especialista = s.Nome,
                            gerente = s.ar.gerente,
                            hora = s.ar.hora,
                            Id = s.ar.Id,
                            operacao = s.ar.operacao,
                            perif = s.ar.perif,
                            produto = s.ar.produto,
                            segmento = s.ar.segmento,
                            terminal = s.ar.terminal,
                            valor = s.ar.valor,
                            AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                            ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                            PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                            ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                            Realocou = s.ar.AplicResgateContatos.Realocou,
                            VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                            Matricula = s.ar.MatriculaConsultor
                        });
                    }
                    else
                    {
                        query =

                       db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                        .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Nome })
                       .Where(r => r.ar.agencia.ToString() == filtros.Agencia &&
                           r.ar.conta.ToString() == filtros.Conta)
                         .Select(s => new AplicacaoResgateViewModel
                         {
                             advisor = s.ar.advisor,
                             agencia = s.ar.agencia,
                             conta = s.ar.conta,
                             data = s.ar.data,
                             enviado = s.ar.enviado,
                             Especialista = s.Nome,
                             gerente = s.ar.gerente,
                             hora = s.ar.hora,
                             Id = s.ar.Id,
                             operacao = s.ar.operacao,
                             perif = s.ar.perif,
                             produto = s.ar.produto,
                             segmento = s.ar.segmento,
                             terminal = s.ar.terminal,
                             valor = s.ar.valor,
                             AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                             ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                             PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                             ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                             Realocou = s.ar.AplicResgateContatos.Realocou,
                             VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                             Matricula = s.ar.MatriculaConsultor
                         });
                    }

                    break;

                case ModoDeFiltro.Agencia:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {
                        query =
                        db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                        .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Nome })
                        .Where(r => r.ar.agencia.ToString() == filtros.Agencia &&
                            r.ar.MatriculaConsultor == matricula)
                         .Select(s => new AplicacaoResgateViewModel
                         {
                             advisor = s.ar.advisor,
                             agencia = s.ar.agencia,
                             conta = s.ar.conta,
                             data = s.ar.data,
                             enviado = s.ar.enviado,
                             Especialista = s.Nome,
                             gerente = s.ar.gerente,
                             hora = s.ar.hora,
                             Id = s.ar.Id,
                             operacao = s.ar.operacao,
                             perif = s.ar.perif,
                             produto = s.ar.produto,
                             segmento = s.ar.segmento,
                             terminal = s.ar.terminal,
                             valor = s.ar.valor,
                             AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                             ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                             PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                             ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                             Realocou = s.ar.AplicResgateContatos.Realocou,
                             VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                             Matricula = s.ar.MatriculaConsultor
                         });
                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {
                        query =

                        db.AplicacaoResgate.Include(s =>s.AplicResgateContatos)
                        .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.MatriculaSupervisor, u.Nome })
                        .Where(r => r.ar.agencia.ToString() == filtros.Agencia &&
                            r.MatriculaSupervisor == matricula)
                        .Select(s => new AplicacaoResgateViewModel
                        {
                            advisor = s.ar.advisor,
                            agencia = s.ar.agencia,
                            conta = s.ar.conta,
                            data = s.ar.data,
                            enviado = s.ar.enviado,
                            Especialista = s.Nome,
                            gerente = s.ar.gerente,
                            hora = s.ar.hora,
                            Id = s.ar.Id,
                            operacao = s.ar.operacao,
                            perif = s.ar.perif,
                            produto = s.ar.produto,
                            segmento = s.ar.segmento,
                            terminal = s.ar.terminal,
                            valor = s.ar.valor,
                            AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                            ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                            PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                            ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                            Realocou = s.ar.AplicResgateContatos.Realocou,
                            VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                            Matricula = s.ar.MatriculaConsultor
                        });
                    }
                    else
                    {
                        query =

                        db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                        .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Nome })
                        .Where(r => r.ar.agencia.ToString() == filtros.Agencia)
                         .Select(s => new AplicacaoResgateViewModel
                         {
                             advisor = s.ar.advisor,
                             agencia = s.ar.agencia,
                             conta = s.ar.conta,
                             data = s.ar.data,
                             enviado = s.ar.enviado,
                             Especialista = s.Nome,
                             gerente = s.ar.gerente,
                             hora = s.ar.hora,
                             Id = s.ar.Id,
                             operacao = s.ar.operacao,
                             perif = s.ar.perif,
                             produto = s.ar.produto,
                             segmento = s.ar.segmento,
                             terminal = s.ar.terminal,
                             valor = s.ar.valor,
                             AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                             ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                             PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                             ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                             Realocou = s.ar.AplicResgateContatos.Realocou,
                             VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                             Matricula = s.ar.MatriculaConsultor
                         });
                    }

                    break;

                case ModoDeFiltro.ApenasData:

                    if (perfil == NivelAcesso.Especialista.ToString())
                    {
                        query =
                           db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                            .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Nome })
                           .Where(r =>
                               r.ar.MatriculaConsultor == matricula)
                            .Select(s => new AplicacaoResgateViewModel
                            {
                                advisor = s.ar.advisor,
                                agencia = s.ar.agencia,
                                conta = s.ar.conta,
                                data = s.ar.data,
                                enviado = s.ar.enviado,
                                Especialista = s.Nome,
                                gerente = s.ar.gerente,
                                hora = s.ar.hora,
                                Id = s.ar.Id,
                                operacao = s.ar.operacao,
                                perif = s.ar.perif,
                                produto = s.ar.produto,
                                segmento = s.ar.segmento,
                                terminal = s.ar.terminal,
                                valor = s.ar.valor,
                                AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                                ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                                PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                                ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                                Realocou = s.ar.AplicResgateContatos.Realocou,
                                VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                                Matricula = s.ar.MatriculaConsultor
                            });

                    }
                    else if (perfil == NivelAcesso.Gestor.ToString())
                    {
                        query =

                           db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                            .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.MatriculaSupervisor, u.Nome })
                           .Where(r => r.MatriculaSupervisor == matricula)
                            .Select(s => new AplicacaoResgateViewModel
                            {
                                advisor = s.ar.advisor,
                                agencia = s.ar.agencia,
                                conta = s.ar.conta,
                                data = s.ar.data,
                                enviado = s.ar.enviado,
                                Especialista = s.Nome,
                                gerente = s.ar.gerente,
                                hora = s.ar.hora,
                                Id = s.ar.Id,
                                operacao = s.ar.operacao,
                                perif = s.ar.perif,
                                produto = s.ar.produto,
                                segmento = s.ar.segmento,
                                terminal = s.ar.terminal,
                                valor = s.ar.valor,
                                AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                                ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                                PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                                ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                                Realocou = s.ar.AplicResgateContatos.Realocou,
                                VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                                Matricula = s.ar.MatriculaConsultor
                            });
                    }
                    else
                    {
                        query =

                          db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                            .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Nome })
                            .Select(s => new AplicacaoResgateViewModel
                            {
                                advisor = s.ar.advisor,
                                agencia = s.ar.agencia,
                                conta = s.ar.conta,
                                data = s.ar.data,
                                enviado = s.ar.enviado,
                                Especialista = s.Nome,
                                gerente = s.ar.gerente,
                                hora = s.ar.hora,
                                Id = s.ar.Id,
                                operacao = s.ar.operacao,
                                perif = s.ar.perif,
                                produto = s.ar.produto,
                                segmento = s.ar.segmento,
                                terminal = s.ar.terminal,
                                valor = s.ar.valor,
                                AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                                ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                                PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                                ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                                Realocou = s.ar.AplicResgateContatos.Realocou,
                                VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                                Matricula = s.ar.MatriculaConsultor
                            });
                    }

                    break;

                case ModoDeFiltro.Equipe:
                    query =
                        db.AplicacaoResgate.Include(s => s.AplicResgateContatos)
                        .Join(db.Usuario, ar => ar.MatriculaConsultor, u => u.Matricula, (ar, u) => new { ar, u.Equipe, u.Nome })
                        .Where(r => r.Equipe == filtros.Equipe)
                        .Select(s => new AplicacaoResgateViewModel
                        {
                            advisor = s.ar.advisor,
                            agencia = s.ar.agencia,
                            conta = s.ar.conta,
                            data = s.ar.data,
                            enviado = s.ar.enviado,
                            Especialista = s.Nome,
                            gerente = s.ar.gerente,
                            hora = s.ar.hora,
                            Id = s.ar.Id,
                            operacao = s.ar.operacao,
                            perif = s.ar.perif,
                            produto = s.ar.produto,
                            segmento = s.ar.segmento,
                            terminal = s.ar.terminal,
                            valor = s.ar.valor,
                            AplicouEmOutroBanco = s.ar.AplicResgateContatos.AplicouEmOutroBanco,
                            ContatouCliente = s.ar.AplicResgateContatos.ContatouCliente,
                            PagamentosUsoDoRecurso = s.ar.AplicResgateContatos.PagamentosUsoDoRecurso,
                            ProblemasDeRelacionamento = s.ar.AplicResgateContatos.ProblemasDeRelacionamento,
                            Realocou = s.ar.AplicResgateContatos.Realocou,
                            VaiAnalisarOferta = s.ar.AplicResgateContatos.VaiAnalisarOferta,
                            Matricula = s.ar.MatriculaConsultor
                        });

                    break;
            }

            if (filtros.De.HasValue && filtros.Ate.HasValue)
            {
                query = query.Where(r => r.data >= filtros.De && r.data <= filtros.Ate);
            }
            else
            {
                query = query.Where(r => r.data == dataAtual);
            }

            var result = query.ToList();

            return result;
        }
    }
}