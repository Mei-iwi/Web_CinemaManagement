using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Models.ADO
{
    public class HoaDonViewModel
    {
        public string TenPhim { get; set; }
        public string SuatChieu { get; set; }
        public string Phong { get; set; }
        public string Ghe { get; set; }     
        public string DichVu { get; set; }     // Ví dụ: Bắp x2
        public string TenKhach { get; set; }
        public string GiamGia { get; set; }    // Ví dụ: 20%
        public decimal TongTien { get; set; }
        public string MaNhanVien { get; set; }
        public DateTime NgayDat { get; set; }
    }
}