using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class FiltrosTelas
    {
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public int? Situacao { get; set; }
        public string Especialista { get; set; }
        public string Equipe { get; set; }
        public string Comentario { get; set; }
        public string Gerente { get; set; }
        public DateTime? De { get; set; }
        public DateTime? Ate { get; set; }
        public string Acao { get; set; }
        public static bool SemFiltros(FiltrosTelas e)
        {
            return
                string.IsNullOrEmpty(e.Agencia) &&
                string.IsNullOrEmpty(e.Conta) &&
                !e.Situacao.HasValue &&
                string.IsNullOrEmpty(e.Especialista) &&
                string.IsNullOrEmpty(e.Equipe) &&
                string.IsNullOrEmpty(e.Comentario) &&
                string.IsNullOrEmpty(e.Gerente) &&
                !e.De.HasValue &&
                !e.Ate.HasValue;
        }
    }
}