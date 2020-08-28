using BradescoPGP.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class CorretoraViewModel
    {
        public bool CorretoraBra { get; set; }
        public string StatusBra { get; set; }
        public bool CorrtoraAGO { get; set; }
        public string StatusAgo { get; set; }

        
        public static CorretoraViewModel Mapear(Corretora Bra , Corretora AGO)
        {
            var corretoraViewModel = default(CorretoraViewModel);

            if (Bra != null && AGO != null)
            {
                corretoraViewModel = new CorretoraViewModel
                {
                    CorretoraBra = Bra.Status != null,
                    StatusBra = Bra.Status,
                    CorrtoraAGO = AGO.Status != null,
                    StatusAgo = AGO.Status
                };
            }
            else if(Bra != null)
            {
                corretoraViewModel =  new CorretoraViewModel
                {
                    CorretoraBra = Bra.Status != null,
                    StatusBra =  Bra.Status,
                    CorrtoraAGO = false,
                    StatusAgo = null
                };
            }
            else if(AGO != null)
            {
                corretoraViewModel = new CorretoraViewModel
                {
                    CorretoraBra = false,
                    StatusBra = null,
                    CorrtoraAGO = AGO.Status != null,
                    StatusAgo = AGO.Status
                };
            }
            else
            {
                corretoraViewModel = new CorretoraViewModel { CorretoraBra = false, CorrtoraAGO = false, StatusBra = null, StatusAgo = null };
            }

            return corretoraViewModel;
            
        }
    }
}