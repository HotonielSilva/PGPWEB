using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Models;
using BradescoPGP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BradescoPGP.Web.Areas.Portabilidade.Interfaces
{
    public interface ISolicitacaoService
    {
        List<Solicitacao> ObterTodas();
        List<Motivo> ObterMotivos(bool inativos = false);
        List<Solicitacao> Obter(FiltrosPortabilidade filtros);

        List<Solicitacao> ObterAVencer(FiltrosPortabilidade filtros);
        List<Solicitacao> ObterAVencer(DateTime inicio, DateTime fim, string matricula);
        List<Solicitacao> ObterAVencer(DateTime inicio, DateTime fim);

        bool AtualizarSolicitacao(OperacionalViewModel solicitacao);
        List<Solicitacao> Obter(DateTime inicio, DateTime fim);
        List<Solicitacao> Obter(DateTime inicio, DateTime fim, string matricula);
        Solicitacao Obter(int id);
        List<string> ObterEntidades();
        List<Solicitacao> ObterSolicitacaoEntidade(string entidade, FiltrosPortabilidade filtros);
        string ObterFundo(string CnpjCessionaria);

    } 
}
