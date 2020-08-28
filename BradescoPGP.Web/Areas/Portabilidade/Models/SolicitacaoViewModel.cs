using BradescoPGP.Repositorio;
using System;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class SolicitacaoViewModel
    {
        public int Id { get; set; }
        public string Segmento { get; set; }
        public string Lideranca { get; set; }
        public string ConsultorMatriz { get; set; }
        public string ConsultorPGP { get; set; }
        public string NomeParticipante { get; set; }
        public long CPF { get; set; }
        public decimal SaldoPrevidencia { get; set; }
        public decimal ValorPrevistoSaida { get; set; }
        public string NomeEntidade { get; set; }
        public System.DateTime DataInicioProcesso { get; set; }
        public DateTime? PrazoAtendimento { get; set; }
        public DateTime? DataRef { get; set; }
        public string CodigoIdentificadorProcesso { get; set; }
        public string CodigoIdentificadorProposta { get; set; }
        public string SusepCedente { get; set; }
        public string SusepCessionaria { get; set; }
        public string CidtfdCnpjCdent { get; set; }
        public string CidtfdCnpjCessionaria { get; set; }
        public string MatriculaConsultor { get; set; }
        public Nullable<decimal> ValorRetido { get; set; }
        public string Observacao { get; set; }
        public int? Agencia { get; set; }
        public int? Conta { get; set; }
        public DateTime? DataConclusao { get; set; }
        public System.DateTime? PrazoFinal { get; set; }
        public string ContatoAgencia { get; internal set; }
        public string DescricaoTipoSolicitacao { get; set; }
        public int? CodigoIdentificadorAgenciaBRA { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string Motivo { get; set; }
        public string Submotivo { get; set; }

        public int StatusId { get; set; }
        public int? SubStatusId { get; set; }
        public int? MotivoId { get; set; }
        public int? SubmotivoId { get; set; }
        public Usuario Usuario { get; set; }

        public static SolicitacaoViewModel Mapear(Solicitacao solicitacao)
        {

            return new SolicitacaoViewModel
            {
                Id = solicitacao.Id,
                DescricaoTipoSolicitacao = solicitacao.DescricaoTipoSolicitacao,
                CodigoIdentificadorProcesso = solicitacao.CodIdentificadorProcesso,
                NomeParticipante = solicitacao.NomeParticipante,
                ValorPrevistoSaida = solicitacao.ValorPrevistoSaida,
                NomeEntidade = solicitacao.NomeEntidade,
                DataInicioProcesso = solicitacao.DataInicioProcesso,
                CodigoIdentificadorProposta = solicitacao.CodIdentificadorProposta,
                SusepCedente = solicitacao.SUSEPCedente,
                SusepCessionaria = solicitacao.SUSEPCessionaria,
                CidtfdCnpjCdent = solicitacao.CIDTFDCNPJCedente,
                CidtfdCnpjCessionaria = solicitacao.CIDTFDCNPJCessionaria,
                CPF = Convert.ToInt64(solicitacao.CPF),
                Lideranca = solicitacao.Lideranca,
                Segmento = solicitacao.Segmento,
                StatusId = solicitacao.StatusId,
                PrazoFinal = solicitacao.PrazoFinal,
                Agencia = solicitacao.CodigoIdentificadorAgenciaBRA,
                ValorRetido = solicitacao.ValorRetido,
                MotivoId = solicitacao.MotivoId,
                SubmotivoId = solicitacao.SubMotivoId,
                Observacao = solicitacao.Observacao,
                DataConclusao = solicitacao.DataConclusao,
                ConsultorMatriz = solicitacao.ConsultorMatriz,
                ConsultorPGP = solicitacao.ConsultorPGP,
                DataRef = solicitacao.DataRef,
                MatriculaConsultor = solicitacao.MatriculaConsultor,
                SaldoPrevidencia = solicitacao.SaldoPrevidencia,
                PrazoAtendimento = solicitacao.PrazoAtendimento,
                SubStatusId = solicitacao.SubStatusId,
                CodigoIdentificadorAgenciaBRA = solicitacao.CodigoIdentificadorAgenciaBRA,
                Conta = solicitacao.Conta,
                ContatoAgencia = solicitacao.ContatoAgencia,
                Motivo = solicitacao.Motivo?.Descricao,
                Submotivo = solicitacao.SubMotivo?.Descricao,
                Status = solicitacao.Status?.Descricao,
                SubStatus = solicitacao.SubStatus?.Descricao

            };
        }
    }
}