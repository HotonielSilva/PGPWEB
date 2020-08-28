using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Modelos_dos_Arquivos
{
    public class PipelineArquivos
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public string Agência { get; set; }
        public string Consultor { get; set; }
        public string Conta { get; set; }
        public string BradescoPrincipalBanco { get; set; }
        public decimal ValoresNoMercado { get; set; }
        public decimal ValorDoPipe { get; set; }
        public string Origem { get; set; }
        public DateTime DataPrevista { get; set; }
        public decimal ValorQueJaConvertemos { get; set; }
        public DateTime DataDaConversão { get; set; }
        public string Observacoes { get; set; }
    }
}