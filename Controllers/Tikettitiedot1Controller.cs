using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TikettiDB.Models;

namespace TikettiDB.Controllers
{
    public class Tikettitiedot1Controller : Controller
    {
        private TikettiDBEntities db = new TikettiDBEntities();

        // GET: Tikettitiedot1
        public ActionResult Index()
        {
            var tikettitiedot = db.Tikettitiedot.Include(t => t.Asiakastiedot).Include(t => t.IT_tukihenkilot).Include(t => t.LaitteenTyyppi).Include(t => t.YhteydenTyyppi);
            return View(tikettitiedot.ToList());
        }

        // GET: Tikettitiedot1/Details/5
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

        // GET: Tikettitiedot1/Create
        public ActionResult Create()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakastiedot, "AsiakasID", "Etunimi");
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Etunimi");
            ViewBag.LaitenumeroID = new SelectList(db.LaitteenTyyppi, "LaitenumeroID", "Laitteen_nimi");
            ViewBag.YhteysID = new SelectList(db.YhteydenTyyppi, "YhteysID", "Yhteyden_tyyppi");
            return View();
        }

        // POST: Tikettitiedot1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TikettiID,YhteysID,LaitenumeroID,AsiakasID,PVM,Ongelman_kuvaus,itHenkiloID,RatkaisunKuvaus,Status")] Tikettitiedot tikettitiedot)
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

        // GET: Tikettitiedot1/Create2
        public ActionResult Create2()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakastiedot, "AsiakasID", "Etunimi");
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Etunimi");
            ViewBag.LaitenumeroID = new SelectList(db.LaitteenTyyppi, "LaitenumeroID", "Laitteen_nimi");
            ViewBag.YhteysID = new SelectList(db.YhteydenTyyppi, "YhteysID", "Yhteyden_tyyppi");
            return View();
        }

        // POST: Tikettitiedot1/Create2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "TikettiID,YhteysID,LaitenumeroID,AsiakasID,PVM,Ongelman_kuvaus,itHenkiloID,RatkaisunKuvaus,Status")] Tikettitiedot tikettitiedot)
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

        // GET: Tikettitiedot1/Edit/5
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

            // Populoidaan ViewBag:it, jotta kaikki tarvittavat valinnat ovat saatavilla näkymässä
            ViewBag.YhteysID = new SelectList(db.YhteydenTyyppi, "YhteysID", "Yhteyden_tyyppi", tikettitiedot.YhteysID);
            ViewBag.Status = new SelectList(db.Tikettitiedot.Select(t => t.Status).Distinct(), tikettitiedot.Status); // Tämä varmistaa että Status-valinnat ovat oikein
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Sahkoposti", tikettitiedot.itHenkiloID);

            return View(tikettitiedot);
        }

        // POST: Tikettitiedot1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TikettiID,RatkaisunKuvaus,Sahkoposti,Status,itHenkiloID,YhteysID")] Tikettitiedot tikettitiedot)
        {
            if (ModelState.IsValid)
            {
                // Tarkistetaan, että YhteysID on olemassa YhteydenTyyppi-taulussa ennen tallennusta
                if (!db.YhteydenTyyppi.Any(y => y.YhteysID == tikettitiedot.YhteysID))
                {
                    ModelState.AddModelError("YhteysID", "Valittu yhteystyyppi ei ole kelvollinen.");
                }
                else
                {
                    db.Entry(tikettitiedot).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
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

            // Jos validointi epäonnistuu, populoidaan ViewBag:it uudelleen, jotta valinnat ovat saatavilla
            ViewBag.YhteysID = new SelectList(db.YhteydenTyyppi, "YhteysID", "Yhteyden_tyyppi", tikettitiedot.YhteysID);
            ViewBag.Status = new SelectList(db.Tikettitiedot.Select(t => t.Status).Distinct(), tikettitiedot.Status);
            ViewBag.itHenkiloID = new SelectList(db.IT_tukihenkilot, "itHenkiloID", "Sahkoposti", tikettitiedot.itHenkiloID);

            return View(tikettitiedot);
        }


        // GET: Tikettitiedot1/Delete/5
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

        // POST: Tikettitiedot1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tikettitiedot tikettitiedot = db.Tikettitiedot.Find(id);
            db.Tikettitiedot.Remove(tikettitiedot);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Tiketti()
        {
            var tikettitiedot = db.Tikettitiedot.Include(t => t.Asiakastiedot).Include(t => t.IT_tukihenkilot).Include(t => t.LaitteenTyyppi).Include(t => t.YhteydenTyyppi);
            return View(tikettitiedot.ToList());
        }

        public ActionResult Tiketti1()
        {
            var tikettitiedot = db.Tikettitiedot.Include(t => t.Asiakastiedot).Include(t => t.IT_tukihenkilot).Include(t => t.LaitteenTyyppi).Include(t => t.YhteydenTyyppi);
            return View(tikettitiedot.ToList());
        }

        public ActionResult Tiketti2()
        {
            var tikettitiedot = db.Tikettitiedot.Include(t => t.Asiakastiedot).Include(t => t.IT_tukihenkilot).Include(t => t.LaitteenTyyppi).Include(t => t.YhteydenTyyppi);
            return View(tikettitiedot.ToList());
        }

        public ActionResult Tiketti3()
        {
            var tikettitiedot = db.Tikettitiedot.Include(t => t.Asiakastiedot).Include(t => t.IT_tukihenkilot).Include(t => t.LaitteenTyyppi).Include(t => t.YhteydenTyyppi);
            return View(tikettitiedot.ToList());
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
                mail.Body = "Tikettisi on ratkaistu onnistuneesti. Tikettisi käsittelynumero on: " + tikettiID;
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
