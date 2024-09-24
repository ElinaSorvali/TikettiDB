namespace TikettiDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Kirjautuminen
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kirjautuminen()
        {
            this.Asiakastiedot = new HashSet<Asiakastiedot>();
            this.IT_tukihenkilot = new HashSet<IT_tukihenkilot>();
        }

        [Required(ErrorMessage = "Anna sähköpostiosoite!")]
        public string Sahkoposti { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Anna salasana!")]
        public string Salasana { get; set; }

        public int Taso { get; set; }
        public string LoginErrorMessage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asiakastiedot> Asiakastiedot { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IT_tukihenkilot> IT_tukihenkilot { get; set; }
    }
}
