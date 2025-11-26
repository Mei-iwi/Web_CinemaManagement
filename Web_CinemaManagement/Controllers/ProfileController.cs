using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;
using Web_CinemaManagement.Helper;
using System.IO;

namespace Web_CinemaManagement.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult ProfileUser()
        {
            CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

            int position = (int)Session["Position"];

            if (position == 0)
            {
                var user = Session["User"] as Customer;
                if (user == null) return HttpNotFound();

                // Lấy thông tin khách hàng
                KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(t => t.MAKH == user.MAKH);
                if (kh == null) return HttpNotFound();

                ViewBag.user = kh;
                ViewBag.position = position;

                int diemTL = kh.DIEMTICHLUY ?? 0; // tránh null

                // Lấy danh sách hạng theo điểm tăng dần
                var hangList = db.HANGTHANHVIENs.OrderBy(h => h.DIEMTOITHIEU).ToList();

                // Lấy hạng hiện tại
                HANGTHANHVIEN currentHang = hangList
                    .Where(h => (h.DIEMTOITHIEU ?? 0) <= diemTL)
                    .OrderByDescending(h => h.DIEMTOITHIEU)
                    .FirstOrDefault() ?? hangList.FirstOrDefault();

                ViewBag.DiemTichLuy = diemTL;
                ViewBag.CurrentHang = currentHang;

                // Lấy hạng tiếp theo để tính % vòng tròn
                int diemMax = hangList
                    .Where(h => (h.DIEMTOITHIEU ?? 0) > diemTL)
                    .Select(h => (int)h.DIEMTOITHIEU)
                    .DefaultIfEmpty(currentHang?.DIEMTOITHIEU ?? 1)
                    .First();

                double percent = diemMax > 0 ? ((double)diemTL / diemMax) * 100 : 0;
                ViewBag.Percent = percent;
                ViewBag.DiemMax = diemMax;

                ViewBag.hang = db.HANGTHANHVIENs.FirstOrDefault(t => t.MAHANG == user.Mahang).TENHANG;


                return View();
            }

            else if (position == 1 || position == 2)
            {
                var user = Session["User"] as Employee;

                NHANVIEN nv = db.NHANVIENs.FirstOrDefault(t => t.MANV == user.MANV);

                ViewBag.user = nv;

                ViewBag.position = position;

                return View();

            }
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "" });
            }

        }

        public ActionResult Update(string id)
        {

            CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

            int position = (int)Session["Position"];


            if (position == 0)
            {
                var user = Session["User"] as Customer;

                KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(t => t.MAKH == id);

                ViewBag.user = kh;

                ViewBag.position = position;

                return View();

            }
            else if (position == 1 || position == 2)
            {
                var user = Session["User"] as Employee;

                NHANVIEN nv = db.NHANVIENs.FirstOrDefault(t => t.MANV == id);

                ViewBag.user = nv;

                ViewBag.position = position;

                return View();

            }
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "" });
            }

        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

                int position = Convert.ToInt32(Session["Position"]);
                string userId = Session["UserID"] as string;

                // Lấy dữ liệu form
                string hoten = Request.Form["HOTEN"];
                string sdt = Request.Form["SDT"];
                string diachi = Request.Form["DIACHI"];
                string phai = Request.Form["PHAI"];
                string ngaysinhStr = Request.Form["NGAYSINH"];
                DateTime? ngaysinh = null;
                if (!string.IsNullOrEmpty(ngaysinhStr))
                {
                    ngaysinh = DateTime.Parse(ngaysinhStr);
                }

                string fileName = null;
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["HINH_ANH"];
                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/wwwroot/PhotoOfPerson/"), fileName);
                        file.SaveAs(path);
                    }
                }

                if (position == 0) // Khách hàng
                {
                    var kh = db.KHACHHANGs.FirstOrDefault(k => k.MAKH == userId);
                    if (kh == null) return Json(new { success = false, message = "Khách hàng không tồn tại" });

                    kh.HOTENKH = hoten;
                    kh.SDT = sdt;
                    kh.DIACHI = diachi;
                    kh.PHAI = phai;
                    if (ngaysinh.HasValue) kh.NGAYSINH = ngaysinh.Value;
                    if (fileName != null) kh.HINH_ANH = fileName;
                }
                else // Nhân viên
                {
                    var nv = db.NHANVIENs.FirstOrDefault(n => n.MANV == userId);
                    if (nv == null) return Json(new { success = false, message = "Nhân viên không tồn tại" });

                    nv.HOTENNV = hoten;
                    nv.SDT = sdt;
                    nv.DIACHI = diachi;
                    nv.PHAI = phai;
                    if (ngaysinh.HasValue) nv.NGAYSINH = ngaysinh.Value;
                    if (fileName != null) nv.HINH_ANH = fileName;
                }

                db.SubmitChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
