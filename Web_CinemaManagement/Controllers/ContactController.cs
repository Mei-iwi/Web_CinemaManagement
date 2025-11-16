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
        {
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
        public ActionResult Contact(string name, string email, string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
                {
                    // Tạo object CONTACT
                    CONTACT contact = new CONTACT
                    {
                        Name = name,
                        Email = email,
                        Message = message,
                        CreatedAt = DateTime.Now
                    };

                    // Thêm vào DB
                    db.CONTACTS.InsertOnSubmit(contact);
                    db.SubmitChanges(); // 1 lệnh submit change

                    ViewBag.Message = "Gửi thành công!";
                    ModelState.Clear(); // Xóa dữ liệu form cũ
                }
                else
                {
                    ViewBag.Message = "Vui lòng nhập đầy đủ thông tin.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Lỗi khi lưu vào DB: " + ex.Message;
            }

            return View();
        }
    }
}