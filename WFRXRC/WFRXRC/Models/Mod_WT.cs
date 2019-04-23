using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFRXRC.Models
{
    public class Mod_WT
    {
        public int id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }//actividad
        public string body { get; set; }//descripcion
        public long proyecto { get; set; }//proyecto
        public long empleado { get; set; }//empleado

        //
        public int idWT { get; set; }
        public int idEmpleado { get; set; }
        public string fInicio { get; set; }
        public string fFin { get; set; }
        public string hInicio { get; set; }
        public string hFin { get; set; }
        public string Asignacion { get; set; }
        //
        public string servicio { get; set; }
        public string ubicacion { get; set; }
        public string cw { get; set; }
        public string crm { get; set; }
        public long cliente { get; set; }
        public string responsable { get; set; }
        public string sOrder { get; set; }
        public long idsap { get; set; }
    }
}