using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Controllers
{
    public class ContactController : Controller
    {
        CinemaManegementLinqDataContext db;

        public ContactController()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
            db = new CinemaManegementLinqDataContext(connString);
        }

        // GET: Contact
        public ActionResult Contact()
        { // Kiểm tra login
            if (Session["User"] == null)
                return RedirectToAction("Login", "Authentication");

            return View();
        }

        public  ActionResult About()
        {
            return View(); 
        }
        public ActionResult AboutTwo()
        {
            return View();
        }
        public ActionResult AboutThree()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string LoaiVanDe, string TieuDe, string NoiDung)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Authentication");

            if (string.IsNullOrEmpty(LoaiVanDe) || string.IsNullOrEmpty(TieuDe) || string.IsNullOrEmpty(NoiDung))
            {
                ViewBag.Message = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }

            try
            {
                string maKH = null;
                string maNV = null;

                // Kiểm tra user đang login là KH hay NV
                if (Session["User"].GetType() == typeof(KHACHHANG))
                {
                    var kh = (KHACHHANG)Session["User"];
                    maKH = kh.MAKH;
                }
                else if (Session["User"].GetType() == typeof(NHANVIEN))
                {
                    var nv = (NHANVIEN)Session["User"];
                    maNV = nv.MANV;
                }

                CSKH ticket = new CSKH()
                {
                    MaKH = maKH,
                    MaNV = maNV,
                    LoaiVanDe = LoaiVanDe,
                    TieuDe = TieuDe,
                    NoiDung = NoiDung,
                    TrangThai = "Chờ xử lý",
                    NgayTao = DateTime.Now
                };

                db.CSKHs.InsertOnSubmit(ticket);
                db.SubmitChanges();

                ViewBag.Message = "Gửi phản hồi thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Lỗi khi lưu vào DB: " + ex.Message;
            }

            return View();
        }
    }
}