using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFRXRC.Entities;

namespace WFRXRC.Controllers
{
    public class HomeController : Controller
    {
        private Tbl_RXREntities db = new Tbl_RXREntities();
        //[Authorize]
        public ActionResult Index()
        {
            try
            {
                var id = Session["idU"].ToString();
                var user = db.ctl_Empleado.Where(x => x.Id_empleado == int.Parse(id)).FirstOrDefault();
                ViewBag.usuario = user;
            }
            catch (Exception e) {

            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}