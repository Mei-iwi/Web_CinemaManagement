using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_CinemaManagement.Controllers
{
    public class ShowtimesController : Controller
    {
        // GET: Showtimes
        public ActionResult Index()
        {
            return View();
        }
    }
}