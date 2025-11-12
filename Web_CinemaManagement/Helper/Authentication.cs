using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Web_CinemaManagement.Helper
{
    public class Authentication
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public int UserType { get; private set; }
        public Authentication() { }
        public Authentication(string username, string password, string str)
        {
            this.Username = username;
            this.Password = password;

            this.UserType = authenticate(str, username);
        }

        public int authenticate(string str, string user)
        {
            int userType = -1; // -1 = không tồn tại

            using (SqlConnection con = new SqlConnection(str))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT dbo.FN_AUTH_USER_TYPE(@USERID)", con))
                    {
                        cmd.Parameters.AddWithValue("@USERID", user);

                        int result = (int)cmd.ExecuteScalar();
                        if (result == 0 || result == 1 || result == 2)
                        {
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
            return userType;
        }

        public Employee getInfomation(string str, string user)
        {
            Employee emp = new Employee();

            using (SqlConnection conn = new SqlConnection(str))
            {
                string sql = "SELECT * FROM NHANVIEN WHERE MANV = @MANV";

                SqlCommand sqlCommand = new SqlCommand(sql, conn);

                sqlCommand.Parameters.AddWithValue("@MANV", user);

                conn.Open();

                SqlDataReader reader = sqlCommand.ExecuteReader();


                while (reader.Read())
                {
                    emp.MANV = reader["MANV"].ToString();
                    emp.FullName = reader["HOTENNV"].ToString();
                    emp.Phone = reader["SDT"].ToString();
                    emp.Address = reader["DIACHI"].ToString();
                    emp.Gender = reader["PHAI"].ToString();
                    emp.Salary = Convert.ToDouble(reader["LUONG"]);
                    emp.Catruc = reader["CATRUC"].ToString();
                    emp.Email = reader["EMAIL"].ToString();
                    emp.BirthDate = Convert.ToDateTime(reader["NGAYSINH"]);
                    emp.Position = reader["CHUCVU"].ToString();
                    emp.Avatar = reader["HINH_ANH"].ToString();
                    emp.MA_NQL = reader["MA_NQL"].ToString();
                }
            }
            return emp;


        }
        public Customer getCustomerInfomation(string str, string user)
        {
            Customer cust = new Customer();
            using (SqlConnection conn = new SqlConnection(str))
            {
                string sql = "SELECT * FROM KHACHHANG WHERE MAKH = @MAKH";
                SqlCommand sqlCommand = new SqlCommand(sql, conn);
                sqlCommand.Parameters.AddWithValue("@MAKH", user);
                conn.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    cust.MAKH = reader["MAKH"].ToString();
                    cust.Mahang = reader["MAHANG"].ToString();
                    cust.FullName = reader["HOTENKH"].ToString();
                    cust.Phone = reader["SDT"].ToString();
                    cust.Address = reader["DIACHI"].ToString();
                    cust.BirthDate = Convert.ToDateTime(reader["NGAYSINH"]);
                    cust.Email = reader["EMAIL"].ToString();
                    cust.Avatar = reader["HINH_ANH"].ToString();
                    cust.Gender = reader["PHAI"].ToString();

                }
            }
            return cust;
        }
    }
}