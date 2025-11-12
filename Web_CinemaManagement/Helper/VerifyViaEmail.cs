using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;


namespace Web_CinemaManagement.Helper
{
    public class VerifyViaEmail
    {
        private string fromEmail = "cm6725098@gmail.com";
        private string fromPassword = "ouargxeuxckrzwcp"; // App Password

        // Hàm tạo mã OTP và gửi mail
        public string SendVerificationCode(string recipientEmail)
        {
            string code = GenerateVerificationCode();

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            message.To.Add(recipientEmail);
            message.Subject = "Password Reset Verification Code";
            message.Body = $"Your verification code is: {code}";
            message.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
            smtp.Send(message);

            return code; // trả về để lưu tạm
        }

        private string GenerateVerificationCode()
        {
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
        }

    }
}
