using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;

// LƯU Ý: Namespace phải có .Areas.Employee.Controllers
namespace Web_CinemaManagement.Areas.Employee.Controllers
{
    public class MemberSearchController : Controller
    {
        CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

        // GET: Employee/MemberSearch
        public ActionResult Index(string searchString)
        {
            var members = from m in db.KHACHHANGs select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                // Tìm theo Tên hoặc SĐT
                members = members.Where(s => s.HOTENKH.Contains(searchString) || s.SDT.Contains(searchString));
            }

            return View(members.ToList());
        }
    }
}