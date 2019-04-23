using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WFRXRC.Entities;
using WFRXRC.Models;

namespace WFRXRC.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private Tbl_RXREntities db = new Tbl_RXREntities();

        // GET: Login
        public ActionResult Index()
        {
            var ctl_Login = db.ctl_Login.Include(c => c.ctl_Empleado);
            return View(ctl_Login.ToList());
        }

        public ActionResult cSesion()
        {
            Session["idU"] = null;
            Session["uNombre"] = null;
            Session["permisos"] = null;

            return View();
        }

        // GET: Login/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Login ctl_Login = db.ctl_Login.Find(id);
            if (ctl_Login == null)
            {
                return HttpNotFound();
            }
            return View(ctl_Login);
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            ViewBag.Id_Empleado = new SelectList(db.ctl_Empleado, "Id_empleado", "Nombre");
            return View();
        }

        // POST: Login/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Empleado,Usuario,Pwd,Ultimo_Acceso,Sesion_Activa,Estatus")] ctl_Login ctl_Login)
        {
            if (ModelState.IsValid)
            {
                db.ctl_Login.Add(ctl_Login);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Empleado = new SelectList(db.ctl_Empleado, "Id_empleado", "Nombre", ctl_Login.Id_Empleado);
            return View(ctl_Login);
        }

        // GET: Login/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Login ctl_Login = db.ctl_Login.Find(id);
            if (ctl_Login == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Empleado = new SelectList(db.ctl_Empleado, "Id_empleado", "Nombre", ctl_Login.Id_Empleado);
            return View(ctl_Login);
        }

        // POST: Login/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Empleado,Usuario,Pwd,Ultimo_Acceso,Sesion_Activa,Estatus")] ctl_Login ctl_Login)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ctl_Login).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Empleado = new SelectList(db.ctl_Empleado, "Id_empleado", "Nombre", ctl_Login.Id_Empleado);
            return View(ctl_Login);
        }

        // GET: Login/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Login ctl_Login = db.ctl_Login.Find(id);
            if (ctl_Login == null)
            {
                return HttpNotFound();
            }
            return View(ctl_Login);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ctl_Login ctl_Login = db.ctl_Login.Find(id);
            db.ctl_Login.Remove(ctl_Login);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                if (AuthenticationManager.User.Identity.IsAuthenticated)
                {
                    AuthenticationManager.SignOut();
                    LoginViewModel _m = new LoginViewModel();
                    //_m.ID = "admin";
                    ViewBag.ReturnUrl = returnUrl;
                    return View(_m);
                }
            }
            catch
            {

            }
            LoginViewModel m = new LoginViewModel();
            //m.ID = "admin";
            //m.Password = "admin";
            ViewBag.ReturnUrl = returnUrl;
            return View(m);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ctl_Login user = new ctl_Login();
            user.Usuario = model.ID;
            user.Pwd = model.Password;

            Cryptography c = new Cryptography();
            //string pass = c.Encrypt(user.Pwd);

            using (Tbl_RXREntities db = new Tbl_RXREntities())
            {
                //user = db.ctl_Login.Where(a => a.Id_Empleado.Equals(user.Id_Empleado) && a.Pwd.Equals(pass)).FirstOrDefault();
                user = db.ctl_Login.Where(a => a.Usuario.Equals(user.Usuario) && a.Pwd.Equals(user.Pwd) && a.Estatus == true).FirstOrDefault();
            }

            //El usuario se enontro en la bade de datos
            if (user != null)
            {
                Session["idU"] = user.Id_Empleado;
                var emp = db.ctl_Empleado.Where(x => x.Id_empleado == user.Id_Empleado).FirstOrDefault();
                Session["uNombre"] = emp.Nombre;

                Session["permisos"] = db.ctl_Login.Where(x => x.Id_Empleado == emp.Id_empleado).FirstOrDefault().Permisos;
                FormsAuthentication.SetAuthCookie(model.ID, false);

                //var authTicket = new FormsAuthenticationTicket(1, user.ID, DateTime.Now, DateTime.Now.AddMinutes(20), false, user.MIEMBROS.FirstOrDefault().ROL.NOMBRE);
                var authTicket = new FormsAuthenticationTicket(1, user.Usuario + '-' + user.Id_Empleado, DateTime.Now, DateTime.Now.AddDays(1), false, "Administrador");
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                if (returnUrl != null)
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }

            else
            {
                ModelState.AddModelError("", "Usuario/contraseña incorrecta.");
                return View(model);
            }
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
