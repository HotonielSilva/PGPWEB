using BradescoPGP.Repositorio;
using BradescoPGP.Web.Areas.Portabilidade.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Servicos
{
    public class UsuarioService : IUsuarioService
    {
        private readonly PGPEntities _context;

        public HashSet<Solicitacao> Solicitacao { get; }

        public UsuarioService(DbContext context)
        {
            _context = context as PGPEntities;
        }
        //public Usuario()
        //{
        //    this.Solicitacao = new HashSet<Solicitacao>();
        //}
        public Usuario ObterUsuario(string matricula)
        {
            return _context.Usuario.FirstOrDefault(u => u.Matricula == matricula);
        }
        public string ObterNomeUsuario(string matricula)
        {
            return _context.Usuario.FirstOrDefault(u => u.Matricula == matricula)?.Nome;
        }
        public List<Usuario> ObterTodosUsuarios()
        {
            return _context.Usuario.ToList();
        }
    }
}