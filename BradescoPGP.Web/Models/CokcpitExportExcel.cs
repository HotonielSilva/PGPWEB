using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class CockpitExportExcel: Cockpit
    {
        public string Equipe { get; set; }
        public string Especialista { get; set; }

        public CockpitExportExcel Mapear(Cockpit cockpit, string equipe, string especialista)
        {
            return new CockpitExportExcel
            {
                CodFuncionalGerente = cockpit.CodFuncionalGerente,
                NomeGerente = cockpit.NomeGerente,
                CPF = cockpit.CPF,
                NomeCliente = cockpit.NomeCliente,
                Conta = cockpit.Conta,
                DataEncarteiramento = cockpit.DataEncarteiramento,
                DataContato = cockpit.DataContato,
                DataRetorno = cockpit.DataRetorno,
                Observacao = cockpit.Observacao,
                ContatoTeveExito = cockpit.ContatoTeveExito,
                DataHoraEdicaoContato = cockpit.DataHoraEdicaoContato,
                MeioContato = cockpit.MeioContato,
                ClienteNaoLocalizado = cockpit.ClienteNaoLocalizado,
                TipoTransacao = cockpit.TipoTransacao,
                Finalizado = cockpit.Finalizado,
                GerenteRegistrouContato = cockpit.GerenteRegistrouContato,
                MatriculaConsultor = cockpit.MatriculaConsultor,
                Equipe = equipe,
                CodigoAgencia = cockpit.CodigoAgencia,
                NomeAgencia = cockpit.NomeAgencia,

            };
        }

    }
}