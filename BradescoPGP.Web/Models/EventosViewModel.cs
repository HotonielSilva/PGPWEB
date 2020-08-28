using BradescoPGP.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class EventosViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataHora { get; set; }     
    }
}

public class EventoViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }
    public string Matricula { get; set; }
    public bool Finalizado { get; set; }

}