using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class StatusViewModel
    {
        public StatusViewModel()
        {
            this.Solicitacao = new HashSet<Solicitacao>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public SubStatus SubStatusId { get; set; }
        public ICollection<Solicitacao> Solicitacao { get; set; }
    }
}