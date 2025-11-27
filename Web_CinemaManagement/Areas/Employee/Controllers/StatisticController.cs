using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Areas.Employee.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Employee/Statistic
        public ActionResult Index()
        {
            return View();
        }
        // Khởi tạo DataContext (Nó sẽ tự lấy chuỗi kết nối trong Web.config)
        CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

        // GET: Employee/Statistic/RevenueByShift
        public ActionResult RevenueByShift()
        {
            // Kiểm tra đăng nhập
            if (Session["User"] == null)
            {
                // Thêm area = "" để nhảy ra controller đăng nhập gốc
                return RedirectToAction("Login", "Authentication", new { area = "" });
            }

            // --- CÂU TRUY VẤN LINQ ---
            var stats = (from v in db.VEs
                         join nv in db.NHANVIENs on v.MANV equals nv.MANV
                         join lv in db.LOAIVEs on v.MALV equals lv.MALV

                         // Group theo ngày và ca
                         group new { v, nv, lv } by new
                         {
                             Ngay = v.NGAYBANVE,
                             Ca = nv.CATRUC
                         } into g

                         // Sắp xếp ngày mới nhất lên đầu
                         orderby g.Key.Ngay descending

                         select new RevenueViewModel
                         {
                             NgayBan = g.Key.Ngay,
                             CaTruc = g.Key.Ca,
                             TongSoVe = g.Count(),

                             // Dùng ?? 0 để xử lý trường hợp null, tránh lỗi crash
                             TongDoanhThu = g.Sum(x => x.lv.DONGIA) ?? 0
                         }).ToList();

            return View(stats);
        }
    }
}