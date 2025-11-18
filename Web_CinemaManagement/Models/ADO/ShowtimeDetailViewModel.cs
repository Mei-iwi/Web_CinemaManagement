using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Models.ADO
{
    public class ShowtimeDetailViewModel
    {
        public string MaSuat { get; set; }
        public string TenPhong { get; set; }
        public TimeSpan GioBatDau { get; set; }
    }
}