using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_CinemaManagement.Helper
{
    public class CheckKey
    {
        public int CheckPrimaryKey(string con, string sql, string key)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@key", key);
                        int result = (int)command.ExecuteScalar();
                        if (result > 0)
                        {
                            return 1; // Key exists
                        }
                        else
                        {
                            return 0; // Key does not exist
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return -1; // Error occurred
            }
        }

        public int CheckDoublePrimaryKey(string con, string key1, string key2, string sql)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@key1", key1);
                        command.Parameters.AddWithValue("@key2", key2);
                        int result = (int)command.ExecuteScalar();
                        if (result > 0)
                        {
                            return 1; // Key exists
                        }
                        else
                        {
                            return 0; // Key does not exist
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return -1; // Error occurred
            }
        }

        public int CheckForeignKey(string con, string sql, string key)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@key", key);
                        int result = (int)command.ExecuteScalar();
                        if (result > 0)
                        {
                            return 1; // Key exists
                        }
                        else
                        {
                            return 0; // Key does not exist
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return -1; // Error occurred
            }
        }
    }
}
