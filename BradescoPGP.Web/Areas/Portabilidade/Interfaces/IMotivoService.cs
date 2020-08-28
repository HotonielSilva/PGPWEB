using BradescoPGP.Repositorio;
using System.Collections.Generic;

namespace BradescoPGP.Web.Areas.Portabilidade.Interfaces
{
    public interface IMotivoService
    {
        List<Motivo> ObterTodosMotivos(bool inativos = false);

        Motivo ObterMotivo(int id);

        bool NovoMotivo(Motivo motivo);
        bool NovoSubMotivo(SubMotivo subMotivo);

        bool ExcluirMotivo(int id);

        bool ExcluirSubmotivo(int idSubMotivo);

        bool EditarMotivo(int idMotivo, string motivo);

        bool EditarSubmotivo(int idSubmotivo, string submotivo);
    }
}
