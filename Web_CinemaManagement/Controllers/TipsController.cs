using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_CinemaManagement.Controllers
{
    public class TipsController : Controller
    {
        // GET: Tips
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTips()
        {
            var tips = new List<object>
            {
                new {
                    Id = 1,
                    Title = "Chọn ghế xem sắc nét nhất",
                    Content = "Hàng F – G là hàng có góc nhìn đẹp nhất. Tránh ngồi sát màn hình.",
                    Image = "/wwwroot/Images/choose-seat.jpg"
                },
                new {
                    Id = 2,
                    Title = "Thời điểm xem ít người",
                    Content = "Suất chiếu buổi sáng hoặc khuya sẽ ít ồn ào, thích hợp để tập trung xem phim.",
                    Image = "/wwwroot/Images/morning-show.jpg"
                },
                new {
                    Id = 3,
                    Title = "Tiết kiệm tiền vé",
                    Content = "Đặt vé online trước 1–2 ngày hoặc xem ngày thứ 2–4 để có giá rẻ hơn.",
                    Image = "/wwwroot/images/save-money.jpg"
                },
                new {
                    Id = 4,
                    Title = "Tránh spoil phim",
                    Content = "Hạn chế đọc review trước, và tắt thông báo các trang phim khi phim mới ra mắt.",
                    Image = "/wwwroot/images/no-spoil.jpg"
                }
            };

            return Json(tips, JsonRequestBehavior.AllowGet);
        }
    }
}