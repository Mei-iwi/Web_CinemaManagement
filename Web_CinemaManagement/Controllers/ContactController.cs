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
        private CinemaManegementLinqDataContext db;

        public ContactController()
        {
            // Khởi tạo DataContext
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
            db = new CinemaManegementLinqDataContext(connString);

        }

        // GET: Contact
        public ActionResult Contact()
        {
            Session["User ID"] = "KH00000001";
            Session["Password"] = "Abc12345!";
            Session["Position"] = 0;

            return View();
        }

        public ActionResult About()
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
        [ValidateAntiForgeryToken]
        public ActionResult Contact(string name, string email, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
                {
                    ViewBag.Message = "Vui lòng nhập đầy đủ thông tin.";
                    ViewData["name"] = name;
                    ViewData["email"] = email;
                    ViewData["message"] = message;
                    return View();
                }

                // --- Tạo MAND tự tăng ND01, ND02 ---
                var lastPhanHoi = db.NOIDUNG_PHANHOIs.OrderByDescending(p => p.MAND).FirstOrDefault();
                string newMAND = "ND01";
                if (lastPhanHoi != null)
                {
                    int lastNumber = 0;
                    if (int.TryParse(lastPhanHoi.MAND.Substring(2), out lastNumber))
                    {
                        newMAND = "ND" + (lastNumber + 1).ToString("D2");
                    }
                }

                // --- Tạo mã yêu cầu mới cho bảng YEUCAUHOTRO ---
                string newMAYEUCAU = "YE" + DateTime.Now.Ticks.ToString().Substring(0, 6); // tối đa 10 ký tự
                YEUCAUHOTRO yc = new YEUCAUHOTRO
                {
                    MAYEUCAU = newMAYEUCAU,
                    // nếu có trường bắt buộc khác, gán ở đây
                };
                db.YEUCAUHOTROs.InsertOnSubmit(yc);
                db.SubmitChanges();

                // --- Tạo record NOIDUNG_PHANHOI ---
                NOIDUNG_PHANHOI phanhoi = new NOIDUNG_PHANHOI
                {
                    MAND = newMAND,
                    MAYEUCAU = newMAYEUCAU,
                    MAKH = Session["User ID"]?.ToString(),
                    MANV = null,
                    NOIDUNG_PHANHOI1 = $"Họ tên: {name}\nEmail: {email}\nNội dung: {message}",
                    NGAYGUI_PHANHOI = DateTime.Now
                };

                db.NOIDUNG_PHANHOIs.InsertOnSubmit(phanhoi);
                db.SubmitChanges();

                TempData["Message"] = "Gửi phản hồi thành công!";
                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                string error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.Message = "Lỗi khi lưu vào DB: " + error;
                ViewData["name"] = name;
                ViewData["email"] = email;
                ViewData["message"] = message;
                return View();
            }
        }
    }
}