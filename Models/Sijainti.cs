namespace TikettiDB.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Sijainti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sijainti()
        {
            this.Asiakastiedot = new HashSet<Asiakastiedot>();
            this.LaitteenTyyppi = new HashSet<LaitteenTyyppi>();
        }

        public int SijaintiID { get; set; }
        public string Osoite { get; set; }
        public string Postinro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asiakastiedot> Asiakastiedot { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LaitteenTyyppi> LaitteenTyyppi { get; set; }
        public virtual Postinumero Postinumero { get; set; }
    }
}
