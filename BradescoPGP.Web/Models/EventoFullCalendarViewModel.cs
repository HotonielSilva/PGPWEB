using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class EventoFullCalendarViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string[] classNames { get; set; }
        public string backgroundColor  { get; set; }
        public string borderColor { get; set; }


        public static List<EventoFullCalendarViewModel> Mapear(AgendaViewModel eventos)
        {
            var eventosFullCalendarList = new List<EventoFullCalendarViewModel>();
            int id = new Random().Next(10000);

            eventos.Vencimentos?.ForEach(v => {

                eventosFullCalendarList.Add(new EventoFullCalendarViewModel
                {
                    description = string.Format("Cliente: {0} <br> Data: {1}", v.Nm_Cliente_Contraparte, v.Dt_Vecto_Contratado.ToShortDateString()),
                    title = string.Format("Vencimento {0:c} {1}",v.SALDO_ATUAL, v.Nm_Cliente_Contraparte),
                    start = v.Dt_Vecto_Contratado,
                    end = v.Dt_Vecto_Contratado.AddHours(2),
                    classNames = new string[] { "external-event", v.Dt_Vecto_Contratado.Date < DateTime.Now.Date ? "bg-red" : "bg-blue" },
                    backgroundColor = v.Dt_Vecto_Contratado.Date < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc",
                    borderColor = v.Dt_Vecto_Contratado.Date < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc",
                    id = id
                });
            });

            eventos.Pipelines?.ForEach(p =>
            {
                eventosFullCalendarList.Add(new EventoFullCalendarViewModel
                {
                    description = p.NomeCliente,
                    title = string.Format("Pipeline {0:c} {1}", p.ValorDoPipe, p.NomeCliente),
                    start = p.DataProrrogada.HasValue ? p.DataProrrogada.Value : p.DataPrevista,
                    end = p.DataProrrogada.HasValue ? p.DataProrrogada.Value.AddHours(3) : p.DataPrevista.AddHours(3),
                    classNames = new string[] { "external-event", p.DataProrrogada.HasValue ? p.DataProrrogada.Value.Date < DateTime.Now.Date ? "bg-red" : "bg-blue" : p.DataPrevista < DateTime.Now.Date ? "bg-red" : "bg-blue" },
                    backgroundColor = p.DataProrrogada.HasValue ? p.DataProrrogada.Value.Date < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc" : p.DataPrevista < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc",
                    borderColor = p.DataProrrogada.HasValue ? p.DataProrrogada.Value.Date < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc" : p.DataPrevista < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc",
                    id = id
                });
            });

            eventos.TEDs?.ForEach(t =>
            {
                eventosFullCalendarList.Add(new EventoFullCalendarViewModel
                {
                    description = t.NomeCliente,
                    title = string.Format("TED {0:c} {1}", t.Valor, t.NomeCliente),
                    start = t.Data,
                    end = t.Data.AddHours(3),
                    classNames = new string[] { "external-event", t.Data.Date < DateTime.Now.Date ? "bg-red" : "bg-blue" },
                    backgroundColor = t.Data.Date < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc",
                    borderColor = t.Data.Date < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc",
                    id = id
                });
            });

            eventos.Aniversariantes?.ForEach(a =>
            {
                eventosFullCalendarList.Add(new EventoFullCalendarViewModel
                {
                    description = a.Nome,
                    title = string.Format("Aniversário - {0}", a.Nome),
                    start = a.DataAniversario,
                    end = a.DataAniversario.AddHours(3),
                    classNames = new string[] { "external-event", a.DataAniversario.Date < DateTime.Now.Date ? "bg-red" : "bg-blue" },
                    backgroundColor = a.DataAniversario.Date < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc",
                    borderColor = a.DataAniversario.Date < DateTime.Now.Date ? "#dd4b39" : "#3c8dbc",
                    id = id
                });
            });

            eventos.Eventos?.ForEach(e =>
            {
                eventosFullCalendarList.Add(new EventoFullCalendarViewModel
                {
                    description = e.Descricao,
                    title = string.Format("Evento - {0} {1}", e.DataHoraInicio.ToShortDateString(), e.Titulo),
                    start = e.DataHoraInicio,
                    end = e.DataFim,
                    classNames = new string[] { "external-event", e.DataHoraInicio < DateTime.Now ? "bg-red" : "bg-blue" },
                    backgroundColor = e.DataHoraInicio < DateTime.Now ? "#dd4b39" : "#3c8dbc",
                    borderColor = e.DataHoraInicio < DateTime.Now ? "#dd4b39" : "#3c8dbc",
                    id = id
                });
            });
            
            return eventosFullCalendarList;
        }

    }
}