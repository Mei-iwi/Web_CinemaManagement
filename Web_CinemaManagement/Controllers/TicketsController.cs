using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;
using Web_CinemaManagement.Helper;

namespace Web_CinemaManagement.Controllers
{
    public class TicketsController : Controller
    {
        // GET: Tickets
        CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();
        public ActionResult BookingHistory()
        {

            if (Session["User"] == null || (int)Session["Position"] == 1 || (int)Session["Position"] == 2)
            {
                return RedirectToAction("Login", "Authentication", new { area = "" });
            }

            Customer kh = Session["User"] as Customer;

            List<VE> ticket = db.VEs.Where(t => t.MAKH == kh.MAKH).ToList();

            return View(ticket);
        }

        public ActionResult Details(string id)
        {
            try
            {
                List<DANGKY> dk = db.DANGKies.Where(t => t.MAVE == id).ToList();

                double sum = 0;

                foreach (var item in dk)
                {
                    sum += (double)(item.SOLUONG * item.DICHVU.DONGIA);
                }

                ViewBag.total = sum;

                return View(dk);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Login", "Authentication", new { area = "" });
            }
        }
    }
}