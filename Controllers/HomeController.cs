using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LAB_FINAL_ACT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Check if user is logged in
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            
            ViewBag.UserName = Session["UserName"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Home()
        {

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}