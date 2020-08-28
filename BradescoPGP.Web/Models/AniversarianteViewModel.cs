using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class AniversarianteViewModel
    {
        public string Nome { get; set; }
        public DateTime DataAniversario { get; set; }
        public int Agencia { get; set; }
        public int Conta { get; set; }

        public static AniversarianteViewModel Mapear(Aniversarios niver)
        {
            return new AniversarianteViewModel
            {
                Agencia = niver.Agencia,
                Conta = niver.Conta,
                DataAniversario = niver.DataNascimento,
            };
        }
    }
}