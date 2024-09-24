using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TikettiDB.Models;
using System.Globalization;
using System.Net.Mail;
using System.Text;


namespace TikettiDB.Controllers
{
    public class TikettitiedotController : Controller
    {
        private TikettiDBEntities db = new TikettiDBEntities();

        // GET: Tikettitiedot
        public ActionResult Index(int? searchID)
        {
            //Rajaus, kuka pääsee millekin sivulle
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1 || userLevel == 3)
            {
                // Käyttäjällä on oikeus kaikkiin sivuihin
            }
            else if (userLevel == 2)
            {
                // Käyttäjällä on taso 2, estä pääsy sivulle
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }

            // Haetaan kirjautuneen käyttäjän säpo että indexissä näkyisi vain asiakkaan säpon jättämät tiketit
            string sahkoposti = Session["Sahkoposti"].ToString();

            // Suodatetaan tiketit kirjautuneen käyttäjän sähköpostiosoitteen perusteella
            var tikettitiedot = db.Tikettitiedot.Include(t => t.Asiakastiedot)
                                    .Where(t => t.Asiakastiedot.Sahkoposti == sahkoposti);


            // Suorita haku, jos searchID on suurempi kuin 0
            if (searchID.HasValue && searchID.Value > 0)
            {
                tikettitiedot = tikettitiedot.Where(t => t.TikettiID == searchID.Value);
            }

            // Tallenna hakuarvo näkymään
            ViewBag.currentFilter1 = searchID;

            return View(tikettitiedot.ToList());
        }

        public ActionResult Tiketti()
        {
            var tikettitiedot = db.Tikettitiedot
                            .Where(t => t.Status == "Uusi")
                            .Include(t => t.Asiakastiedot)
                            .Include(t => t.IT_tukihenkilot)
                            .Include(t => t.LaitteenTyyppi)
                            .Include(t => t.YhteydenTyyppi)
                            .ToList();

            return View(tikettitiedot.ToList());
        }
        public ActionResult Tiketti1(string hakutoiminto)
        {
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1 || userLevel == 2)
            {
                // Käyttäjällä on oikeus kaikkiin sivuihin
            }
            else if (userLevel == 3)
            {
                // Käyttäjällä on taso 3, estää pääsyn sivuille
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }

            var tikettitiedot = db.Tikettitiedot
                .Where(t => t.Status == "Uusi")
                .Include(t => t.Asiakastiedot)
                .Include(t => t.IT_tukihenkilot)
                .Include(t => t.LaitteenTyyppi)
                .Include(t => t.YhteydenTyyppi);


            //haku TikettiID:n tai asiakkaan nimen perusteella, jos "hakutoiminto" ei ole tyhjä
            if (!string.IsNullOrEmpty(hakutoiminto))
            {
                //hakukysely numeroksi TikettiID:tä varten
                int tikettiID;
                bool isNumeric = int.TryParse(hakutoiminto, out tikettiID);

                if (isNumeric)
                {
                    //Hae TikettiID:n perusteella osittain (muunna string että voi hakea vain osalla)
                    tikettitiedot = tikettitiedot.Where(t => t.TikettiID.ToString().Contains(hakutoiminto));
                }
                else
                {
                    // Hae asiakkaan etu- tai sukunimen perusteella
                    tikettitiedot = tikettitiedot.Where(t => t.Asiakastiedot.Etunimi.Contains(hakutoiminto)
                                                        || t.Asiakastiedot.Sukunimi.Contains(hakutoiminto));
                }
            }

            // tallenna näkymään
            ViewBag.currentFilter1 = hakutoiminto;

            return View(tikettitiedot.ToList());

        }

        public ActionResult Tiketti2(string hakutoiminto)
        {
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1 || userLevel == 2)
            {
                // Käyttäjällä on oikeus kaikkiin sivuihin
            }
            else if (userLevel == 3)
            {
                // Käyttäjällä on taso 3, estää pääsyn sivuille
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }
            var tikettitiedot = db.Tikettitiedot
                .Where(t => t.Status == "Kesken")
                .Include(t => t.Asiakastiedot)
                .Include(t => t.IT_tukihenkilot)
                .Include(t => t.LaitteenTyyppi)
                .Include(t => t.YhteydenTyyppi);

            //haku TikettiID:n tai asiakkaan nimen perusteella, jos "hakutoiminto" ei ole tyhjä
            if (!string.IsNullOrEmpty(hakutoiminto))
            {
                //hakukysely numeroksi TikettiID:tä varten
                int tikettiID;
                bool isNumeric = int.TryParse(hakutoiminto, out tikettiID);

                if (isNumeric)
                {
                    // Hae TikettiID:n perusteella osittain (muunna merkkijonoksi)
                    tikettitiedot = tikettitiedot.Where(t => t.TikettiID.ToString().Contains(hakutoiminto));
                }
                else
                {
                    // Hae asiakkaan etu- tai sukunimen perusteella
                    tikettitiedot = tikettitiedot.Where(t => t.Asiakastiedot.Etunimi.Contains(hakutoiminto)
                                                        || t.Asiakastiedot.Sukunimi.Contains(hakutoiminto));
                }
            }

            // tallenna näkymään
            ViewBag.currentFilter1 = hakutoiminto;

            return View(tikettitiedot.ToList());

        }

        public ActionResult Tiketti3(string hakutoiminto)
        {
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1 || userLevel == 2)
            {
                // Käyttäjällä on oikeus kaikkiin sivuihin
            }
            else if (userLevel == 3)
            {
                // Käyttäjällä on taso 3, estää pääsyn sivuille
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }
            var tikettitiedot = db.Tikettitiedot
                .Where(t => t.Status == "Valmis")
                .Include(t => t.Asiakastiedot)
                .Include(t => t.IT_tukihenkilot)
                .Include(t => t.LaitteenTyyppi)
                .Include(t => t.YhteydenTyyppi);

            //haku TikettiID:n tai asiakkaan nimen perusteella, jos "hakutoiminto" ei ole tyhjä
            if (!string.IsNullOrEmpty(hakutoiminto))
            {
                //hakukysely numeroksi TikettiID:tä varten
                int tikettiID;
                bool isNumeric = int.TryParse(hakutoiminto, out tikettiID);

                if (isNumeric)
                {
                    // Hae TikettiID:n perusteella osittain (muunna merkkijonoksi)
                    tikettitiedot = tikettitiedot.Where(t => t.TikettiID.ToString().Contains(hakutoiminto));
                }
                else
                {
                    // Hae asiakkaan etu- tai sukunimen perusteella
                    tikettitiedot = tikettitiedot.Where(t => t.Asiakastiedot.Etunimi.Contains(hakutoiminto)
                                                        || t.Asiakastiedot.Sukunimi.Contains(hakutoiminto));
                }
            }

            // tallenna näkymään
            ViewBag.currentFilter1 = hakutoiminto;

            return View(tikettitiedot.ToList());

        }

        // Laiteongelman create
        public ActionResult Create()
        {
            //Rajaus, kuka pääsee millekin sivulle
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1 || userLevel == 3)
            {
                // Käyttäjällä on oikeus kaikkiin sivuihin
            }
            else if (userLevel == 2)
            {
                // Käyttäjällä on taso 2, estä pääsy sivulle
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }
            return View();
        }

        // POST: Tiketti/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Etunimi,Sukunimi,Sahkoposti,Osoite,Postinro,Laitteen_nimi,Ongelman_kuvaus")] Tikettitiedot tikettitiedot)
        {

            if (ModelState.IsValid)
            {
                tikettitiedot.Status = "Uusi";
                // Etsi Asiakastiedot-taulusta
                Asiakastiedot asiakastiedot = db.Asiakastiedot.SingleOrDefault(a => a.Sahkoposti == tikettitiedot.Sahkoposti);
                if (asiakastiedot == null)
                {
                    // Jos asiakasta ei löydy, voit antaa virheviestin
                    ModelState.AddModelError("", "Anna oikea sähköpostiosoite");
                    return View(tikettitiedot);
                }
                // Etsi Sijainti-taulusta
                Sijainti sijainti = db.Sijainti.SingleOrDefault(s => s.SijaintiID == asiakastiedot.SijaintiID);
                //LaitteenTyyppi
                LaitteenTyyppi uusiLaitteenTyyppi = db.LaitteenTyyppi.SingleOrDefault(l => l.LaitenumeroID == tikettitiedot.LaitenumeroID);

                if (uusiLaitteenTyyppi == null)
                {
                    if (tikettitiedot.Laitteen_nimi == null)
                    {
                        ModelState.AddModelError("", "Laitteen kuvaus ei voi olla tyhjä.");
                        return View(tikettitiedot);
                    }

                    Sijainti sijaintiLaitteelle = db.Sijainti.SingleOrDefault(s => s.SijaintiID == asiakastiedot.SijaintiID);

                    if (sijaintiLaitteelle == null)
                    {
                        // Jos sijaintia ei löydy, virheviesti
                        ModelState.AddModelError("", "Sijaintia ei löytynyt annetuilla tiedoilla.");

                        return View(tikettitiedot);
                    }

                    uusiLaitteenTyyppi = new LaitteenTyyppi
                    {
                        Laitteen_nimi = tikettitiedot.Laitteen_nimi,
                        SijaintiID = sijaintiLaitteelle.SijaintiID
                    };

                    db.LaitteenTyyppi.Add(uusiLaitteenTyyppi);
                    db.SaveChanges(); // Tallenna muutokset LaitteenTyyppi-tauluun
                }
                if (tikettitiedot.Ongelman_kuvaus == null)
                {
                    ModelState.AddModelError("", "Ongelman kuvaus ei voi olla tyhjä.");
                    return View(tikettitiedot);
                }

                // Luo uusi tieto Asiakastiedot-tauluun
                Tikettitiedot tikettitieto = new Tikettitiedot
                {
                    Etunimi = asiakastiedot.Etunimi,
                    Sukunimi = asiakastiedot.Sukunimi,
                    Puhelinnro = asiakastiedot.Puhelinnro,
                    Osoite = sijainti.Osoite,
                    Postinro = sijainti.Postinro,
                    Sahkoposti = tikettitiedot.Sahkoposti,
                    Laitteen_nimi = uusiLaitteenTyyppi.Laitteen_nimi,
                    Ongelman_kuvaus = tikettitiedot.Ongelman_kuvaus,
                    AsiakasID = asiakastiedot.AsiakasID,
                    YhteysID = 10001, //YhteysID haluamalle arvolle
                    itHenkiloID = 2000,
                    PVM = DateTime.Today,
                    LaitenumeroID = uusiLaitteenTyyppi.LaitenumeroID,
                    Status = tikettitiedot.Status

                };

                // Lisää Tikettitiedot tietokantaan
                db.Tikettitiedot.Add(tikettitieto);

                // Tallenna muutokset
                db.SaveChanges();

                // Sähköpostin lähetys
                TikettiLuotu(tikettitieto.Sahkoposti, tikettitieto.TikettiID);

                return RedirectToAction("Kuittaus", new { tikettiID = tikettitieto.TikettiID });
            }

            return View(tikettitiedot);

        }

        // Yhteysongelman create
        public ActionResult Create2()
        {
            //Rajaus, kuka pääsee millekin sivulle
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1 || userLevel == 3)
            {
                // Käyttäjällä on oikeus kaikkiin sivuihin
            }
            else if (userLevel == 2)
            {
                // Käyttäjällä on taso 2, estä pääsy sivulle
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }
            ViewBag.Yhteyden_tyyppi = new SelectList(db.YhteydenTyyppi, "Yhteyden_tyyppi", "Yhteyden_tyyppi");
            return View();
        }

        // POST: Tiketti/Create2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "Etunimi,Sukunimi,Sahkoposti,Osoite,Postinro,Laitteen_nimi,Ongelman_kuvaus,Yhteyden_tyyppi")] Tikettitiedot tikettitiedot)
        {
            if (ModelState.IsValid)
            {
                tikettitiedot.Status = "Uusi";
                // Etsi Asiakastiedot-taulusta
                Asiakastiedot asiakastiedot = db.Asiakastiedot.SingleOrDefault(a => a.Sahkoposti == tikettitiedot.Sahkoposti);

                if (asiakastiedot == null)
                {
                    // Jos asiakasta ei löydy, antaa virheviestin
                    ModelState.AddModelError("", "Anna oikea sähköpostiosoite");
                    ViewBag.Yhteyden_tyyppi = new SelectList(db.YhteydenTyyppi, "Yhteyden_tyyppi", "Yhteyden_tyyppi", tikettitiedot.Yhteyden_tyyppi);
                    return View(tikettitiedot);
                }

                // Etsi Sijainti-taulusta
                Sijainti sijainti = db.Sijainti.SingleOrDefault(s => s.SijaintiID == asiakastiedot.SijaintiID);
                // Haetaan LaitteenTyyppi tietokannasta LaitenumeroID:n perusteella
                LaitteenTyyppi uusiLaitteenTyyppi = db.LaitteenTyyppi.SingleOrDefault(l => l.LaitenumeroID == tikettitiedot.LaitenumeroID);

                // jos ei löydy laitteentyyppiä
                if (uusiLaitteenTyyppi == null)
                {
                    // Jos Laitteen_nimi on tyhjä, anna virheviesti
                    if (tikettitiedot.Laitteen_nimi == null)
                    {
                        ModelState.AddModelError("", "Laitteen kuvaus ei voi olla tyhjä.");
                        ViewBag.Yhteyden_tyyppi = new SelectList(db.YhteydenTyyppi, "Yhteyden_tyyppi", "Yhteyden_tyyppi", tikettitiedot.Yhteyden_tyyppi);
                        return View(tikettitiedot);
                    }
                    // Jos laitetyyppiä ei löydy, luo uusi
                    Sijainti sijaintiLaitteelle = db.Sijainti.SingleOrDefault(s => s.SijaintiID == asiakastiedot.SijaintiID);

                    if (sijaintiLaitteelle == null)
                    {
                        // Jos sijaintia ei löydy, virheviesti
                        ModelState.AddModelError("", "Sijaintia ei löytynyt annetuilla tiedoilla.");
                        ViewBag.Yhteyden_tyyppi = new SelectList(db.YhteydenTyyppi, "Yhteyden_tyyppi", "Yhteyden_tyyppi", tikettitiedot.Yhteyden_tyyppi);
                        return View(tikettitiedot);
                    }
                    //luodaan uusi laitteentyyppi annetuilla tiedoilla
                    uusiLaitteenTyyppi = new LaitteenTyyppi
                    {
                        Laitteen_nimi = tikettitiedot.Laitteen_nimi,
                        SijaintiID = sijaintiLaitteelle.SijaintiID
                    };
                    //lisätään ja tallennetaan
                    db.LaitteenTyyppi.Add(uusiLaitteenTyyppi);
                    db.SaveChanges(); // Tallenna muutokset LaitteenTyyppi-tauluun
                }

                // Hae oikea YhteysID valitun pudotusvalikon perusteella
                tikettitiedot.YhteysID = db.YhteydenTyyppi
                    .Where(y => y.Yhteyden_tyyppi == tikettitiedot.Yhteyden_tyyppi)
                    .Select(y => y.YhteysID)
                    .FirstOrDefault();

                // Tarkista, ettei Ongelman_kuvaus ole tyhjä
                if (tikettitiedot.Ongelman_kuvaus == null)
                {
                    ModelState.AddModelError("", "Ongelman kuvaus ei voi olla tyhjä.");
                    ViewBag.Yhteyden_tyyppi = new SelectList(db.YhteydenTyyppi, "Yhteyden_tyyppi", "Yhteyden_tyyppi", tikettitiedot.Yhteyden_tyyppi);
                    return View(tikettitiedot);
                }


                // Luo uusi tieto
                Tikettitiedot tikettitieto = new Tikettitiedot
                {
                    Etunimi = asiakastiedot.Etunimi,
                    Sukunimi = asiakastiedot.Sukunimi,
                    Puhelinnro = asiakastiedot.Puhelinnro,
                    Osoite = sijainti.Osoite,
                    Postinro = sijainti.Postinro,
                    Sahkoposti = tikettitiedot.Sahkoposti,
                    Laitteen_nimi = uusiLaitteenTyyppi.Laitteen_nimi,
                    Ongelman_kuvaus = tikettitiedot.Ongelman_kuvaus,
                    AsiakasID = asiakastiedot.AsiakasID,
                    YhteysID = tikettitiedot.YhteysID,
                    itHenkiloID = 2000,
                    PVM = DateTime.UtcNow,
                    LaitenumeroID = uusiLaitteenTyyppi.LaitenumeroID,
                    Status = tikettitiedot.Status
                };

                // Lisää Tikettitiedot tietokantaan
                db.Tikettitiedot.Add(tikettitieto);

                // Tallenna muutokset
                db.SaveChanges();

                // Sähköpostin lähetys
                TikettiLuotu(tikettitieto.Sahkoposti, tikettitieto.TikettiID);

                return RedirectToAction("Kuittaus", new { tikettiID = tikettitieto.TikettiID });
            }

            ViewBag.Yhteyden_tyyppi = new SelectList(db.YhteydenTyyppi, "Yhteyden_tyyppi", "Yhteyden_tyyppi", tikettitiedot.Yhteyden_tyyppi);
            return View(tikettitiedot);
        }


        // GET: Tikettitiedot/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1 || userLevel == 2)
            {
                // Käyttäjällä on oikeus kaikkiin sivuihin
            }
            else if (userLevel == 3)
            {
                // Käyttäjällä on taso 3, estää pääsyn sivuille
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tikettitiedot tikettitiedot = db.Tikettitiedot.Find(id);
            if (tikettitiedot == null)
            {
                return HttpNotFound();
            }
            // Hae kirjautuneen käyttäjän sähköposti istunnosta
            string userEmail = Session["Sahkoposti"]?.ToString();

            // Hae kirjautuneen käyttäjän itHenkiloID sähköpostin perusteella
            int? loggedInItHenkiloID = db.IT_tukihenkilot
                .Where(h => h.Sahkoposti == userEmail)
                .Select(h => h.itHenkiloID)
                .FirstOrDefault();

            // Aseta tikettitiedot.Sahkoposti istunnosta
            tikettitiedot.Sahkoposti = userEmail;

            // Jos itHenkiloID ei ole sama kuin kirjautuneen käyttäjän ID, päivitä se
            if (tikettitiedot.itHenkiloID != loggedInItHenkiloID && loggedInItHenkiloID.HasValue)
            {
                tikettitiedot.itHenkiloID = loggedInItHenkiloID.Value;
            }

            ViewBag.Status = new SelectList(db.Tikettitiedot, "Status", "Status", tikettitiedot.Status);
            ViewBag.Sahkoposti = tikettitiedot.Sahkoposti;
            return View(tikettitiedot);
        }

        // POST: Tikettitiedot/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TikettiID,RatkaisunKuvaus,Sahkoposti,Status,itHenkiloID")] Tikettitiedot tikettitiedot)
        {
            if (ModelState.IsValid)
            {

                // Etsi olemassa oleva tieto Tikettitiedot tietokannasta
                Tikettitiedot vanhaTiketti = db.Tikettitiedot.Find(tikettitiedot.TikettiID);

                if (vanhaTiketti == null)
                {
                    ModelState.AddModelError("", "Tikettiä ei löytynyt annetulla TikettiID:llä.");
                    return View(tikettitiedot);
                }

                // Päivittää
                vanhaTiketti.RatkaisunKuvaus = tikettitiedot.RatkaisunKuvaus;
                vanhaTiketti.Status = tikettitiedot.Status;
                // Hae kirjautuneen käyttäjän sähköposti istunnosta
                string userEmail = Session["Sahkoposti"]?.ToString();

                // Hae kirjautuneen käyttäjän itHenkiloID sähköpostin perusteella
                int? loggedInItHenkiloID = db.IT_tukihenkilot
                    .Where(h => h.Sahkoposti == userEmail)
                    .Select(h => h.itHenkiloID)
                    .FirstOrDefault();

                // Päivitä vanhaTiketti.Sahkoposti kirjautuneen käyttäjän sähköpostilla
                vanhaTiketti.Sahkoposti = userEmail;

                // Jos itHenkiloID ei ole sama, päivitä itHenkiloID
                if (vanhaTiketti.itHenkiloID != loggedInItHenkiloID && loggedInItHenkiloID.HasValue)
                {
                    vanhaTiketti.itHenkiloID = loggedInItHenkiloID.Value;
                }
                //Tallenna muutettu tietue tietokantaan
                db.SaveChanges();

                // Hae asiakkaan sähköpostiosoite Asiakastiedot-taulusta
                var asiakkaanSahkoposti = db.Asiakastiedot
                    .Where(a => a.AsiakasID == vanhaTiketti.AsiakasID)
                    .Select(a => a.Sahkoposti)
                    .FirstOrDefault();

                // Lähetä sähköposti, jos Status on "Valmis" ja sähköpostiosoite ei ole tyhjä
                if (tikettitiedot.Status == "Valmis" && !string.IsNullOrEmpty(asiakkaanSahkoposti))
                {
                    try
                    {
                        TikettiValmis(asiakkaanSahkoposti, tikettitiedot.TikettiID);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Sähköpostin lähetys epäonnistui: " + ex.Message);
                    }
                }

                // Siirry Tiketti-sivulle, jos Status on päivitetty
                switch (tikettitiedot.Status)
                {
                    case "Uusi":
                        return RedirectToAction("Tiketti1");
                    case "Kesken":
                        return RedirectToAction("Tiketti2");
                    case "Valmis":
                        return RedirectToAction("Tiketti3");
                }
            }

            ViewBag.Status = new SelectList(db.Tikettitiedot, "Status", "Status", tikettitiedot.Status);
            //ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Sahkoposti", tikettitiedot.itHenkiloID);
            ViewBag.itHenkiloID = db.IT_tukihenkilot.FirstOrDefault(k => k.Sahkoposti == tikettitiedot.Sahkoposti);
            return View(tikettitiedot);
        }

        // GET: Tikettitiedot/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1 || userLevel == 2)
            {
                // Käyttäjällä on oikeus kaikkiin sivuihin
            }
            else if (userLevel == 3)
            {
                // Käyttäjällä on taso 3, estää pääsyn sivuille
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tikettitiedot tikettitiedot = db.Tikettitiedot.Find(id);
            if (tikettitiedot == null)
            {
                return HttpNotFound();
            }
            return View(tikettitiedot);
        }

        // POST: Tikettitiedot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tikettitiedot tikettitiedot = db.Tikettitiedot.Find(id);
            db.Tikettitiedot.Remove(tikettitiedot);
            db.SaveChanges();
            return RedirectToAction("Tiketti3");
        }


        public ActionResult Kuittaus(int? TikettiID)
        {
            if (TikettiID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tikettitieto = db.Tikettitiedot.Find(TikettiID);

            if (tikettitieto == null)
            {
                return HttpNotFound();
            }

            return View(tikettitieto);
        }

        public ActionResult OmatTiketit(int? searchID)
        {
            //Rajaus, kuka pääsee millekin sivulle
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1 || userLevel == 2)
            {
                // Käyttäjällä on oikeus
            }
            else if (userLevel == 3)
            {
                // Käyttäjällä on taso 3, estä pääsy sivulle
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }

            // Haetaan kirjautuneen käyttäjän säpo että sivulla näkyisi vain tietyn säpon jättämät tiketit
            string sahkoposti = Session["Sahkoposti"].ToString();

            // Suodatetaan tiketit kirjautuneen käyttäjän sähköpostiosoitteen perusteella, sekä
            //sen perusteella onko tiketin status kesken vai valmis, että pääkäyttäjä näkisi saman
            //näkymän kuin it-tukihenkilö, asiakkaalle näkyy ómat tiketit etusivulla
            //ToList piti siirtää return viewiin, että haku toimi
            var tikettitiedot = db.Tikettitiedot.Include(t => t.IT_tukihenkilot)
                                    .Where(t => t.IT_tukihenkilot.Sahkoposti == sahkoposti)
                                    .Where(t => t.Status == "Kesken" || t.Status == "Valmis");

            // Suorita haku, jos searchID on suurempi kuin 0
            if (searchID.HasValue && searchID.Value > 0)
            {
                tikettitiedot = tikettitiedot.Where(t => t.TikettiID == searchID.Value);
            }

            // Tallenna hakuarvo näkymään
            ViewBag.currentFilter1 = searchID;

            return View(tikettitiedot.ToList());
        }

        // Sähköpostin lähetysfunktio tiketin luomisesta
        private void TikettiLuotu(string email, int tikettiID)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("tikettisovellus@gmail.com"); // Lähettäjän sähköpostiosoite
                mail.To.Add(email);
                mail.Subject = "Uusi tiketti luotu";
                mail.Body = "Uusi tiketti on luotu onnistuneesti. Tikettisi käsittelynumero on: " + tikettiID;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("tikettisovellus@gmail.com", "ennf waps osjw lxue");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        // Sähköpostin lähetysfunktio tiketin valmistumisesta
        private void TikettiValmis(string email, int tikettiID)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("tikettisovellus@gmail.com"); // Lähettäjän sähköpostiosoite
                mail.To.Add(email);
                mail.Subject = "Tikettisi on ratkaistu";
                mail.Body = "Tikettisi on nyt ratkaistu. Näet tiketin ratkaisun kirjautumalla Tikettijärjestelmään. Tikettisi käsittelynumero on " + tikettiID;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("tikettisovellus@gmail.com", "ennf waps osjw lxue");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
