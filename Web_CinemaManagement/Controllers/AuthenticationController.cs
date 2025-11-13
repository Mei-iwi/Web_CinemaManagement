using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Helper;

namespace Web_CinemaManagement.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly string secretKey = "6LcTNgssAAAAANc7iZ0lIiT3IASUfGxCzViqUkkB";
        // GET: Authentication
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Login(string UserName, string Password, string recaptchaToken)
        {

            var captcha = new VerifyCaptcha(secretKey);
            bool captchaValid = await captcha.VerifyCaptchaAsync(recaptchaToken);

            if (!captchaValid)
            {
                ModelState.AddModelError("", "Chúng tôi không thể xác nhận bạn không phải robot.");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}