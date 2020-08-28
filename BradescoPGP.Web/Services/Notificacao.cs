using BradescoPGP.Repositorio;
using Microsoft.AspNet.SignalR;
using System.Linq;

namespace BradescoPGP.Web.Services
{
    public class Notificacao : Hub
    {
        public void Notificar(string matriula)
        {
            using (var db = new PGPEntities())
            {
                var nome = db.Usuario.FirstOrDefault(s => s.Matricula == matriula)?.Nome;

                Clients.Others.dispararNotificacao(nome);
            }
        }
    }
}