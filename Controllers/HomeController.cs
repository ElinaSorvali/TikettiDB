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
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Kirjautuminen LoginModel)
        {
            TikettiDBEntities db = new TikettiDBEntities();
            //Haetaan käyttäjän tiedot annetuilla tunnustiedoilla tietokannasta LINQ -kyselyllä
            var LoggedUser = db.Kirjautuminen.SingleOrDefault(x => x.Sahkoposti == LoginModel.Sahkoposti && x.Salasana == LoginModel.Salasana);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "In";
                Session["Sahkoposti"] = LoggedUser.Sahkoposti;
                //Session["LoginID"] = LoggedUser.LoginId;
                return RedirectToAction("Index", "Home"); //Tässä määritellään mihin onnistunut kirjautuminen johtaa --> Home/Index
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessfull";
                ViewBag.LoginError = 1;
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Index", LoginModel);
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