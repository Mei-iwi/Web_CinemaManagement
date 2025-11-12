using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_CinemaManagement.Helper
{
    public class Employee : User
    {
        public string MANV { get; set; }      // NV00000001

        public double Salary { get; set; }    // 15000000

        public string Catruc { get; set; }   // Ca sáng
        public string Position { get; set; }   // Quản lý

        public string MA_NQL { get; set; }    // NV00000002

        public Employee()
        {
        }

        public Employee(string name, string phone, string address, DateTime birthDate, string email, string image, string gender, string manv, double salary, string catruc, string position, string ma_nql)
         : base(name, phone, address, birthDate, email, image, gender)
        {
            MANV = manv;
            Salary = salary;
            Catruc = catruc;
            Position = position;
            MA_NQL = ma_nql;
        }
    }
}
