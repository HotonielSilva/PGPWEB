using BradescoPGP.Repositorio;

namespace BradescoPGP.Web.Models
{
    public class SubmotivosViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public static SubmotivosViewModel Mapear(SubMotivo submotivo)
        {
            return new SubmotivosViewModel
            {
                Id = submotivo.Id,
                Descricao = submotivo.Descricao
            };
        }
    }
}