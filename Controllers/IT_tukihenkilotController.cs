using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TikettiDB.Models;

namespace TikettiDB.Controllers
{
    public class IT_tukihenkilotController : Controller
    {
        private TikettiDBEntities db = new TikettiDBEntities();

        // GET: IT_tukihenkilot
        public ActionResult Index()
        {
            #region//Rajaus kuka pääsee millekin sivulle
            //if (Session["Sahkoposti"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            //int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            //// Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            //if (userLevel == 1)
            //{
            //    // Käyttäjällä on oikeus kaikkiin sivuihin
            //}
            //else if (userLevel == 2 || userLevel == 3)
            //{
            //    // Käyttäjällä on taso 2 tai 3, estä pääsy haluamillesi sivuille
            //    ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
            //    return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            //}
            #endregion

            var iT_tukihenkilot = db.IT_tukihenkilot.Include(i => i.Kirjautuminen);
            return View(iT_tukihenkilot.ToList());
        }


        // GET: IT_tukihenkilot/Create
        public ActionResult Create()
        {
            #region//Rajaus kuka pääsee millekin sivulle
            //if (Session["Sahkoposti"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            //int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            //// Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            //if (userLevel == 1)
            //{
            //    // Käyttäjällä on oikeus kaikkiin sivuihin
            //}
            //else if (userLevel == 2 || userLevel == 3)
            //{
            //    // Käyttäjällä on taso 2 tai 3, estä pääsy haluamillesi sivuille
            //    ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
            //    return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            //}
            #endregion

            return View();
        }

        public ActionResult _CreateModal()
        {
            return PartialView();
        }

        [HttpPost]
        //Testejä varten poistettu tai poistetaan
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Etunimi,Sukunimi,Puhelinnro,Sahkoposti,Salasana,Taso")] IT_tukihenkilot iT_tukihenkilot)
        {
            if (ModelState.IsValid)
            {
                // Tarkista, että kaikki tarvittavat tiedot ovat annettu
                if (string.IsNullOrEmpty(iT_tukihenkilot.Etunimi) ||
                    string.IsNullOrEmpty(iT_tukihenkilot.Sukunimi) ||
                    string.IsNullOrEmpty(iT_tukihenkilot.Puhelinnro) ||
                    string.IsNullOrEmpty(iT_tukihenkilot.Sahkoposti) ||
                    string.IsNullOrEmpty(iT_tukihenkilot.Salasana) ||
                    iT_tukihenkilot.Taso <= 0)
                {
                    ModelState.AddModelError("", "Kaikki tiedot ovat pakollisia.");
                    return View(iT_tukihenkilot);
                }

                Kirjautuminen kirjautuminen = db.Kirjautuminen.SingleOrDefault(k => k.Sahkoposti == iT_tukihenkilot.Sahkoposti);

                if (kirjautuminen == null)
                {
                    kirjautuminen = new Kirjautuminen
                    {
                        Sahkoposti = iT_tukihenkilot.Sahkoposti,
                        Salasana = iT_tukihenkilot.Salasana,
                        Taso = iT_tukihenkilot.Taso
                    };
                    db.Kirjautuminen.Add(kirjautuminen);
                }

                IT_tukihenkilot ittukihenkilo = new IT_tukihenkilot
                {
                    Etunimi = iT_tukihenkilot.Etunimi,
                    Sukunimi = iT_tukihenkilot.Sukunimi,
                    Puhelinnro = iT_tukihenkilot.Puhelinnro,
                    Sahkoposti = kirjautuminen.Sahkoposti,
                    Salasana = kirjautuminen.Salasana,
                    Taso = kirjautuminen.Taso
                };
                db.IT_tukihenkilot.Add(ittukihenkilo);
                db.SaveChanges();

                return RedirectToAction("Index");

                //Vaihda ylläoleva ja allaoleva päittäin testejä varten
                //Palauta juuri luodun objektin ID JSON-vastauksena
                //return Json(new { itHenkiloID = ittukihenkilo.itHenkiloID }, JsonRequestBehavior.AllowGet);

            }
            // ModelState ei ole validi, palaa näkymään
            return PartialView();
        }

        [HttpPost]
        //Testejä varten poistettu tai poistetaan
        [ValidateAntiForgeryToken]
        public ActionResult _CreateModal([Bind(Include = "Etunimi,Sukunimi,Puhelinnro,Sahkoposti,Salasana,Taso")] IT_tukihenkilot iT_tukihenkilot)
        {
            if (ModelState.IsValid)
            {
                // Tarkista, että kaikki tarvittavat tiedot ovat annettu
                if (string.IsNullOrEmpty(iT_tukihenkilot.Etunimi) ||
                    string.IsNullOrEmpty(iT_tukihenkilot.Sukunimi) ||
                    string.IsNullOrEmpty(iT_tukihenkilot.Puhelinnro) ||
                    string.IsNullOrEmpty(iT_tukihenkilot.Sahkoposti) ||
                    string.IsNullOrEmpty(iT_tukihenkilot.Salasana) ||
                    iT_tukihenkilot.Taso <= 0)
                {
                    ModelState.AddModelError("", "Kaikki tiedot ovat pakollisia.");
                    // Palautetaan osittainen näkymä, jossa virheviesti
                    return PartialView(iT_tukihenkilot);
                }

                Kirjautuminen kirjautuminen = db.Kirjautuminen.SingleOrDefault(k => k.Sahkoposti == iT_tukihenkilot.Sahkoposti);

                if (kirjautuminen == null)
                {
                    kirjautuminen = new Kirjautuminen
                    {
                        Sahkoposti = iT_tukihenkilot.Sahkoposti,
                        Salasana = iT_tukihenkilot.Salasana,
                        Taso = iT_tukihenkilot.Taso
                    };
                    db.Kirjautuminen.Add(kirjautuminen);
                }

                IT_tukihenkilot ittukihenkilo = new IT_tukihenkilot
                {
                    Etunimi = iT_tukihenkilot.Etunimi,
                    Sukunimi = iT_tukihenkilot.Sukunimi,
                    Puhelinnro = iT_tukihenkilot.Puhelinnro,
                    Sahkoposti = kirjautuminen.Sahkoposti,
                    Salasana = kirjautuminen.Salasana,
                    Taso = kirjautuminen.Taso
                };
                db.IT_tukihenkilot.Add(ittukihenkilo);
                db.SaveChanges();

                return RedirectToAction("Index");

                //Vaihda ylläoleva ja allaoleva päittäin testejä varten
                //Palauta juuri luodun objektin ID JSON-vastauksena
                //return Json(new { itHenkiloID = ittukihenkilo.itHenkiloID }, JsonRequestBehavior.AllowGet);

            }
            // ModelState ei ole validi, palaa näkymään
            return PartialView();
        }

        // GET: IT_tukihenkilot/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            // Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            if (userLevel == 1)
            {
                // Käyttäjällä on oikeus kaikkiin sivuihin
            }
            else if (userLevel == 2 || userLevel == 3)
            {
                // Käyttäjällä on taso 2 tai 3, estä pääsy haluamillesi sivuille
                ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
                return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IT_tukihenkilot iT_tukihenkilot = db.IT_tukihenkilot.Find(id);
            if (iT_tukihenkilot == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Taso", "Taso", iT_tukihenkilot.Taso);
            return View(iT_tukihenkilot);
        }

        // POST: IT_tukihenkilot/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "itHenkiloID,Etunimi,Sukunimi,Puhelinnro,Sahkoposti,Salasana,Taso")] IT_tukihenkilot iT_tukihenkilot)
        {
            if (ModelState.IsValid)
            {
                // Päivitä Kirjautuminen-olio
                Kirjautuminen kirjautuminen = db.Kirjautuminen.SingleOrDefault(k => k.Sahkoposti == iT_tukihenkilot.Sahkoposti);

                if (kirjautuminen == null)
                {
                    // Jos ei ole olemassa, lisää uusi Kirjautuminen, salasanaa ei päivitetä
                    db.Kirjautuminen.Add(new Kirjautuminen
                    {
                        Sahkoposti = iT_tukihenkilot.Sahkoposti,
                        //Salasana = iT_tukihenkilot.Salasana,
                        Taso = iT_tukihenkilot.Taso
                    });
                }
                else
                {
                    // Jos on olemassa, päivitä tiedot
                    //kirjautuminen.Salasana = iT_tukihenkilot.Salasana;
                    kirjautuminen.Taso = iT_tukihenkilot.Taso;
                }

                // Päivitä IT_tukihenkilot-olio
                db.Entry(iT_tukihenkilot).State = EntityState.Modified;

                // Tallenna muutokset
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Taso", "Taso", iT_tukihenkilot.Sahkoposti);
            ViewBag.Salasana = db.Kirjautuminen.FirstOrDefault(k => k.Sahkoposti == iT_tukihenkilot.Sahkoposti)?.Salasana;
            return View(iT_tukihenkilot);
        }


        public ActionResult _EditModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IT_tukihenkilot iT_tukihenkilot = db.IT_tukihenkilot.Find(id);
            if (iT_tukihenkilot == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Taso", "Taso", iT_tukihenkilot.Taso);
            Kirjautuminen kirjautuminen = db.Kirjautuminen.FirstOrDefault(k => k.Sahkoposti == iT_tukihenkilot.Sahkoposti);
            ViewBag.Salasana = kirjautuminen?.Salasana;
            return PartialView(iT_tukihenkilot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _EditModal([Bind(Include = "itHenkiloID,Etunimi,Sukunimi,Puhelinnro,Sahkoposti,Salasana,Taso")] IT_tukihenkilot iT_tukihenkilot)
        {
            if (ModelState.IsValid)
            {
                // Päivitä Kirjautuminen-olio
                Kirjautuminen kirjautuminen = db.Kirjautuminen.SingleOrDefault(k => k.Sahkoposti == iT_tukihenkilot.Sahkoposti);

                if (kirjautuminen == null)
                {
                    // Jos ei ole olemassa, lisää uusi Kirjautuminen, salasanaa ei päivitetä
                    db.Kirjautuminen.Add(new Kirjautuminen
                    {
                        Sahkoposti = iT_tukihenkilot.Sahkoposti,
                        //Salasana = iT_tukihenkilot.Salasana,
                        Taso = iT_tukihenkilot.Taso
                    });
                }
                else
                {
                    // Jos on olemassa, päivitä tiedot
                    //kirjautuminen.Salasana = iT_tukihenkilot.Salasana;
                    kirjautuminen.Taso = iT_tukihenkilot.Taso;
                }

                // Päivitä IT_tukihenkilot-olio
                db.Entry(iT_tukihenkilot).State = EntityState.Modified;

                // Tallenna muutokset
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Taso", "Taso", iT_tukihenkilot.Sahkoposti);
            ViewBag.Salasana = db.Kirjautuminen.FirstOrDefault(k => k.Sahkoposti == iT_tukihenkilot.Sahkoposti)?.Salasana;
            return PartialView(iT_tukihenkilot);
        }

        

        // GET: IT_tukihenkilot/Delete/5
        public ActionResult Delete(int? id)
        {
            #region//Rajaus kuka pääsee millekin sivulle
            //if (Session["Sahkoposti"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            //int userLevel = (int)Session["Taso"]; // Ota käyttäjän taso istunnosta

            //// Tarkista käyttäjän taso ja estä pääsy tietyille sivuille
            //if (userLevel == 1)
            //{
            //    // Käyttäjällä on oikeus kaikkiin sivuihin
            //}
            //else if (userLevel == 2 || userLevel == 3)
            //{
            //    // Käyttäjällä on taso 2 tai 3, estä pääsy haluamillesi sivuille
            //    ViewBag.ErrorMessage = "Pääsy tälle sivulle vain pääkäyttäjän toimesta!";
            //    return View("Error"); // Luo virhesivu tai ohjaa käyttäjä virhesivulle
            //}
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //IT_tukihenkilot iT_tukihenkilot = db.IT_tukihenkilot.Find(id);
            //if (iT_tukihenkilot == null)
            //{
            //    return HttpNotFound();
            //}
            #endregion

            return View();
        }

        public ActionResult _DeleteModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IT_tukihenkilot iT_tukihenkilot = db.IT_tukihenkilot.Find(id);
            if (iT_tukihenkilot == null)
            {
                return HttpNotFound();
            }
            return PartialView(iT_tukihenkilot);
        }

        // POST: IT_tukihenkilot/Delete/5
        [HttpPost, ActionName("_DeleteModal")]
        //Testejä varten poistettu tai poistetaan
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteModalConfirmed(int id)
        {
            // Hae poistettava IT-tukihenkilö
            IT_tukihenkilot iT_tukihenkilot = db.IT_tukihenkilot.Find(id);

            if (iT_tukihenkilot != null)
            {
                // Poista kaikki Tikettitiedot, jotka viittaavat tähän itHenkiloID:hen
                var liittyvatTikettitiedot = db.Tikettitiedot.Where(t => t.itHenkiloID == id).ToList();
                db.Tikettitiedot.RemoveRange(liittyvatTikettitiedot);

                // Poista ensin IT_tukihenkilot-rivi
                db.IT_tukihenkilot.Remove(iT_tukihenkilot);
                db.SaveChanges();

                // Poista liittyvät tiedot Kirjautuminen-taulusta vasta tämän jälkeen
                Kirjautuminen kirjautuminen = db.Kirjautuminen.FirstOrDefault(k => k.Sahkoposti == iT_tukihenkilot.Sahkoposti);
                if (kirjautuminen != null)
                {
                    db.Kirjautuminen.Remove(kirjautuminen);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IT_tukihenkilot iT_tukihenkilot = db.IT_tukihenkilot.Find(id);
            if (iT_tukihenkilot == null)
            {
                return HttpNotFound();
            }
            return View(iT_tukihenkilot);
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
