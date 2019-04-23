using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WFRXRC.Entities;
using WFRXRC.Models;

namespace WFRXRC.Controllers
{
    public class ClienteController : Controller
    {
        private Tbl_RXREntities db = new Tbl_RXREntities();

        // GET: Cliente
        public ActionResult Index()
        {
            int pagina = 301; //ID EN BASE DE DATOS
            using (Tbl_RXREntities db = new Tbl_RXREntities())
            {
                //string u = User.Identity.Name;
                //var _u = db.ctl_Empleado.Where(a => a.Nombre.Equals(u)).FirstOrDefault();
                //var user = db.ctl_Login.Where(a => a.Id_Empleado.Equals(u)).FirstOrDefault();
                //ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                //ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.returnUrl = Request.Url.PathAndQuery;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals("ES")).FirstOrDefault().TXT50;
            }
            var ctl_Cliente = db.ctl_Cliente.Where(x => x.Estatus == true);
            return View(ctl_Cliente.ToList());
        }

        // GET: Cliente/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Cliente ctl_Cliente = db.ctl_Cliente.Find(id);
            int pagina = 305; //ID EN BASE DE DATOS
            using (Tbl_RXREntities db = new Tbl_RXREntities())
            {
                //string u = User.Identity.Name;
                //var _u = db.ctl_Empleado.Where(a => a.Nombre.Equals(u)).FirstOrDefault();
                //var user = db.ctl_Login.Where(a => a.Id_Empleado.Equals(u)).FirstOrDefault();
                //ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                //ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.returnUrl = Request.Url.PathAndQuery;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals("ES")).FirstOrDefault().TXT50;
            }
            if (ctl_Cliente == null)
            {
                return HttpNotFound();
            }
            var v = db.ctl_ContactosCliente.Where(x => x.Id_Cliente == ctl_Cliente.Id_Cliente).ToList();
            ViewBag.Contactos1 = new SelectList(v, "Contacto", "Contacto");
            return View(ctl_Cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            int pagina = 302; //ID EN BASE DE DATOS
            using (Tbl_RXREntities db = new Tbl_RXREntities())
            {
                //string u = User.Identity.Name;
                //var _u = db.ctl_Empleado.Where(a => a.Nombre.Equals(u)).FirstOrDefault();
                //var user = db.ctl_Login.Where(a => a.Id_Empleado.Equals(u)).FirstOrDefault();
                //ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                //ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.returnUrl = Request.Url.PathAndQuery;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals("ES")).FirstOrDefault().TXT50;
            }
            return View();
        }

        // POST: Cliente/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Cliente,RazonSocial,RazonComercial,Direccion,Telefono,RFC,RFC_Direccion,RFC_Correo,Credito,TiempoCredito,Estatus")] Mod_Cliente ctl_Cliente, List<string> contactos)
        {
            int pagina = 302;
            string errorString = "";
            if (ModelState.IsValid)
            {
                try
                {
                    ctl_Cliente cl = new ctl_Cliente();
                    cl.RazonSocial = ctl_Cliente.RazonSocial;
                    cl.RazonComercial = ctl_Cliente.RazonComercial;
                    cl.Direccion = ctl_Cliente.Direccion;
                    cl.Telefono = ctl_Cliente.Telefono;
                    cl.RFC = ctl_Cliente.RFC;
                    cl.RFC_Direccion = ctl_Cliente.RFC_Direccion;
                    cl.RFC_Correo = ctl_Cliente.RFC_Correo;
                    cl.Credito = ctl_Cliente.Credito;
                    cl.TiempoCredito = ctl_Cliente.TiempoCredito;
                    cl.Estatus = true;
                    db.ctl_Cliente.Add(cl);
                    db.SaveChanges();
                    //Retorno del id generado
                    long _id = 0;
                    _id = cl.Id_Cliente;

                    if (_id > 0)
                    {
                        for (int i = 0; i < contactos.Count; i++)
                        {
                            ctl_ContactosCliente ccl = new ctl_ContactosCliente();
                            ccl.Id_Cliente = _id;
                            ccl.Contacto = contactos[i];
                            db.ctl_ContactosCliente.Add(ccl);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                { }               
            }
            else
            {
                string validationErrors = string.Join(",",
                   ModelState.Values.Where(E => E.Errors.Count > 0)
                   .SelectMany(E => E.Errors)
                   .Select(E => E.ErrorMessage)
                   .ToArray());

                errorString += validationErrors;
            }
            ViewBag.returnUrl = Request.Url.PathAndQuery;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals("ES")).FirstOrDefault().TXT50;
            ViewBag.errors = errorString;
            return View(ctl_Cliente);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Cliente ctl_Cliente = db.ctl_Cliente.Find(id);
            if (ctl_Cliente == null)
            {
                return HttpNotFound();
            }
            int pagina = 303; //ID EN BASE DE DATOS
            using (Tbl_RXREntities db = new Tbl_RXREntities())
            {
                //string u = User.Identity.Name;
                //var _u = db.ctl_Empleado.Where(a => a.Nombre.Equals(u)).FirstOrDefault();
                //var user = db.ctl_Login.Where(a => a.Id_Empleado.Equals(u)).FirstOrDefault();
                //ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                //ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.returnUrl = Request.Url.PathAndQuery;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals("ES")).FirstOrDefault().TXT50;
            }
            var v = db.ctl_ContactosCliente.Where(x => x.Id_Cliente == ctl_Cliente.Id_Cliente).ToList();
            ViewBag.Contactos1 = new SelectList(v, "Contacto", "Contacto");
            return View(ctl_Cliente);
        }

        // POST: Cliente/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Cliente,RazonSocial,RazonComercial,Direccion,Telefono,RFC,RFC_Direccion,RFC_Correo,Credito,TiempoCredito,Estatus")] ctl_Cliente ctl_Cliente, List<string> contactos)
        {
            int pagina = 303;
            string errorString = "";
            if (ModelState.IsValid)
            {
                try
                {
                    ctl_Cliente cl = new ctl_Cliente();
                    cl = db.ctl_Cliente.Where(x => x.Id_Cliente == ctl_Cliente.Id_Cliente).FirstOrDefault();
                    cl.RazonSocial = ctl_Cliente.RazonSocial;
                    cl.RazonComercial = ctl_Cliente.RazonComercial;
                    cl.Direccion = ctl_Cliente.Direccion;
                    cl.Telefono = ctl_Cliente.Telefono;
                    cl.RFC = ctl_Cliente.RFC;
                    cl.RFC_Direccion = ctl_Cliente.RFC_Direccion;
                    cl.RFC_Correo = ctl_Cliente.RFC_Correo;
                    cl.Credito = ctl_Cliente.Credito;
                    cl.TiempoCredito = ctl_Cliente.TiempoCredito;
                    cl.Estatus = true;
                    db.Entry(cl).State = EntityState.Modified;
                    db.SaveChanges();

                    //remover todos los contactos
                    var ctlCon = db.ctl_ContactosCliente.Where(x => x.Id_Cliente == cl.Id_Cliente).ToList();
                    for (int i = 0; i < ctlCon.Count; i++)
                    {
                        db.Entry(ctlCon[i]).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                    //ingresar los nuevos contactos
                    if (cl.Id_Cliente > 0)
                    {
                        for (int i = 0; i < contactos.Count; i++)
                        {
                            ctl_ContactosCliente ccl = new ctl_ContactosCliente();
                            ccl.Id_Cliente = cl.Id_Cliente;
                            ccl.Contacto = contactos[i];
                            db.ctl_ContactosCliente.Add(ccl);
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                { }
                return RedirectToAction("Index");
            }
            else
            {
                string validationErrors = string.Join(",",
                   ModelState.Values.Where(E => E.Errors.Count > 0)
                   .SelectMany(E => E.Errors)
                   .Select(E => E.ErrorMessage)
                   .ToArray());

                errorString += validationErrors;
            }
            ViewBag.returnUrl = Request.Url.PathAndQuery;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals("ES")).FirstOrDefault().TXT50;
            ViewBag.errors = errorString;
            var v = db.ctl_ContactosCliente.Where(x => x.Id_Cliente == ctl_Cliente.Id_Cliente).ToList();
            ViewBag.Contactos1 = new SelectList(v, "Contacto", "Contacto");
            return View(ctl_Cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Cliente ctl_Cliente = db.ctl_Cliente.Find(id);
            int pagina = 304; //ID EN BASE DE DATOS
            using (Tbl_RXREntities db = new Tbl_RXREntities())
            {
                //string u = User.Identity.Name;
                //var _u = db.ctl_Empleado.Where(a => a.Nombre.Equals(u)).FirstOrDefault();
                //var user = db.ctl_Login.Where(a => a.Id_Empleado.Equals(u)).FirstOrDefault();
                //ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                //ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.returnUrl = Request.Url.PathAndQuery;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals("ES")).FirstOrDefault().TXT50;
            }
            if (ctl_Cliente == null)
            {
                return HttpNotFound();
            }
            var v = db.ctl_ContactosCliente.Where(x => x.Id_Cliente == ctl_Cliente.Id_Cliente).ToList();
            ViewBag.Contactos1 = new SelectList(v, "Contacto", "Contacto");
            return View(ctl_Cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                ctl_Cliente cl = db.ctl_Cliente.Where(x => x.Id_Cliente == id).FirstOrDefault();
                cl.Estatus = false;//darlo de baja
                db.Entry(cl).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            { }
            return RedirectToAction("Index");
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
