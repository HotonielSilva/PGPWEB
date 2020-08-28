using BradescoPGP.Repositorio;
using System.Globalization;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class TabelaStatusViewModel
    {
        public int Id { get; set; }
        public string NomeParticipante { get; set; }
        public string ConsultorPGP { get; set; }
        public int? Agencia { get; set; }
        public string DataSolicitacao { get; set; }
        public string NumeroProcesso { get; set; }
        public string NumeroProposta { get; set; }
        public string Entidade { get; set; }
        public string ValorSolicitado { get; set; }
        public decimal ValorPrevistoSaida { get; set; }
        public decimal? ValorRetido { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string Detalhe { get; set; } = "<button type=\"button\" id=\"btn-detalhe\" class=\"btn btn-primary m-r-10\" data-backdrop=\"static\" data-toggle=\"modal\" data-target=\"#modal-detalhe\" style=\"border:0\">" +
                                        "<i class=\"fa fa-user  text-center icone-detalhe\"></i>" +
                                    "</button>";

        public static TabelaStatusViewModel Mapear(Solicitacao s) {

            return new TabelaStatusViewModel
            {
                Id = s.Id,
                NomeParticipante = s.NomeParticipante,
                Agencia = s.Agencia,
                DataSolicitacao = s.DataInicioProcesso.ToShortDateString(),
                NumeroProcesso = s.CodIdentificadorProcesso,
                NumeroProposta = s.CodIdentificadorProposta,
                Entidade = s.NomeEntidade,
                ValorSolicitado = s.ValorPrevistoSaida.ToString("N2", new CultureInfo("pt-br")),
                ValorRetido = s.ValorRetido,
                Status = s.Status?.Descricao,
                SubStatus = s.SubStatus?.Descricao,
                ConsultorPGP = s.ConsultorPGP,
                ValorPrevistoSaida = s.ValorPrevistoSaida
                
            };
        }
    }
}