namespace TikettiDB.Models
{
    using System;
    using System.Collections.Generic;

    public partial class LaitteenTyyppi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LaitteenTyyppi()
        {
            this.Tikettitiedot = new HashSet<Tikettitiedot>();
        }

        public int LaitenumeroID { get; set; }
        public string Laitteen_nimi { get; set; }
        public Nullable<int> SijaintiID { get; set; }

        public virtual Sijainti Sijainti { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tikettitiedot> Tikettitiedot { get; set; }
    }
}
