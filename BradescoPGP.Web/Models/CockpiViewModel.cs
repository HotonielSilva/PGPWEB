using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class CockpitViewModel
    {
        public int Id { get; set; }
        public int CodFuncionalGerente { get; set; }
        public string NomeGerente { get; set; }
        public string CPF { get; set; }
        public string NomeCliente { get; set; }
        public int CodigoAgencia { get; set; }
        public string NomeAgencia { get; set; }
        public int Conta { get; set; }
        public DateTime? DataEncarteiramento { get; set; }
        public DateTime DataContato { get; set; }
        public DateTime? DataRetorno { get; set; }
        public string Observacao { get; set; }
        public bool ContatoTeveExito { get; set; }
        public DateTime? DataHoraEdicaoContato { get; set; }
        public string MeioContato { get; set; }
        public bool ClienteNaoLocalizado { get; set; }
        public string TipoTransacao { get; set; }
        public bool? Finalizado { get; set; }
        public int? GerenteRegistrouContato { get; set; }
        public string MatriculaConsultor { get; set; }

        public static CockpitViewModel Mapear(Cockpit cockpit)
        {
            if (cockpit == null)
                return new CockpitViewModel();

            return new CockpitViewModel
            {
                ClienteNaoLocalizado = cockpit.ClienteNaoLocalizado,
                CodFuncionalGerente = cockpit.CodFuncionalGerente,
                CodigoAgencia = cockpit.CodigoAgencia,
                Conta = cockpit.Conta,
                ContatoTeveExito = cockpit.ContatoTeveExito,
                CPF = cockpit.CPF,
                DataContato = cockpit.DataContato,
                DataEncarteiramento = cockpit.DataEncarteiramento,
                DataHoraEdicaoContato = cockpit.DataHoraEdicaoContato,
                DataRetorno = cockpit.DataRetorno,
                Finalizado = cockpit.Finalizado,
                GerenteRegistrouContato = cockpit.GerenteRegistrouContato,
                Id = cockpit.Id,
                MatriculaConsultor = cockpit.MatriculaConsultor,
                MeioContato = cockpit.MeioContato,
                NomeAgencia = HttpUtility.HtmlDecode(cockpit.NomeAgencia),
                NomeCliente = HttpUtility.HtmlDecode(cockpit.NomeCliente),
                NomeGerente = HttpUtility.HtmlDecode(cockpit.NomeGerente),
                Observacao = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(cockpit.Observacao)),
                TipoTransacao = cockpit.TipoTransacao
            };
        }
    }
}