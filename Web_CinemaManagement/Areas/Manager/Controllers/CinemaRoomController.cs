using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Helper;
using Web_CinemaManagement.Models.ModelLinq;

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
            return View(db.PHONGCHIEUs.ToList());
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

        public ActionResult TimKiemTheoTen(string keyword)
        {
            IQueryable<PHONGCHIEU> phongs = db.PHONGCHIEUs;

            if (!String.IsNullOrEmpty(keyword))
            {
                phongs = phongs.Where(p => p.TENPHONG.Contains(keyword));
            }

            ViewBag.CurrentFilter = keyword;
            return View("CinemaRoomIndex", phongs.ToList());
        }


        //
        // GET: /CinemaRoom/Edit
        //
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Lấy phòng chiếu từ DB
            PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);

            // Kiểm tra xem có tìm thấy không
            if (room == null)
            {
                return HttpNotFound();
            }

            // Trả về View "Edit.cshtml" với model là 'room'
            return View(room);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PHONGCHIEU room) // 'room' là object đã được cập nhật từ Form
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // 1. Lấy đối tượng GỐC từ CSDL
                    PHONGCHIEU originalRoom = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == room.MAPHONG);

                    if (originalRoom == null)
                    {
                        return HttpNotFound();
                    }

                    // 2. Cập nhật các giá trị mới từ form vào đối tượng gốc
                    originalRoom.TENPHONG = room.TENPHONG;
                    originalRoom.TONGSOGHE = room.TONGSOGHE;


                    // 3. Gọi SubmitChanges()

                    db.SubmitChanges();

                    // 4. Chuyển hướng về trang Index
                    return RedirectToAction("CinemaRoomIndex");
                }
            }
            catch (Exception ex)
            {
                // Thêm lỗi vào model để hiển thị cho người dùng
                ModelState.AddModelError("", "Lỗi khi cập nhật CSDL: " + ex.Message);
            }

            return View(room);
        }






        public ActionResult Details(string id)
        {
            PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);
            return View(room);
        }
        //
        // GET: /CinemaRoom/Delete/
        // Hiển thị trang xác nhận xóa
        //
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Lấy phòng chiếu từ DB
            PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);

            // Kiểm tra xem có tìm thấy không
            if (room == null)
            {
                return HttpNotFound();
            }

            // Trả về View "Delete.cshtml"
            return View(room);
        }

        //
        // POST: /CinemaRoom/Delete/P001
        // Xử lý logic xóa sau khi người dùng xác nhận
        //
        [HttpPost, ActionName("Delete")] // Đặt tên này để khớp với form
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id) // Chỉ cần ID để xóa
        {
            try
            {
                // Lấy phòng chiếu từ DB
                PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);

                if (room == null)
                {
                    return HttpNotFound();
                }

                // 1. Dùng 'DeleteOnSubmit' (của LINQ to SQL) để xóa
                db.PHONGCHIEUs.DeleteOnSubmit(room);

                // 2. Lưu thay đổi
                db.SubmitChanges();

                // 3. Chuyển về trang Index
                return RedirectToAction("CinemaRoomIndex");
            }
            catch (Exception ex)
            {
                PHONGCHIEU room = db.PHONGCHIEUs.FirstOrDefault(x => x.MAPHONG == id);
                ModelState.AddModelError("", "Lỗi khi xóa CSDL: " + ex.Message + ". (Có thể phòng này đang được sử dụng ở một bảng khác.)");

                return View("Delete", room);
            }
        }

    }
}