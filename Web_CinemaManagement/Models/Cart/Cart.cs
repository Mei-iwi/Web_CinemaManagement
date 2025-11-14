using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Models.Cart
{
    public class Cart
    {
        public List<CartItem> cart;
        public Cart()
        {
            cart = new List<CartItem>();
        }

        public Cart(List<CartItem> list)
        {
            cart = list;
        }

        public int SoLuongSP()
        {
            return cart.Count;
        }

        public int TongSoluongSP()
        {
            return cart.Sum(x => x.SoLuong);
        }
        public decimal TongTien()
        {
            return cart.Sum(x => x.ThanhTien);
        }

        public int Them(string id)
        {
            CartItem item = cart.Find(x => x.MaSP == id);
            if (item != null)
            {
                item.SoLuong++;
            }
            else
            {
                CartItem newItem = new CartItem(id);
                cart.Add(newItem);
            }
            return 1;
        }

        public int Xoa(string id)
        {
            CartItem item = cart.Find(x => x.MaSP == id);
            try { 
                if (item != null)
                {
                    cart.Remove(item);
                }
                else {return -1; }
            }
            catch (Exception)
            {
                return -1;
            }
            return 1;
        }
    }
}