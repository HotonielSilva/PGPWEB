using System;
using System.Collections.Generic;

namespace BradescoPGP.Web
{
    public static class DateTimeExtension
    {
        public static bool IsWeekend(this DateTime value)
        {
            return (value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday);
        }

        public static DateTime AdicionarDiasUteis(this DateTime dataInicial, int dias)
        {
            dataInicial = dataInicial.AddDays(-1);

            while (dias > -1)
            {
                if (dataInicial.DayOfWeek == DayOfWeek.Sunday)
                {
                    dataInicial = dataInicial.AddDays(1);
                }

                else if (dataInicial.DayOfWeek == DayOfWeek.Saturday)
                {
                    dataInicial = dataInicial.AddDays(2);
                }

                else if (dias > 0)
                {
                    dataInicial = dataInicial.AddDays(1);
                    dias -= 1;
                }

                else if (dias == 0)
                {
                    while (dataInicial.DayOfWeek == DayOfWeek.Saturday || dataInicial.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dataInicial = dataInicial.AddDays(1);
                    }
                    dias = -1;
                }
            }

            return dataInicial;
        }

        public static List<DateTime> DiasUteisMes(this DateTime data)
        {
            var diasDoMes = DateTime.DaysInMonth(data.Year, data.Month);

            var result = new List<DateTime>();

            for (int i = 1; i <= diasDoMes; i++)
            {
                var dataLoop = new DateTime(data.Year, data.Month, i);

                if (dataLoop.DayOfWeek != DayOfWeek.Saturday && dataLoop.DayOfWeek != DayOfWeek.Sunday)
                {
                    result.Add(dataLoop);
                }
            }

            return result;
        }
    }
}