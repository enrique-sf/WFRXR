using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WFRXRC.Entities;
using WFRXRC.Models;

namespace WFRXRC.Controllers
{
    public class WorkTimeController : Controller
    {
        private Tbl_RXREntities db = new Tbl_RXREntities();

        // GET: WorkTime
        public ActionResult Index()
        {
            var ide = Session["idU"];
            var p = Session["permisos"];
            List<ctl_WorkTime> res = new List<ctl_WorkTime>();
            if (p.ToString() == "1")
            {
                res = db.ctl_WorkTime.ToList();
            }
            else if (p.ToString() != "1")
            {
                var _ide = int.Parse(ide.ToString());
                res = db.ctl_WorkTime.Where(x => x.Id_Empleado == _ide).ToList();
            }
            return View(res);
        }

        // GET: WorkTime/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_WorkTime ctl_WorkTime = db.ctl_WorkTime.Find(id);
            ctl_SAP sap = new ctl_SAP();
            if (ctl_WorkTime.Asignacion_SAP == "X")
            {
            }
            sap = db.ctl_SAP.Where(x => x.idWT == ctl_WorkTime.id_WT).FirstOrDefault();
            ViewBag.OSap = sap;
            ViewBag.tasig = ctl_WorkTime.Asignacion_SAP;
            var fechas = ctl_WorkTime.Fecha_Carga.Split('-');
            ViewBag.finicio = fechas[0];
            ViewBag.ffin = fechas[1];
            ViewBag.asig = ctl_WorkTime.Asignacion_SAP;


            //
            List<string> lstP = new List<string>();
            lstP.Add("Elegir Opción");
            lstP.Add("No Asignado");
            lstP.Add("Vacaciones");
            lstP.Add("Incapacidad");
            ViewBag.proyecto = new SelectList(lstP, lstP[1]);
            var _p = db.ctl_Proyecto.Where(x => x.Estatus == true).ToList();
            ViewBag.lstProyectos = new SelectList(_p, "Id_Proyecto", "Nombre_Proyecto", _p.Where(x => x.Id_Proyecto == ctl_WorkTime.Id_Proyecto).FirstOrDefault());
            ViewBag.lstClientes = new SelectList(db.ctl_Cliente.Where(x => x.Estatus == true).ToList(), "Id_Cliente", "RazonSocial", ctl_WorkTime.id_Cliente);
            if (ctl_WorkTime == null)
            {
                return HttpNotFound();
            }
            return View(ctl_WorkTime);
        }

        // GET: WorkTime/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: WorkTime
        public ActionResult Descarga()
        {
            return View();
        }

        // POST: WorkTime/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Empleado,Id_Proyecto,Dias_Laborados,Horas_Laboradas,Asignacion_SAP,Id_Cargo,Fecha_Carga,estatus")] ctl_WorkTime ctl_WorkTime)
        {
            if (ModelState.IsValid)
            {
                db.ctl_WorkTime.Add(ctl_WorkTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ctl_WorkTime);
        }

        //Para guardar lo Creado y vaciado en tabla SAP
        [HttpPost]
        public JsonResult procesarTablaSap(List<Mod_TSAP> data)
        {
            if (data.Count > 0)
            {
                long _b = 0;
                ctl_SAP csap = new ctl_SAP();
                for (int i = 0; i < data.Count; i++)
                {
                    try
                    {
                        var inicio = data[i].inicio;
                        var fin = data[i].fin;
                        //saco todas las fechas entre ese rango de inicio a fin 
                        var rangoFechas = DateRange(data[i].inicio, data[i].fin).ToList();
                        //Traigo la cantidad de Meses
                        var arrM = (from n in rangoFechas
                                    select n.Month).Distinct().ToList();
                        for (int x = 0; x < arrM.Count; x++)
                        {
                            var _bymonth = rangoFechas.Where(y => y.Month == arrM[x]).ToList();

                            //Se agrega uno por cada mes
                            csap = new ctl_SAP();
                            csap.Id_Empleado = data[i].id_empleado;
                            csap.Id_SAP = 0;
                            csap.Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(arrM[x]);
                            csap.Servicio = data[i].servicio;
                            csap.Ubicacion = data[i].ubicacion;
                            csap.Desc_CRM = data[i].desc_crm;
                            csap.Id_Cliente = data[i].cliente;
                            csap.CW = data[i].cw;
                            csap.Fecha_Asignacion = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                            //csap.Responsable_SAP = data[i].rsap;
                            csap.Cantidad_Dias = _bymonth.Count;
                            csap.Service_Order = data[i].sorder;
                            db.ctl_SAP.Add(csap);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
                JsonResult jc = Json(_b, JsonRequestBehavior.AllowGet);
                return jc;
            }
            else
            {
                JsonResult jc = Json("404", JsonRequestBehavior.AllowGet);
                return jc;
            }
        }

        public IEnumerable<DateTime> DateRange(DateTime fromDate, DateTime toDate)
        {
            return Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1)
                             .Select(d => fromDate.AddDays(d));
        }

        //Para guardar lo Creado y vaciado en tabla dia
        [HttpPost]
        public JsonResult procesarJson(List<Mod_TSAP> data)
        {
            if (data.Count > 0)
            {
                long _b = 0;
                ctl_Dia cd = new ctl_Dia();
                for (int i = 0; i < data.Count; i++)
                {
                    try
                    {
                        //si cuando se carga por dias, cae en sabado o domingo no lo ingreso a la tabla de dia
                        if (data[i].inicio.DayOfWeek != DayOfWeek.Saturday && data[i].inicio.DayOfWeek != DayOfWeek.Sunday)
                        {
                            cd = new ctl_Dia();
                            cd.cargaxDiaI = data[i].inicio;
                            var fi = data[i].inicio;
                            cd.horaI = fi.ToString("HH:mm tt");//el HH mayusculas es para formato de 24 hrs y el hh minusculas para 12 hrs
                            cd.cargaxDiaF = data[i].fin;
                            var ff = data[i].fin;
                            cd.horaF = ff.ToString("HH:mm tt");//el HH mayusculas es para formato de 24 hrs y el hh minusculas para 12 hrs
                            cd.Explicacion = data[i].desc_crm;
                            cd.idEmpleado = data[i].rsap;
                            cd.titulo = data[i].servicio;
                            var _i = data[i].inicio.Hour;
                            var _f = data[i].fin.Hour;
                            cd.totHoras = _f - _i;
                            cd.serviceO = data[i].sorder;
                            db.ctl_Dia.Add(cd);
                            db.SaveChanges();
                            _b = cd.id_Dia;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
                JsonResult jc = Json(_b, JsonRequestBehavior.AllowGet);
                return jc;
            }
            else
            {
                JsonResult jc = Json("404", JsonRequestBehavior.AllowGet);
                return jc;
            }
        }

        [HttpPost]
        public JsonResult actualizarFecha(List<Mod_WT> data)
        {
            try
            {
                var id = data[0].id;
                var cd = db.ctl_Dia.Where(x => x.id_Dia == id).FirstOrDefault();
                cd.cargaxDiaI = data[0].start;
                var fi = data[0].start;
                cd.horaI = fi.ToString("HH:mm tt");//el HH mayusculas es para formato de 24 hrs y el hh minusculas para 12 hrs
                cd.cargaxDiaF = data[0].end;
                var ff = data[0].end;
                cd.horaF = ff.ToString("HH:mm tt");//el HH mayusculas es para formato de 24 hrs y el hh minusculas para 12 hrs
                var _i = data[0].start.Hour;
                var _f = data[0].end.Hour;
                cd.totHoras = _f - _i;

                db.Entry(cd).State = EntityState.Modified;
                db.SaveChanges();
                JsonResult jc = Json("999", JsonRequestBehavior.AllowGet);
                return jc;
            }
            catch (Exception e)
            {
                JsonResult jc = Json("404", JsonRequestBehavior.AllowGet);
                return jc;
            }
        }

        [HttpPost]
        public JsonResult actualizarEvento(List<Mod_WT> data)
        {

            if (data.Count > 0)
            {
                try
                {
                    var id = data[0].id;
                    var cd = db.ctl_Dia.Where(x => x.id_Dia == id).FirstOrDefault();
                    cd.cargaxDiaI = data[0].start;
                    var fi = data[0].start;
                    cd.horaI = fi.ToString("HH:mm tt");//el HH mayusculas es para formato de 24 hrs y el hh minusculas para 12 hrs
                    cd.cargaxDiaF = data[0].end;
                    var ff = data[0].end;
                    cd.horaF = ff.ToString("HH:mm tt");//el HH mayusculas es para formato de 24 hrs y el hh minusculas para 12 hrs
                    cd.Explicacion = data[0].body;
                    cd.idEmpleado = data[0].empleado;
                    cd.titulo = data[0].title;
                    var _i = data[0].start.Hour;
                    var _f = data[0].end.Hour;
                    cd.totHoras = _f - _i;
                    db.Entry(cd).State = EntityState.Modified;
                    db.SaveChanges();
                    JsonResult jc = Json("999", JsonRequestBehavior.AllowGet);
                    return jc;
                }
                catch (Exception e)
                {
                    JsonResult jc = Json("404", JsonRequestBehavior.AllowGet);
                    return jc;
                }
            }
            else
            {
                JsonResult jc = Json("404", JsonRequestBehavior.AllowGet);
                return jc;
            }
        }

        [HttpPost]//lej 18.02.2019
        public JsonResult getClientes()
        {
            var res = db.ctl_Cliente.Where(x => x.Estatus == true).ToList();
            List<Mod_Cliente> mc = new List<Mod_Cliente>();
            for (int i = 0; i < res.Count; i++)
            {
                Mod_Cliente m = new Mod_Cliente();
                m.Id_Cliente = res[i].Id_Cliente;
                m.RazonSocial = res[i].RazonSocial;
                mc.Add(m);
            }
            JsonResult cc = Json(mc, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpPost]//lej 10.03.2019
        public JsonResult getProyectos()
        {
            var res = db.ctl_Proyecto.Where(x => x.Estatus == true).ToList();
            List<Mod_Proyecto> mc = new List<Mod_Proyecto>();
            for (int i = 0; i < res.Count; i++)
            {
                Mod_Proyecto m = new Mod_Proyecto();
                m.Id_Proyecto = res[i].Id_Proyecto;
                m.Nombre_Proyecto = res[i].Nombre_Proyecto;
                mc.Add(m);
            }
            JsonResult cc = Json(mc, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpPost]
        public JsonResult borrarEvento(long id)
        {
            try
            {
                ctl_Dia cd = db.ctl_Dia.Where(i => i.id_Dia == id).FirstOrDefault();
                if (cd != null)
                {
                    db.ctl_Dia.Remove(cd);
                    db.SaveChanges();
                    JsonResult cc = Json("999", JsonRequestBehavior.AllowGet);
                    return cc;
                }
                else
                {
                    JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                    return cc;
                }
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }
        //busqueda para con asignacion
        [HttpPost]
        public JsonResult busquedaRegistroIns(List<Mod_WT> data)
        {
            try
            {
                Mod_WT mwt = data[0];
                var str = mwt.fInicio + " - " + mwt.fFin;
                //si res trae datos significa que hay mas casos en esa fecha
                var res = db.ctl_WorkTime.Where(x => x.Fecha_Carga == str).ToList();
                //si res2 trae datos significa que hay mas casos con ese service order
                var res2 = db.ctl_SAP.Where(x => x.Service_Order == mwt.sOrder).ToList();
                if (res.Count > 0 && res2.Count > 0)//indica que de ambos tiene mas de 1 registro
                {
                    JsonResult cc = Json("M1", JsonRequestBehavior.AllowGet);
                    return cc;
                }
                else
                {
                    JsonResult cc = Json("0", JsonRequestBehavior.AllowGet);
                    return cc;
                }
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        //busqueda para con asignacion
        [HttpPost]
        public JsonResult busquedaRegistroSas(List<Mod_WT> data)
        {
            try
            {
                Mod_WT mwt = data[0];
                var str = mwt.fInicio + " - " + mwt.fFin;
                //si res trae datos significa que hay mas casos en esa fecha
                var res = db.ctl_WorkTime.Where(x => x.Fecha_Carga == str && x.Asignacion_SAP != "X").ToList();
                if (res.Count > 0)//indica que ya tiene 1 registro
                {
                    JsonResult cc = Json("M1", JsonRequestBehavior.AllowGet);
                    return cc;
                }
                else
                {
                    JsonResult cc = Json("0", JsonRequestBehavior.AllowGet);
                    return cc;
                }
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        //Sin asignacion
        [HttpPost]
        public JsonResult guardadoSAsignacion(List<Mod_WT> data)
        {
            try
            {
                //Siempre se mandara solo 1 registro, aunque se declare como lista
                Mod_WT r0 = data[0];
                ctl_WorkTime wt = new ctl_WorkTime();
                //Calculo de horas
                var hi = DateTime.Parse(r0.hInicio);
                var hf = DateTime.Parse(r0.hFin);
                TimeSpan span = hf - hi;

                //saco todas las fechas entre ese rango de inicio a fin 
                var rangoFechas = DateRange(DateTime.Parse(r0.fInicio), DateTime.Parse(r0.fFin)).ToList();
                //Traigo la cantidad de Meses
                var arrM = (from n in rangoFechas
                            select n.Month).Distinct().ToList();
                for (int x = 0; x < arrM.Count; x++)
                {
                    wt = new ctl_WorkTime();
                    var _bymonth = rangoFechas.Where(y => y.Month == arrM[x]).ToList();
                    wt.Id_Empleado = r0.idEmpleado;
                    wt.Id_Proyecto = r0.proyecto;//0--1--2
                                                 //
                    wt.Dias_Laborados = _bymonth.Count;
                    //separo horas completas y minutos
                    var arrH = span.ToString().Split(':');
                    var horas = int.Parse(arrH[0]);
                    var frHora = arrH[1];
                    //cantidad de minutos entre 60 min(1 hora) por los dias
                    var op = (int.Parse(frHora) / 60) * _bymonth.Count;
                    wt.Horas_Laboradas = (horas * _bymonth.Count) + (op);
                    wt.Asignacion_SAP = r0.Asignacion;
                    wt.id_Cliente = r0.cliente;
                    //
                    wt.Fecha_Carga = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    wt.estatus = true;
                    db.ctl_WorkTime.Add(wt);
                    db.SaveChanges();
                }
                JsonResult cc = Json(wt.id_WT, JsonRequestBehavior.AllowGet);
                return cc;
            }
            catch (Exception e)
            {
                JsonResult cc = Json("990004009", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        [HttpPost]
        public JsonResult terminadoSAsignacion(List<Mod_WT> data)
        {
            try
            {
                //Siempre se mandara solo 1 registro, aunque se declare como lista
                Mod_WT r0 = data[0];
                ctl_WorkTime wt = new ctl_WorkTime();
                wt.Id_Empleado = r0.idEmpleado;
                wt.Id_Proyecto = r0.proyecto;//0--1--2
                //Calculo de horas
                var hi = DateTime.Parse(r0.hInicio);
                var hf = DateTime.Parse(r0.hFin);
                TimeSpan span = hf - hi;
                //saco todas las fechas entre ese rango de inicio a fin 
                var rangoFechas = DateRange(DateTime.Parse(r0.fInicio), DateTime.Parse(r0.fFin)).ToList();
                //Traigo la cantidad de Meses
                var arrM = (from n in rangoFechas
                            select n.Month).Distinct().ToList();
                var idc = "";
                for (int x = 0; x < arrM.Count; x++)
                {
                    var _bymonth = rangoFechas.Where(y => y.Month == arrM[x]).ToList();
                    //
                    wt = new ctl_WorkTime();
                    wt.Dias_Laborados = 0;
                    //separo horas completas y minutos
                    var arrH = span.ToString().Split(':');
                    var horas = int.Parse(arrH[0]);
                    var frHora = arrH[1];
                    //cantidad de minutos entre 60 min(1 hora) por los dias
                    var op = (int.Parse(frHora) / 60) * _bymonth.Count;
                    wt.Horas_Laboradas = (horas * _bymonth.Count) + (op);
                    wt.Asignacion_SAP = r0.Asignacion;
                    //
                    wt.Fecha_Carga = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    wt.estatus = true;
                    db.ctl_WorkTime.Add(wt);
                    db.SaveChanges();

                    //actualizar id_Cargo
                    var wkt = db.ctl_WorkTime.Where(y => y.id_WT == wt.id_WT).FirstOrDefault();
                    wkt.Id_Cargo = generarFolio(wt.id_WT);
                    idc = wkt.Id_Cargo;
                    db.Entry(wt).State = EntityState.Modified;
                    db.SaveChanges();
                }

                JsonResult cc = Json(idc, JsonRequestBehavior.AllowGet);
                return cc;
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        [HttpPost]
        public JsonResult editarSAsignacion(List<Mod_WT> data)
        {
            try
            {
                //Siempre se mandara solo 1 registro, aunque se declare como lista
                Mod_WT r0 = data[0];
                ctl_WorkTime wt = db.ctl_WorkTime.Where(x => x.id_WT == r0.idWT).FirstOrDefault();
                //saco todas las fechas entre ese rango de inicio a fin 
                var rangoFechas = DateRange(DateTime.Parse(r0.fInicio), DateTime.Parse(r0.fFin)).ToList();
                //Traigo la cantidad de Meses
                var arrM = (from n in rangoFechas
                            select n.Month).Distinct().ToList();
                var mc = 0;
                for (int x = 0; x < arrM.Count; x++)
                {
                    var _bymonth = rangoFechas.Where(y => y.Month == arrM[x]).ToList();
                    mc += _bymonth.Count;
                }
                //Calculo de horas
                var hi = DateTime.Parse(r0.hInicio);
                var hf = DateTime.Parse(r0.hFin);
                TimeSpan span = hf - hi;
                var idc = "";
                wt.Id_Proyecto = r0.proyecto;//0--1--2
                                             //separo horas completas y minutos
                var arrH = span.ToString().Split(':');
                var horas = int.Parse(arrH[0]);
                var frHora = arrH[1];
                //cantidad de minutos entre 60 min(1 hora) por los dias
                var op = (int.Parse(frHora) / 60) * mc;  //
                wt.Dias_Laborados = 0;
                wt.Horas_Laboradas = (horas * mc) + (op);
                wt.Asignacion_SAP = r0.Asignacion;
                //
                wt.Fecha_Carga = r0.fInicio + "-" + r0.fFin;
                wt.estatus = true;
                db.Entry(wt).State = EntityState.Modified;
                db.SaveChanges();
                JsonResult cc = Json(wt.id_WT, JsonRequestBehavior.AllowGet);
                return cc;
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        [HttpPost]
        public JsonResult editarTSAsignacion(List<Mod_WT> data)
        {
            try
            {
                //Siempre se mandara solo 1 registro, aunque se declare como lista
                Mod_WT r0 = data[0];
                ctl_WorkTime wt = db.ctl_WorkTime.Where(x => x.id_WT == r0.idWT).FirstOrDefault();
                //saco todas las fechas entre ese rango de inicio a fin 
                var rangoFechas = DateRange(DateTime.Parse(r0.fInicio), DateTime.Parse(r0.fFin)).ToList();
                //Traigo la cantidad de Meses
                var arrM = (from n in rangoFechas
                            select n.Month).Distinct().ToList();
                var mc = 0;
                for (int x = 0; x < arrM.Count; x++)
                {
                    var _bymonth = rangoFechas.Where(y => y.Month == arrM[x]).ToList();
                    mc += _bymonth.Count;
                }
                //Calculo de horas
                var hi = DateTime.Parse(r0.hInicio);
                var hf = DateTime.Parse(r0.hFin);
                TimeSpan span = hf - hi;
                var idc = "";
                //wt.Id_Empleado = r0.idEmpleado;
                wt.Id_Proyecto = r0.proyecto;//0--1--2
                //
                wt.Dias_Laborados = 0;
                var arrH = span.ToString().Split(':');
                var horas = int.Parse(arrH[0]);
                var frHora = arrH[1];
                //cantidad de minutos entre 60 min(1 hora) por los dias
                var op = (int.Parse(frHora) / 60) * mc;  //
                wt.Horas_Laboradas = (horas * mc) + (op);
                wt.Asignacion_SAP = r0.Asignacion;
                //
                wt.Fecha_Carga = r0.fInicio + "-" + r0.fFin;
                wt.Id_Cargo = generarFolio(wt.id_WT);
                wt.estatus = true;
                db.Entry(wt).State = EntityState.Modified;
                db.SaveChanges();
                JsonResult cc = Json(wt.Id_Cargo, JsonRequestBehavior.AllowGet);
                return cc;
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        //Con asignacion
        [HttpPost]
        public JsonResult guardadoCAsignacion(List<Mod_WT> data)
        {
            try
            {
                int v = 0;
                //Siempre se mandara solo 1 registro, aunque se declare como lista
                Mod_WT r0 = data[0];
                ctl_SAP csap = new ctl_SAP();
                //Calculo de horas
                var hi = DateTime.Parse(r0.hInicio);
                var hf = DateTime.Parse(r0.hFin);
                TimeSpan span = hf - hi;
                //saco todas las fechas entre ese rango de inicio a fin 
                var rangoFechas = DateRange(DateTime.Parse(r0.fInicio), DateTime.Parse(r0.fFin)).ToList();
                //Traigo la cantidad de Meses
                var arrM = (from n in rangoFechas
                            select n.Month).Distinct().ToList();
                for (int x = 0; x < arrM.Count; x++)
                {
                    var _bymonth = rangoFechas.Where(y => y.Month == arrM[x]).ToList();

                    //Se agrega un registro por cada mes en tabla WORKTIME
                    ctl_WorkTime wt = new ctl_WorkTime();
                    wt.Id_Empleado = r0.idEmpleado;
                    wt.Id_Proyecto = r0.proyecto;
                    wt.id_Cliente = r0.cliente;
                    //separo horas completas y minutos
                    var arrH = span.ToString().Split(':');
                    var horas = int.Parse(arrH[0]);
                    var frHora = arrH[1];
                    //cantidad de minutos entre 60 min(1 hora) por los dias
                    var op = (int.Parse(frHora) / 60) * _bymonth.Count;
                    //
                    wt.Dias_Laborados = _bymonth.Count;
                    wt.Horas_Laboradas = (horas * _bymonth.Count) + (op);
                    wt.Asignacion_SAP = "X";
                    //
                    wt.Fecha_Carga = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    wt.estatus = true;
                    db.ctl_WorkTime.Add(wt);
                    db.SaveChanges();

                    //Se agrega un registro por cada mes en tabla SAP
                    csap = new ctl_SAP();
                    csap.Id_Empleado = r0.idEmpleado;
                    csap.Id_SAP = r0.idsap;
                    csap.Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(arrM[x]);
                    csap.Servicio = r0.servicio;
                    csap.Ubicacion = r0.ubicacion;
                    csap.Desc_CRM = r0.crm;
                    csap.Id_Cliente = r0.cliente;
                    csap.CW = r0.cw;
                    csap.Fecha_Asignacion = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    csap.Responsable_SAP = r0.responsable;
                    csap.Cantidad_Dias = _bymonth.Count;
                    csap.Service_Order = r0.sOrder;
                    csap.idWT = wt.id_WT;
                    db.ctl_SAP.Add(csap);
                    db.SaveChanges();
                    v++;
                }
                JsonResult cc = Json(v, JsonRequestBehavior.AllowGet);
                return cc;
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        [HttpPost]
        public JsonResult terminarCAsignacion(List<Mod_WT> data)
        {
            try
            {
                //Siempre se mandara solo 1 registro, aunque se declare como lista
                Mod_WT r0 = data[0];
                ctl_SAP csap = new ctl_SAP();
                //Calculo de horas
                var hi = DateTime.Parse(r0.hInicio);
                var hf = DateTime.Parse(r0.hFin);
                TimeSpan span = hf - hi;
                //saco todas las fechas entre ese rango de inicio a fin 
                var rangoFechas = DateRange(DateTime.Parse(r0.fInicio), DateTime.Parse(r0.fFin)).ToList();
                //Traigo la cantidad de Meses
                var arrM = (from n in rangoFechas
                            select n.Month).Distinct().ToList();
                List<string> folios = new List<string>();
                for (int x = 0; x < arrM.Count; x++)
                {
                    var _bymonth = rangoFechas.Where(y => y.Month == arrM[x]).ToList();

                    //Se agrega un registro por cada mes en tabla WORKTIME
                    ctl_WorkTime wt = new ctl_WorkTime();
                    wt.Id_Empleado = r0.idEmpleado;
                    wt.Id_Proyecto = r0.proyecto;
                    //separo horas completas y minutos
                    var arrH = span.ToString().Split(':');
                    var horas = int.Parse(arrH[0]);
                    var frHora = arrH[1];
                    //cantidad de minutos entre 60 min(1 hora) por los dias
                    var op = (int.Parse(frHora) / 60) * _bymonth.Count;
                    //
                    //
                    wt.Dias_Laborados = _bymonth.Count;
                    wt.Horas_Laboradas = (horas * _bymonth.Count) + (op);
                    wt.Asignacion_SAP = "X";
                    wt.Id_Cargo = generarFolio(wt.id_WT);
                    folios.Add(wt.Id_Cargo);
                    //
                    wt.Fecha_Carga = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    wt.estatus = true;
                    db.ctl_WorkTime.Add(wt);
                    db.SaveChanges();

                    //Se agrega un registro por cada mes en tabla SAP
                    csap = new ctl_SAP();
                    csap.Id_Empleado = r0.idEmpleado;
                    csap.Id_SAP = r0.idsap;
                    csap.Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(arrM[x]);
                    csap.Servicio = r0.servicio;
                    csap.Ubicacion = r0.ubicacion;
                    csap.Desc_CRM = r0.crm;
                    csap.Id_Cliente = r0.cliente;
                    csap.CW = r0.cw;
                    csap.Fecha_Asignacion = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    csap.Responsable_SAP = r0.responsable;
                    csap.Cantidad_Dias = _bymonth.Count;
                    csap.Service_Order = r0.sOrder;
                    csap.idWT = wt.id_WT;
                    db.ctl_SAP.Add(csap);
                    db.SaveChanges();

                }
                var strF = "";
                for (int i = 0; i < folios.Count; i++)
                {
                    if (i == (folios.Count - 1))
                    {
                        strF += folios[i];
                    }
                    else
                    {
                        strF += folios[i] + "/";
                    }
                }
                JsonResult cc = Json(strF, JsonRequestBehavior.AllowGet);
                return cc;
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        [HttpPost]
        public JsonResult editarCAsignacion(List<Mod_WT> data)
        {
            try
            {
                //Siempre se mandara solo 1 registro, aunque se declare como lista
                Mod_WT r0 = data[0];
                ctl_SAP csap = new ctl_SAP();
                //Calculo de horas
                var hi = DateTime.Parse(r0.hInicio);
                var hf = DateTime.Parse(r0.hFin);
                TimeSpan span = hf - hi;
                //saco todas las fechas entre ese rango de inicio a fin 
                var rangoFechas = DateRange(DateTime.Parse(r0.fInicio), DateTime.Parse(r0.fFin)).ToList();
                //Traigo la cantidad de Meses
                var arrM = (from n in rangoFechas
                            select n.Month).Distinct().ToList();
                for (int x = 0; x < arrM.Count; x++)
                {
                    var _bymonth = rangoFechas.Where(y => y.Month == arrM[x]).ToList();

                    //Se agrega un registro por cada mes en tabla WORKTIME
                    ctl_WorkTime wt = db.ctl_WorkTime.Where(y => y.id_WT == r0.idWT).FirstOrDefault();
                    wt.Id_Proyecto = r0.proyecto;
                    //separo horas completas y minutos
                    var arrH = span.ToString().Split(':');
                    var horas = int.Parse(arrH[0]);
                    var frHora = arrH[1];
                    //cantidad de minutos entre 60 min(1 hora) por los dias
                    var op = (int.Parse(frHora) / 60) * _bymonth.Count;
                    //
                    wt.Dias_Laborados = _bymonth.Count;
                    wt.Horas_Laboradas = (horas * _bymonth.Count) + (op);
                    wt.Asignacion_SAP = "X";
                    //
                    wt.Fecha_Carga = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    wt.estatus = true;
                    db.Entry(wt).State = EntityState.Modified;
                    db.SaveChanges();

                    //Se agrega un registro por cada mes en tabla SAP
                    csap = new ctl_SAP();
                    csap = db.ctl_SAP.Where(y => y.idWT == wt.id_WT).FirstOrDefault();
                    csap.Id_Empleado = r0.idEmpleado;
                    csap.Id_SAP = r0.idsap;
                    csap.Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(arrM[x]);
                    csap.Servicio = r0.servicio;
                    csap.Ubicacion = r0.ubicacion;
                    csap.Desc_CRM = r0.crm;
                    csap.Id_Cliente = r0.cliente;
                    csap.CW = r0.cw;
                    csap.Fecha_Asignacion = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    csap.Responsable_SAP = r0.responsable;
                    csap.Cantidad_Dias = _bymonth.Count;
                    csap.Service_Order = r0.sOrder;
                    csap.idWT = wt.id_WT;
                    db.Entry(csap).State = EntityState.Modified;
                    db.SaveChanges();

                }
                JsonResult cc = Json("E", JsonRequestBehavior.AllowGet);
                return cc;
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        [HttpPost]
        public JsonResult editarTCAsignacion(List<Mod_WT> data)
        {
            try
            {
                //Siempre se mandara solo 1 registro, aunque se declare como lista
                Mod_WT r0 = data[0];
                ctl_SAP csap = new ctl_SAP();
                //Calculo de horas
                var hi = DateTime.Parse(r0.hInicio);
                var hf = DateTime.Parse(r0.hFin);
                TimeSpan span = hf - hi;
                //saco todas las fechas entre ese rango de inicio a fin 
                var rangoFechas = DateRange(DateTime.Parse(r0.fInicio), DateTime.Parse(r0.fFin)).ToList();
                //Traigo la cantidad de Meses
                var arrM = (from n in rangoFechas
                            select n.Month).Distinct().ToList();
                var idc = "";
                for (int x = 0; x < arrM.Count; x++)
                {
                    var _bymonth = rangoFechas.Where(y => y.Month == arrM[x]).ToList();

                    //Se agrega un registro por cada mes en tabla WORKTIME
                    ctl_WorkTime wt = db.ctl_WorkTime.Where(y => y.id_WT == r0.idWT).FirstOrDefault();
                    wt.Id_Proyecto = r0.proyecto;
                    //
                    wt.Dias_Laborados = _bymonth.Count;
                    //separo horas completas y minutos
                    var arrH = span.ToString().Split(':');
                    var horas = int.Parse(arrH[0]);
                    var frHora = arrH[1];
                    //cantidad de minutos entre 60 min(1 hora) por los dias
                    var op = (int.Parse(frHora) / 60) * _bymonth.Count;
                    wt.Horas_Laboradas = (horas * _bymonth.Count) + (op);
                    wt.Asignacion_SAP = "X";
                    //
                    wt.Id_Cargo = generarFolio(wt.id_WT);
                    idc = wt.Id_Cargo;
                    wt.Fecha_Carga = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    wt.estatus = true;
                    db.Entry(wt).State = EntityState.Modified;
                    db.SaveChanges();

                    //Se agrega un registro por cada mes en tabla SAP
                    csap = new ctl_SAP();
                    csap = db.ctl_SAP.Where(y => y.idWT == wt.id_WT).FirstOrDefault();
                    csap.Id_Empleado = r0.idEmpleado;
                    csap.Id_SAP = r0.idsap;
                    csap.Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(arrM[x]);
                    csap.Servicio = r0.servicio;
                    csap.Ubicacion = r0.ubicacion;
                    csap.Desc_CRM = r0.crm;
                    csap.Id_Cliente = r0.cliente;
                    csap.CW = r0.cw;
                    csap.Fecha_Asignacion = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                    csap.Responsable_SAP = r0.responsable;
                    csap.Cantidad_Dias = _bymonth.Count;
                    csap.Service_Order = r0.sOrder;
                    csap.idWT = wt.id_WT;
                    db.Entry(csap).State = EntityState.Modified;
                    db.SaveChanges();

                }
                JsonResult cc = Json(idc, JsonRequestBehavior.AllowGet);
                return cc;
            }
            catch (Exception e)
            {
                JsonResult cc = Json("404", JsonRequestBehavior.AllowGet);
                return cc;
            }
        }

        public string generarFolio(long id)
        {
            string f = "";
            f = DateTime.Now.ToShortDateString() + '-' + id + '-' + DateTime.Now.ToString("HH:mm:ss");
            return f;
        }

        [HttpPost]
        public FileResult Descargar(string mes, string anho)
        {
            var ide = Session["idU"];
            var p = Session["permisos"];
            List<ctl_WorkTime> res = new List<ctl_WorkTime>();
            List<Mod_Reporte> lstRep = new List<Mod_Reporte>();
            if (p.ToString() == "1")
            {
                var wt = db.ctl_WorkTime.Where(x => x.Id_Cargo != "").ToList();
                for (int i = 0; i < wt.Count; i++)
                {
                    var arf = wt[i].Fecha_Carga.Split('-');
                    //corroboramos que es el mismo año
                    if (DateTime.Parse(arf[0]).Year == int.Parse(anho) && DateTime.Parse(arf[1]).Year == int.Parse(anho))
                    {
                        //si el la fecha carga(aa/mm/yyy-aa/mm/yyyy) coinciden los meses
                        if (DateTime.Parse(arf[0]).Month == int.Parse(mes))
                        {
                            if (DateTime.Parse(arf[1]).Month == int.Parse(mes))
                            {
                                Mod_Reporte mwt = new Mod_Reporte();
                                mwt.Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(mes));
                                //buscamos si tiene coincidencia en la tabla sap, OSEA que sea asignado
                                var _id = wt[i].id_WT;
                                var _tsap = db.ctl_SAP.Where(y => y.idWT == _id).FirstOrDefault();
                                if (_tsap != null)//significa que encontro una coincidencia
                                {
                                    mwt.Servicio = _tsap.Servicio;
                                    mwt.Ubicacion = _tsap.Ubicacion;
                                    mwt.Desc_CRM = _tsap.Desc_CRM;
                                    //mwt.id_Cliente = _tsap.Id_Cliente;//*
                                    mwt.ClienteRS = db.ctl_Cliente.Where(x => x.Id_Cliente == _tsap.Id_Cliente).FirstOrDefault().RazonSocial;
                                    mwt.CW = _tsap.CW;
                                    mwt.Fecha_Asignacion = _tsap.Fecha_Asignacion;
                                    mwt.Responsable_SAP = _tsap.Responsable_SAP;
                                    mwt.Dias_Laborados = _tsap.Cantidad_Dias;
                                    //mwt.Id_Empleado = _tsap.Id_Empleado;//*
                                    var emp = db.ctl_Empleado.Where(x => x.Id_empleado == _tsap.Id_Empleado).FirstOrDefault();
                                    mwt.EmpleadoN = emp.Nombre + " " + emp.ApellidoM;
                                    mwt.Service_Order = _tsap.Service_Order;
                                }
                                else
                                {
                                    mwt.Servicio = "";
                                    mwt.Ubicacion = "";
                                    mwt.Desc_CRM = "";
                                    //mwt.id_Cliente = wt[i].id_Cliente.Value;//*
                                    var c = wt[i].id_Cliente.Value;
                                    mwt.ClienteRS = db.ctl_Cliente.Where(x => x.Id_Cliente == c).FirstOrDefault().RazonSocial;
                                    mwt.CW = "";
                                    mwt.Fecha_Asignacion = wt[i].Fecha_Carga;
                                    mwt.Responsable_SAP = "";
                                    mwt.Dias_Laborados = wt[i].Dias_Laborados.Value;
                                    //mwt.Id_Empleado = wt[i].Id_Empleado;//*
                                    var _i = wt[i].Id_Empleado;
                                    var emp = db.ctl_Empleado.Where(x => x.Id_empleado == _i).FirstOrDefault();
                                    mwt.EmpleadoN = emp.Nombre + " " + emp.ApellidoM;
                                    mwt.Service_Order = "";
                                }
                                lstRep.Add(mwt);
                            }
                            else
                            {
                                //quiere decir que un mes coincide con el criterio y el otro no
                                Mod_Reporte mwt = new Mod_Reporte();
                                //saco todas las fechas entre ese rango de inicio a fin 
                                var rangoFechas = DateRange(DateTime.Parse(arf[0]), DateTime.Parse(arf[0])).ToList();
                                //Traigo la cantidad de Meses
                                var arrM = (from n in rangoFechas
                                            select n.Month).Distinct().ToList();
                                for (int x = 0; x < arrM.Count; x++)
                                {
                                    var _bymonth = rangoFechas.Where(y => y.Month == int.Parse(mes) && y.Year == int.Parse(anho)).ToList();
                                    //buscamos si tiene coincidencia en la tabla sap, OSEA que sea asignado
                                    var _id = wt[i].id_WT;
                                    var _tsap = db.ctl_SAP.Where(y => y.idWT == _id).FirstOrDefault();
                                    if (_tsap != null)//significa que encontro una coincidencia
                                    {
                                        mwt.Servicio = _tsap.Servicio;
                                        mwt.Ubicacion = _tsap.Ubicacion;
                                        mwt.Desc_CRM = _tsap.Desc_CRM;
                                        //mwt.id_Cliente = _tsap.Id_Cliente;//*
                                        mwt.ClienteRS = db.ctl_Cliente.Where(a => a.Id_Cliente == _tsap.Id_Cliente).FirstOrDefault().RazonSocial;
                                        mwt.CW = _tsap.CW;
                                        mwt.Fecha_Asignacion = _tsap.Fecha_Asignacion;
                                        mwt.Horas_Laboradas = wt[x].Horas_Laboradas.Value;
                                        mwt.Responsable_SAP = _tsap.Responsable_SAP;
                                        mwt.Dias_Laborados = _tsap.Cantidad_Dias;
                                        //mwt.Id_Empleado = _tsap.Id_Empleado;//*
                                        var emp = db.ctl_Empleado.Where(a => a.Id_empleado == _tsap.Id_Empleado).FirstOrDefault();
                                        mwt.EmpleadoN = emp.Nombre + " " + emp.ApellidoM;
                                        mwt.Service_Order = _tsap.Service_Order;
                                    }
                                    else
                                    {
                                        mwt.Servicio = "";
                                        mwt.Ubicacion = "";
                                        mwt.Desc_CRM = "";
                                        //mwt.id_Cliente = wt[i].id_Cliente.Value;//*
                                        var c = wt[i].id_Cliente.Value;
                                        mwt.ClienteRS = db.ctl_Cliente.Where(a => a.Id_Cliente == c).FirstOrDefault().RazonSocial;
                                        mwt.CW = "";
                                        mwt.Horas_Laboradas = wt[x].Horas_Laboradas.Value;
                                        mwt.Fecha_Asignacion = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                                        mwt.Responsable_SAP = "";
                                        mwt.Dias_Laborados = wt[i].Dias_Laborados.Value;
                                        //mwt.Id_Empleado = wt[i].Id_Empleado;//*
                                        var _i = wt[i].Id_Empleado;
                                        var emp = db.ctl_Empleado.Where(a => a.Id_empleado == _i).FirstOrDefault();
                                        mwt.EmpleadoN = emp.Nombre + " " + emp.ApellidoM;
                                        mwt.Service_Order = "";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (p.ToString() != "1")
            {
                var _idE = int.Parse(ide.ToString());
                var wt = db.ctl_WorkTime.Where(x => x.Id_Cargo != "" && x.Id_Empleado == _idE).ToList();
                for (int i = 0; i < wt.Count; i++)
                {
                    var arf = wt[i].Fecha_Carga.Split('-');
                    //corroboramos que es el mismo año
                    if (DateTime.Parse(arf[0]).Year == int.Parse(anho) && DateTime.Parse(arf[1]).Year == int.Parse(anho))
                    {
                        //si el la fecha carga(aa/mm/yyy-aa/mm/yyyy) coinciden los meses
                        if (DateTime.Parse(arf[0]).Month == int.Parse(mes))
                        {
                            if (DateTime.Parse(arf[1]).Month == int.Parse(mes))
                            {
                                Mod_Reporte mwt = new Mod_Reporte();
                                mwt.Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(mes));
                                //buscamos si tiene coincidencia en la tabla sap, OSEA que sea asignado
                                var _id = wt[i].id_WT;
                                var _tsap = db.ctl_SAP.Where(y => y.idWT == _id).FirstOrDefault();
                                if (_tsap != null)//significa que encontro una coincidencia
                                {
                                    mwt.Servicio = _tsap.Servicio;
                                    mwt.Ubicacion = _tsap.Ubicacion;
                                    mwt.Desc_CRM = _tsap.Desc_CRM;
                                    //mwt.id_Cliente = _tsap.Id_Cliente;//*
                                    mwt.ClienteRS = db.ctl_Cliente.Where(x => x.Id_Cliente == _tsap.Id_Cliente).FirstOrDefault().RazonSocial;
                                    mwt.CW = _tsap.CW;
                                    mwt.Fecha_Asignacion = _tsap.Fecha_Asignacion;
                                    mwt.Responsable_SAP = _tsap.Responsable_SAP;
                                    mwt.Dias_Laborados = _tsap.Cantidad_Dias;
                                    //mwt.Id_Empleado = _tsap.Id_Empleado;//*
                                    var emp = db.ctl_Empleado.Where(x => x.Id_empleado == _tsap.Id_Empleado).FirstOrDefault();
                                    mwt.EmpleadoN = emp.Nombre + " " + emp.ApellidoM;
                                    mwt.Service_Order = _tsap.Service_Order;
                                }
                                else
                                {
                                    mwt.Servicio = "";
                                    mwt.Ubicacion = "";
                                    mwt.Desc_CRM = "";
                                    //mwt.id_Cliente = wt[i].id_Cliente.Value;//*
                                    var c = wt[i].id_Cliente.Value;
                                    mwt.ClienteRS = db.ctl_Cliente.Where(x => x.Id_Cliente == c).FirstOrDefault().RazonSocial;
                                    mwt.CW = "";
                                    mwt.Fecha_Asignacion = wt[i].Fecha_Carga;
                                    mwt.Responsable_SAP = "";
                                    mwt.Dias_Laborados = wt[i].Dias_Laborados.Value;
                                    //mwt.Id_Empleado = wt[i].Id_Empleado;//*
                                    var _i = wt[i].Id_Empleado;
                                    var emp = db.ctl_Empleado.Where(x => x.Id_empleado == _i).FirstOrDefault();
                                    mwt.EmpleadoN = emp.Nombre + " " + emp.ApellidoM;
                                    mwt.Service_Order = "";
                                }
                                lstRep.Add(mwt);
                            }
                            else
                            {
                                //quiere decir que un mes coincide con el criterio y el otro no
                                Mod_Reporte mwt = new Mod_Reporte();
                                //saco todas las fechas entre ese rango de inicio a fin 
                                var rangoFechas = DateRange(DateTime.Parse(arf[0]), DateTime.Parse(arf[0])).ToList();
                                //Traigo la cantidad de Meses
                                var arrM = (from n in rangoFechas
                                            select n.Month).Distinct().ToList();
                                for (int x = 0; x < arrM.Count; x++)
                                {
                                    var _bymonth = rangoFechas.Where(y => y.Month == int.Parse(mes) && y.Year == int.Parse(anho)).ToList();
                                    //buscamos si tiene coincidencia en la tabla sap, OSEA que sea asignado
                                    var _id = wt[i].id_WT;
                                    var _tsap = db.ctl_SAP.Where(y => y.idWT == _id).FirstOrDefault();
                                    if (_tsap != null)//significa que encontro una coincidencia
                                    {
                                        mwt.Servicio = _tsap.Servicio;
                                        mwt.Ubicacion = _tsap.Ubicacion;
                                        mwt.Desc_CRM = _tsap.Desc_CRM;
                                        //mwt.id_Cliente = _tsap.Id_Cliente;//*
                                        mwt.ClienteRS = db.ctl_Cliente.Where(a => a.Id_Cliente == _tsap.Id_Cliente).FirstOrDefault().RazonSocial;
                                        mwt.CW = _tsap.CW;
                                        mwt.Fecha_Asignacion = _tsap.Fecha_Asignacion;
                                        mwt.Horas_Laboradas = wt[x].Horas_Laboradas.Value;
                                        mwt.Responsable_SAP = _tsap.Responsable_SAP;
                                        mwt.Dias_Laborados = _tsap.Cantidad_Dias;
                                        //mwt.Id_Empleado = _tsap.Id_Empleado;//*
                                        var emp = db.ctl_Empleado.Where(a => a.Id_empleado == _tsap.Id_Empleado).FirstOrDefault();
                                        mwt.EmpleadoN = emp.Nombre + " " + emp.ApellidoM;
                                        mwt.Service_Order = _tsap.Service_Order;
                                    }
                                    else
                                    {
                                        mwt.Servicio = "";
                                        mwt.Ubicacion = "";
                                        mwt.Desc_CRM = "";
                                        //mwt.id_Cliente = wt[i].id_Cliente.Value;//*
                                        var c = wt[i].id_Cliente.Value;
                                        mwt.ClienteRS = db.ctl_Cliente.Where(a => a.Id_Cliente == c).FirstOrDefault().RazonSocial;
                                        mwt.CW = "";
                                        mwt.Horas_Laboradas = wt[x].Horas_Laboradas.Value;
                                        mwt.Fecha_Asignacion = _bymonth[0].ToShortDateString() + " - " + _bymonth[_bymonth.Count - 1].ToShortDateString();
                                        mwt.Responsable_SAP = "";
                                        mwt.Dias_Laborados = wt[i].Dias_Laborados.Value;
                                        //mwt.Id_Empleado = wt[i].Id_Empleado;//*
                                        var _i = wt[i].Id_Empleado;
                                        var emp = db.ctl_Empleado.Where(a => a.Id_empleado == _i).FirstOrDefault();
                                        mwt.EmpleadoN = emp.Nombre + " " + emp.ApellidoM;
                                        mwt.Service_Order = "";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            generarExcelHome(lstRep, Server.MapPath("~/pdfTemp/"));
            return File(Server.MapPath("~/pdfTemp/ReporteRXR" + DateTime.Now.ToShortDateString() + ".xlsx"), "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RXR-ReporteDIAS-PROY-SAP-" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(mes)) + " " + anho + ".xlsx");
        }

        public void generarExcelHome(List<Mod_Reporte> lst, string ruta)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            try
            {
                //Creamos el encabezado
                worksheet.Cell("B1").Value = new[]
            {
                  new {
                      BANNER = "Month"
                      },
                    };
                worksheet.Cell("C1").Value = new[]
            {
                  new {
                      BANNER = "Service"
                      },
                    };
                worksheet.Cell("D1").Value = new[]
           {
                  new {
                      BANNER = "Location"
                      },
                    };
                worksheet.Cell("E1").Value = new[]
            {
                  new {
                      BANNER = "Service Description"
                      },
                    };
                worksheet.Cell("F1").Value = new[]
            {
                  new {
                      BANNER = "Customer"
                      },
                    };
                worksheet.Cell("G1").Value = new[]
            {
                  new {
                      BANNER = "CW"
                      },
                    };
                worksheet.Cell("H1").Value = new[]
           {
                  new {
                      BANNER = "Dates"
                      },
                    };
                worksheet.Cell("I1").Value = new[]
           {
                  new {
                      BANNER = "SAP Responsible"
                      },
                    };
                worksheet.Cell("J1").Value = new[]
          {
                  new {
                      BANNER = "Days Qty"
                      },
                    };
                worksheet.Cell("K1").Value = new[]
          {
                  new {
                      BANNER = "Consultant"
                      },
                    };
                worksheet.Cell("L1").Value = new[]
          {
                  new {
                      BANNER = "Internal Order"
                      },
                    };
                worksheet.Cell("M1").Value = new[]
          {
                  new {
                      BANNER = "Service Order"
                      },
                    };
                for (int i = 2; i <= (lst.Count + 1); i++)
                {
                    worksheet.Cell("B" + i).Value = new[]
                {
                  new {
                      BANNER       = lst[i-2].Mes
                      },
                    };
                    worksheet.Cell("C" + i).Value = new[]
                 {
                    new {
                        BANNER       = lst[i-2].Servicio
                        },
                      };
                    worksheet.Cell("D" + i).Value = new[]
                 {
                    new {
                        BANNER       =lst[i-2].Ubicacion
                        },
                      };
                    worksheet.Cell("E" + i).Value = new[]
             {
                  new {
                      BANNER       = lst[i-2].Desc_CRM
                      },
                    };
                    worksheet.Cell("F" + i).Value = new[]
                 {
                    new {
                        BANNER       =lst[i-2].ClienteRS
                        },
                      };
                    worksheet.Cell("G" + i).Value = new[]
                 {
                    new {
                        BANNER       = lst[i-2].CW
                        },
                      };
                    worksheet.Cell("H" + i).Value = new[]
            {
                  new {
                      BANNER       = lst[i-2].Fecha_Asignacion
                      },
                    };
                    worksheet.Cell("I" + i).Value = new[]
                 {
                    new {
                        BANNER       = lst[i-2].EmpleadoN
                        },
                      };
                    worksheet.Cell("J" + i).Value = new[]
                 {
                    new {
                        BANNER       =lst[i-2].Cantidad_Dias
                        },
                      };
                    worksheet.Cell("K" + i).Value = new[]
             {
                  new {
                      BANNER       = lst[i-2].Responsable_SAP
                      },
                    };
                    worksheet.Cell("L" + i).Value = new[]
                 {
                    new {
                        BANNER       =""
                        },
                      };
                    worksheet.Cell("M" + i).Value = new[]
                 {
                    new {
                        BANNER       = lst[i-2].Service_Order
                        },
                      };
                }
                var rt = ruta + @"\ReporteRXR" + DateTime.Now.ToShortDateString() + ".xlsx";
                workbook.SaveAs(rt);
            }
            catch (Exception e)
            {
                var ex = e.ToString();
            }

        }

        [HttpPost]//lej 18.02.2019
        public JsonResult getEvents()
        {
            var res = db.ctl_Dia.ToList();
            var res2 = new List<Mod_Dia>();
            for (int i = 0; i < res.Count; i++)
            {
                Mod_Dia md = new Mod_Dia();
                md.id_Dia = res[i].id_Dia;
                md.idEmpleado = res[i].idEmpleado;
                md.cargaxDiaI = res[i].cargaxDiaI.ToShortDateString();
                md.horaI = res[i].horaI;
                md.totHoras = res[i].totHoras;
                md.Explicacion = res[i].Explicacion;
                md.cargaxDiaF = res[i].cargaxDiaF.ToShortDateString();
                md.horaF = res[i].horaF;
                md.titulo = res[i].titulo;
                md.sorder = res[i].serviceO;
                res2.Add(md);
            }
            JsonResult cc = Json(res2, JsonRequestBehavior.AllowGet);
            return cc;
        }

        // GET: WorkTime/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_WorkTime ctl_WorkTime = db.ctl_WorkTime.Where(x => x.id_WT == id).FirstOrDefault();
            //significa que tiene contenido en la tabla de SAP
            if (ctl_WorkTime.Asignacion_SAP == "X")
            {
                ctl_SAP sap = db.ctl_SAP.Where(x => x.idWT == ctl_WorkTime.id_WT).FirstOrDefault();
                ViewBag.OSap = sap;
                ViewBag.lstClientes = new SelectList(db.ctl_Cliente.Where(x => x.Estatus == true).ToList(), "Id_Cliente", "RazonSocial", sap.Id_Cliente);
            }
            ViewBag.tasig = ctl_WorkTime.Asignacion_SAP;
            var fechas = ctl_WorkTime.Fecha_Carga.Split('-');
            ViewBag.finicio = fechas[0];
            ViewBag.ffin = fechas[1];
            ViewBag.asig = ctl_WorkTime.Asignacion_SAP;

            //
            List<string> lstP = new List<string>();
            lstP.Add("Elegir Opción");
            lstP.Add("No Asignado");
            lstP.Add("Vacaciones");
            lstP.Add("Incapacidad");
            ViewBag.proyecto = new SelectList(lstP, lstP[1]);
            var _p = db.ctl_Proyecto.Where(x => x.Estatus == true).ToList();
            ViewBag.lstProyectos = new SelectList(_p, "Id_Proyecto", "Nombre_Proyecto", _p.Where(x => x.Id_Proyecto == ctl_WorkTime.Id_Proyecto).FirstOrDefault());



            if (ctl_WorkTime == null)
            {
                return HttpNotFound();
            }
            return View(ctl_WorkTime);
        }

        // POST: WorkTime/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Empleado,Id_Proyecto,Dias_Laborados,Horas_Laboradas,Asignacion_SAP,Id_Cargo,Fecha_Carga,estatus")] ctl_WorkTime ctl_WorkTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ctl_WorkTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ctl_WorkTime);
        }

        // GET: WorkTime/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctl_WorkTime ctl_WorkTime = db.ctl_WorkTime.Find(id);
            if (ctl_WorkTime == null)
            {
                return HttpNotFound();
            }
            return View(ctl_WorkTime);
        }

        // POST: WorkTime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ctl_WorkTime ctl_WorkTime = db.ctl_WorkTime.Find(id);
            db.ctl_WorkTime.Remove(ctl_WorkTime);
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
