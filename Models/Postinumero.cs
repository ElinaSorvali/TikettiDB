namespace TikettiDB.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Postinumero
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Postinumero()
        {
            this.Sijainti = new HashSet<Sijainti>();
        }

        public string Postinro { get; set; }
        public string Postitmp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sijainti> Sijainti { get; set; }
    }
}
