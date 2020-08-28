using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Modelos_dos_Arquivos
{
    [Table("Hierarquias")]
    public class Hierarquia
    {
        public int Id { get; set; }
        public string Consultor { get; set; }
        public string MatriculaConsultor { get; set; }
        public string Supervisor { get; set; }
        public string MatriculaSupervisor { get; set; }
        public string Equipe { get; set; }
        public string TipoAcesso { get; set; }

    }
}