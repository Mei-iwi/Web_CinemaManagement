using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;
using Web_CinemaManagement.Models.ADO;

namespace Web_CinemaManagement.Areas.Employee.Controllers
{
    public class SellServicesController : Controller
    {
        // GET: Employee/SellServices
        public ActionResult Sell()
        {
            using (var db = new CinemaManegementLinqDataContext())
            {
                var modelVM = db.DICHVUs.Select(p => new ProductVM
                {
                    MASP = p.MASP,
                    TENSP = p.TENSP,
                    DONGIA = p.DONGIA,
                    HINH_ANH = p.HINH_ANH
                }).ToList();

                return View(modelVM);
            }

        }

        // ===============================================
        // 1. Thêm sản phẩm vào bảng DANGKY (hóa đơn tạm)
        // ===============================================
        [HttpPost]
        public JsonResult AddToDangKy(string maSP, string maVe, int soLuong)
        {
            try
            {
                if (string.IsNullOrEmpty(maSP) || soLuong <= 0)
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });

                var db = new CinemaManegementLinqDataContext();

                // Kiểm tra sản phẩm có tồn tại không
                var sp = db.DICHVUs.FirstOrDefault(d => d.MASP == maSP);
                if (sp == null)
                    return Json(new { success = false, message = "Sản phẩm không tồn tại!" });

                // Nếu muốn mỗi nhân viên chỉ có 1 hóa đơn tạm đang mở → kiểm tra session hoặc tạo hóa đơn chính thức
                // Ở đây mình làm đơn giản: cho phép thêm nhiều dòng vào bảng DANGKY (sau này sẽ gắn với MAHD)

                var dangKy = new DANGKY
                {
                    MASP = maSP,
                    MAVE = string.IsNullOrEmpty(maVe) ? null : maVe,
                    SOLUONG = soLuong,
                    // Các trường khác nếu cần
                    // NGAYLAP = DateTime.Now,
                    // MANV = Session["MaNV"].ToString(),
                };

                db.DANGKies.InsertOnSubmit(dangKy);
                db.SubmitChanges();

                // Tạo HTML row để trả về cho client (giống hệt JS đang append)
                decimal thanhTien = (sp.DONGIA) * soLuong;
                string rowHtml = $@"
            <tr data-key=""{maSP}|{maVe ?? ""}"" data-total=""{thanhTien}"">
                <td class=""masp""><strong>{maSP}</strong></td>
                <td>{HttpUtility.HtmlEncode(sp.TENSP)}</td>
                <td class=""mave"">{(string.IsNullOrEmpty(maVe) ? "-" : maVe)}</td>
                <td class=""qty text-center fw-bold"">{soLuong}</td>
                <td class=""total text-end text-danger fw-bold"">{thanhTien:N0} đ</td>
                <td>
                    <button type=""button"" class=""btn btn-danger btn-sm btnDelete"">
                        Xóa
                    </button>
                </td>
            </tr>";

                return Json(new
                {
                    success = true,
                    htmlRow = rowHtml,
                    message = "Đã thêm vào hóa đơn!"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // ===============================================
        // 2. Xóa sản phẩm khỏi bảng DANGKY
        // ===============================================
        [HttpPost]
        public JsonResult DeleteFromDangKy(string maSP, string maVe)
        {
            try
            {
                if (string.IsNullOrEmpty(maSP))
                    return Json(new { success = false, message = "Thiếu mã sản phẩm!" });

                var db = new CinemaManegementLinqDataContext();

                // Tìm dòng cần xóa (cho phép MAVE = null)
                var dangKy = db.DANGKies.FirstOrDefault(d =>
                    d.MASP == maSP &&
                    (d.MAVE == maVe || (string.IsNullOrEmpty(d.MAVE) && string.IsNullOrEmpty(maVe))));

                if (dangKy == null)
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm trong hóa đơn!" });

                db.DANGKies.DeleteOnSubmit(dangKy);
                db.SubmitChanges();

                return Json(new { success = true, message = "Đã xóa khỏi hóa đơn!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

    }
}