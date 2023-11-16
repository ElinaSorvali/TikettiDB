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
    public class AsiakastiedotController : Controller
    {
        private TikettiDBEntities db = new TikettiDBEntities();

        // GET: Asiakastiedot
        public ActionResult Index()
        {
            var asiakastiedot = db.Asiakastiedot.Include(a => a.Kirjautuminen).Include(a => a.Sijainti);
            return View(asiakastiedot.ToList());
        }

        // GET: Asiakastiedot/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asiakastiedot asiakastiedot = db.Asiakastiedot.Find(id);
            if (asiakastiedot == null)
            {
                return HttpNotFound();
            }
            return View(asiakastiedot);
        }

        // GET: Asiakastiedot/Create
        public ActionResult Create()
        {
            ViewBag.SijaintiID = new SelectList(db.Sijainti, "SijaintiID", "Osoite", "Postinro");
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana");
            //ViewBag.Osoite = new SelectList(db.Sijainti, "Osoite");
            ViewBag.Postinro = new SelectList(db.Postinumero, "Postinro", "Postinro");

            return View();
        }

        // POST: Asiakastiedot/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Etunimi,Sukunimi,Puhelinnro,Osoite,Postinro,Sahkoposti,Salasana")] Asiakastiedot asiakastiedot)
        {
            if (ModelState.IsValid)
            {
                // Etsi tai luo Postinumero
                Postinumero postinumero = db.Postinumero.SingleOrDefault(p => p.Postinro == asiakastiedot.Postinro);
                if (postinumero == null)
                {
                    postinumero = new Postinumero
                    {
                        Postinro = asiakastiedot.Postinro
                    };
                    db.Postinumero.Add(postinumero);
                }

                // Etsi tai luo Kirjautuminen
                Kirjautuminen kirjautuminen = db.Kirjautuminen.SingleOrDefault(k => k.Sahkoposti == asiakastiedot.Sahkoposti);
                if (kirjautuminen == null)
                {
                    kirjautuminen = new Kirjautuminen
                    {
                        Sahkoposti = asiakastiedot.Sahkoposti,
                        Salasana = asiakastiedot.Salasana,
                        Taso = 3
                    };
                    db.Kirjautuminen.Add(kirjautuminen);
                }

                // Luo uusi Sijainti-tauluun
                Sijainti sijainti = new Sijainti
                {
                    Osoite = asiakastiedot.Osoite,
                    Postinro = postinumero.Postinro,
                };
                db.Sijainti.Add(sijainti);

                // Luo uusi tietue Asiakastiedot-tauluun
                Asiakastiedot asiakastieto = new Asiakastiedot
                {
                    Etunimi = asiakastiedot.Etunimi,
                    Sukunimi = asiakastiedot.Sukunimi,
                    Puhelinnro = asiakastiedot.Puhelinnro,
                    Osoite = sijainti.Osoite,
                    Postinro = sijainti.Postinro,
                    Sahkoposti = kirjautuminen.Sahkoposti,
                    Salasana = kirjautuminen.Salasana,
                    SijaintiID = sijainti.SijaintiID,
                };

                // Lisää Asiakastiedot tietokantaan
                db.Asiakastiedot.Add(asiakastieto);

                // Tallenna muutokset
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Jos ModelState ei ole validi, palaa Create-näkymään
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana", asiakastiedot.Sahkoposti);
            return View(asiakastiedot);
        }


        // GET: Asiakastiedot/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asiakastiedot asiakastiedot = db.Asiakastiedot.Find(id);
            if (asiakastiedot == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana", asiakastiedot.Sahkoposti);
            ViewBag.SijaintiID = new SelectList(db.Sijainti, "SijaintiID", "Osoite", asiakastiedot.SijaintiID);
            return View(asiakastiedot);
        }

        // POST: Asiakastiedot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AsiakasID,Etunimi,Sukunimi,Puhelinnro,Sahkoposti,SijaintiID")] Asiakastiedot asiakastiedot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asiakastiedot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana", asiakastiedot.Sahkoposti);
            ViewBag.SijaintiID = new SelectList(db.Sijainti, "SijaintiID", "Osoite", asiakastiedot.SijaintiID);
            return View(asiakastiedot);
        }

        // GET: Asiakastiedot/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asiakastiedot asiakastiedot = db.Asiakastiedot.Find(id);
            if (asiakastiedot == null)
            {
                return HttpNotFound();
            }
            return View(asiakastiedot);
        }

        // POST: Asiakastiedot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asiakastiedot asiakastiedot = db.Asiakastiedot.Find(id);
            db.Asiakastiedot.Remove(asiakastiedot);
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
