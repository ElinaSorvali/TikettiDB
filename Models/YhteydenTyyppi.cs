namespace TikettiDB.Models
{
    using System;
    using System.Collections.Generic;

    public partial class YhteydenTyyppi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public YhteydenTyyppi()
        {
            this.Tikettitiedot = new HashSet<Tikettitiedot>();
        }

        public int YhteysID { get; set; }
        public string Yhteyden_tyyppi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tikettitiedot> Tikettitiedot { get; set; }
    }
}
