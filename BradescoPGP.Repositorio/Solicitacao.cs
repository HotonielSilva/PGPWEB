//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BradescoPGP.Repositorio
{
    using System;
    using System.Collections.Generic;
    
    public partial class Solicitacao
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
        public System.DateTime DataInicioProcesso { get; set; }
        public Nullable<System.DateTime> PrazoAtendimento { get; set; }
        public Nullable<System.DateTime> DataRef { get; set; }
        public string CodIdentificadorProcesso { get; set; }
        public string CodIdentificadorProposta { get; set; }
        public string SUSEPCedente { get; set; }
        public string SUSEPCessionaria { get; set; }
        public string CIDTFDCNPJCedente { get; set; }
        public string CIDTFDCNPJCessionaria { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> MotivoId { get; set; }
        public Nullable<int> SubMotivoId { get; set; }
        public string MatriculaConsultor { get; set; }
        public Nullable<decimal> ValorRetido { get; set; }
        public string Observacao { get; set; }
        public Nullable<int> Agencia { get; set; }
        public Nullable<int> Conta { get; set; }
        public Nullable<System.DateTime> DataConclusao { get; set; }
        public Nullable<System.DateTime> DataInclusao { get; set; }
        public Nullable<int> SubStatusId { get; set; }
        public Nullable<System.DateTime> PrazoFinal { get; set; }
        public string ContatoAgencia { get; set; }
        public string DescricaoTipoSolicitacao { get; set; }
        public Nullable<int> CodigoIdentificadorAgenciaBRA { get; set; }
    
        public virtual Motivo Motivo { get; set; }
        public virtual Status Status { get; set; }
        public virtual SubMotivo SubMotivo { get; set; }
        public virtual SubStatus SubStatus { get; set; }
    }
}
