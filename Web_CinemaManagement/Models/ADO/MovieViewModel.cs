using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Models.ADO
{
    public class MovieViewModel
    {
        public string MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string HinhAnh { get; set; }
        public int ThoiLuong { get; set; }
        public List<ShowtimeDetailViewModel> AvailableShowtimes { get; set; }
    }
}