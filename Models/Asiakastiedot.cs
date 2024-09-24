namespace TikettiDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Asiakastiedot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Asiakastiedot()
        {
            this.Tikettitiedot = new HashSet<Tikettitiedot>();
        }

        public int AsiakasID { get; set; }
        public string Etunimi { get; set; }
        public string Sukunimi { get; set; }
        public string Puhelinnro { get; set; }
        public string Sahkoposti { get; set; }
        public string Osoite { get; set; }
        public string Postinro { get; set; }

        [DataType(DataType.Password)]
        public string Salasana { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public int SijaintiID { get; set; }
        public virtual Kirjautuminen Kirjautuminen { get; set; }
        public virtual Sijainti Sijainti { get; set; }
        public virtual Postinumero Postinumero { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public string ErrorMessage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Tikettitiedot> Tikettitiedot { get; set; }
    }
}
