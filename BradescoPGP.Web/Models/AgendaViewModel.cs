using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class AgendaViewModel
    {
        public List<Vencimento> Vencimentos { get; set; }
        public List<Pipeline> Pipelines { get; set; }
        public List<TED> TEDs { get; set; }
        public List<AniversarianteViewModel> Aniversariantes { get; set; }
        public List<Evento> Eventos { get; set; }


        public static AgendaViewModel Mapear(List<Vencimento> vencimentos, List<Pipeline> pipelines, List<TED> teds, List<AniversarianteViewModel> aniversariantes, List<Evento> eventos)
        {
            return new AgendaViewModel
            {
                Aniversariantes = aniversariantes,
                Vencimentos = vencimentos,
                TEDs = teds,
                Pipelines = pipelines,
                Eventos = eventos
            };
        }
    }

    

    
}