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
            var iT_tukihenkilot = db.IT_tukihenkilot.Include(i => i.Kirjautuminen);
            return View(iT_tukihenkilot.ToList());
        }

        // GET: IT_tukihenkilot/Details/5
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

        // GET: IT_tukihenkilot/Create
        public ActionResult Create()
        {
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana");
            return View();
        }

        // POST: IT_tukihenkilot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "itHenkiloID,Etunimi,Sukunimi,Puhelinnro,Sahkoposti")] IT_tukihenkilot iT_tukihenkilot)
        {
            if (ModelState.IsValid)
            {
                db.IT_tukihenkilot.Add(iT_tukihenkilot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana", iT_tukihenkilot.Sahkoposti);
            return View(iT_tukihenkilot);
        }

        // GET: IT_tukihenkilot/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana", iT_tukihenkilot.Sahkoposti);
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
            ViewBag.Sahkoposti = new SelectList(db.Kirjautuminen, "Sahkoposti", "Salasana", iT_tukihenkilot.Sahkoposti);
            return View(iT_tukihenkilot);
        }

        // GET: IT_tukihenkilot/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: IT_tukihenkilot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IT_tukihenkilot iT_tukihenkilot = db.IT_tukihenkilot.Find(id);
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
