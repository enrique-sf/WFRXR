using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFRXRC.Models
{
    public class Mod_TSAP
    {
        public long id_empleado { get; set; }
        public long Id_SAP { get; set; }
        public DateTime inicio { get; set; }
        public DateTime fin { get; set; }
        public string servicio { get; set; }
        public string ubicacion { get; set; }
        public string desc_crm { get; set; }
        public long cliente { get; set; }
        public string cw { get; set; }       
        public long rsap { get; set; }
        public string sorder { get; set; }
    }
}