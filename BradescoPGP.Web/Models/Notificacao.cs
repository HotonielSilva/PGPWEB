using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public enum PosicaoNotificacao
    {
        center,
        right,
        left,
    }
    public enum TipoNotificacao
    {
        error,
        info,
        warning,
        success
    }

    public class Notificacao
    {
        public string Mensagem { get; set; }
        public PosicaoNotificacao Posicao { get; set; }
        public TipoNotificacao Tipo { get; set; }
    }
}