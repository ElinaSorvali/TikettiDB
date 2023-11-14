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
    public class TikettitiedotController : Controller
    {
        private TikettiDBEntities db = new TikettiDBEntities();

        // GET: Tikettitiedot
        public ActionResult Index()
        {
            var tikettitiedot = db.Tikettitiedot.Include(t => t.Asiakastiedot).Include(t => t.IT_tukihenkilot).Include(t => t.LaitteenTyyppi).Include(t => t.YhteydenTyyppi);
            return View(tikettitiedot.ToList());
        }

        // GET: Tikettitiedot/Details/5
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

        // GET: Tikettitiedot/Create
        public ActionResult Create()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakastiedot, "AsiakasID", "Etunimi");
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Etunimi");
            ViewBag.LaitenumeroID = new SelectList(db.LaitteenTyyppi, "LaitenumeroID", "Laitteen_nimi");
            ViewBag.YhteysID = new SelectList(db.YhteydenTyyppi, "YhteysID", "Yhteyden_tyyppi");
            return View();
        }

        // POST: Tikettitiedot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TikettiID,YhteysID,LaitenumeroID,AsiakasID,PVM,Ongelman_kuvaus,itHenkiloID")] Tikettitiedot tikettitiedot)
        {
            if (ModelState.IsValid)
            {
                db.Tikettitiedot.Add(tikettitiedot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsiakasID = new SelectList(db.Asiakastiedot, "AsiakasID", "Etunimi", tikettitiedot.AsiakasID);
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Etunimi", tikettitiedot.itHenkiloID);
            ViewBag.LaitenumeroID = new SelectList(db.LaitteenTyyppi, "LaitenumeroID", "Laitteen_nimi", tikettitiedot.LaitenumeroID);
            ViewBag.YhteysID = new SelectList(db.YhteydenTyyppi, "YhteysID", "Yhteyden_tyyppi", tikettitiedot.YhteysID);
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
            ViewBag.AsiakasID = new SelectList(db.Asiakastiedot, "AsiakasID", "Etunimi", tikettitiedot.AsiakasID);
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Etunimi", tikettitiedot.itHenkiloID);
            ViewBag.LaitenumeroID = new SelectList(db.LaitteenTyyppi, "LaitenumeroID", "Laitteen_nimi", tikettitiedot.LaitenumeroID);
            ViewBag.YhteysID = new SelectList(db.YhteydenTyyppi, "YhteysID", "Yhteyden_tyyppi", tikettitiedot.YhteysID);
            return View(tikettitiedot);
        }

        // POST: Tikettitiedot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TikettiID,YhteysID,LaitenumeroID,AsiakasID,PVM,Ongelman_kuvaus,itHenkiloID")] Tikettitiedot tikettitiedot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tikettitiedot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakastiedot, "AsiakasID", "Etunimi", tikettitiedot.AsiakasID);
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Etunimi", tikettitiedot.itHenkiloID);
            ViewBag.LaitenumeroID = new SelectList(db.LaitteenTyyppi, "LaitenumeroID", "Laitteen_nimi", tikettitiedot.LaitenumeroID);
            ViewBag.YhteysID = new SelectList(db.YhteydenTyyppi, "YhteysID", "Yhteyden_tyyppi", tikettitiedot.YhteysID);
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
