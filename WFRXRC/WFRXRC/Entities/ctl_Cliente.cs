//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WFRXRC.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ctl_Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ctl_Cliente()
        {
            this.ctl_ContactosCliente = new HashSet<ctl_ContactosCliente>();
            this.ctl_Proyecto = new HashSet<ctl_Proyecto>();
            this.ctl_SAP = new HashSet<ctl_SAP>();
        }
    
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ctl_ContactosCliente> ctl_ContactosCliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ctl_Proyecto> ctl_Proyecto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ctl_SAP> ctl_SAP { get; set; }
    }
}
