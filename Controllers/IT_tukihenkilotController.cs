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
            var iT_tukihenkilot = db.IT_tukihenkilot.Include(i => i.Kirjautuminen);
            return View(iT_tukihenkilot.ToList());
        }

        // GET: IT_tukihenkilot/Details/5
        public ActionResult Details(int? id)
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
            return View(iT_tukihenkilot);
        }

        // GET: IT_tukihenkilot/Create
        public ActionResult Create()
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
            return View();
        }

        public ActionResult _CreateModal()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Etunimi,Sukunimi,Puhelinnro,Sahkoposti,Salasana,Taso")] IT_tukihenkilot iT_tukihenkilot)
        {
            if (ModelState.IsValid)
            {
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
            }

            // ModelState ei ole validi, palaa näkymään
            return View(iT_tukihenkilot);
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
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Sahkoposti", iT_tukihenkilot.Sahkoposti);
            return View(iT_tukihenkilot);
        }

        // POST: IT_tukihenkilot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "itHenkiloID,Etunimi,Sukunimi,Puhelinnro,Sahkoposti")] IT_tukihenkilot iT_tukihenkilot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iT_tukihenkilot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Sahkoposti", iT_tukihenkilot.Sahkoposti);
            return View(iT_tukihenkilot);
        }

        // GET: IT_tukihenkilot/Delete/5
        public ActionResult Delete(int? id)
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
            return View(iT_tukihenkilot);
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
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteModalConfirmed(int id)
        {
            IT_tukihenkilot iT_tukihenkilot = db.IT_tukihenkilot.Find(id);

            // Poista liittyvät tiedot Kirjautuminen-taulusta
            Kirjautuminen kirjautuminen = db.Kirjautuminen.FirstOrDefault(k => k.Sahkoposti == iT_tukihenkilot.Kirjautuminen.Sahkoposti);
            db.Kirjautuminen.Remove(kirjautuminen);

            db.IT_tukihenkilot.Remove(iT_tukihenkilot);
            db.SaveChanges();
            return RedirectToAction("Index");
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
