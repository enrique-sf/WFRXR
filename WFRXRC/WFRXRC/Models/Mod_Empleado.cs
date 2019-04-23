using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFRXRC.Models
{
    public class Mod_Empleado
    {
        public long Id_empleado { get; set; }
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string Calle { get; set; }
        public string NumExt { get; set; }
        public string NumInt { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string Tel_emergencia { get; set; }
        public string Contacto_emergencia { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public Nullable<long> VacacionesN { get; set; }
        public string IMSS { get; set; }
        public string CURP { get; set; }
        public string INE_N { get; set; }
        public string Pasaporte_N { get; set; }
        public long Id_SAP { get; set; }
        public string Email { get; set; }
        public bool Estatus { get; set; }
        //Nomina
        public string Sueldo { get; set; }
        public long Dias_Vacaciones { get; set; }
        public string Bono_Monto { get; set; }
        public long Dias_Asignados { get; set; }
        public long Dias_SAginacion { get; set; }
        //Login
        public string Usuario { get; set; }
        public string Pwd { get; set; }
        public int Permisos { get; set; }
        public Nullable<System.DateTime> Ultimo_Acceso { get; set; }
        public Nullable<bool> Sesion_Activa { get; set; }
        public int PERFIL { get; set; }
    }
}