using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnboardingProj.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Customers()
        {
            return View();
        }

        public ActionResult Stores()
        {
            return View();
        }

        public ActionResult Sales()
        {
            return View();
        }
    }
}