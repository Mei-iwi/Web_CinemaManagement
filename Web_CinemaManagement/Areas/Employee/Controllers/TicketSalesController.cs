using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web_CinemaManagement.Models;
using Web_CinemaManagement.Models.ADO;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Areas.Employee.Controllers
{
    public class TicketSalesController : Controller
    {
        private CinemaManegementLinqDataContext db;

        public TicketSalesController()
        {
            // Lấy chuỗi kết nối
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
            db = new CinemaManegementLinqDataContext(conn);
        }

        // 1. TRANG CHỦ
        public ActionResult Index()
        {
            var listSuat = db.SUATCHIEUs
                             .Where(s => s.NGAYCHIEU >= DateTime.Today)
                             .OrderBy(s => s.NGAYCHIEU).ThenBy(s => s.GIOBATDAU)
                             .ToList();
            return View(listSuat);
        }

        // 2. TRANG BÁN VÉ
        [HttpGet]
        public ActionResult BanVe(string idSuat)
        {
            if (string.IsNullOrEmpty(idSuat)) return RedirectToAction("Index");

            var suat = db.SUATCHIEUs.SingleOrDefault(s => s.MASUAT == idSuat);
            if (suat == null) return HttpNotFound();

            var phim = suat.PHIM;
            var dangPhim = db.DANGPHIMs.FirstOrDefault(dp => dp.MADP == phim.MADP);
            ViewBag.TenDinhDang = dangPhim != null ? dangPhim.DANGPHIM1 : "2D";

            // A. Ghế đã bán
            var gheDaBan = db.VEs.Where(v => v.MASUAT == idSuat).Select(v => v.MAGHE.Trim()).ToList();

            // B. Ghế trống(Lấy từ bảng GHE gốc)
            var gheTrong = db.GHEs.Select(g => new { MaGhe = g.MAGHE.Trim() }).AsEnumerable()
                                  .Where(x => !gheDaBan.Contains(x.MaGhe))
                                  .OrderBy(x => x.MaGhe)
                                  .Select(x => new { x.MaGhe }).ToList();

            // C. Loại vé
            var listLoaiVe = db.LOAIVEs.Select(lv => new { lv.MALV, lv.TENLV, lv.DONGIA }).AsEnumerable()
                               .Select(lv => new { MaLV = lv.MALV, TenHienThi = lv.TENLV + " - " + ((decimal)lv.DONGIA).ToString("N0") + "đ" }).ToList();

            ViewBag.SuatChieu = suat;
            ViewBag.ListGheTrong = new SelectList(gheTrong, "MaGhe", "MaGhe");
            ViewBag.ListLoaiVe = new SelectList(listLoaiVe, "MaLV", "TenHienThi");

            return View(db.DICHVUs.ToList());
        }

        // 3. XỬ LÝ THANH TOÁN
        [HttpPost]
        public ActionResult DatVe(string idSuat, string strGhe, string strDichVu, string sdtKhach)
        {
            if (string.IsNullOrEmpty(strGhe))
            {
                TempData["Loi"] = "Bạn chưa chọn ghế nào!";
                return RedirectToAction("BanVe", new { idSuat = idSuat });
            }

            try
            {
                // ---XỬ LÝ KHÁCH HÀNG
                string maKhach = null;
                string tenKhach = "Khách vãng lai";
                decimal phanTramGiam = 0;
                string txtGiamGia = "0%";

                if (!string.IsNullOrEmpty(sdtKhach))
                {
                    sdtKhach = sdtKhach.Trim();
                    var kh = db.KHACHHANGs.FirstOrDefault(k => k.SDT == sdtKhach);
                    if (kh != null)
                    {
                        maKhach = kh.MAKH;
                        tenKhach = kh.HOTENKH;
                        var hang = db.HANGTHANHVIENs.FirstOrDefault(h => h.MAHANG == kh.MAHANG);
                        if (hang != null && !string.IsNullOrEmpty(hang.UUDAI))
                        {
                            string so = new String(hang.UUDAI.Where(Char.IsDigit).ToArray());
                            int num;
                            if (int.TryParse(so, out num))
                            {
                                phanTramGiam = (decimal)num / 100;
                                txtGiamGia = num + "% (" + hang.TENHANG + ")";
                            }
                        }
                    }
                }

                // ---XỬ LÝ LƯU VÉ
                decimal tongTien = 0;
                var mangGhe = strGhe.Split(';');
                List<string> dsTenGhe = new List<string>();
                List<string> dsTenDV = new List<string>();

                foreach (var item in mangGhe)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    var parts = item.Split('_');
                    if (parts.Length < 2) continue;

                    string maGhe = parts[0];
                    string maLV = parts[1];
                    dsTenGhe.Add(maGhe);

                    var lv = db.LOAIVEs.FirstOrDefault(x => x.MALV == maLV);
                    decimal giaGoc = lv != null ? (decimal)lv.DONGIA : 0;
                    decimal giaBan = giaGoc * (1 - phanTramGiam);

                    VE ve = new VE();
                    ve.MAVE = TaoMaVeTuDong();
                    ve.MASUAT = idSuat;
                    ve.MAGHE = maGhe;
                    ve.MALV = maLV;
                    ve.MAKH = maKhach;
                    ve.MANV = "NV00000001";
                    ve.NGAYBANVE = DateTime.Now;

                    db.VEs.InsertOnSubmit(ve);
                    db.SubmitChanges(); // Lưu vé

                    tongTien += giaBan;

                    // --- XỬ LÝ BẮP NƯỚC ---
                    if (item == mangGhe[0] && !string.IsNullOrEmpty(strDichVu))
                    {
                        var mangDV = strDichVu.Split(';');
                        foreach (var dvStr in mangDV)
                        {
                            if (string.IsNullOrEmpty(dvStr)) continue;
                            var p = dvStr.Split('_');
                            int sl = 0;
                            
                            if (p.Length >= 2 && int.TryParse(p[1], out sl) && sl > 0)
                            {
                                DANGKY dk = new DANGKY();
                                dk.MAVE = ve.MAVE;
                                dk.MASP = p[0];
                                dk.SOLUONG = sl;
                                db.GetTable<DANGKY>().InsertOnSubmit(dk);

                                var sp = db.DICHVUs.FirstOrDefault(s => s.MASP == p[0]);
                                tongTien += (decimal)sp.DONGIA * sl;
                                dsTenDV.Add(sp.TENSP + " (" + sl + ")");
                            }
                        }
                    }
                }
                db.SubmitChanges(); // Lưu bắp nước

                // --- TẠO HÓA ĐƠN ---
                var suatInfo = db.SUATCHIEUs.FirstOrDefault(s => s.MASUAT == idSuat);
                var bill = new HoaDonViewModel();
                bill.TenPhim = suatInfo.PHIM.TENPHIM;

                //  Xử lý giờ thủ công
                string gioChieu = suatInfo.GIOBATDAU.ToString();
                if (gioChieu.Length > 5) gioChieu = gioChieu.Substring(0, 5); // Lấy HH:mm

                bill.SuatChieu = gioChieu + " - " + string.Format("{0:dd/MM/yyyy}", suatInfo.NGAYCHIEU);
                bill.Phong = suatInfo.PHONGCHIEU.TENPHONG;
                bill.Ghe = string.Join(", ", dsTenGhe);
                bill.DichVu = dsTenDV.Count > 0 ? string.Join(", ", dsTenDV) : "Không";
                bill.TenKhach = tenKhach;
                bill.GiamGia = txtGiamGia;
                bill.TongTien = tongTien;
                bill.MaNhanVien = "NV00000001";

                TempData["Bill"] = bill;
                return RedirectToAction("BanVe", new { idSuat = idSuat });
            }
            catch (Exception ex)
            {
                TempData["Loi"] = "Lỗi xử lý: " + ex.Message;
                return RedirectToAction("BanVe", new { idSuat = idSuat });
            }
        }

        public string TaoMaVeTuDong()
        {
            try
            {
                var lastVe = db.VEs.OrderByDescending(x => x.MAVE).FirstOrDefault();
                if (lastVe == null) return "V00000001";
                // Nếu mã cũ lỗi -> Reset GUID
                if (!lastVe.MAVE.StartsWith("V") || lastVe.MAVE.Length > 10) return "V" + Guid.NewGuid().ToString().Substring(0, 8);

                string phanSo = lastVe.MAVE.Substring(1);
                long soMoi;
                if (long.TryParse(phanSo, out soMoi)) return "V" + (soMoi + 1).ToString("D8");
                else return "V" + Guid.NewGuid().ToString().Substring(0, 8);
            }
            catch { return "V" + Guid.NewGuid().ToString().Substring(0, 8); }
        }
    }
}