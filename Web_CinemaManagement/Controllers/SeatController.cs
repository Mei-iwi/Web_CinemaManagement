using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;
using Web_CinemaManagement.Models.ADO;
using Web_CinemaManagement.Helper;

namespace Web_CinemaManagement.Controllers
{
    public class SeatController : Controller
    {
        // GET: Seat

        CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

        private string createdID()
        {
            string currentcode = null;

            // Lấy MAVE mới nhất
            var lastVE = db.VEs.OrderByDescending(t => t.MAVE).FirstOrDefault();
            if (lastVE != null)
                currentcode = lastVE.MAVE;

            int newID = 1;

            if (!string.IsNullOrEmpty(currentcode))
            {
                string currentID = currentcode;

                // Định dạng V + 8 số
                if (currentID.Length >= 9 && currentID.StartsWith("V") &&
                    int.TryParse(currentID.Substring(1, 8), out int num))
                {
                    newID = num + 1;
                }
            }

            return "V" + newID.ToString("D8"); // VD: V00000001
        }

        public ActionResult SelectSeat()
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult BuyTicket([FromBody] TicketModel ticket)
        {

            int pos = (int)Session["Position"];

            if(pos != 0)
            {
                return RedirectToAction("Login", "Authentication", new { area = "" });
            }

            try
            {
                string mave = createdID();

                string restore = "G000000" + ticket.MAGHE;

                var user = Session["User"] as Customer;

                using (var db = new CinemaManegementLinqDataContext())
                {
                    // 1. Thêm vé
                    var ve = new VE
                    {
                        MASUAT = ticket.MASUAT,
                        MALV = ticket.LOAIVE,
                        MAVE = mave,
                        MAGHE = restore,
                        MAKH = user.MAKH,
                        MANV = null,
                        NGAYBANVE = DateTime.Now
                    };
                    db.VEs.InsertOnSubmit(ve);


                    var ghe = db.CT_GHE_PHONGs.FirstOrDefault(g => g.MAGHE == restore && g.MAPHONG == ticket.MAPHONG);
                    if (ghe != null)
                    {
                        ghe.TRANGTHAI = "true";
                    }

                    db.SubmitChanges();

                    return Json(new { success = true, message = "Mua vé thành công!", mave = mave });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}