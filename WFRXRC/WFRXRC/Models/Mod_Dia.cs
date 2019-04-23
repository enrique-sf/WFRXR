using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFRXRC.Models
{
    public class Mod_Dia
    {
        public long id_Dia { get; set; }
        public long idProyecto { get; set; }
        public long idEmpleado { get; set; }
        public string cargaxDiaI { get; set; }
        public string horaI { get; set; }
        public decimal totHoras { get; set; }
        public string Explicacion { get; set; }
        public string cargaxDiaF { get; set; }
        public string horaF { get; set; }
        //
        public string titulo { get; set; }
        public string sorder { get; set; }
        public string servicio { get; set; }
        public string ubicacion { get; set; }
        public string cliente { get; set; }
        public string cw { get; set; }
        public string resap { get; set; }
    }
}