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
        [ValidateAntiForgeryToken]
     
        public ActionResult Contact(string name, string email, string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(message))
                {
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
                    string newMAYEUCAU = "YEUC" + DateTime.Now.Ticks.ToString().Substring(0, 6); // tạo mã duy nhất
                    YEUCAUHOTRO yc = new YEUCAUHOTRO
                    {
                        MAYEUCAU = newMAYEUCAU,
                        // Nếu có các trường bắt buộc khác trong YEUCAUHOTRO, gán ở đây
                        // Ví dụ: NGAYYEUCAU = DateTime.Now
                    };
                    db.YEUCAUHOTROs.InsertOnSubmit(yc);
                    db.SubmitChanges(); // Lưu mã yêu cầu mới

                    // --- Tạo phản hồi mới ---
                    NOIDUNG_PHANHOI phanhoi = new NOIDUNG_PHANHOI
                    {
                        MAND = newMAND,
                        MAYEUCAU = newMAYEUCAU,
                        MAKH = Session["User ID"]?.ToString(),
                        MANV = null,
                        NOIDUNG_PHANHOI1 = message,
                        NGAYGUI_PHANHOI = DateTime.Now
                    };

                    db.NOIDUNG_PHANHOIs.InsertOnSubmit(phanhoi);
                    db.SubmitChanges();

                    TempData["Message"] = "Gửi phản hồi thành công!";
                    return RedirectToAction("Contact");
                }
                else
                {
                    ViewBag.Message = "Vui lòng nhập đầy đủ thông tin.";
                    ViewData["name"] = name;
                    ViewData["email"] = email;
                    ViewData["message"] = message;
                }
            }
            catch (Exception ex)
            {
                string error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.Message = "Lỗi khi lưu vào DB: " + error;
                ViewData["name"] = name;
                ViewData["email"] = email;
                ViewData["message"] = message;
            }

            return View();
        }


    }
}