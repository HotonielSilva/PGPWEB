using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BradescoPGP.Web.Areas.Portabilidade.Interfaces
{
    public interface IStatusSubStatusService
    {
        List<Status> ObterStatus();
        List<SubStatus> ObterSubStatus();
        bool EditarStatus(int id, string descricao);
        bool NovoStatus(string descricao);
        bool NovoSubStatus(int idStatus, string descricao);
        bool EditarSubStatus(int idSubstatus, string descricao);

    }
}
