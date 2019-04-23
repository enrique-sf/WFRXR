using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WFRXRC.Entities;

namespace WFRXRC.Controllers
{
    public class ProyectoController : Controller
    {
        private Tbl_RXREntities db = new Tbl_RXREntities();

        // GET: Proyecto
        public ActionResult Index()
        {
            var ctl_Proyecto = db.ctl_Proyecto.ToList();
            return View(ctl_Proyecto);
        }

        // GET: Proyecto/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Proyecto ctl_Proyecto = db.ctl_Proyecto.Find(id);
            if (ctl_Proyecto == null)
            {
                return HttpNotFound();
            }
            return View(ctl_Proyecto);
        }

        // GET: Proyecto/Create
        public ActionResult Create()
        {
            ViewBag.Id_Cliente = new SelectList(db.ctl_Cliente, "Id_Cliente", "RazonSocial");
            ViewBag.Id_Empleado = new SelectList(db.ctl_Empleado, "Id_empleado", "Nombre");
            return View();
        }

        // POST: Proyecto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Proyecto,Id_Cliente,Id_Empleado,Nombre_Proyecto,Dias_Asignados,Horas_Asignadas,Costo_Dia,Costo_Tiempo,Fecha_Inicio,Estatus")] ctl_Proyecto ctl_Proyecto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctl_Proyecto pro = new ctl_Proyecto();
                    pro.Id_Cliente = ctl_Proyecto.Id_Cliente;
                    pro.Id_Empleado = ctl_Proyecto.Id_Empleado;
                    pro.Nombre_Proyecto = ctl_Proyecto.Nombre_Proyecto;
                    pro.Dias_Asignados = ctl_Proyecto.Dias_Asignados;
                    pro.Horas_Asignadas = ctl_Proyecto.Horas_Asignadas;
                    pro.Costo_Dia = ctl_Proyecto.Costo_Dia;
                    pro.Costo_Tiempo = ctl_Proyecto.Costo_Tiempo;
                    pro.Fecha_Inicio = ctl_Proyecto.Fecha_Inicio;
                    pro.Estatus = true;
                    db.ctl_Proyecto.Add(pro);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    string errorEV = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            errorEV += string.Format("- Campo: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    ViewBag.errors = errorEV;
                }
            }
            else
            {
                string errorString = "";
                string validationErrors = string.Join(",",
                      ModelState.Values.Where(E => E.Errors.Count > 0)
                      .SelectMany(E => E.Errors)
                      .Select(E => E.ErrorMessage)
                      .ToArray());

                errorString += validationErrors;
                ViewBag.errorM = errorString;
            }

            ViewBag.Id_Cliente = new SelectList(db.ctl_Cliente, "Id_Cliente", "RazonSocial");
            ViewBag.Id_Empleado = new SelectList(db.ctl_Empleado, "Id_empleado", "Nombre");
            return View(ctl_Proyecto);
        }

        // GET: Proyecto/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Proyecto ctl_Proyecto = db.ctl_Proyecto.Find(id);
            if (ctl_Proyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Proyecto = new SelectList(db.ctl_Cliente, "Id_Cliente", "RazonSocial", ctl_Proyecto.Id_Proyecto);
            ViewBag.Id_Empleado = new SelectList(db.ctl_Empleado, "Id_empleado", "Nombre", ctl_Proyecto.Id_Empleado);
            return View(ctl_Proyecto);
        }

        // POST: Proyecto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Proyecto,Id_Cliente,Id_Empleado,Nombre_Proyecto,Dias_Asignados,Horas_Asignadas,Costo_Dia,Costo_Tiempo,Fecha_Inicio,Estatus")] ctl_Proyecto ctl_Proyecto)
        {
            if (ModelState.IsValid)
            {
                ctl_Proyecto p = db.ctl_Proyecto.Where(x => x.Id_Proyecto == ctl_Proyecto.Id_Proyecto).FirstOrDefault();
                p.Nombre_Proyecto = ctl_Proyecto.Nombre_Proyecto;
                p.Dias_Asignados = ctl_Proyecto.Dias_Asignados;
                p.Horas_Asignadas = ctl_Proyecto.Horas_Asignadas;
                p.Costo_Dia = ctl_Proyecto.Costo_Dia;
                p.Costo_Tiempo = ctl_Proyecto.Costo_Tiempo;
                p.Fecha_Inicio = ctl_Proyecto.Fecha_Inicio;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Proyecto = new SelectList(db.ctl_Cliente, "Id_Cliente", "RazonSocial", ctl_Proyecto.Id_Proyecto);
            ViewBag.Id_Empleado = new SelectList(db.ctl_Empleado, "Id_empleado", "Nombre", ctl_Proyecto.Id_Empleado);
            return View(ctl_Proyecto);
        }

        // GET: Proyecto/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Proyecto ctl_Proyecto = db.ctl_Proyecto.Find(id);
            if (ctl_Proyecto == null)
            {
                return HttpNotFound();
            }
            return View(ctl_Proyecto);
        }

        // POST: Proyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ctl_Proyecto ctl_Proyecto = db.ctl_Proyecto.Find(id);
            db.ctl_Proyecto.Remove(ctl_Proyecto);
            db.SaveChanges();
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
