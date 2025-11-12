using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_CinemaManagement.Helper
{
    public abstract class User
    {

        public string FullName { get; set; }       // Lê Quang Hiển
        public string Phone { get; set; }          // 0901234567
        public string Address { get; set; }        // Hải Phòng
        public DateTime BirthDate { get; set; }    // 2000-01-01
        public string Email { get; set; }          // quanghien0101@gmail.com
        public string Avatar { get; set; }         // KH01.jpg
        public string Gender { get; set; }         // Nam

        // Constructor mặc định
        public User() { }

        // Constructor đầy đủ tham số
        public User(string fullName,  string phone, string address, DateTime birthDate, string email, string avatar, string gender)

        {
            FullName = fullName;
            Phone = phone;
            Address = address;
            BirthDate = birthDate;
            Email = email;
            Avatar = avatar;
            Gender = gender;
        }

    }
}
