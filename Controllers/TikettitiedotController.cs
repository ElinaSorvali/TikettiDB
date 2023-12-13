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


namespace TikettiDB.Controllers
{
    public class TikettitiedotController : Controller
    {
        private TikettiDBEntities db = new TikettiDBEntities();

        // GET: Tikettitiedot
        public ActionResult Index()
        {
            var tikettitiedot = db.Tikettitiedot.Include(t => t.Asiakastiedot).Include(t => t.IT_tukihenkilot).Include(t => t.LaitteenTyyppi).Include(t => t.YhteydenTyyppi);
            return View(tikettitiedot.ToList());
        }

        public ActionResult Tiketti1()
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

        public ActionResult Tiketti2()
        {
            var tikettitiedot = db.Tikettitiedot
                .Where(t => t.Status == "Kesken")
                .Include(t => t.Asiakastiedot)
                .Include(t => t.IT_tukihenkilot)
                .Include(t => t.LaitteenTyyppi)
                .Include(t => t.YhteydenTyyppi)
                .ToList();

            return View(tikettitiedot.ToList());
        }

        public ActionResult Tiketti3()
        {
            var tikettitiedot = db.Tikettitiedot
                .Where(t => t.Status == "Valmis")
                .Include(t => t.Asiakastiedot)
                .Include(t => t.IT_tukihenkilot)
                .Include(t => t.LaitteenTyyppi)
                .Include(t => t.YhteydenTyyppi)
                .ToList();

            return View(tikettitiedot.ToList());
        }


        // POISTA JOS ET TARVII
        public ActionResult Details(int? id)
        {
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

        // Laiteongelman create
        public ActionResult Create()
        {
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
                LaitteenTyyppi uusiLaitteenTyyppi = db.LaitteenTyyppi.SingleOrDefault(l => l.Laitteen_nimi == tikettitiedot.Laitteen_nimi);

                if (uusiLaitteenTyyppi == null)
                {
                    if (tikettitiedot.Laitteen_nimi == null)
                    {
                        ModelState.AddModelError("", "Laitteen kuvaus ei voi olla tyhjä.");
                        return View(tikettitiedot);
                    }
                    // Jos laitetyyppiä ei löydy, luo uusi
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
                    PVM = DateTime.Today,  // KORJAA TÄMÄ
                    LaitenumeroID = uusiLaitteenTyyppi.LaitenumeroID,
                    Status = tikettitiedot.Status

                };

                // Lisää Tikettitiedot tietokantaan
                db.Tikettitiedot.Add(tikettitieto);

                // Tallenna muutokset
                db.SaveChanges();

                return RedirectToAction("Kuittaus", new { tikettiID = tikettitieto.TikettiID });
            }

            return View(tikettitiedot);

        }

        // Yhteysongelman create
        public ActionResult Create2()
        {
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
                    // Jos asiakasta ei löydy, voit antaa virheviestin
                    ModelState.AddModelError("", "Anna oikea sähköpostiosoite");
                    ViewBag.Yhteyden_tyyppi = new SelectList(db.YhteydenTyyppi, "Yhteyden_tyyppi", "Yhteyden_tyyppi", tikettitiedot.Yhteyden_tyyppi);
                    return View(tikettitiedot);
                }

                // Etsi Sijainti-taulusta
                Sijainti sijainti = db.Sijainti.SingleOrDefault(s => s.SijaintiID == asiakastiedot.SijaintiID);
                //LaitteenTyyppi
                LaitteenTyyppi uusiLaitteenTyyppi = db.LaitteenTyyppi.SingleOrDefault(l => l.Laitteen_nimi == tikettitiedot.Laitteen_nimi);

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

                    //if (sijaintiLaitteelle == null)
                    //{
                    //    // Jos sijaintia ei löydy, virheviesti
                    //    ModelState.AddModelError("", "Sijaintia ei löytynyt annetuilla tiedoilla.");
                    //    ViewBag.Yhteyden_tyyppi = new SelectList(db.YhteydenTyyppi, "Yhteyden_tyyppi", "Yhteyden_tyyppi", tikettitiedot.Yhteyden_tyyppi);
                    //    return View(tikettitiedot);
                    //}

                    uusiLaitteenTyyppi = new LaitteenTyyppi
                    {
                        Laitteen_nimi = tikettitiedot.Laitteen_nimi,
                        SijaintiID = sijaintiLaitteelle.SijaintiID
                    };

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
                    PVM = DateTime.UtcNow,  // Aseta PVM nykyhetkeen
                    LaitenumeroID = uusiLaitteenTyyppi.LaitenumeroID,
                    Status = tikettitiedot.Status
                };

                // Lisää Tikettitiedot tietokantaan
                db.Tikettitiedot.Add(tikettitieto);

                // Tallenna muutokset
                db.SaveChanges();

                return RedirectToAction("Kuittaus", new { tikettiID = tikettitieto.TikettiID });
            }

            ViewBag.Yhteyden_tyyppi = new SelectList(db.YhteydenTyyppi, "Yhteyden_tyyppi", "Yhteyden_tyyppi", tikettitiedot.Yhteyden_tyyppi);
            return View(tikettitiedot);
        }


        // GET: Tikettitiedot/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tikettitiedot tikettitiedot = db.Tikettitiedot.Find(id);
            if (tikettitiedot == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = new SelectList(db.Tikettitiedot, "Status", "Status", tikettitiedot.Status);
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Sahkoposti", tikettitiedot.itHenkiloID);
            return View(tikettitiedot);
        }

        // POST: Tikettitiedot/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TikettiID,RatkaisunKuvaus,Sahkoposti,Status")] Tikettitiedot tikettitiedot)
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

                //Tallenna muutettu tietue tietokantaan
                db.SaveChanges();

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
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Sahkoposti", tikettitiedot.itHenkiloID);

            return View(tikettitiedot);
        }


        // GET: Tikettitiedot/Delete/5
        public ActionResult Delete(int? id)
        {
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
            return RedirectToAction("Tiketti");
        }

        public ActionResult Tiketti()
        {
            return View();
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
