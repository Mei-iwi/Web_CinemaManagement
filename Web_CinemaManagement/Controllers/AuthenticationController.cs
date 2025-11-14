using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Helper;
using Web_CinemaManagement.Models.ModelLinq;
using System.Data.SqlClient;
using System.Data;

namespace Web_CinemaManagement.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly string secretKey = "6LcTNgssAAAAANc7iZ0lIiT3IASUfGxCzViqUkkB";

        private string ConnecionString = "";

        private int position = 0;

        private string code = "";
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

            ConnecionString = ConnectionHelper.getConnectionString(UserName, Password);

            bool testConnection = DataAccess.DataProvider.TestConnection(ConnecionString);

            if (testConnection)
            {

                Session["User"] = null;


                Authentication authentication = new Authentication();

                position = authentication.authenticate(ConnecionString, UserName);

                Session["UserID"] = UserName;

                Session["Password"] = Password;

                Session["Position"] = position;

                if (position == 0)
                {

                    Customer cus = new Customer();

                    cus = authentication.getCustomerInfomation(ConnecionString, UserName);

                    Session["User"] = cus;


                }
                else
                {
                    Employee em = new Employee();

                    em = authentication.getInfomation(ConnecionString, UserName);


                    if (position == 1)
                    {
                        Session["User"] = em;

                        return RedirectToAction("DashBoard", "Management");
                    }
                    else if (position == 2)
                    {
                        Session["User"] = em;

                        return RedirectToAction("DashBoard", "Employee");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể xác thực được tài khoảng đăng nhập");
                        return View();
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");

                Session["UserID"] = "JustWatch";

                Session["Password"] = "Abc12345!";

                Session["Position"] = -1;



                return View();
            }

        }

        public ActionResult Logout()
        {
            Session["User"] = null;

            Session["UserID"] = "JustWatch";

            Session["Password"] = "Abc12345!";

            Session["Position"] = -1;


            return RedirectToAction("Index", "Home");
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        public JsonResult getCode(string username, string email)
        {

            string str = Helper.ConnectionHelper.getConnectionString("sqlserver", "123456789");

            bool checkUser = DataAccess.DataProvider.TestConnection(str);

            if (checkUser)
            {


                Authentication auth = new Authentication();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
                {
                    return Json(new { sucess = false, message = "Không xác định được người dùng" }, JsonRequestBehavior.AllowGet);
                }

                if (username.Substring(0, 2) == "NV")
                {
                    Employee em = auth.getInfomation(str, username);
                    if (em != null)
                    {
                        if (em.Email != email)
                        {
                            return Json(new { sucess = false, message = "Không xác định được người dùng" }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                else
                {
                    Customer cus = auth.getCustomerInfomation(str, username);
                    if (cus != null)
                    {
                        if (cus.Email != email)
                        {
                            return Json(new { sucess = false, message = "Không xác định được người dùng" }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }

            }
            else
            {
                return Json(new { sucess = false, message = "Kết nối cơ sở dữ liệu thất bại" }, JsonRequestBehavior.AllowGet);
            }

            VerifyViaEmail verifier = new VerifyViaEmail();

            code = verifier.SendVerificationCode(email);

            return Json(new { success = true, code = code }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ChangePass(string User, string NewPass)
        {
            try
            {
                string str = Helper.ConnectionHelper.getConnectionString("sqlserver", "123456789");

                UpdatePassword update = new UpdatePassword();
                int kq = update.changePassword(str, User, NewPass);
                if (kq > 0)
                {
                    ViewBag.mess = "Cập nhật mật khẩu thành công!";
                }
                else
                {
                    ViewBag.err = "Cập nhật mật khẩu thất bại!, mật khẩu phải tối thiểu 8 kí tự và có chứa kí tự đặc biệt";
                }
            }
            catch (Exception ex)
            {
                ViewBag.err = "Lỗi khi cập nhật mật khẩu!";
            }
            return View("ForgetPassword");
        }
        public ActionResult Registration(string currentPage, string currentController)
        {

            ViewBag.currentPage = (currentPage != null) ? currentPage : "/Authentication/Login";

            ViewBag.currentController = (currentController != null) ? currentController : "Authentication";

            return View();
        }
    }
}