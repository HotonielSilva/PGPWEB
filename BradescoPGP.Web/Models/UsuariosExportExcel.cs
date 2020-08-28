using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class UsuariosExportExcel
    {
        public string Usuario { get; set; }
        public string Matricula { get; set; }
        public string Supervisor { get; set; }
        public string MatriculaGestor { get; set; }
        public string Equipe { get; set; }
        public string TipoDeAcesso { get; set; }
        public string Username { get; set; }

        public static UsuariosExportExcel Mapear(Usuario usuario)
        {
            return new UsuariosExportExcel
            {
                Equipe = usuario.Equipe,
                Matricula = usuario.Matricula,
                MatriculaGestor = usuario.MatriculaSupervisor,
                Supervisor = usuario.NomeSupervisor,
                TipoDeAcesso = usuario.Perfil?.Descricao,
                Username = usuario.NomeUsuario,
                Usuario = usuario.Nome
            };
        }

    }
}