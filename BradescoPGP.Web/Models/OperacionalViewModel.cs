using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class OperacionalViewModel
    {
        public int Id { get; set; }
        public string Segmento { get; set; }
        public string Lideranca { get; set; }
        public string ConsultorMatriz { get; set; }
        public string ConsultorPGP { get; set; }
        public string NomeParticipante { get; set; }
        public string CPF { get; set; }
        public decimal SaldoPrevidencia { get; set; }
        public decimal ValorPrevistoSaida { get; set; }
        public string NomeEntidade { get; set; }
        public DateTime DataInicioProcesso { get; set; }
        public DateTime? PrazoAtendimento { get; set; }
        public DateTime? DataRef { get; set; }
        public string CodIdentificadorProcesso { get; set; }
        public string CodIdentificadorProposta { get; set; }
        public string SUSEPCedente { get; set; }
        public string SUSEPCessionaria { get; set; }
        public string CIDTFDCNPJCedente { get; set; }
        public string CIDTFDCNPJCessionaria { get; set; }
        public int StatusId { get; set; }
        public int? MotivoId { get; set; }
        public int? SubMotivoId { get; set; }
        public string MatriculaConsultor { get; set; }
        public decimal? ValorRetido { get; set; }
        public string Observacao { get; set; }
        public int? Agencia { get; set; }
        public int? Conta { get; set; }
        public DateTime? DataConclusao { get; set; }
        public DateTime? DataInclusao { get; set; }

        public string Especialista { get; set; }
        public string Motivo { get; set; }
        public string Status { get; set; }
        public string SubMotivo { get; set; }
        public int? SubStatusId { get; set; }
        public string SubStatus { get; set; }
        public List<KeyValuePair<int,string>> MotivosCombo { get; set; }

        public static OperacionalViewModel Mapear(Solicitacao solicitacao)
        {
            var opeViewModel = new OperacionalViewModel();
            
            opeViewModel.Id = solicitacao.Id;
            opeViewModel.CPF = solicitacao.CPF;
            opeViewModel.DataConclusao = solicitacao.DataConclusao;
            opeViewModel.Segmento = solicitacao.Segmento;
            opeViewModel.DataInicioProcesso = solicitacao.DataInicioProcesso;
            opeViewModel.Lideranca = solicitacao.Lideranca;
            opeViewModel.MatriculaConsultor = solicitacao.MatriculaConsultor;
            opeViewModel.MotivoId = solicitacao.MotivoId;
            opeViewModel.NomeEntidade = solicitacao.NomeEntidade;
            opeViewModel.NomeParticipante = solicitacao.NomeParticipante;
            opeViewModel.Observacao = solicitacao.Observacao;
            opeViewModel.SaldoPrevidencia = solicitacao.SaldoPrevidencia;
            opeViewModel.StatusId = solicitacao.StatusId;
            opeViewModel.SubMotivoId = solicitacao.SubMotivoId;
            opeViewModel.SUSEPCedente = solicitacao.SUSEPCedente;
            opeViewModel.SUSEPCessionaria = solicitacao.SUSEPCessionaria;
            opeViewModel.ValorPrevistoSaida = solicitacao.ValorPrevistoSaida;
            opeViewModel.ValorRetido = solicitacao.ValorRetido;
            opeViewModel.Conta = solicitacao.Conta;
            opeViewModel.Agencia =solicitacao.Agencia;
            opeViewModel.CIDTFDCNPJCedente = solicitacao.CIDTFDCNPJCedente;
            opeViewModel.CIDTFDCNPJCessionaria = solicitacao.CIDTFDCNPJCessionaria;
            opeViewModel.CodIdentificadorProcesso = solicitacao.CodIdentificadorProcesso;
            opeViewModel.CodIdentificadorProposta = solicitacao.CodIdentificadorProposta;
            opeViewModel.ConsultorMatriz = solicitacao.ConsultorMatriz;
            opeViewModel.ConsultorPGP = solicitacao.ConsultorPGP;
            opeViewModel.DataInclusao = solicitacao.DataInclusao;
            opeViewModel.DataRef = solicitacao.DataRef;
            opeViewModel.PrazoAtendimento =solicitacao.PrazoAtendimento;
            opeViewModel.Status = solicitacao.Status?.Descricao;
            opeViewModel.Motivo = solicitacao.Motivo?.Descricao;
            opeViewModel.SubStatusId = solicitacao.SubStatusId;
            opeViewModel.SubStatus = solicitacao.SubStatus?.Descricao;

            return opeViewModel;
        }
    }
}