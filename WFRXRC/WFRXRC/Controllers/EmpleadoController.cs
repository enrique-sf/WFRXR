using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WFRXRC.Entities;
using WFRXRC.Models;

namespace WFRXRC.Controllers
{
    public class EmpleadoController : Controller
    {
        private Tbl_RXREntities db = new Tbl_RXREntities();

        // GET: Empleado
        public ActionResult Index()
        {
            int pagina = 201; //ID EN BASE DE DATOS
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
            var ctl_Empleado = db.ctl_Empleado.Where(x => x.Estatus == true).ToList();
            return View(ctl_Empleado);
        }

        // GET: Empleado/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_Empleado ctl_Empleado = db.ctl_Empleado.Find(id);
            int pagina = 205; //ID EN BASE DE DATOS
            ctl_Login login = db.ctl_Login.Where(i => i.Id_Empleado == id).FirstOrDefault();
            ctl_Nomina nom = db.ctl_Nomina.Where(i => i.Id_Empleado == id).FirstOrDefault();
            Mod_Empleado emp = new Mod_Empleado();
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
            //Llenado del modelo Empleado
            emp.Id_empleado = ctl_Empleado.Id_empleado;
            emp.Nombre = ctl_Empleado.Nombre;
            emp.ApellidoP = ctl_Empleado.ApellidoP;
            if (ctl_Empleado.ApellidoM != null)
            {
                emp.ApellidoM = ctl_Empleado.ApellidoM;//puede ser null 
            }
            else
            {
                emp.ApellidoM = "";//puede ser null 
            }
            emp.FechaNacimiento = ctl_Empleado.FechaNacimiento;
            emp.Calle = ctl_Empleado.Calle;
            emp.NumExt = ctl_Empleado.NumExt;
            if (ctl_Empleado.NumInt != null)
            {
                emp.NumInt = ctl_Empleado.NumInt;//puede ser null
            }
            else
            {
                emp.NumInt = "";
            }
            emp.Colonia = ctl_Empleado.Colonia;
            emp.Ciudad = ctl_Empleado.Ciudad;
            emp.Tel_emergencia = ctl_Empleado.Tel_emergencia;
            if (ctl_Empleado.Contacto_emergencia != null)
            {
                emp.Contacto_emergencia = ctl_Empleado.Contacto_emergencia;//puede ser null
            }
            else
            {
                emp.Contacto_emergencia = "";
            }
            emp.FechaIngreso = ctl_Empleado.FechaIngreso;
            if (ctl_Empleado.VacacionesN != null)
            {
                emp.VacacionesN = ctl_Empleado.VacacionesN;//puede ser null
            }
            else
            {
                emp.VacacionesN = 0;
            }
            emp.IMSS = ctl_Empleado.IMSS;
            emp.CURP = ctl_Empleado.CURP;
            emp.INE_N = ctl_Empleado.INE_N;
            emp.Pasaporte_N = ctl_Empleado.Pasaporte_N;
            emp.Id_SAP = ctl_Empleado.Id_SAP; //Id provisional en lo que se llena la tabla sap
            emp.Email = ctl_Empleado.Email;
            emp.Estatus = true;
            //Llenado del modelo Login
            emp.Usuario = login.Usuario;
            emp.Pwd = login.Pwd;
            emp.Permisos = login.Permisos;//Id provisional en lo que se definen los permisos
            //Llenado del modelo Nomina
            emp.Bono_Monto = nom.Bono_Monto;
            emp.Sueldo = nom.Sueldo;
            emp.Dias_Vacaciones = nom.Dias_Vacaciones;
            emp.Dias_Asignados = nom.Dias_Asignados.GetValueOrDefault();
            emp.Dias_Asignados = nom.Dias_SAginacion.GetValueOrDefault();
            if (ctl_Empleado == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            int pagina = 202; //ID EN BASE DE DATOS
            using (Tbl_RXREntities db = new Tbl_RXREntities())
            {
                //string u = User.Identity.Name;
                //var _u = db.ctl_Empleado.Where(a => a.Nombre.Equals(u)).FirstOrDefault();
                //var user = db.ctl_Login.Where(a => a.Id_Empleado.Equals(u)).FirstOrDefault();
                //ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                //ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.returnUrl = Request.Url.PathAndQuery;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals("ES")).FirstOrDefault().TXT50;
                ViewBag.PERFIL = new SelectList(db.ctl_Perfil.ToList(), "id", "descripcion");
            }
            return View();
        }

        // POST: Empleado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_empleado,Nombre,ApellidoP,ApellidoM,FechaNacimiento,Calle,NumExt,NumInt,Colonia,Ciudad,Tel_emergencia,Contacto_emergencia,FechaIngreso,VacacionesN,IMSS,CURP,INE_N,Pasaporte_N,Id_SAP,Email,Estatus," +
            "Usuario,Pwd,Ultimo_Acceso,Sesion_Activa,Sueldo,Dias_Vacaciones,Bono_Monto,Dias_Asignados,Dias_SAginacion,PERFIL")]Mod_Empleado ctl_Empleado)
        {
            int pagina = 202;
            string errorString = "";
            if (ModelState.IsValid)
            {
                try
                {
                    ctl_Empleado emp = new ctl_Empleado();
                    //paso de los valores capturados al tipo de objeto
                    emp.Nombre = ctl_Empleado.Nombre;
                    emp.ApellidoP = ctl_Empleado.ApellidoP;
                    emp.ApellidoM = ctl_Empleado.ApellidoM;//puede ser null                   
                    string _f = ctl_Empleado.FechaNacimiento.ToString("dd-MM-yyyy");
                    emp.FechaNacimiento = DateTime.Parse(_f);
                    emp.Calle = ctl_Empleado.Calle;
                    emp.NumExt = ctl_Empleado.NumExt;
                    emp.NumInt = ctl_Empleado.NumInt;//puede ser null
                    emp.Colonia = ctl_Empleado.Colonia;
                    emp.Ciudad = ctl_Empleado.Ciudad;
                    emp.Tel_emergencia = ctl_Empleado.Tel_emergencia;
                    emp.Contacto_emergencia = ctl_Empleado.Contacto_emergencia;//puede ser null
                    string _fi = ctl_Empleado.FechaIngreso.ToString("dd-MM-yyyy");
                    emp.FechaIngreso = DateTime.Parse(_fi);
                    emp.VacacionesN = ctl_Empleado.VacacionesN;//puede ser null
                    emp.IMSS = ctl_Empleado.IMSS;
                    emp.CURP = ctl_Empleado.CURP;
                    emp.INE_N = ctl_Empleado.INE_N;
                    emp.Pasaporte_N = ctl_Empleado.Pasaporte_N;
                    emp.Id_SAP = 501;//Id provisional en lo que se llena la tabla sap
                    emp.Email = ctl_Empleado.Email;
                    emp.Estatus = true;

                    //Guardado del objeto Empleado creado
                    db.ctl_Empleado.Add(emp);
                    db.SaveChanges();
                    //Retorno del id generado
                    long _id = 0;
                    _id = emp.Id_empleado;

                    if (_id > 0)
                    {
                        //posterior a la insercion de datos en la tabla empleado, paso a ingresar datos a ocupar en tabla de login
                        ctl_Login _log = new ctl_Login();
                        _log.Id_Empleado = _id;
                        _log.Usuario = ctl_Empleado.Usuario;
                        _log.Pwd = ctl_Empleado.Pwd;
                        _log.Ultimo_Acceso = ctl_Empleado.Ultimo_Acceso;//puede ser null
                        _log.Sesion_Activa = ctl_Empleado.Sesion_Activa;//puede ser null
                        _log.Permisos = ctl_Empleado.PERFIL;//Id provisional en lo que se definen los permisos
                        _log.Estatus = true;
                        //Guardado del objeto Login creado
                        db.ctl_Login.Add(_log);
                        db.SaveChanges();
                        //De igual manera paso a la insercion de datos en la tabla de nomina
                        ctl_Nomina cnom = new ctl_Nomina();
                        cnom.Id_Empleado = _id;
                        cnom.Sueldo = ctl_Empleado.Sueldo;
                        cnom.Dias_Vacaciones = ctl_Empleado.Dias_Vacaciones;
                        cnom.Dias_Asignados = ctl_Empleado.Dias_Asignados;
                        cnom.Dias_SAginacion = ctl_Empleado.Dias_SAginacion;
                        cnom.Bono_Monto = ctl_Empleado.Bono_Monto;
                        cnom.Estatus = true;
                        //Guardado del objeto nomina creado
                        db.ctl_Nomina.Add(cnom);
                        db.SaveChanges();
                    }
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
                string validationErrors = string.Join(",",
                   ModelState.Values.Where(E => E.Errors.Count > 0)
                   .SelectMany(E => E.Errors)
                   .Select(E => E.ErrorMessage)
                   .ToArray());
            }

            ViewBag.returnUrl = Request.Url.PathAndQuery;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals("ES")).FirstOrDefault().TXT50;
            ViewBag.errorM = errorString;
            return View(ctl_Empleado);
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int pagina = 203; //ID EN BASE DE DATOS
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
            ctl_Empleado ctl_Empleado = db.ctl_Empleado.Where(a => a.Id_empleado == id).FirstOrDefault();
            ctl_Login login = db.ctl_Login.Where(i => i.Id_Empleado == id).FirstOrDefault();
            ctl_Nomina nom = db.ctl_Nomina.Where(i => i.Id_Empleado == id).FirstOrDefault();
            Mod_Empleado emp = new Mod_Empleado();

            //Llenado del modelo Empleado
            emp.Id_empleado = ctl_Empleado.Id_empleado;
            emp.Nombre = ctl_Empleado.Nombre;
            emp.ApellidoP = ctl_Empleado.ApellidoP;
            if (ctl_Empleado.ApellidoM != null)
            {
                emp.ApellidoM = ctl_Empleado.ApellidoM;//puede ser null 
            }
            else
            {
                emp.ApellidoM = "";//puede ser null 
            }
            emp.FechaNacimiento = ctl_Empleado.FechaNacimiento;
            emp.Calle = ctl_Empleado.Calle;
            emp.NumExt = ctl_Empleado.NumExt;
            if (ctl_Empleado.NumInt != null)
            {
                emp.NumInt = ctl_Empleado.NumInt;//puede ser null
            }
            else
            {
                emp.NumInt = "";
            }
            emp.Colonia = ctl_Empleado.Colonia;
            emp.Ciudad = ctl_Empleado.Ciudad;
            emp.Tel_emergencia = ctl_Empleado.Tel_emergencia;
            if (ctl_Empleado.Contacto_emergencia != null)
            {
                emp.Contacto_emergencia = ctl_Empleado.Contacto_emergencia;//puede ser null
            }
            else
            {
                emp.Contacto_emergencia = "";
            }
            emp.FechaIngreso = ctl_Empleado.FechaIngreso;
            if (ctl_Empleado.VacacionesN != null)
            {
                emp.VacacionesN = ctl_Empleado.VacacionesN;//puede ser null
            }
            else
            {
                emp.VacacionesN = 0;
            }
            emp.IMSS = ctl_Empleado.IMSS;
            emp.CURP = ctl_Empleado.CURP;
            emp.INE_N = ctl_Empleado.INE_N;
            emp.Pasaporte_N = ctl_Empleado.Pasaporte_N;
            emp.Id_SAP = ctl_Empleado.Id_SAP; //Id provisional en lo que se llena la tabla sap
            emp.Email = ctl_Empleado.Email;
            emp.Estatus = true;
            //Llenado del modelo Login
            emp.Usuario = login.Usuario;
            emp.Pwd = login.Pwd;
            emp.Permisos = login.Permisos;//Id provisional en lo que se definen los permisos
            //Llenado del modelo Nomina
            emp.Bono_Monto = nom.Bono_Monto;
            emp.Sueldo = nom.Sueldo;
            emp.Dias_Vacaciones = nom.Dias_Vacaciones;
            emp.Dias_Asignados = nom.Dias_Asignados.GetValueOrDefault();
            emp.Dias_Asignados = nom.Dias_SAginacion.GetValueOrDefault();
            if (ctl_Empleado == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // POST: Empleado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_empleado,Nombre,ApellidoP,ApellidoM,FechaNacimiento,Calle,NumExt,NumInt,Colonia,Ciudad,Tel_emergencia,Contacto_emergencia,FechaIngreso,VacacionesN,IMSS,CURP,INE_N,Pasaporte_N,Id_SAP,Email,Estatus," +
               "Usuario,Pwd,Ultimo_Acceso,Sesion_Activa")]Mod_Empleado ctl_Empleado)
        {
            int pagina = 203;
            string errorString = "";
            if (ModelState.IsValid)
            {
                try
                {
                    ctl_Empleado emp = db.ctl_Empleado.Where(x => x.Id_empleado == ctl_Empleado.Id_empleado).FirstOrDefault();
                    //paso de los valores capturados al tipo de objeto
                    emp.Nombre = ctl_Empleado.Nombre;
                    emp.ApellidoP = ctl_Empleado.ApellidoP;
                    emp.ApellidoM = ctl_Empleado.ApellidoM;//puede ser null                   
                    string _f = ctl_Empleado.FechaNacimiento.ToString("dd-MM-yyyy");
                    emp.FechaNacimiento = DateTime.Parse(_f);
                    emp.Calle = ctl_Empleado.Calle;
                    emp.NumExt = ctl_Empleado.NumExt;
                    emp.NumInt = ctl_Empleado.NumInt;//puede ser null
                    emp.Colonia = ctl_Empleado.Colonia;
                    emp.Ciudad = ctl_Empleado.Ciudad;
                    emp.Tel_emergencia = ctl_Empleado.Tel_emergencia;
                    emp.Contacto_emergencia = ctl_Empleado.Contacto_emergencia;//puede ser null
                    string _fi = ctl_Empleado.FechaIngreso.ToString("dd-MM-yyyy");
                    emp.FechaIngreso = DateTime.Parse(_fi);
                    emp.VacacionesN = ctl_Empleado.VacacionesN;//puede ser null
                    emp.IMSS = ctl_Empleado.IMSS;
                    emp.CURP = ctl_Empleado.CURP;
                    emp.INE_N = ctl_Empleado.INE_N;
                    emp.Pasaporte_N = ctl_Empleado.Pasaporte_N;
                    emp.Id_SAP = 501;//Id provisional en lo que se llena la tabla sap
                    emp.Email = ctl_Empleado.Email;
                    emp.Estatus = true;

                    //Guardado del objeto Empleado creado
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();

                    //posterior a la actualizacion de datos en la tabla empleado, paso a modificar los datos en tabla de login
                    ctl_Login _log = db.ctl_Login.Where(x => x.Id_Empleado == ctl_Empleado.Id_empleado).FirstOrDefault();
                    _log.Usuario = ctl_Empleado.Usuario;
                    _log.Pwd = ctl_Empleado.Pwd;
                    _log.Ultimo_Acceso = ctl_Empleado.Ultimo_Acceso;//puede ser null
                    _log.Sesion_Activa = ctl_Empleado.Sesion_Activa;//puede ser null
                    _log.Permisos = 1;//Id provisional en lo que se definen los permisos
                    _log.Estatus = true;
                    //Guardado del objeto Login modificado
                    db.Entry(_log).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
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
            return View(ctl_Empleado);
        }

        // GET: Empleado/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int pagina = 204; //ID EN BASE DE DATOS
            ctl_Empleado ctl_Empleado = db.ctl_Empleado.Find(id);
            ctl_Login login = db.ctl_Login.Where(i => i.Id_Empleado == id).FirstOrDefault();
            ctl_Nomina nom = db.ctl_Nomina.Where(i => i.Id_Empleado == id).FirstOrDefault();
            Mod_Empleado emp = new Mod_Empleado();
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
            //Llenado del modelo Empleado
            emp.Id_empleado = ctl_Empleado.Id_empleado;
            emp.Nombre = ctl_Empleado.Nombre;
            emp.ApellidoP = ctl_Empleado.ApellidoP;
            if (ctl_Empleado.ApellidoM != null)
            {
                emp.ApellidoM = ctl_Empleado.ApellidoM;//puede ser null 
            }
            else
            {
                emp.ApellidoM = "";//puede ser null 
            }
            emp.FechaNacimiento = ctl_Empleado.FechaNacimiento;
            emp.Calle = ctl_Empleado.Calle;
            emp.NumExt = ctl_Empleado.NumExt;
            if (ctl_Empleado.NumInt != null)
            {
                emp.NumInt = ctl_Empleado.NumInt;//puede ser null
            }
            else
            {
                emp.NumInt = "";
            }
            emp.Colonia = ctl_Empleado.Colonia;
            emp.Ciudad = ctl_Empleado.Ciudad;
            emp.Tel_emergencia = ctl_Empleado.Tel_emergencia;
            if (ctl_Empleado.Contacto_emergencia != null)
            {
                emp.Contacto_emergencia = ctl_Empleado.Contacto_emergencia;//puede ser null
            }
            else
            {
                emp.Contacto_emergencia = "";
            }
            emp.FechaIngreso = ctl_Empleado.FechaIngreso;
            if (ctl_Empleado.VacacionesN != null)
            {
                emp.VacacionesN = ctl_Empleado.VacacionesN;//puede ser null
            }
            else
            {
                emp.VacacionesN = 0;
            }
            emp.IMSS = ctl_Empleado.IMSS;
            emp.CURP = ctl_Empleado.CURP;
            emp.INE_N = ctl_Empleado.INE_N;
            emp.Pasaporte_N = ctl_Empleado.Pasaporte_N;
            emp.Id_SAP = ctl_Empleado.Id_SAP; //Id provisional en lo que se llena la tabla sap
            emp.Email = ctl_Empleado.Email;
            emp.Estatus = true;
            //Llenado del modelo Login
            emp.Usuario = login.Usuario;
            emp.Pwd = login.Pwd;
            emp.Permisos = login.Permisos;//Id provisional en lo que se definen los permisos
            //Llenado del modelo Nomina
            emp.Bono_Monto = nom.Bono_Monto;
            emp.Sueldo = nom.Sueldo;
            emp.Dias_Vacaciones = nom.Dias_Vacaciones;
            emp.Dias_Asignados = nom.Dias_Asignados.GetValueOrDefault();
            emp.Dias_Asignados = nom.Dias_SAginacion.GetValueOrDefault();
            if (ctl_Empleado == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            //Borrado Logico para ctl_empleado
            ctl_Empleado ctl_Empleado = db.ctl_Empleado.Where(x => x.Id_empleado == id).FirstOrDefault();
            ctl_Empleado.Estatus = false;
            db.Entry(ctl_Empleado).State = EntityState.Modified;
            db.SaveChanges();
            //Borrado Logico para ctl_login
            ctl_Login _l = db.ctl_Login.Where(x => x.Id_Empleado == id).FirstOrDefault();
            _l.Estatus = false;
            db.Entry(_l).State = EntityState.Modified;
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
