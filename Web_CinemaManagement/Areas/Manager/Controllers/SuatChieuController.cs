using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Helper;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Areas.Manager.Controllers
{
    public class SuatChieuController : Controller
    {
        CinemaManegementLinqDataContext db;
        string connString;

        public SuatChieuController()
        {
            connString = ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
            db = new CinemaManegementLinqDataContext(connString);
        }
        public ActionResult SuatChieuIndex()
        { 
            return View(db.SUATCHIEUs.ToList());
        }

        public ActionResult TimKiem(string keyword)
        {
            var sc = from s in db.SUATCHIEUs
                     select s;

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();

                sc = sc.Where(s =>
                    (s.MAPHIM != null && s.MAPHIM.ToLower().Contains(keyword)) ||
                    (s.MAPHONG != null && s.MAPHONG.ToLower().Contains(keyword)) ||
                    (s.PHIM != null && s.PHIM.TENPHIM.ToLower().Contains(keyword))
                );
            }

            return View("SuatChieuIndex", sc.ToList()); // Sử dụng lại view Index
        }

        public ActionResult ThemSuatchieu()
        {
            ViewBag.DanhSachPhim = db.PHIMs.ToList();
            ViewBag.DanhSachPhong = db.PHONGCHIEUs.ToList(); // thêm dòng này

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemSuatchieuCOMMIT(SUATCHIEU suatchieu)
        {
            // Validation MASUAT tự động theo định dạng SC00000001
            var lastSuat = db.SUATCHIEUs
                             .OrderByDescending(s => s.MASUAT)
                             .FirstOrDefault();

            if (lastSuat != null)
            {
                // Lấy phần số, tăng 1
                long lastNumber = long.Parse(lastSuat.MASUAT.Substring(2)); // Bỏ "SC"
                long nextNumber = lastNumber + 1;
                suatchieu.MASUAT = "SC" + nextNumber.ToString("D8"); // D8 => 8 chữ số
            }
            else
            {
                suatchieu.MASUAT = "SC00000001";
            }

            // Kiểm tra giờ bắt đầu < giờ kết thúc
            if (suatchieu.GIOKETTHUC.HasValue && suatchieu.GIOBATDAU >= suatchieu.GIOKETTHUC.Value)
            {
                ModelState.AddModelError("", "Giờ kết thúc phải lớn hơn giờ bắt đầu.");
            }

            // Kiểm tra giờ hợp lệ (constraint CHK_TIME)
            TimeSpan minTime = new TimeSpan(8, 0, 0);
            TimeSpan maxTime = new TimeSpan(23, 0, 0);
            if (suatchieu.GIOBATDAU < minTime || suatchieu.GIOBATDAU > maxTime ||
                (suatchieu.GIOKETTHUC.HasValue && (suatchieu.GIOKETTHUC.Value < minTime || suatchieu.GIOKETTHUC.Value > maxTime)))
            {
                ModelState.AddModelError("", "Giờ chiếu phải từ 08:00 đến 23:00.");
            }

            if (ModelState.IsValid)
            {
                db.SUATCHIEUs.InsertOnSubmit(suatchieu);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            // Nếu lỗi, load lại dữ liệu cho form
            ViewBag.DanhSachPhim = db.PHIMs.ToList();
            ViewBag.DanhSachPhong = db.PHONGCHIEUs.ToList();
            return View("ThemSuatchieu", suatchieu);
        }

        public ActionResult SuaSuatChieu(string id)
        {
            var suatchieu = db.SUATCHIEUs.FirstOrDefault(s => s.MASUAT == id);
            if (suatchieu == null)
            {
                return HttpNotFound();
            }

            return View(suatchieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaCOMMIT(SUATCHIEU sc)
        {
            var suatchieu = db.SUATCHIEUs.FirstOrDefault(s => s.MASUAT == sc.MASUAT);
            if (suatchieu != null)
            {
                suatchieu.MAPHONG = sc.MAPHONG;
                suatchieu.MAPHIM = sc.MAPHIM;
                suatchieu.GIOBATDAU = sc.GIOBATDAU;
                suatchieu.GIOKETTHUC = sc.GIOKETTHUC;
                suatchieu.NGAYCHIEU = sc.NGAYCHIEU;
                suatchieu.SOLUONG = sc.SOLUONG;

                db.SubmitChanges();
            }

            return RedirectToAction("Index");
        }
    }
}