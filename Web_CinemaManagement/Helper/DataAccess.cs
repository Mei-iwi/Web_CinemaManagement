using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_CinemaManagement.Helper
{
    public class DataAccess
    {
        public class DataProvider
        {
            public static bool TestConnection(string connectionString)
            {
                try
                {
                    using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                    {
                        connection.Open();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
