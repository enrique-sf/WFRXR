using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFRXRC.Models
{
    public class Mod_Proyecto
    {
        public long Id_Proyecto { get; set; }
        public long Id_Cliente { get; set; }
        public long Id_Empleado { get; set; }
        public string Nombre_Proyecto { get; set; }
        public long Dias_Asignados { get; set; }
        public long Horas_Asignadas { get; set; }
        public string Costo_Dia { get; set; }
        public string Costo_Tiempo { get; set; }
        public System.DateTime Fecha_Inicio { get; set; }
        public bool Estatus { get; set; }
    }
}