using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Servicos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    
    public class TabelaMotivoSubmotivoViewModel
    {
        public int Id { get; set; }
        public string Especialista { get; set; }
        public string NomeDoCliente { get; set; }
        public string CPF { get; set; }
        public string Segmento { get; set; }
        public string CodigoIdentificadorAgenciaBRA { get; set; }
        public string DataInicioProcesso { get; set; }
        public string CodIdentificadorProcesso { get; set; }
        public string CodIdentificadorProposta { get; set; }
        public string ConsultorMatriz { get; set; }
        public string ConsultorPGP { get; set; }
        public string NomeEntidade { get; set; }
        public System.DateTime? PrazoFinal { get; set; }
        public string ValorPrevistoSaida { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string ValorRetido { get; set; }
        public string Atendimento { get; set; } = "<button type=\"button\" id=\"btn-detalhe\" class=\"button btn-primary m-r-10\" data-backdrop=\"static\" data-toggle=\"modal\" data-target=\"#modal-detalhe\" style=\"border:0\">" +
                                        "<i class=\"fa fa-user icone-detalhe\"></i>" +
                                    "</button>";


        public static TabelaMotivoSubmotivoViewModel Mapear(Solicitacao s, string especialista)
        {

            return new TabelaMotivoSubmotivoViewModel
            {
                CodigoIdentificadorAgenciaBRA = s.CodigoIdentificadorAgenciaBRA.ToString(),
                CodIdentificadorProcesso = s.CodIdentificadorProcesso,
                CodIdentificadorProposta = s.CodIdentificadorProposta,
                CPF = s.CPF.ToString().PadLeft(11, '0'),
                PrazoFinal = s.PrazoFinal,
                DataInicioProcesso = s.DataInicioProcesso.ToShortDateString(),
                Id = s.Id,
                NomeEntidade = s.NomeEntidade,
                ConsultorMatriz = s.ConsultorMatriz,
                ConsultorPGP = s.ConsultorPGP,
              
                NomeDoCliente = s.NomeParticipante,
                Segmento = s.Segmento,
                Status = s.Status?.Descricao,
                SubStatus = s.SubStatus?.Descricao,
                ValorPrevistoSaida = s.ValorPrevistoSaida.ToString("N2", new CultureInfo("pt-br")),
                ValorRetido = s.ValorRetido?.ToString("N2", new CultureInfo("pt-br")),
                Especialista = especialista
            };
        }
    }
}