using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using WebAPI_Finder_Test.Controllersз;
using WebAPI_Finder_Test.Models;

namespace WebAPI_Finder_Test.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View("Register");
        }

        public ActionResult List()
        {
            //db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            var users = db.Users.AsNoTracking().Include(xr => xr.City).Include(xr => xr.City.Country).OrderByDescending(xr => xr.RegistrationDate).ToList();

            return View(users);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Create()
        {
            var cities = db.Cities.AsNoTracking().ToList();
            ViewBag.CityId = new SelectList(cities, "Id", "Name");
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Create(RegisterBindingModel model)
        {
            AccountController acc = new AccountController();
            acc.Request = new System.Net.Http.HttpRequestMessage();
            acc.Configuration = new HttpConfiguration();
            acc.ActionContext = new System.Web.Http.Controllers.HttpActionContext();
            var res = acc.Register(model);

            
            if (res != null)
            {
                acc.InsertAvatar(model.Email);
            }


            return Redirect("List");
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
