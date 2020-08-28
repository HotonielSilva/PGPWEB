using BradescoPGP.Common;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace BradescoPGP.Web.Services
{
    public class CarteiraClienteService
    {
        public IQueryable<CarteiraClienteViewModel> ObterClusterizacoes(PGPEntities db, string matricula, NivelAcesso perfil)
        {
            //var clusterizacoes = new List<CarteiraClienteViewModel>();
                if(perfil == NivelAcesso.Especialista)
                {
                    return db.Clusterizacoes.Join(db.Encarteiramento,
                    clus => new { agencia = clus.AGENCIA.ToString(), conta = clus.CONTA.ToString() },
                    enc => new { agencia = enc.Agencia, conta = enc.Conta },
                    (clus, enc) => new { clus,  enc.Matricula })

                    .Join(db.Cockpit,
                    res => new { agencia = res.clus.AGENCIA, conta = res.clus.CONTA },
                    cock => new { agencia = cock.CodigoAgencia, conta = cock.Conta },
                    (res, cock) => new { res.clus, res.Matricula, cock.NomeCliente,
                        cock.DataContato, cock.ContatoTeveExito })

                    .Join(db.Usuario,
                    resp => resp.Matricula,
                    usu => usu.Matricula,
                    (resp, usu) => new { resp.clus, resp.Matricula, resp.NomeCliente, resp.DataContato, resp.ContatoTeveExito, usu.Nome, usu.Equipe })
                    .Where(res => res.Matricula == matricula)

                    .GroupBy(g => new { g.clus.AGENCIA, g.clus.CONTA })

                    .Select(e => new CarteiraClienteViewModel
                    {
                        Matricula = e.FirstOrDefault().Matricula,
                        Especialista = e.FirstOrDefault().Nome,
                        NomeCliente = e.FirstOrDefault().NomeCliente,
                        Agencia = e.FirstOrDefault().clus.AGENCIA.ToString(),
                        Conta = e.FirstOrDefault().clus.CONTA.ToString(),
                        CPF = e.FirstOrDefault().clus.CPF_CNPJ,
                        PerfilApi = e.FirstOrDefault().clus.PERFIL_API,
                        NIVEL_DESENQ_FX_RISCO = e.FirstOrDefault().clus.NIVEL_DESENQ_FX_RISCO,
                        MES_VCTO_API = e.FirstOrDefault().clus.MES_VCTO_API,
                        //SALDO_TOTAL_M3 = e.FirstOrDefault().clus.SALDO_TOTAL_M3,
                        SALDO_TOTAL = e.FirstOrDefault().clus.SALDO_TOTAL,
                        //SALDO_CORRETORA_BRA = e.FirstOrDefault().clus.SALDO_CORRETORA_BRA,
                        //SALDO_CORRETORA_AGORA = e.FirstOrDefault().clus.SALDO_CORRETORA_AGORA,
                        //SALDO_CORRETORA = e.FirstOrDefault().clus.SALDO_CORRETORA,
                        //SALDO_PREVIDENCIA = e.FirstOrDefault().clus.SALDO_PREVIDENCIA,
                        //SALDO_POUPANCA = e.FirstOrDefault().clus.SALDO_POUPANCA,
                        //SALDO_INVESTS = e.FirstOrDefault().clus.SALDO_INVESTS,
                        //SALDO_DAV_20K = e.FirstOrDefault().clus.SALDO_DAV_20K,
                        //SALDO_COMPROMISSADAS = e.FirstOrDefault().clus.SALDO_COMPROMISSADAS,
                        //SALDO_ISENTOS = e.FirstOrDefault().clus.SALDO_ISENTOS,
                        //SALDO_LF = e.FirstOrDefault().clus.SALDO_LF,
                        //SALDO_CDB = e.FirstOrDefault().clus.SALDO_CDB,
                        //SALDO_FUNDOS = e.FirstOrDefault().clus.SALDO_FUNDOS,
                        NomeGerente = e.FirstOrDefault().clus.GER_RELC,
                        UltimaTentativa = e.Max(d=> d.DataContato),
                        UltimoContato = e.FirstOrDefault().ContatoTeveExito ? (DateTime?)e.FirstOrDefault().DataContato : null,
                        DiasCorridosÚltimoContato = DbFunctions.DiffDays(e.FirstOrDefault().DataContato, DateTime.Now).Value,
                        Situacao = e.FirstOrDefault().clus.Situacao,
                        Equipe = e.FirstOrDefault().Equipe
                    });

                }
                else if(perfil == NivelAcesso.Gestor)
                {
                    return db.Clusterizacoes.Join(db.Encarteiramento,
                     clus => new { agencia = clus.AGENCIA.ToString(), conta = clus.CONTA.ToString() },
                     enc => new { agencia = enc.Agencia, conta = enc.Conta },
                     (clus, enc) => new { clus, matricula = enc.Matricula })

                     .Join(db.Cockpit,
                     res => new { agencia = res.clus.AGENCIA, conta = res.clus.CONTA },
                     cock => new { agencia = cock.CodigoAgencia, conta = cock.Conta },
                     (res, cock) => new {
                         res.clus,
                         res.matricula,
                         cock.NomeCliente,
                         cock.DataContato,
                         cock.ContatoTeveExito
                     })

                     .Join(db.Usuario,
                        resp => resp.matricula,
                        usu => usu.Matricula,
                        (resp, usu) => new { resp.clus, resp.matricula, resp.NomeCliente, resp.DataContato, resp.ContatoTeveExito, usu.Nome, usu.Equipe, usu.MatriculaSupervisor  })
                        .Where(r => r.MatriculaSupervisor == matricula)
                        .GroupBy(g => new { g.clus.AGENCIA, g.clus.CONTA })

                    .Select(e => new CarteiraClienteViewModel
                    {
                        Matricula = e.FirstOrDefault().matricula,
                        Especialista = e.FirstOrDefault().Nome,
                        NomeCliente = e.FirstOrDefault().NomeCliente,
                        Agencia = e.FirstOrDefault().clus.AGENCIA.ToString(),
                        Conta = e.FirstOrDefault().clus.CONTA.ToString(),
                        CPF = e.FirstOrDefault().clus.CPF_CNPJ,
                        PerfilApi = e.FirstOrDefault().clus.PERFIL_API,
                        NIVEL_DESENQ_FX_RISCO = e.FirstOrDefault().clus.NIVEL_DESENQ_FX_RISCO,
                        MES_VCTO_API = e.FirstOrDefault().clus.MES_VCTO_API,
                        //SALDO_TOTAL_M3 = e.FirstOrDefault().clus.SALDO_TOTAL_M3,
                        SALDO_TOTAL = e.FirstOrDefault().clus.SALDO_TOTAL,
                        //SALDO_CORRETORA_BRA = e.FirstOrDefault().clus.SALDO_CORRETORA_BRA,
                        //SALDO_CORRETORA_AGORA = e.FirstOrDefault().clus.SALDO_CORRETORA_AGORA,
                        //SALDO_CORRETORA = e.FirstOrDefault().clus.SALDO_CORRETORA,
                        //SALDO_PREVIDENCIA = e.FirstOrDefault().clus.SALDO_PREVIDENCIA,
                        //SALDO_POUPANCA = e.FirstOrDefault().clus.SALDO_POUPANCA,
                        //SALDO_INVESTS = e.FirstOrDefault().clus.SALDO_INVESTS,
                        //SALDO_DAV_20K = e.FirstOrDefault().clus.SALDO_DAV_20K,
                        //SALDO_COMPROMISSADAS = e.FirstOrDefault().clus.SALDO_COMPROMISSADAS,
                        //SALDO_ISENTOS = e.FirstOrDefault().clus.SALDO_ISENTOS,
                        //SALDO_LF = e.FirstOrDefault().clus.SALDO_LF,
                        //SALDO_CDB = e.FirstOrDefault().clus.SALDO_CDB,
                        //SALDO_FUNDOS = e.FirstOrDefault().clus.SALDO_FUNDOS,
                        NomeGerente = e.FirstOrDefault().clus.GER_RELC,
                        UltimaTentativa = e.Max(d => d.DataContato),
                        UltimoContato = e.FirstOrDefault().ContatoTeveExito ? (DateTime?)e.FirstOrDefault().DataContato : null,
                        DiasCorridosÚltimoContato = DbFunctions.DiffDays(e.FirstOrDefault().DataContato, DateTime.Now).Value,
                        Situacao = e.FirstOrDefault().clus.Situacao,
                        Equipe = e.FirstOrDefault().Equipe
                    });
            }
                else
                {
                return db.Clusterizacoes.Join(db.Encarteiramento,
                 clus => new { agencia = clus.AGENCIA.ToString(), conta = clus.CONTA.ToString() },
                 enc => new { agencia = enc.Agencia, conta = enc.Conta },
                 (clus, enc) => new { clus, matricula = enc.Matricula })

                 .Join(db.Cockpit,
                 res => new { agencia = res.clus.AGENCIA, conta = res.clus.CONTA },
                 cock => new { agencia = cock.CodigoAgencia, conta = cock.Conta },
                 (res, cock) => new {
                     res.clus,
                     res.matricula,
                     cock.NomeCliente,
                     cock.DataContato,
                     cock.ContatoTeveExito
                 })

                 .Join(db.Usuario,
                    resp => resp.matricula,
                    usu => usu.Matricula,
                    (resp, usu) => new { resp.clus, resp.matricula, resp.NomeCliente, resp.DataContato, resp.ContatoTeveExito, usu.Nome, usu.Equipe, usu.MatriculaSupervisor })

                .GroupBy(g => new { g.clus.AGENCIA, g.clus.CONTA })

                .Select(e => new CarteiraClienteViewModel
                {
                    Matricula = e.FirstOrDefault().matricula,
                    Especialista = e.FirstOrDefault().Nome,
                    NomeCliente = e.FirstOrDefault().NomeCliente,
                    Agencia = e.FirstOrDefault().clus.AGENCIA.ToString(),
                    Conta = e.FirstOrDefault().clus.CONTA.ToString(),
                    CPF = e.FirstOrDefault().clus.CPF_CNPJ,
                    PerfilApi = e.FirstOrDefault().clus.PERFIL_API,
                    NIVEL_DESENQ_FX_RISCO = e.FirstOrDefault().clus.NIVEL_DESENQ_FX_RISCO,
                    MES_VCTO_API = e.FirstOrDefault().clus.MES_VCTO_API,
                    //SALDO_TOTAL_M3 = e.FirstOrDefault().clus.SALDO_TOTAL_M3,
                    SALDO_TOTAL = e.FirstOrDefault().clus.SALDO_TOTAL,
                    //SALDO_CORRETORA_BRA = e.FirstOrDefault().clus.SALDO_CORRETORA_BRA,
                    //SALDO_CORRETORA_AGORA = e.FirstOrDefault().clus.SALDO_CORRETORA_AGORA,
                    //SALDO_CORRETORA = e.FirstOrDefault().clus.SALDO_CORRETORA,
                    //SALDO_PREVIDENCIA = e.FirstOrDefault().clus.SALDO_PREVIDENCIA,
                    //SALDO_POUPANCA = e.FirstOrDefault().clus.SALDO_POUPANCA,
                    //SALDO_INVESTS = e.FirstOrDefault().clus.SALDO_INVESTS,
                    //SALDO_DAV_20K = e.FirstOrDefault().clus.SALDO_DAV_20K,
                    //SALDO_COMPROMISSADAS = e.FirstOrDefault().clus.SALDO_COMPROMISSADAS,
                    //SALDO_ISENTOS = e.FirstOrDefault().clus.SALDO_ISENTOS,
                    //SALDO_LF = e.FirstOrDefault().clus.SALDO_LF,
                    //SALDO_CDB = e.FirstOrDefault().clus.SALDO_CDB,
                    //SALDO_FUNDOS = e.FirstOrDefault().clus.SALDO_FUNDOS,
                    NomeGerente = e.FirstOrDefault().clus.GER_RELC,
                    UltimaTentativa = e.Max(d => d.DataContato),
                    UltimoContato = e.FirstOrDefault().ContatoTeveExito ? (DateTime?)e.FirstOrDefault().DataContato : null,
                    DiasCorridosÚltimoContato = DbFunctions.DiffDays(e.FirstOrDefault().DataContato, DateTime.Now).Value,
                    Situacao = e.FirstOrDefault().clus.Situacao,
                    Equipe = e.FirstOrDefault().Equipe
                });
            }
        }

        public IQueryable<CarteiraClienteTopTierViewModel> ObterClusterizacoesTopTier(PGPEntities db)
        {
            return db.vwClusterTopTier.ToList().ConvertAll(d => CarteiraClienteTopTierViewModel.Mapear(d)).AsQueryable();
        }

        public IQueryable<CarteiraClienteExportExcel> ObterClusterizacoesExportacao(PGPEntities db, string matricula, NivelAcesso perfil)
        {
            //var clusterizacoes = new List<CarteiraClienteViewModel>();
            if (perfil == NivelAcesso.Especialista)
            {
                return db.Clusterizacoes.Join(db.Encarteiramento,
                clus => new { agencia = clus.AGENCIA.ToString(), conta = clus.CONTA.ToString() },
                enc => new { agencia = enc.Agencia, conta = enc.Conta },
                (clus, enc) => new { clus, enc.Matricula })

                .Join(db.Cockpit,
                res => new { agencia = res.clus.AGENCIA, conta = res.clus.CONTA },
                cock => new { agencia = cock.CodigoAgencia, conta = cock.Conta },
                (res, cock) => new {
                    res.clus,
                    res.Matricula,
                    cock.NomeCliente,
                    cock.DataContato,
                    cock.ContatoTeveExito
                })

                .Join(db.Usuario,
                resp => resp.Matricula,
                usu => usu.Matricula,
                (resp, usu) => new { resp.clus, resp.Matricula, resp.NomeCliente, resp.DataContato, resp.ContatoTeveExito, usu.Nome, usu.Equipe })
                .Where(res => res.Matricula == matricula)
                .GroupBy(g => new { g.clus.AGENCIA, g.clus.CONTA })

                .Select(e => new CarteiraClienteExportExcel
                {
                    Matricula = e.FirstOrDefault().Matricula,
                    Especialista = e.FirstOrDefault().Nome,
                    NomeCliente = e.FirstOrDefault().NomeCliente,
                    Agencia = e.FirstOrDefault().clus.AGENCIA.ToString(),
                    Conta = e.FirstOrDefault().clus.CONTA.ToString(),
                    CPF = e.FirstOrDefault().clus.CPF_CNPJ,
                    PerfilApi = e.FirstOrDefault().clus.PERFIL_API,
                    NIVEL_DESENQ_FX_RISCO = e.FirstOrDefault().clus.NIVEL_DESENQ_FX_RISCO,
                    MES_VCTO_API = e.FirstOrDefault().clus.MES_VCTO_API,
                    SALDO_TOTAL_M3 = e.FirstOrDefault().clus.SALDO_TOTAL_M3,
                    SALDO_TOTAL = e.FirstOrDefault().clus.SALDO_TOTAL,
                    SALDO_CORRETORA_BRA = e.FirstOrDefault().clus.SALDO_CORRETORA_BRA,
                    SALDO_CORRETORA_AGORA = e.FirstOrDefault().clus.SALDO_CORRETORA_AGORA,
                    SALDO_CORRETORA = e.FirstOrDefault().clus.SALDO_CORRETORA,
                    SALDO_PREVIDENCIA = e.FirstOrDefault().clus.SALDO_PREVIDENCIA,
                    SALDO_POUPANCA = e.FirstOrDefault().clus.SALDO_POUPANCA,
                    SALDO_INVESTS = e.FirstOrDefault().clus.SALDO_INVESTS,
                    SALDO_DAV_20K = e.FirstOrDefault().clus.SALDO_DAV_20K,
                    SALDO_COMPROMISSADAS = e.FirstOrDefault().clus.SALDO_COMPROMISSADAS,
                    SALDO_ISENTOS = e.FirstOrDefault().clus.SALDO_ISENTOS,
                    SALDO_LF = e.FirstOrDefault().clus.SALDO_LF,
                    SALDO_CDB = e.FirstOrDefault().clus.SALDO_CDB,
                    SALDO_FUNDOS = e.FirstOrDefault().clus.SALDO_FUNDOS,
                    NomeGerente = e.FirstOrDefault().clus.GER_RELC,
                    UltimaTentativa = e.Max(d => d.DataContato),
                    UltimoContato = e.FirstOrDefault().ContatoTeveExito ? (DateTime?)e.FirstOrDefault().DataContato : null,
                    DiasCorridosÚltimoContato = DbFunctions.DiffDays(e.FirstOrDefault().DataContato, DateTime.Now).Value,
                    Situacao = e.FirstOrDefault().clus.Situacao,
                    Equipe = e.FirstOrDefault().Equipe
                });

            }
            else if (perfil == NivelAcesso.Gestor)
            {
                return db.Clusterizacoes.Join(db.Encarteiramento,
                 clus => new { agencia = clus.AGENCIA.ToString(), conta = clus.CONTA.ToString() },
                 enc => new { agencia = enc.Agencia, conta = enc.Conta },
                 (clus, enc) => new { clus, matricula = enc.Matricula })

                 .Join(db.Cockpit,
                 res => new { agencia = res.clus.AGENCIA, conta = res.clus.CONTA },
                 cock => new { agencia = cock.CodigoAgencia, conta = cock.Conta },
                 (res, cock) => new {
                     res.clus,
                     res.matricula,
                     cock.NomeCliente,
                     cock.DataContato,
                     cock.ContatoTeveExito
                 })

                 .Join(db.Usuario,
                    resp => resp.matricula,
                    usu => usu.Matricula,
                    (resp, usu) => new { resp.clus, resp.matricula, resp.NomeCliente, resp.DataContato, resp.ContatoTeveExito, usu.Nome, usu.Equipe, usu.MatriculaSupervisor })
                    .Where(r => r.MatriculaSupervisor == matricula)
                    .GroupBy(g => new { g.clus.AGENCIA, g.clus.CONTA })

                .Select(e => new CarteiraClienteExportExcel
                {
                    Matricula = e.FirstOrDefault().matricula,
                    Especialista = e.FirstOrDefault().Nome,
                    NomeCliente = e.FirstOrDefault().NomeCliente,
                    Agencia = e.FirstOrDefault().clus.AGENCIA.ToString(),
                    Conta = e.FirstOrDefault().clus.CONTA.ToString(),
                    CPF = e.FirstOrDefault().clus.CPF_CNPJ,
                    PerfilApi = e.FirstOrDefault().clus.PERFIL_API,
                    NIVEL_DESENQ_FX_RISCO = e.FirstOrDefault().clus.NIVEL_DESENQ_FX_RISCO,
                    MES_VCTO_API = e.FirstOrDefault().clus.MES_VCTO_API,
                    SALDO_TOTAL_M3 = e.FirstOrDefault().clus.SALDO_TOTAL_M3,
                    SALDO_TOTAL = e.FirstOrDefault().clus.SALDO_TOTAL,
                    SALDO_CORRETORA_BRA = e.FirstOrDefault().clus.SALDO_CORRETORA_BRA,
                    SALDO_CORRETORA_AGORA = e.FirstOrDefault().clus.SALDO_CORRETORA_AGORA,
                    SALDO_CORRETORA = e.FirstOrDefault().clus.SALDO_CORRETORA,
                    SALDO_PREVIDENCIA = e.FirstOrDefault().clus.SALDO_PREVIDENCIA,
                    SALDO_POUPANCA = e.FirstOrDefault().clus.SALDO_POUPANCA,
                    SALDO_INVESTS = e.FirstOrDefault().clus.SALDO_INVESTS,
                    SALDO_DAV_20K = e.FirstOrDefault().clus.SALDO_DAV_20K,
                    SALDO_COMPROMISSADAS = e.FirstOrDefault().clus.SALDO_COMPROMISSADAS,
                    SALDO_ISENTOS = e.FirstOrDefault().clus.SALDO_ISENTOS,
                    SALDO_LF = e.FirstOrDefault().clus.SALDO_LF,
                    SALDO_CDB = e.FirstOrDefault().clus.SALDO_CDB,
                    SALDO_FUNDOS = e.FirstOrDefault().clus.SALDO_FUNDOS,
                    NomeGerente = e.FirstOrDefault().clus.GER_RELC,
                    UltimaTentativa = e.Max(d => d.DataContato),
                    UltimoContato = e.FirstOrDefault().ContatoTeveExito ? (DateTime?)e.FirstOrDefault().DataContato : null,
                    DiasCorridosÚltimoContato = DbFunctions.DiffDays(e.FirstOrDefault().DataContato, DateTime.Now).Value,
                    Situacao = e.FirstOrDefault().clus.Situacao,
                    Equipe = e.FirstOrDefault().Equipe
                });
            }
            else
            {
                return db.Clusterizacoes.Join(db.Encarteiramento,
                 clus => new { agencia = clus.AGENCIA.ToString(), conta = clus.CONTA.ToString() },
                 enc => new { agencia = enc.Agencia, conta = enc.Conta },
                 (clus, enc) => new { clus, matricula = enc.Matricula })

                 .Join(db.Cockpit,
                 res => new { agencia = res.clus.AGENCIA, conta = res.clus.CONTA },
                 cock => new { agencia = cock.CodigoAgencia, conta = cock.Conta },
                 (res, cock) => new {
                     res.clus,
                     res.matricula,
                     cock.NomeCliente,
                     cock.DataContato,
                     cock.ContatoTeveExito
                 })

                 .Join(db.Usuario,
                    resp => resp.matricula,
                    usu => usu.Matricula,
                    (resp, usu) => new { resp.clus, resp.matricula, resp.NomeCliente, resp.DataContato, resp.ContatoTeveExito, usu.Nome, usu.Equipe, usu.MatriculaSupervisor })

                .GroupBy(g => new { g.clus.AGENCIA, g.clus.CONTA })

                .Select(e => new CarteiraClienteExportExcel
                {
                    Matricula = e.FirstOrDefault().matricula,
                    Especialista = e.FirstOrDefault().Nome,
                    NomeCliente = e.FirstOrDefault().NomeCliente,
                    Agencia = e.FirstOrDefault().clus.AGENCIA.ToString(),
                    Conta = e.FirstOrDefault().clus.CONTA.ToString(),
                    CPF = e.FirstOrDefault().clus.CPF_CNPJ,
                    PerfilApi = e.FirstOrDefault().clus.PERFIL_API,
                    NIVEL_DESENQ_FX_RISCO = e.FirstOrDefault().clus.NIVEL_DESENQ_FX_RISCO,
                    MES_VCTO_API = e.FirstOrDefault().clus.MES_VCTO_API,
                    SALDO_TOTAL_M3 = e.FirstOrDefault().clus.SALDO_TOTAL_M3,
                    SALDO_TOTAL = e.FirstOrDefault().clus.SALDO_TOTAL,
                    SALDO_CORRETORA_BRA = e.FirstOrDefault().clus.SALDO_CORRETORA_BRA,
                    SALDO_CORRETORA_AGORA = e.FirstOrDefault().clus.SALDO_CORRETORA_AGORA,
                    SALDO_CORRETORA = e.FirstOrDefault().clus.SALDO_CORRETORA,
                    SALDO_PREVIDENCIA = e.FirstOrDefault().clus.SALDO_PREVIDENCIA,
                    SALDO_POUPANCA = e.FirstOrDefault().clus.SALDO_POUPANCA,
                    SALDO_INVESTS = e.FirstOrDefault().clus.SALDO_INVESTS,
                    SALDO_DAV_20K = e.FirstOrDefault().clus.SALDO_DAV_20K,
                    SALDO_COMPROMISSADAS = e.FirstOrDefault().clus.SALDO_COMPROMISSADAS,
                    SALDO_ISENTOS = e.FirstOrDefault().clus.SALDO_ISENTOS,
                    SALDO_LF = e.FirstOrDefault().clus.SALDO_LF,
                    SALDO_CDB = e.FirstOrDefault().clus.SALDO_CDB,
                    SALDO_FUNDOS = e.FirstOrDefault().clus.SALDO_FUNDOS,
                    NomeGerente = e.FirstOrDefault().clus.GER_RELC,
                    UltimaTentativa = e.Max(d => d.DataContato),
                    UltimoContato = e.FirstOrDefault().ContatoTeveExito ? (DateTime?)e.FirstOrDefault().DataContato : null,
                    DiasCorridosÚltimoContato = DbFunctions.DiffDays(e.FirstOrDefault().DataContato, DateTime.Now).Value,
                    Situacao = e.FirstOrDefault().clus.Situacao,
                    Equipe = e.FirstOrDefault().Equipe
                });
            }

        }

    }
}