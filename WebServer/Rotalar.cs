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
    
    public partial class Rotalar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rotalar()
        {
            this.Duraklar = new HashSet<Duraklar>();
        }
    
        public int rotaId { get; set; }
        public int okulAracId { get; set; }
        public int okulId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Duraklar> Duraklar { get; set; }
        public virtual Okullar Okullar { get; set; }
        public virtual OkulServisleri OkulServisleri { get; set; }
    }
}
