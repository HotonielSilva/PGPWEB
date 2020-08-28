using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class UsuarioViewModel
    {
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Nome do usuário")]
        public string Nome { get; set; }

        [DisplayName("Código do usuário")]
        [StringLength(30)]
        [Required]
        public string NomeUsuario { get; set; }

        [DisplayName("Matrícula do usuário (Somente números)")]
        [Required(ErrorMessage ="Este campo aceita apenas números")]
        [StringLength(30)]
        public string Matricula { get; set; }

        [DisplayName("Nome do Gestor")]
        [StringLength(50)]
        [Required]
        public string NomeSupervisor { get; set; }
        [Required]
        [StringLength(30)]
        [DisplayName("Matrícula do Gestor (Somente números)")]
        public string MatriculaSupervisor { get; set; }

        [Required]
        [StringLength(100)]
        public string Equipe { get; set; }

        public int PerfilId { get; set; }

        [DisplayName("Tipo de usuário")]
        public string TipoUsuario { get; set; }

        [DisplayName("Receber notificação sobre eventos cadastrados")]


        public bool ReceberNotificacaoEvento { get; set; }

        [DisplayName("Receber notificação sobre Pipelines")]
        public bool ReceberNotificacaoPipeline { get; set; }


        public Perfil Perfil { get; set; }

        public List<Perfil> Perfis { get; set; }
    }
}