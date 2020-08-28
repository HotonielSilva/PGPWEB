using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BradescoPGP.Web.Models
{
    public class EventoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string  Observacao { get; set; }
        public string Status { get; set; }
        public string Relacao { get; set; }

       //Mapear
    }
}