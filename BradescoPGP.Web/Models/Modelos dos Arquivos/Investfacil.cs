using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Modelos_dos_Arquivos
{
    public class Investfacil
    {
        public int Id { get; set; }

        [StringLength(10)]
        public string CONTA { get; set; }

        [StringLength(10)]
        public string AGENCIA { get; set; }
        public decimal Vlr_Evento { get; set; }
    }
}