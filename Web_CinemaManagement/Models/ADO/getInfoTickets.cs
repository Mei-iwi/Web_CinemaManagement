using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_CinemaManagement.Helper;
using System.Data;
using System.Data.SqlClient;

namespace Web_CinemaManagement.Models.ADO
{
    public class getInfoTickets
    {
        string str = Helper.ConnectionHelper.getConnectionString("sqlserver", "123456789");

        public List<InfoTickets_Model> getInfo()
        {
            List<InfoTickets_Model> ls = new List<InfoTickets_Model>();
            using (SqlConnection con = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM VE", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.Text;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        InfoTickets_Model info = new InfoTickets_Model();

                        info.mave = reader["MAVE"].ToString();
                        info.masuat = reader["MASUAT"].ToString();
                        info.malv = reader["MALV"].ToString();
                        info.makh = reader["MAKH"].ToString();
                        info.manv = reader["MANV"].ToString();
                        info.maghe = reader["MAGHE"].ToString();
                        info.ngaybanve = DateTime.Parse(reader["NGAYBANVE"].ToString());

                        ls.Add(info);
                    }

                }
                return ls;
            }
        }
    }
}