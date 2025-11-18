using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Areas.Employee.Controllers
{
    public class TicketManagementController : Controller
    {
        // GET: Employee/TicketManagement
        public ActionResult TicketManagement()
        {
            int position = (int)Session["Position"];

            if (position != 1)
            {
                return RedirectToAction("Login", "Authentication", new { area = "" });
            }

            return View();
        }

        public ActionResult Details(string id)
        {
            CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

            VE ticket = db.VEs.FirstOrDefault(t => t.MAVE == id);

            if (string.IsNullOrEmpty(id) || ticket == null)
            {
                return RedirectToAction("TicketManagement", "TicketManagement");
            }

            return View(ticket);
        }
        [ChildActionOnly]
        public ActionResult TicketServices(string id)
        {
            CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

            var services = db.DANGKies.Where(d => d.MAVE == id).ToList();

            ViewBag.total = services.Sum(t => t.SOLUONG * t.DICHVU.DONGIA);

            return PartialView("_TicketServices", services);
        }
    }
}