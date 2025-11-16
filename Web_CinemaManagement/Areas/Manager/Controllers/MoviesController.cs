using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq; 
using System.Data.Linq;
using System.IO;
using System.Configuration;
using Web_CinemaManagement.Models;

namespace Web_CinemaManagement.Areas.Manager.Controllers
{
    public class MoviesController : Controller
    {
        // Khai báo 'db' nhưng không khởi tạo ở đây
        CinemaManegementLinqDataContext db;
        string connString;

        public MoviesController()
        {
            connString = ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
            db = new CinemaManegementLinqDataContext(connString);
        }

        // --- HÀM HỖ TRỢ ---
        private void LoadViewBagData()
        {
            ViewBag.DanhSachPhim = db.PHIMs.OrderByDescending(p => p.NGAYCAPNHAT).ToList();
            ViewBag.DanhSachTheLoai = db.THELOAIPHIMs.ToList();
            ViewBag.DanhSachDangPhim = db.DANGPHIMs.ToList();
            ViewBag.TheLoaiOptions = new SelectList(db.THELOAIPHIMs, "MATHELOAI", "TENTHELOAI");
            ViewBag.DangPhimOptions = new SelectList(db.DANGPHIMs, "MADP", "DANGPHIM1");
        }

        // --- TRANG CHÍNH ---
        public ActionResult Index()
        {
            LoadViewBagData();
            return View();
        }

        //===================================================================
        //  1. CÁC ACTION QUẢN LÝ PHIM
        //===================================================================

        // GET: Manager/Movies/CreatePhim
        public ActionResult CreatePhim()
        {
            ViewBag.TheLoaiOptions = new SelectList(db.THELOAIPHIMs, "MATHELOAI", "TENTHELOAI");
            ViewBag.DangPhimOptions = new SelectList(db.DANGPHIMs, "MADP", "DANGPHIM1");
            return View("CreatePhim", new PHIM());
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePhim(PHIM phim, HttpPostedFileBase HINH_ANH_Upload, int? ThoiLuongInput)
        {
            ViewBag.TheLoaiOptions = new SelectList(db.THELOAIPHIMs, "MATHELOAI", "TENTHELOAI", phim.MATHELOAI);
            ViewBag.DangPhimOptions = new SelectList(db.DANGPHIMs, "MADP", "DANGPHIM1", phim.MADP);

            if (db.PHIMs.Any(p => p.MAPHIM == phim.MAPHIM))
            {
                ModelState.AddModelError("MAPHIM", "Mã Phim '" + phim.MAPHIM + "' đã tồn tại. Vui lòng nhập mã khác.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (HINH_ANH_Upload != null && HINH_ANH_Upload.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(HINH_ANH_Upload.FileName);
                        var path = Path.Combine(Server.MapPath("~/wwwroot/PhotoOfTheFilm/"), fileName);
                        if (!Directory.Exists(Server.MapPath("~/wwwroot/PhotoOfTheFilm/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/wwwroot/PhotoOfTheFilm/"));
                        }
                        HINH_ANH_Upload.SaveAs(path);
                        phim.HINH_ANH = "~/wwwroot/PhotoOfTheFilm/" + fileName;
                    }
                    if (ThoiLuongInput.HasValue && ThoiLuongInput > 0)
                    {
                        if (ThoiLuongInput > 1439)
                        {
                            ModelState.AddModelError("ThoiLuongInput", "Thời lượng không được vượt quá 1439 phút (23:59).");
                            return View("CreatePhim", phim);
                        }
                        phim.THOILUONG = TimeSpan.FromMinutes((double)ThoiLuongInput.Value);
                    }
                    else
                    {
                        phim.THOILUONG = null;
                    }
                    phim.NGAYCAPNHAT = System.DateTime.Now;
                    db.PHIMs.InsertOnSubmit(phim);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi CSDL: " + ex.Message);
                }
            }
            return View("CreatePhim", phim);
        }

        // GET: Manager/Movies/EditPhim/5
        public ActionResult EditPhim(string id)
        {
            PHIM phim = db.PHIMs.SingleOrDefault(p => p.MAPHIM == id);
            if (phim == null) return HttpNotFound();
            ViewBag.MATHELOAI = new SelectList(db.THELOAIPHIMs, "MATHELOAI", "TENTHELOAI", phim.MATHELOAI);
            ViewBag.MADP = new SelectList(db.DANGPHIMs, "MADP", "DANGPHIM1", phim.MADP);
            return View("EditPhim", phim);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhim(PHIM model, HttpPostedFileBase HINH_ANH_Upload, int? ThoiLuongInput)
        {
            ViewBag.MATHELOAI = new SelectList(db.THELOAIPHIMs, "MATHELOAI", "TENTHELOAI", model.MATHELOAI);
            ViewBag.MADP = new SelectList(db.DANGPHIMs, "MADP", "DANGPHIM1", model.MADP);
            if (ModelState.IsValid)
            {
                try
                {
                    PHIM phimToUpdate = db.PHIMs.SingleOrDefault(p => p.MAPHIM == model.MAPHIM);
                    if (phimToUpdate == null) return HttpNotFound();

                    phimToUpdate.TENPHIM = model.TENPHIM;
                    phimToUpdate.MATHELOAI = model.MATHELOAI;
                    phimToUpdate.MADP = model.MADP;
                    phimToUpdate.NHASX = model.NHASX;
                    phimToUpdate.NGAYKHOICHIEU = model.NGAYKHOICHIEU;
                    phimToUpdate.NGAYKETTHUC = model.NGAYKETTHUC;
                    phimToUpdate.NGAYCAPNHAT = System.DateTime.Now;
                    if (HINH_ANH_Upload != null && HINH_ANH_Upload.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(HINH_ANH_Upload.FileName);
                        var path = Path.Combine(Server.MapPath("~/wwwroot/PhotoOfTheFilm/"), fileName);
                        if (!Directory.Exists(Server.MapPath("~/wwwroot/PhotoOfTheFilm/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/wwwroot/PhotoOfTheFilm/"));
                        }
                        HINH_ANH_Upload.SaveAs(path);
                        phimToUpdate.HINH_ANH = "~/wwwroot/PhotoOfTheFilm/" + fileName;
                    }
                    if (ThoiLuongInput.HasValue && ThoiLuongInput > 0)
                    {
                        if (ThoiLuongInput > 1439)
                        {
                            ModelState.AddModelError("ThoiLuongInput", "Thời lượng không được vượt quá 1439 phút (23:59).");
                            return View("EditPhim", model);
                        }
                        phimToUpdate.THOILUONG = TimeSpan.FromMinutes((double)ThoiLuongInput.Value);
                    }
                    else
                    {
                        phimToUpdate.THOILUONG = null;
                    }
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật CSDL: " + ex.Message);
                }
            }
            return View("EditPhim", model);
        }

        // GET: Manager/Movies/DeletePhim/5
        public ActionResult DeletePhim(string id)
        {
            PHIM phim = db.PHIMs.SingleOrDefault(p => p.MAPHIM == id);
            if (phim == null) return HttpNotFound();
            return View("DeletePhim", phim);
        }

        // POST: Manager/Movies/DeletePhim/5
        [HttpPost, ActionName("DeletePhim")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePhimConfirmed(string id)
        {
            try
            {
                PHIM phim = db.PHIMs.SingleOrDefault(p => p.MAPHIM == id);
                if (phim == null) return HttpNotFound();
                db.PHIMs.DeleteOnSubmit(phim);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LoadViewBagData();
                ViewBag.ErrorMessage = "Lỗi khi xóa CSDL: " + ex.Message;
                return View("Index");
            }
        }

        //=====================================================================
        // 2. CÁC ACTION QUẢN LÝ THỂ LOẠI
        //=====================================================================

        public ActionResult CreateTheLoai()
        {
            return View("CreateTheLoai", new THELOAIPHIM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTheLoai(THELOAIPHIM theloai)
        {
            if (db.THELOAIPHIMs.Any(t => t.MATHELOAI == theloai.MATHELOAI))
            {
                ModelState.AddModelError("MATHELOAI", "Mã Thể Loại '" + theloai.MATHELOAI + "' đã tồn tại. Vui lòng nhập mã khác.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.THELOAIPHIMs.InsertOnSubmit(theloai);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi CSDL: " + ex.Message);
                }
            }
            return View("CreateTheLoai", theloai);
        }

        public ActionResult EditTheLoai(string id)
        {
            THELOAIPHIM theloai = db.THELOAIPHIMs.SingleOrDefault(t => t.MATHELOAI == id);
            if (theloai == null)
            {
                return HttpNotFound();
            }
            return View("EditTheLoai", theloai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTheLoai(THELOAIPHIM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    THELOAIPHIM tlToUpdate = db.THELOAIPHIMs.SingleOrDefault(t => t.MATHELOAI == model.MATHELOAI);
                    if (tlToUpdate != null)
                    {
                        tlToUpdate.TENTHELOAI = model.TENTHELOAI;
                        db.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật CSDL: " + ex.Message);
                }
            }
            return View("EditTheLoai", model);
        }

        public ActionResult DeleteTheLoai(string id)
        {
            THELOAIPHIM theloai = db.THELOAIPHIMs.SingleOrDefault(t => t.MATHELOAI == id);
            if (theloai == null)
            {
                return HttpNotFound();
            }
            return View("DeleteTheLoai", theloai);
        }

        [HttpPost, ActionName("DeleteTheLoai")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTheLoaiConfirmed(string id)
        {
            try
            {
                THELOAIPHIM theloai = db.THELOAIPHIMs.SingleOrDefault(t => t.MATHELOAI == id);
                if (theloai != null)
                {
                    db.THELOAIPHIMs.DeleteOnSubmit(theloai);
                    db.SubmitChanges();
                }
                return RedirectToAction("Index");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                LoadViewBagData();
                if (ex.Number == 547)
                {
                    ViewBag.ErrorMessage = "KHÔNG THỂ XÓA: Thể loại này đang được sử dụng bởi một hoặc nhiều bộ phim. Bạn phải xóa các phim đó trước.";
                }
                else
                {
                    ViewBag.ErrorMessage = "Lỗi CSDL: " + ex.Message;
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                LoadViewBagData();
                ViewBag.ErrorMessage = "Lỗi: " + ex.Message;
                return View("Index");
            }
        }


        //=====================================================================
        //  3. CÁC ACTION QUẢN LÝ DẠNG PHIM
        //=====================================================================

        public ActionResult CreateDangPhim()
        {
            return View("CreateDangPhim", new DANGPHIM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDangPhim(DANGPHIM dangphim)
        {
            if (db.DANGPHIMs.Any(d => d.MADP == dangphim.MADP))
            {
                ModelState.AddModelError("MADP", "Mã Dạng Phim '" + dangphim.MADP + "' đã tồn tại. Vui lòng nhập mã khác.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.DANGPHIMs.InsertOnSubmit(dangphim);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi CSDL: " + ex.Message);
                }
            }
            return View("CreateDangPhim", dangphim);
        }

        public ActionResult EditDangPhim(string id)
        {
            DANGPHIM dangphim = db.DANGPHIMs.SingleOrDefault(d => d.MADP == id);
            if (dangphim == null)
            {
                return HttpNotFound();
            }
            return View("EditDangPhim", dangphim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDangPhim(DANGPHIM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DANGPHIM dpToUpdate = db.DANGPHIMs.SingleOrDefault(d => d.MADP == model.MADP);
                    if (dpToUpdate != null)
                    {
                        dpToUpdate.DANGPHIM1 = model.DANGPHIM1;
                        dpToUpdate.TrangThai = model.TrangThai;
                        db.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật CSDL: " + ex.Message);
                }
            }
            return View("EditDangPhim", model);
        }

        public ActionResult DeleteDangPhim(string id)
        {
            DANGPHIM dangphim = db.DANGPHIMs.SingleOrDefault(d => d.MADP == id);
            if (dangphim == null)
            {
                return HttpNotFound();
            }
            return View("DeleteDangPhim", dangphim);
        }

        [HttpPost, ActionName("DeleteDangPhim")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDangPhimConfirmed(string id)
        {
            DANGPHIM dangphim = db.DANGPHIMs.SingleOrDefault(d => d.MADP == id);
            if (dangphim != null)
            {
                db.DANGPHIMs.DeleteOnSubmit(dangphim);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // --- DỌN DẸP ---
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}