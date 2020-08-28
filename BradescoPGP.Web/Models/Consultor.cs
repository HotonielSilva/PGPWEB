using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class Consultor
    {
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public virtual List<ClienteViewModel> Clientes { get; set; }
        public List<Qualitativo> Qualitativos { get; set; }

    }
}