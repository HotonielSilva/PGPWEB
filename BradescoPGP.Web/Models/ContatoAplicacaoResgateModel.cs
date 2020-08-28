using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class ContatoAplicacaoResgateModel
    {
        public int IdAplicResgate { get; set; }
        public bool ContatouCliente { get; set; }
        public bool Realocou { get; set; }
        public bool PagamentosUsoDoRecurso { get; set; }
        public bool AplicouEmOutroBanco { get; set; }
        public bool ProblemasDeRelacionamento { get; set; }
        public bool VaiAnalisarOferta { get; set; }
    }
}