using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Areas.Manager.Controllers
{
    public class ServicesController : Controller
    {
        CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

        // GET: Manager/Services
        public ActionResult Index()
        {
            return View(db.DICHVUs.ToList());
        }

        // GET: Manager/Services/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Services/Create
        [HttpPost]
        public ActionResult Create(DICHVU s)
        {
            if (ModelState.IsValid)
            {
                db.DICHVUs.InsertOnSubmit(s);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(s);
        }

        // GET: Manager/Services/Edit/ID
        public ActionResult Edit(string id)
        {
            var s = db.DICHVUs.FirstOrDefault(x => x.MASP == id);
            if (s == null) return HttpNotFound();
            return View(s);
        }

        // POST: Manager/Services/Edit
        [HttpPost]
        public ActionResult Edit(DICHVU model)
        {
            var s = db.DICHVUs.FirstOrDefault(x => x.MASP == model.MASP);
            if (s == null) return HttpNotFound();

            if (ModelState.IsValid)
            {
                s.TENSP = model.TENSP;
                s.DONGIA = model.DONGIA;
                s.HINH_ANH = model.HINH_ANH;

                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Manager/Services/Details/ID
        public ActionResult Details(string id)
        {
            var s = db.DICHVUs.FirstOrDefault(x => x.MASP == id);
            if (s == null) return HttpNotFound();
            return View(s);
        }

        // GET: Manager/Services/Delete/ID
        public ActionResult Delete(string id)
        {
            var s = db.DICHVUs.FirstOrDefault(x => x.MASP == id);
            if (s == null) return HttpNotFound();
            return View(s);
        }

        // POST: Manager/Services/Delete
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var s = db.DICHVUs.FirstOrDefault(x => x.MASP == id);
            if (s != null)
            {
                db.DICHVUs.DeleteOnSubmit(s);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
