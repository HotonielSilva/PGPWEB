using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class ProdutividadeModel
    {
        public int IdMinRetencao { get; set; }

        public decimal ValorMinRetencao { get; set; }

        public DateTime DeMinRetencao { get; set; }

        public int IdMinContato { get; set; }

        public decimal ValorMinContato { get; set; }

        public DateTime DeMinContato { get; set; }
    }
}