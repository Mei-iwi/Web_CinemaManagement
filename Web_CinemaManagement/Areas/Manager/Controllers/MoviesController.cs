using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;
using System.Data.Linq;
using System.IO;

namespace Web_CinemaManagement.Areas.Manager.Controllers
{
    public class MoviesController : Controller
    {
        // Khởi tạo DataContext
        CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext(); 
        // GET: Manager/Movies
        public ActionResult MoviesIndex()
        {
            // Lấy tất cả phim, sắp xếp theo ngày cập nhật
            var dsPhim = db.PHIMs.OrderByDescending(p => p.NGAYCAPNHAT).ToList();
            return View(dsPhim);
        }

        // --- CHỨC NĂNG THÊM MỚI ---
        public ActionResult Create()
        {
            // Gửi danh sách Thể Loại và Dạng Phim cho DropDownList
            ViewBag.MATHELOAI = new SelectList(db.THELOAIPHIMs, "MATHELOAI", "TENTHELOAI");
            ViewBag.MADP = new SelectList(db.DANGPHIMs, "MADP", "DANGPHIM"); // Giả sử DANGPHIM là tên (2D, 3D...)
            return View();
        }

        [HttpPost]
        [ValidateInput(false)] // Cho phép nhập HTML nếu cần (ví dụ: mô tả phim)
        public ActionResult Create(PHIM phim, HttpPostedFileBase HINH_ANH_Upload)
        {
            // Nạp lại DropDownList khi validation thất bại
            ViewBag.MATHELOAI = new SelectList(db.THELOAIPHIMs, "MATHELOAI", "TENTHELOAI");
            ViewBag.MADP = new SelectList(db.DANGPHIMs, "MADP", "DANGPHIM");

            if (ModelState.IsValid)
            {
                // Xử lý upload file hình ảnh
                if (HINH_ANH_Upload != null && HINH_ANH_Upload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HINH_ANH_Upload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);

                    // Kiểm tra nếu thư mục không tồn tại thì tạo mới
                    if (!Directory.Exists(Server.MapPath("~/Content/Images/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Images/"));
                    }

                    HINH_ANH_Upload.SaveAs(path);
                    phim.HINH_ANH = "/Content/Images/" + fileName; // Lưu đường dẫn vào CSDL
                }

                phim.NGAYCAPNHAT = System.DateTime.Now; // Tự động cập nhật ngày

                // Lưu vào CSDL
                db.PHIMs.InsertOnSubmit(phim);
                db.SubmitChanges();

                return RedirectToAction("Index");
            }

            return View(phim); // Trả về View với model nếu có lỗi
        }

        // --- CHỨC NĂNG CHỈNH SỬA ---
        public ActionResult Edit(string id)
        {
            PHIM phim = db.PHIMs.SingleOrDefault(p => p.MAPHIM == id);
            if (phim == null)
            {
                return HttpNotFound();
            }

            // Gửi danh sách DropDownList với giá trị đã chọn
            ViewBag.MATHELOAI = new SelectList(db.THELOAIPHIMs, "MATHELOAI", "TENTHELOAI", phim.MATHELOAI);
            ViewBag.MADP = new SelectList(db.DANGPHIMs, "MADP", "DANGPHIM", phim.MADP);

            return View(phim);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(PHIM model, HttpPostedFileBase HINH_ANH_Upload)
        {
            // Nạp lại DropDownList khi validation thất bại
            ViewBag.MATHELOAI = new SelectList(db.THELOAIPHIMs, "MATHELOAI", "TENTHELOAI", model.MATHELOAI);
            ViewBag.MADP = new SelectList(db.DANGPHIMs, "MADP", "DANGPHIM", model.MADP);

            if (ModelState.IsValid)
            {
                // Lấy đối tượng PHIM gốc từ CSDL
                PHIM phimToUpdate = db.PHIMs.SingleOrDefault(p => p.MAPHIM == model.MAPHIM);

                if (phimToUpdate != null)
                {
                    // Cập nhật các trường
                    phimToUpdate.TENPHIM = model.TENPHIM;
                    phimToUpdate.MATHELOAI = model.MATHELOAI;
                    phimToUpdate.MADP = model.MADP;
                    phimToUpdate.NHASX = model.NHASX;
                    phimToUpdate.NGAYKHOICHIEU = model.NGAYKHOICHIEU;
                    phimToUpdate.NGAYKETTHUC = model.NGAYKETTHUC;
                    phimToUpdate.THOILUONG = model.THOILUONG;
                    phimToUpdate.NGAYCAPNHAT = System.DateTime.Now; // Luôn cập nhật ngày

                    // Xử lý nếu có upload file hình ảnh mới
                    if (HINH_ANH_Upload != null && HINH_ANH_Upload.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(HINH_ANH_Upload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);

                        HINH_ANH_Upload.SaveAs(path);
                        phimToUpdate.HINH_ANH = "/Content/Images/" + fileName; // Cập nhật đường dẫn mới
                    }

                    // Lưu thay đổi
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(model); // Trả về View với model nếu có lỗi
        }

        // --- CHỨC NĂNG XÓA ---

        // GET: QuanLyPhim/Delete/5
        public ActionResult Delete(string id)
        {
            PHIM phim = db.PHIMs.SingleOrDefault(p => p.MAPHIM == id);
            if (phim == null)
            {
                return HttpNotFound();
            }
            return View(phim);
        }

        // POST: QuanLyPhim/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            PHIM phim = db.PHIMs.SingleOrDefault(p => p.MAPHIM == id);
            if (phim != null)
            {
                db.PHIMs.DeleteOnSubmit(phim);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}