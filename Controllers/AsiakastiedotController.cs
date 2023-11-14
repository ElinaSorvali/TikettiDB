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
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana");
            ViewBag.SijaintiID = new SelectList(db.Sijainti, "SijaintiID", "Osoite");
            return View();
        }

        // POST: Asiakastiedot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AsiakasID,Etunimi,Sukunimi,Puhelinnro,Sahkoposti,SijaintiID")] Asiakastiedot asiakastiedot)
        {
            if (ModelState.IsValid)
            {
                db.Asiakastiedot.Add(asiakastiedot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana", asiakastiedot.Sahkoposti);
            ViewBag.SijaintiID = new SelectList(db.Sijainti, "SijaintiID", "Osoite", asiakastiedot.SijaintiID);
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
