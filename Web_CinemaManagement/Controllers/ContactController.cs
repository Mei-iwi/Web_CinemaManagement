using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_CinemaManagement.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Contact()
        {
            return View();
        }
        public  ActionResult About()
        {
            return View(); 
        }
        public ActionResult AboutTwo()
        {
            return View();
        }
        public ActionResult AboutThree()
        {
            return View();
        }
    }
}