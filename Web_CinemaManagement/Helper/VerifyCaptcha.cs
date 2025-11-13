using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Web_CinemaManagement.Helper
{
    public class VerifyCaptcha
    {

        private readonly string secretKey;

        public VerifyCaptcha(string secretKey)
        {
            this.secretKey = secretKey;
        }

        public async Task<bool> VerifyCaptchaAsync(string token)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}", null);

                string json = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(json);

                Console.WriteLine(result);

                return result.success == true && result.score >= 0.5; // Kiểm tra score
            }
        }
    }
}