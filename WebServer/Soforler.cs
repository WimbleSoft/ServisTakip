//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServisTakip
{
    using System;
    using System.Collections.Generic;
    
    public partial class Soforler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Soforler()
        {
            this.FirmaSoforleri = new HashSet<FirmaSoforleri>();
            this.ServisSoforleri = new HashSet<ServisSoforleri>();
        }
    
        public int sofId { get; set; }
        public string sofAd { get; set; }
        public string sofSoyad { get; set; }
        public string sofEmail { get; set; }
        public string sofParola { get; set; }
        public string sofGsm { get; set; }
        public string sofAdres { get; set; }
        public int ilce_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FirmaSoforleri> FirmaSoforleri { get; set; }
        public virtual Ilceler Ilceler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServisSoforleri> ServisSoforleri { get; set; }
    }
}
