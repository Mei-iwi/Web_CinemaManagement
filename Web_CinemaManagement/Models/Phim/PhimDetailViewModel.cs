using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Models.Phim
{
    public class PhimDetailViewModel
    {
        public string MaPhim { get; set; }

        public string TenPhim { get; set; }

        public string HinhAnh { get; set; } 

        public TimeSpan ThoiLuong { get; set; }

        public DateTime NgayKhoiChieu { get; set; }

        public string NhaSX { get; set; }

        public string TenTheLoai { get; set; }
        public string NoiDung { get; set; }

        public string DaoDien { get; set; }

        public string DienVien { get; set; }

        public string QuocGia { get; set; }

        public string GioiHanTuoi { get; set; }
        public List<PhimDetailViewModel> PhimDeCu { get; set; } = new List<PhimDetailViewModel>();
    }
}