namespace TikettiDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Tikettitiedot
    {
        public string Etunimi { get; set; }
        public string Sukunimi { get; set; }
        public string Puhelinnro { get; set; }
        public string Sahkoposti { get; set; }
        public string Osoite { get; set; }
        public string Postinro { get; set; }
        public string Postitmp { get; set; }
        public string Laitteen_nimi { get; set; }
        public int TikettiID { get; set; }
        public int YhteysID { get; set; }
        public int? LaitenumeroID { get; set; }
        public int AsiakasID { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PVM { get; set; }
        public string Ongelman_kuvaus { get; set; }
        public int itHenkiloID { get; set; }
        public string Yhteyden_tyyppi { get; set; }
        public string RatkaisunKuvaus { get; set; }
        public string Status { get; set; }

        public virtual Asiakastiedot Asiakastiedot { get; set; }
        public virtual IT_tukihenkilot IT_tukihenkilot { get; set; }
        public virtual LaitteenTyyppi LaitteenTyyppi { get; set; }
        public virtual YhteydenTyyppi YhteydenTyyppi { get; set; }
        public virtual Postinumero Postinumero { get; set; }
        public virtual Sijainti Sijainti { get; set; }
    }
}
