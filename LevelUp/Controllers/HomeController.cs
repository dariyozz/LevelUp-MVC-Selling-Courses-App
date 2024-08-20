using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LevelUp.Models;

namespace LevelUp.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var popularCourses = db.Courses
                .OrderByDescending(c => c.Students.Count)
                .Take(3) // Adjust the number of courses to display as needed
                .ToList();
            return View(popularCourses);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}