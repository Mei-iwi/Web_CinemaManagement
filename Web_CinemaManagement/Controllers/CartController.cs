using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web_CinemaManagement.Models.Cart; 
using Web_CinemaManagement.Models.ModelLinq; 

public class CartController : Controller
{ 
    CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();
    public ActionResult DichVu()
    {
        var model = db.DICHVUs.ToList();
        return View(model);
    }
    private Cart GetCart()
    {
        Cart cart = Session["Cart"] as Cart;
        if (cart == null)
        {
            cart = new Cart();
            Session["Cart"] = cart;
        }
        return cart;
    }
    public ActionResult AddToCart(string id)
    {
        Cart cart = GetCart();
        cart.Them(id);

        return RedirectToAction("DichVu");
    }

    public ActionResult RemoveFromCart(string id)
    {
        Cart cart = GetCart();
        cart.Xoa(id);

        return RedirectToAction("DichVu");
    }

    [ChildActionOnly]
    public ActionResult CartPartial()
    {
        Cart cart = GetCart();
        ViewBag.TongSoLuong = cart.TongSoluongSP();
        ViewBag.TongTien = cart.TongTien();

        return PartialView(cart.cart); 
    }

}