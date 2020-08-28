using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models.Modelos_dos_Arquivos
{
    [Table("VencimentosCompleto")]
    public class Vencimento
    {
        public int Id { get; set; }
        public string Cod_Agencia { get; set; }

        public string Cod_Conta_Corrente { get; set; }

        public DateTime? Dt_Vecto_Contratado { get; set; }

        public string Nm_Cliente_Contraparte { get; set; }

        public double Perc_Indexador { get; set; }

        public string Nome_produto_sistema_origem { get; set; }

        public decimal SALDO_ATUAL { get; set; }
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public ClienteViewModel Cliente { get; set; }
    }
}