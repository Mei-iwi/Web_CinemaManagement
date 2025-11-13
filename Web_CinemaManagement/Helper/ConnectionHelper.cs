using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Web_CinemaManagement.Helper
{
    public static class ConnectionHelper
    {

        private static string DataSource = "34.133.93.201";

        private static string InitialCatalog = "QL_RAP_PHIM";


        public static string getConnectionString(string UserID, string Password)
        {
            return $"Data Source={DataSource};Initial Catalog={InitialCatalog};User ID={UserID};Password={Password}";
        }

        public static string getEFConnectionString(string UserID, string Password)
        {
            string providerConn = $"Data Source={DataSource};" +
                                  $"Initial Catalog={InitialCatalog};" +
                                  $"User ID={UserID};" +
                                  $"Password={Password};" +
                                  $"MultipleActiveResultSets=True;" +
                                  $"TrustServerCertificate=True;" +
                                  $"Application Name=EntityFramework";

            // Chuỗi EF metadata (giữ nguyên tên model của bạn)
            string efConn = $"metadata=res://*/Models.ModelEF.CinemaManagementEF.csdl|" +
                            $"res://*/Models.ModelEF.CinemaManagementEF.ssdl|" +
                            $"res://*/Models.ModelEF.CinemaManagementEF.msl;" +
                            $"provider=System.Data.SqlClient;" +
                            $"provider connection string=\"{providerConn}\"";

            return efConn;
        }

        public static string getLinqConnectionString(string UserID, string Password)
        {
            return $"Data Source={DataSource};" +
                  $"Initial Catalog={InitialCatalog};" +
                  $"User ID={UserID};" +
                  $"Password={Password};" +
                  $"MultipleActiveResultSets=True;" +  // chú ý: không dấu cách
                  $"TrustServerCertificate=True;" +
                  $"Application Name=EntityFramework";
        }

        public static string getConnectionStringEFAdmin()
        {
            return ConfigurationManager.ConnectionStrings["QL_RAP_PHIMEntities"].ConnectionString;
        }

        public static string getConnectionStringLinqAdmin()
        {
            return ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
        }
    }
}