using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TikettiDB.Models;

namespace TikettiDB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Statukset kirjautumistilanteen mukaan ja toinen status yläpalkkia varten
            if (Session["Sahkoposti"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //Tämä hakee kirjautuneen nimen tervetulotoivotukseen
                string userName = Session["Sahkoposti"].ToString();
                ViewBag.LoggedStatus = "Tervetuloa " + userName + "!";
                return View();
            }
            
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Kirjautuminen LoginModel)
        {
            TikettiDBEntities db = new TikettiDBEntities();
            // Haetaan käyttäjän tiedot annetuilla tunnustiedoilla tietokannasta LINQ -kyselyllä
            var LoggedUser = db.Kirjautuminen.SingleOrDefault(x => x.Sahkoposti == LoginModel.Sahkoposti && x.Salasana == LoginModel.Salasana);

            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "In";
                Session["Sahkoposti"] = LoggedUser.Sahkoposti;

                // Hae käyttäjän taso tietokannasta Layoutissa olevaa navbaria varten
                int userLevel = LoggedUser.Taso; 
                Session["Taso"] = userLevel;

                switch (userLevel)
                {
                    //Tässä on vain esimerkkisivut, jotka pitää muuttaa kun sivut ovat olemassa
                    case 1:
                        return RedirectToAction("Index", "Asiakastiedot");
                    case 2:
                        return RedirectToAction("Index", "IT_tukihenkilot");
                    case 3:
                        return RedirectToAction("Index", "Tikettitiedot");
                    default:
                        // Käyttäjällä ei ole määriteltyä tasoa
                        return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessfull";
                ViewBag.LoginError = 1;
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Login", LoginModel);
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home"); //Uloskirjautumisen jälkeen kirjautumissivulle
        }

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}