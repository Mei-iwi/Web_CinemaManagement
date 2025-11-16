using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;
using System.Configuration;

namespace Web_CinemaManagement.Controllers
{
    public class ShowtimesController : Controller
    {
        CinemaManegementLinqDataContext db;
        string connString;

        public ShowtimesController()
        {
            connString = ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
            db = new CinemaManegementLinqDataContext(connString);
        }

        // GET: Showtimes
        // ===== CẬP NHẬT: Thêm 2 tham số tìm kiếm =====
        public ActionResult ShowtimesIndex(string searchMovie, DateTime? searchDate)
        {
            // 1. Tạo query cơ bản
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

            // 2. Lọc theo điều kiện tìm kiếm

            // Lọc theo ngày
            if (searchDate != null)
            {
                // Nếu người dùng chọn ngày, lọc chính xác theo ngày đó
                query = query.Where(sc => sc.NGAYCHIEU == searchDate.Value.Date);
            }
            else
            {
                // Mặc định: Chỉ lấy suất chiếu từ hôm nay trở đi
                query = query.Where(sc => sc.NGAYCHIEU >= DateTime.Today);
            }

            // Lọc theo tên phim
            if (!String.IsNullOrEmpty(searchMovie))
            {
                query = query.Where(sc => sc.TENPHIM.Contains(searchMovie));
            }

            // 3. Sắp xếp và lấy dữ liệu
            var upcomingShowtimes = query.OrderBy(sc => sc.NGAYCHIEU)
                                         .ThenBy(sc => sc.TENPHIM)
                                         .ThenBy(sc => sc.GIOBATDAU);

            // 4. Lưu lại giá trị tìm kiếm để hiển thị lại trên View
            ViewBag.CurrentMovieSearch = searchMovie;
            ViewBag.CurrentDateSearch = searchDate;


            // 5. Nhóm dữ liệu
            var groupedData = upcomingShowtimes.AsEnumerable()
                .GroupBy(sc => ((DateTime)sc.NGAYCHIEU).Date) // Nhóm theo NGÀY
                .Select(dateGroup => new
                {
                    Date = dateGroup.Key,
                    MovieGroups = dateGroup
                        .GroupBy(sc => sc.MAPHIM) // Nhóm theo PHIM
                        .Select(movieGroup => new
                        {
                            MaPhim = movieGroup.Key,
                            TenPhim = movieGroup.First().TENPHIM,
                            HinhAnh = movieGroup.First().HINH_ANH,
                            ThoiLuong = (int)(movieGroup.First().THOILUONG?.TotalMinutes ?? 0),
                            AvailableShowtimes = movieGroup
                                .Select(showtime => new
                                {
                                    MaSuat = showtime.MASUAT,
                                    TenPhong = showtime.TENPHONG,
                                    GioBatDau = (TimeSpan)showtime.GIOBATDAU
                                })
                                .OrderBy(s => s.GioBatDau)
                                .ToList()
                        })
                        .OrderBy(m => m.TenPhim)
                        .ToList()
                })
                .OrderBy(d => d.Date)
                .ToList();

            // 6. Gán dữ liệu đã nhóm vào ViewBag
            ViewBag.GroupedShowtimes = groupedData;

            return View();
        }
    }
}