using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Helper;
using Web_CinemaManagement.Models.ModelLinq;
using PagedList;

namespace Web_CinemaManagement.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public ActionResult Dashboard(int? page)
        {
            int position = Session["Position"] != null ? (int)Session["Position"] : -1;

            CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

            List<PHIM> p = db.PHIMs.ToList();

            if (position == -1)
            {
                Session["UserID"] = "JustWatch";
                Session["Password"] = "Abc12345!";
                Session["Position"] = -1;
            }

            // Phân trang
            int pageNumber = page ?? 1;
            int pageSize = 8;
            var pageList = p.ToPagedList(pageNumber, pageSize);

            return View(pageList);
        }


    }
}