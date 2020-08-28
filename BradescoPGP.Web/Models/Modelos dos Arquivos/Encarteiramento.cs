using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Modelos_dos_Arquivos
{
    public class Encarteiramento
    {
        public int Id { get; set; }
        public string Matricula { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string TIP_CLIENTE { get; set; }
        public string CPF { get; set; }
        public DateTime? DATA { get; set; }
        public string AG_PRINC { get; set; }
        public string CONTA_PRINC { get; set; }
        public string CONSULTOR { get; set; }
        public string EQUIPE_RESPONSAVEL { get; set; }
        public string EQUIPE_MESA { get; set; }
        public string DIR_REG_AG_PRINC { get; set; }
        public string GER_REG_AG_PRINC { get; set; }
    }
}