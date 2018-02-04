using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI_Finder_Test.Models;

namespace WebAPI_Finder_Test.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ViewBag.Image = db.Users.First(u => u.Email == "rinat@gmail.com").AvatarImage;
            return View();
        }
    }
}