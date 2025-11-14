using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Helper;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

       

        public ActionResult Index()
        {
            DataGlobal.getInformationUser("sqlserver", "123456789", 0);
            CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

            List<PHIM> p = db.PHIMs.ToList();
            return View(p);
        }

       
    }
}