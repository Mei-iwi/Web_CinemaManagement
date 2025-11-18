using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Areas.Manager.Controllers
{
    public class QLKhachHangController : Controller
    {
        CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();
        // GET: Manager/QLKhachHang
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult KhachHang()
        {

            return View(db.KHACHHANGs.ToList());
        }

        public ActionResult Create()
        {
            return View(new KHACHHANG());
        }

        public ActionResult Edit(string id)
        {


            KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(n => n.MAKH == id);
            return View(kh);
        }

        public ActionResult Delete(string id)
        {



            KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(n => n.MAKH == id);
            return View(kh);
        }

        public ActionResult Details(string id)
        {


            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(n => n.MAKH == id);

            // DÒNG KIỂM TRA NÀY LÀ QUAN TRỌNG NHẤT
            if (kh == null)
            {
                return HttpNotFound();
            }

            return View(kh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KHACHHANG kh)
        {


            db.KHACHHANGs.InsertOnSubmit(kh);

            db.SubmitChanges();

            return RedirectToAction("KhachHang");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KHACHHANG kh)
        {


            KHACHHANG khachhang = db.KHACHHANGs.FirstOrDefault(n => n.MAKH == kh.MAKH);
            khachhang.HOTENKH = kh.HOTENKH;
            khachhang.SDT = kh.SDT;
            khachhang.DIACHI = kh.DIACHI;
            db.SubmitChanges();
            return RedirectToAction("KhachHang");
        }
        [HttpPost]
        public ActionResult Delete(KHACHHANG kh)
        {

            KHACHHANG khachhang = db.KHACHHANGs.FirstOrDefault(n => n.MAKH == kh.MAKH);
            db.KHACHHANGs.DeleteOnSubmit(khachhang);
            db.SubmitChanges();
            return RedirectToAction("KhachHang");
        }


    }
}