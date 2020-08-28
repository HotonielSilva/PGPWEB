using BradescoPGP.Repositorio;
using System.Collections.Generic;

namespace BradescoPGP.Web.Areas.Portabilidade.Interfaces
{
    public interface IUsuarioService
    {
        Usuario ObterUsuario(string matricula);
        string ObterNomeUsuario(string matricula);
        List<Usuario> ObterTodosUsuarios();
    }
}