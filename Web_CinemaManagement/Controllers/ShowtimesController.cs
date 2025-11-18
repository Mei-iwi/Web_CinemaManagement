using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;
using System.Configuration;
using Web_CinemaManagement.Models.ADO;

namespace Web_CinemaManagement.Controllers
{
    public class ShowtimesController : Controller
    {
        CinemaManegementLinqDataContext db;

        public ShowtimesController()
        {
            db = new CinemaManegementLinqDataContext();
        }

 


        // GET: Showtimes
        // ===== CẬP NHẬT: Thêm 2 tham số tìm kiếm =====
        public ActionResult ShowtimesIndex(string searchMovie, DateTime? searchDate)
        {
            var query = from sc in db.SUATCHIEUs
                        join p in db.PHIMs on sc.MAPHIM equals p.MAPHIM
                        join r in db.PHONGCHIEUs on sc.MAPHONG equals r.MAPHONG
                        select new
                        {
                            sc.MASUAT,
                            sc.NGAYCHIEU,
                            sc.GIOBATDAU,
                            p.MAPHIM,
                            p.TENPHIM,
                            p.HINH_ANH,
                            p.THOILUONG,
                            r.TENPHONG
                        };

            if (searchDate != null)
                query = query.Where(sc => sc.NGAYCHIEU == searchDate.Value.Date);
            else
                query = query.Where(sc => sc.NGAYCHIEU >= DateTime.Today);

            if (!string.IsNullOrEmpty(searchMovie))
                query = query.Where(sc => sc.TENPHIM.Contains(searchMovie));

            var upcomingShowtimes = query.OrderBy(sc => sc.NGAYCHIEU)
                                         .ThenBy(sc => sc.TENPHIM)
                                         .ThenBy(sc => sc.GIOBATDAU)
                                         .ToList();

            // Nhóm dữ liệu thành ViewModel
            var groupedData = upcomingShowtimes
                .GroupBy(sc => ((DateTime)sc.NGAYCHIEU).Date)
                .Select(dateGroup => new ShowtimeViewModel
                {
                    Date = dateGroup.Key,
                    MovieGroups = dateGroup
                        .GroupBy(sc => sc.MAPHIM)
                        .Select(movieGroup => new MovieViewModel
                        {
                            MaPhim = movieGroup.Key,
                            TenPhim = movieGroup.First().TENPHIM,
                            HinhAnh = movieGroup.First().HINH_ANH,
                            ThoiLuong = (int)(movieGroup.First().THOILUONG?.TotalMinutes ?? 0),
                            AvailableShowtimes = movieGroup
                                .Select(s => new ShowtimeDetailViewModel
                                {
                                    MaSuat = s.MASUAT,
                                    TenPhong = s.TENPHONG,
                                    GioBatDau = s.GIOBATDAU
                                })
                                .OrderBy(s => s.GioBatDau)
                                .ToList()
                        })
                        .OrderBy(m => m.TenPhim)
                        .ToList()
                })
                .OrderBy(d => d.Date)
                .ToList();

            return View(groupedData);
        }
    }
}