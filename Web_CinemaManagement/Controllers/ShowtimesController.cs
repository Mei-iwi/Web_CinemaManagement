using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq; // Đảm bảo namespace này đúng
using System.Configuration;

namespace Web_CinemaManagement.Controllers
{
    public class ShowtimesController : Controller
    {
        CinemaManegementLinqDataContext db;
        string connString;

        // Khởi tạo CSDL
        public ShowtimesController()
        {
            connString = ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
            db = new CinemaManegementLinqDataContext(connString);
        }

        /// <summary>
        /// Trang chính: Liệt kê tất cả suất chiếu
        /// </summary>
        public ActionResult ShowtimesIndex()
        {
            // 1. Lấy tất cả suất chiếu TỪ HÔM NAY trở về sau
            var allShowtimes = db.SUATCHIEUs
                .Where(sc => sc.NGAYCHIEU >= DateTime.Today)
                .OrderBy(sc => sc.NGAYCHIEU)      // Sắp xếp theo Ngày
                .ThenBy(sc => sc.PHIM.TENPHIM)  // Rồi theo Tên Phim
                .ThenBy(sc => sc.GIOBATDAU)     // Rồi theo Giờ chiếu
                .ToList();

            // 2. Gửi danh sách đã sắp xếp này sang View
            // View (Razor) sẽ tự xử lý việc nhóm (grouping)
            return View(allShowtimes);
        }

        /// <summary>
        /// Trang "Chọn ghế" (sẽ được gọi khi bấm vào nút giờ chiếu)
        /// </summary>
        public ActionResult SelectSeats(string maSuat)
        {
            if (string.IsNullOrEmpty(maSuat))
            {
                return HttpNotFound();
            }

            // 1. Lấy thông tin Suất chiếu (Phim, Giờ, Phòng)
            var showtime = db.SUATCHIEUs.SingleOrDefault(sc => sc.MASUAT == maSuat);
            if (showtime == null)
            {
                return HttpNotFound();
            }

            // 2. Lấy toàn bộ ghế thuộc phòng chiếu của suất chiếu này
            var seatsInRoom = db.CT_GHE_PHONGs
                .Where(g => g.MAPHONG == showtime.MAPHONG)
                .OrderBy(g => g.MAGHE) // Sắp xếp ghế cho dễ hiển thị
                .ToList();

            // 3. (QUAN TRỌNG) Lấy danh sách các ghế ĐÃ ĐƯỢC ĐẶT của suất chiếu này
            // (Giả sử bạn có bảng VE (Vé) hoặc CT_VE (Chi tiết vé) có cột MASUAT và MAGHE)
            // var bookedSeatIDs = db.VEs.Where(v => v.MASUAT == maSuat).Select(v => v.MAGHE).ToList();

            // ViewBag.BookedSeats = bookedSeatIDs; // Gửi danh sách ghế đã đặt sang View
            ViewBag.Seats = seatsInRoom; // Gửi danh sách TẤT CẢ ghế sang View

            // Trả về View "SelectSeats.cshtml" với model là thông tin suất chiếu
            return View(showtime);
        }

        // --- DỌN DẸP ---
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}