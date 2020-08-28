using BradescoPGP.Web.Areas.Portabilidade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Areas.Portabilidade.Servicos
{
    public enum ModoFiltro
    {
        Especialista,
        CPFStatus,
        NomeStatus,
        Null,
        Nome,
        CPF,
        Status
    }
    public static class SetaModoFiltro
    {
        public static ModoFiltro ObterModoFiltro(FiltrosPortabilidade filtros)
        {
            var modoFiltro = default(ModoFiltro);

            modoFiltro = !string.IsNullOrEmpty(filtros.CPF) && filtros.Status.HasValue ? ModoFiltro.CPFStatus : ModoFiltro.Null;
            if (modoFiltro != ModoFiltro.Null) return modoFiltro;

            modoFiltro = !string.IsNullOrEmpty(filtros.Nome) && filtros.Status.HasValue ? ModoFiltro.NomeStatus : ModoFiltro.Null;
            if (modoFiltro != ModoFiltro.Null) return modoFiltro;

            modoFiltro = !string.IsNullOrEmpty(filtros.CPF) ? ModoFiltro.CPF : ModoFiltro.Null;
            if (modoFiltro != ModoFiltro.Null) return modoFiltro;

            modoFiltro = !string.IsNullOrEmpty(filtros.Especialista) ? ModoFiltro.Especialista : ModoFiltro.Null;
            if (modoFiltro != ModoFiltro.Null) return modoFiltro;

            modoFiltro = !string.IsNullOrEmpty(filtros.Nome) ? ModoFiltro.Nome : ModoFiltro.Null;
            if (modoFiltro != ModoFiltro.Null) return modoFiltro;

            modoFiltro = filtros.Status.HasValue ? ModoFiltro.Status : ModoFiltro.Null;
            if (modoFiltro != ModoFiltro.Null) return modoFiltro;

            return modoFiltro;
        }
    }

}