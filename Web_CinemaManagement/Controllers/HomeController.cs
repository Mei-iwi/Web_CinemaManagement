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

        public ActionResult Dashboard()
        {
            int position = Session["Position"] != null ? (int)Session["Position"] : -1;

            CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

            List<PHIM> p;

            if (position == -1)
            {

                Session["UserID"] = "JustWatch";

                Session["Password"] = "Abc12345!";

                Session["Position"] = -1;

                db = new CinemaManegementLinqDataContext();

                p = db.PHIMs.ToList();

                return View(p);
            }


            p = db.PHIMs.ToList();


            return View(p);
        }


    }
}