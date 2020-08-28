using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BradescoPGP.Repositorio;
namespace BradescoPGP.Web.Models
{
    public class ConfiguracaoViewModel
    {
        public ConfiguracaoViewModel(List<WindowsServiceConfig> configs, List<Links> links, List<TEDFaixaEquipe>faixasTedsPorEquipe)
        {
            Importacoes = configs;
            Externos = links;
            TedFaixasEquipe = faixasTedsPorEquipe;
        }

        public List<WindowsServiceConfig> Importacoes { get; set; }

        public List<Links> Externos { get; set; }

        public List<TEDFaixaEquipe> TedFaixasEquipe { get; set; }


    }
}