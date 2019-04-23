using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFRXRC.Models
{
    public class Mod_Reporte
    {
        //  WT
        public long id_WT { get; set; }
        public long id_Cliente { get; set; }
        public long Id_Empleado { get; set; }
        public long Id_Proyecto { get; set; }
        public long Dias_Laborados { get; set; }
        public long Horas_Laboradas { get; set; }
        public string Asignacion_SAP { get; set; }
        public string Id_Cargo { get; set; }
        public string Fecha_Carga { get; set; }

        //sap
        public string EmpleadoN { get; set; }
        public string ClienteRS { get; set; }
        public long id_ctlSap { get; set; }
        public long Id_SAP { get; set; }
        public string Mes { get; set; }
        public string Servicio { get; set; }
        public string Ubicacion { get; set; }
        public string Desc_CRM { get; set; }
        public string CW { get; set; }
        public string Fecha_Asignacion { get; set; }
        public string Responsable_SAP { get; set; }
        public long Cantidad_Dias { get; set; }
        public string Service_Order { get; set; }
        public long idWT { get; set; }
    }
}