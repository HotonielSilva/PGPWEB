using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Models
{
    public class GraficoEquipeViewModel
    {
        public List<String> Labels { get; set; }
        public DataSet Retido { get; set; }
        public DataSet NaoRetido { get; set; }
    }
}