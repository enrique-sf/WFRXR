using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFRXRC.Models
{
    public class Mod_Cliente
    {
        public long Id_Cliente { get; set; }
        public string RazonSocial { get; set; }
        public string RazonComercial { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string RFC { get; set; }
        public string RFC_Direccion { get; set; }
        public string RFC_Correo { get; set; }
        public string Credito { get; set; }
        public string TiempoCredito { get; set; }
        public bool Estatus { get; set; }
        public string contacto { get; set; }
        public List<string> Contactos { get; set; }
    }
}