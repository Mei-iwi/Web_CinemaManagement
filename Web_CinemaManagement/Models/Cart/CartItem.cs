using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Models.Cart
{
    public class CartItem
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public string HinhAnh { get; set; }
        public decimal ThanhTien
        {
            get { return SoLuong * DonGia; }
        }

        CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();
        public CartItem(string id)
        {

            DICHVU dv = db.DICHVUs.SingleOrDefault(x => x.MASP == id);
            MaSP = dv.MASP.ToString();
            TenSP = dv.TENSP;
            HinhAnh = dv.HINH_ANH;
            DonGia = Convert.ToDecimal(dv.DONGIA);
            SoLuong = 1;
        }

    }
}