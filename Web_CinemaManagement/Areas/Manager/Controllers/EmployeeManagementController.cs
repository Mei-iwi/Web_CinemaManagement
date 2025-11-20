using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;
using System.Net;
using PagedList;

namespace Web_CinemaManagement.Areas.Manager.Controllers
{
    public class EmployeeManagementController : Controller
    {
        CinemaManegementLinqDataContext db;

        public EmployeeManagementController()
        {
            // Kết nối DB
            db = new CinemaManegementLinqDataContext();
        }

        // GET: Manager/EmployeeManagement
        public ActionResult Index(string keyword, int page = 1)
        {
            // Kiểm tra phân quyền
            var position = (int?)Session["Position"];
            if (Session["User"] == null || position == null || position == -1 || position == 0 || position == 1)
            {
                return RedirectToAction("Login", "Authentication", new { area = "" });
            }

            int pageSize = 10;

            var employeesQuery = db.NHANVIENs.AsQueryable();
            // Search
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                employeesQuery = employeesQuery.Where(nv =>
                    nv.HOTENNV.ToLower().Contains(keyword) ||
                    nv.SDT.Contains(keyword) ||
                    nv.EMAIL.ToLower().Contains(keyword)
                );
                // Lưu từ khóa hiện tại để hiển thị lại trên ô input
                ViewBag.CurrentKeyword = keyword;
            }

            // paging
            int totalEmployeeCount = employeesQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalEmployeeCount / pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var pagedEmployees = employeesQuery
                                    .OrderBy(nv => nv.MANV)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.TotalEmployeeCount = totalEmployeeCount;

            return View(pagedEmployees);
        }

        public ActionResult AddEmployee()
        {
            ViewBag.DanhSachQuanLy = db.NHANVIENs
                                        .Select(nv => new { nv.MANV, nv.HOTENNV })
                                        .ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee(NHANVIEN nhanvien)
        {
            var lastEmployee = db.NHANVIENs
                                 .OrderByDescending(nv => nv.MANV)
                                 .FirstOrDefault();

            if (lastEmployee != null)
            {
                long lastNumber = long.Parse(lastEmployee.MANV.Substring(2)); // Bỏ "NV"
                long nextNumber = lastNumber + 1;
                nhanvien.MANV = "NV" + nextNumber.ToString("D8"); // D8 => 8 chữ số
            }
            else
            {
                nhanvien.MANV = "NV00000001";
            }

            if (ModelState.IsValid)
            {
                db.NHANVIENs.InsertOnSubmit(nhanvien);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            // Nếu lỗi, load lại dữ liệu cho form
            ViewBag.DanhSachQuanLy = db.NHANVIENs.Select(nv => new { nv.MANV, nv.HOTENNV }).ToList();
            return View("AddEmployee", nhanvien);
        }

        public ActionResult DetailEmployee(string id)
        {
            // Kiểm tra id có hợp lệ không
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var nhanvien = db.NHANVIENs.FirstOrDefault(nv => nv.MANV == id);

            if (nhanvien == null)
                return HttpNotFound();

            // Lấy tên người quản lý (nếu có) để hiển thị
            ViewBag.TenQuanLy = db.NHANVIENs
                                  .Where(nv => nv.MANV == nhanvien.MA_NQL)
                                  .Select(nv => nv.HOTENNV)
                                  .FirstOrDefault();

            return View(nhanvien);
        }

        public ActionResult UpdateEmployee(string id)
        {
            var nhanvien = db.NHANVIENs.FirstOrDefault(nv => nv.MANV == id);
            if (nhanvien == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách nhân viên để chọn Người quản lý (loại trừ chính nhân viên đang sửa)
            var listQuanLy = db.NHANVIENs
                               .Where(nv => nv.MANV != id)
                               .ToList();

            // Danh sách quản lý (Select List)
            ViewBag.ListQuanLy = new SelectList(listQuanLy, "MANV", "HOTENNV", nhanvien.MA_NQL);

            return View(nhanvien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEmployee(NHANVIEN nv)
        {
            var nhanvienCanSua = db.NHANVIENs.FirstOrDefault(n => n.MANV == nv.MANV);

            if (nhanvienCanSua != null)
            {
                // Cập nhật các trường
                nhanvienCanSua.HOTENNV = nv.HOTENNV;
                nhanvienCanSua.SDT = nv.SDT;
                nhanvienCanSua.DIACHI = nv.DIACHI;
                nhanvienCanSua.NGAYSINH = nv.NGAYSINH;
                nhanvienCanSua.EMAIL = nv.EMAIL;
                nhanvienCanSua.PHAI = nv.PHAI;
                nhanvienCanSua.CHUCVU = nv.CHUCVU;
                nhanvienCanSua.LUONG = nv.LUONG;
                nhanvienCanSua.MA_NQL = nv.MA_NQL;
                nhanvienCanSua.LUONG = nv.LUONG;
                nhanvienCanSua.HINH_ANH = nv.HINH_ANH;

                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteEmployee(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var nhanvien = db.NHANVIENs.SingleOrDefault(nv => nv.MANV == id);
            if (nhanvien == null)
            {
                return HttpNotFound();
            }

            // Lấy tên người quản lý (nếu có) để hiển thị trên trang xác nhận xóa
            ViewBag.TenQuanLy = db.NHANVIENs
                                  .Where(nv => nv.MANV == nhanvien.MA_NQL)
                                  .Select(nv => nv.HOTENNV)
                                  .FirstOrDefault();

            return View(nhanvien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmployeeCommit(string MANV)
        {
            // Cần đảm bảo không có ràng buộc khóa ngoại (ví dụ: vé, CSKH) trỏ đến nhân viên này. 
            // Nếu có, bạn cần xử lý (xóa các bản ghi liên quan, hoặc báo lỗi).

            var nhanvien = db.NHANVIENs.SingleOrDefault(nv => nv.MANV == MANV);

            if (nhanvien != null)
            {
                db.NHANVIENs.DeleteOnSubmit(nhanvien);
                db.SubmitChanges();
            }

            return RedirectToAction("Index");
        }
    }
}