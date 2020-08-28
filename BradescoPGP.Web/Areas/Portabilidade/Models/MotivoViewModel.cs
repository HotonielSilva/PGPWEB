using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class MotivoViewModel
    {
        public MotivoViewModel()
        {
            this.Solicitacao = new HashSet<Solicitacao>();
            this.SubMotivo = new HashSet<SubMotivo>();
        }
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool? EmUso { get; set; }
        public ICollection<Solicitacao> Solicitacao { get; set; }
        public ICollection<SubMotivo> SubMotivo { get; set; }
    }
}