//using BradescoPGP.Repositorio.ServicosGPA;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace BradescoPGP.Web.Controllers
//{
//    [Authorize]
//    public class GPAController : Controller
//    {
//        // GET: GPA
//        public ActionResult Index()
//        {
//            var gpa = new ServicoGPA();

//            var result = gpa.Obter(69);

//            var segmentos =  gpa.SegmentosCombo();

//            var combo = segmentos.ConvertAll(s => Segmento.MapearCombo(s));
//            combo.Insert(0, new SelectListItem { Text = ":: Selecione ::", Value = "", Selected = true });

//            ViewBag.Segmentos = combo;

//            ViewBag.Titulo = "GPA";

//            ViewBag.Especialistas = gpa.EspecialistasFormatoGPA();

//            return View();
//        }
//    }
//}