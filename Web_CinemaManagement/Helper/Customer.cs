using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_CinemaManagement.Helper
{
    public class Customer : User
    {

        public string MAKH { get; set; }      // KH00000001S
        public string Mahang { get; set; }    // H00000001

        public Customer()
        {
        }

        public Customer(string name, string phone, string address, DateTime birthDate, string email, string image, string gender, string makh, string mahang)
            : base(name, phone, address, birthDate, email, image, gender)
        {
            this.MAKH = makh;
            this.Mahang = mahang;
        }
    }
}
