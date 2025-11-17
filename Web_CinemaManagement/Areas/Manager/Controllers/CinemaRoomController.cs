using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Helper;
using Web_CinemaManagement.Models.ModelLinq;
using Web_CinemaManagement.Models.RoomSeat;


namespace Web_CinemaManagement.Areas.Manager.Controllers
{
    public class CinemaRoomController : Controller
    {
        CinemaManegementLinqDataContext db;
        string connString;

        public CinemaRoomController()
        {
            connString = ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
            db = new CinemaManegementLinqDataContext(connString);
        }


        public ActionResult CinemaRoomIndex()
        {
            var rooms = db.PHONGCHIEUs.ToList();
            var seatTypes = db.GHEs.ToList(); 

            var viewModel = new CinemaRoomIndexViewModel
            {
                PhongChieus = rooms,
                Ghes = seatTypes
            };

            return View(viewModel);
        }

        public ActionResult TimKiemTheoTen(string keyword)
        {
            IQueryable<PHONGCHIEU> phongs = db.PHONGCHIEUs;

            if (!String.IsNullOrEmpty(keyword))
            {
                phongs = phongs.Where(p => p.TENPHONG.Contains(keyword));
            }

            var seatTypes = db.GHEs.ToList();


            var viewModel = new CinemaRoomIndexViewModel
            {
                PhongChieus = phongs.ToList(),
                Ghes = seatTypes
            };

            ViewBag.CurrentFilter = keyword;
            return View("CinemaRoomIndex", viewModel);
        }


        public ActionResult Create()
        {
            return View(new PHONGCHIEU());
        }

        [HttpPost]
        public ActionResult CreateOnSubmit(PHONGCHIEU room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.PHONGCHIEUs.InsertOnSubmit(room);
                    db.SubmitChanges();
                    return RedirectToAction("CinemaRoomIndex");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi lưu vào CSDL: " + ex.Message);
            }
            return View("Create", room);
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PHONGCHIEU room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PHONGCHIEU originalRoom = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == room.MAPHONG);
                    if (originalRoom == null)
                    {
                        return HttpNotFound();
                    }
                    originalRoom.TENPHONG = room.TENPHONG;
                    originalRoom.TONGSOGHE = room.TONGSOGHE;
                    db.SubmitChanges();
                    return RedirectToAction("CinemaRoomIndex");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi cập nhật CSDL: " + ex.Message);
            }
            return View(room);
        }

        public ActionResult Details(string id)
        {
            PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);

            if (room == null)
            {
                return HttpNotFound();
            }


            int tongSoGhe = room.TONGSOGHE ?? 0; // Lấy tổng số ghế

            if (tongSoGhe > 0)
            {

                int soGheMoiHang = (int)Math.Ceiling(Math.Sqrt(tongSoGhe));

                int soHangGhe = (int)Math.Ceiling((double)tongSoGhe / soGheMoiHang);

                ViewBag.SoHang = soHangGhe;
                ViewBag.SoCot = soGheMoiHang;
            }
            else
            {
                ViewBag.SoHang = 0;
                ViewBag.SoCot = 0;
            }

            return View(room);
        }

        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);
                if (room == null)
                {
                    return HttpNotFound();
                }
                db.PHONGCHIEUs.DeleteOnSubmit(room);
                db.SubmitChanges();
                return RedirectToAction("CinemaRoomIndex");
            }
            catch (Exception ex)
            {
                PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);
                ModelState.AddModelError("", "Lỗi khi xóa CSDL: " + ex.Message + ". (Có thể phòng này đang được sử dụng ở một bảng khác.)");
                return View("Delete", room);
            }
        }

        #region CRUD Loại Ghế

        // GET: /CinemaRoom/CreateSeatType
        public ActionResult CreateSeatType()
        {
            return View(new GHE());
        }

        // POST: /CinemaRoom/CreateSeatType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSeatType(GHE seatType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.GHEs.InsertOnSubmit(seatType);
                    db.SubmitChanges();
                    return RedirectToAction("CinemaRoomIndex");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi: " + ex.Message);
            }
            // Nếu lỗi, trả về view CreateSeatType với dữ liệu đã nhập
            return View(seatType);
        }

        // GET: /CinemaRoom/EditSeatType/
        public ActionResult EditSeatType(string id) 
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            GHE seatType = db.GHEs.FirstOrDefault(s => s.MAGHE == id);
            if (seatType == null)
            {
                return HttpNotFound();
            }
            return View(seatType);
        }

        // POST: /CinemaRoom/EditSeatType/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSeatType(GHE seatType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GHE original = db.GHEs.FirstOrDefault(s => s.MAGHE == seatType.MAGHE);
                    if (original == null)
                    {
                        return HttpNotFound();
                    }
                    original.LOAIGHE = seatType.LOAIGHE;

                    db.SubmitChanges();
                    return RedirectToAction("CinemaRoomIndex");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi: " + ex.Message);
            }
            return View(seatType);
        }


        // GET: /CinemaRoom/DeleteSeatType/VIP
        public ActionResult DeleteSeatType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            GHE seatType = db.GHEs.FirstOrDefault(s => s.MAGHE == id);
            if (seatType == null)
            {
                return HttpNotFound();
            }
            return View(seatType);
        }

        // POST: /CinemaRoom/DeleteSeatType/VIP
        [HttpPost, ActionName("DeleteSeatType")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSeatTypeConfirmed(string id)
        {
            try
            {
                GHE seatType = db.GHEs.FirstOrDefault(s => s.MAGHE == id);
                if (seatType == null)
                {
                    return HttpNotFound();
                }
                db.GHEs.DeleteOnSubmit(seatType);
                db.SubmitChanges();
                return RedirectToAction("CinemaRoomIndex");
            }
            catch (Exception ex)
            {
                GHE seatType = db.GHEs.FirstOrDefault(s => s.MAGHE == id);
                ModelState.AddModelError("", "Lỗi khi xóa: " + ex.Message + ". (Có thể loại ghế này đang được sử dụng.)");
                return View("DeleteSeatType", seatType);
            }
        }
        #endregion
    }
}