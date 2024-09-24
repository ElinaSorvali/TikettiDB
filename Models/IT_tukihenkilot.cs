namespace TikettiDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class IT_tukihenkilot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IT_tukihenkilot()
        {
            this.Tikettitiedot = new HashSet<Tikettitiedot>();
        }

        public int itHenkiloID { get; set; }
        public string Etunimi { get; set; }
        public string Sukunimi { get; set; }
        public string Puhelinnro { get; set; }
        public string Sahkoposti { get; set; }

        [DataType(DataType.Password)]
        //Koitin tehdä salasanatarkistuksen näin, muttei toiminut
        //[Required(ErrorMessage = "Password is Required")]
        public string Salasana { get; set; }

        [DataType(DataType.Password)]
        //[Required(ErrorMessage = "Confirm Password is Required")]
        //[Compare("Salasana", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }
        public int Taso { get; set; }

        public virtual Kirjautuminen Kirjautuminen { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tikettitiedot> Tikettitiedot { get; set; }
    }
}