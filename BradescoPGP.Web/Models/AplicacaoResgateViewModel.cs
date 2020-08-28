using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class AplicacaoResgateViewModel
    {
        public int Id { get; set; }
        public int agencia { get; set; }
        public int conta { get; set; }
        public DateTime data { get; set; }
        public TimeSpan hora { get; set; }
        public string operacao { get; set; }
        public string perif { get; set; }
        public string produto { get; set; }
        public string terminal { get; set; }
        public decimal valor { get; set; }
        public string gerente { get; set; }
        public string advisor { get; set; }
        public string segmento { get; set; }
        public bool? enviado { get; set; }
        public string Especialista { get; set; }
        public bool? ContatouCliente { get; set; }
        public bool? Realocou { get; set; }
        public bool? PagamentosUsoDoRecurso { get; set; }
        public bool? AplicouEmOutroBanco { get; set; }
        public bool? ProblemasDeRelacionamento { get; set; }
        public bool? VaiAnalisarOferta { get; set; }
        public string Matricula { get; set; }

        public static AplicacaoResgateViewModel Mapear(AplicacaoResgate aplicacaoResgate, string especialista)
        {
            return new AplicacaoResgateViewModel
            {
                advisor = aplicacaoResgate.advisor,
                agencia = aplicacaoResgate.agencia,
                conta = aplicacaoResgate.conta,
                data = aplicacaoResgate.data,
                enviado = aplicacaoResgate.enviado,
                Especialista = especialista,
                gerente = aplicacaoResgate.gerente,
                hora = aplicacaoResgate.hora,
                Id = aplicacaoResgate.Id,
                operacao = aplicacaoResgate.operacao,
                perif = aplicacaoResgate.perif,
                produto = aplicacaoResgate.produto,
                segmento = aplicacaoResgate.segmento,
                terminal = aplicacaoResgate.terminal,
                valor = aplicacaoResgate.valor,
                Matricula = aplicacaoResgate.MatriculaConsultor,
                AplicouEmOutroBanco = aplicacaoResgate.AplicResgateContatos?.AplicouEmOutroBanco,
                ContatouCliente = aplicacaoResgate.AplicResgateContatos?.ContatouCliente,
                PagamentosUsoDoRecurso = aplicacaoResgate.AplicResgateContatos?.PagamentosUsoDoRecurso,
                ProblemasDeRelacionamento = aplicacaoResgate.AplicResgateContatos?.ProblemasDeRelacionamento,
                Realocou = aplicacaoResgate.AplicResgateContatos?.Realocou,
                VaiAnalisarOferta = aplicacaoResgate.AplicResgateContatos?.VaiAnalisarOferta
            };
        }
    }

}