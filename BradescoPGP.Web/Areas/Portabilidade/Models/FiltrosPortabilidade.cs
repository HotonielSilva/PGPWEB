using System;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class FiltrosPortabilidade
    {
        public string CPF { get; set; }

        public string Especialista { get; set; }

        public int? Status { get; set; }

        public DateTime? De { get; set; }

        public DateTime? Ate { get; set; }

        public string Nome { get; set; }

        public bool TemFiltro()
        {
            return !string.IsNullOrEmpty(CPF) || !string.IsNullOrEmpty(Especialista) || Status.HasValue || De.HasValue || Ate.HasValue || !string.IsNullOrEmpty(Nome);

        }
    }
}