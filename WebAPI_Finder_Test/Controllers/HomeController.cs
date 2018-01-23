using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPI_Finder_Test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Register");
        }




        public ActionResult Register()
        {
            ViewBag.Title = "Register";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Title = "Login";

            return View();
        }
    }
}
