using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Graficos
{
    public class SerieGraficoPie
    {
        public List<string> labels { get; set; }
        public List<decimal> series { get; set; }
    }
}