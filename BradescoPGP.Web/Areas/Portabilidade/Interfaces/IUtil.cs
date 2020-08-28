using BradescoPGP.Repositorio;
using System.Collections.Generic;

namespace BradescoPGP.Web.Areas.Portabilidade.Interfaces
{
    public interface IUtil
    {
        byte[] GerarExcelPortabilidade<T>(List<T> dados, string nomePlainlha);
        List<Produtividade> ObterProdutividade();
    }
}