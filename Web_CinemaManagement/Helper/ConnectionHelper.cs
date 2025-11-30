using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;

namespace Web_CinemaManagement.Helper
{
    public static class ConnectionHelper
    {
        // 1. Đã đổi tên Server về localhost
        private static string DataSource = "localhost";

        private static string InitialCatalog = "QL_RAP_PHIM";

        public static string getConnectionString(string UserID, string Password)
        {
            // 2. Đã sửa sang chế độ Windows Authentication (Integrated Security=True)
            // Bỏ qua UserID và Password truyền vào để tránh lỗi đăng nhập
            return $"Data Source={DataSource};Initial Catalog={InitialCatalog};Integrated Security=True;TrustServerCertificate=True";
        }

        public static string getEFConnectionString(string UserID, string Password)
        {
            // Cấu hình lại chuỗi kết nối cho Entity Framework dùng Windows Auth
            string providerConn = $"Data Source={DataSource};" +
                           $"Initial Catalog={InitialCatalog};" +
                           $"Integrated Security=True;" + // Dùng quyền Windows
                           $"MultipleActiveResultSets=True;" +
                           $"TrustServerCertificate=True;" +
                           $"Application Name=EntityFramework";

            // Dùng tên assembly Library là DatabaseEF
            string efConn = $"metadata=res://DatabaseEF/Models.ModelEF.CinemaManagementEF.csdl|" +
                            $"res://DatabaseEF/Models.ModelEF.CinemaManagementEF.ssdl|" +
                            $"res://DatabaseEF/Models.ModelEF.CinemaManagementEF.msl;" +
                            "provider=System.Data.SqlClient;" +
                            $"provider connection string=\"{providerConn}\"";

            return efConn;
        }

        public static string getLinqConnectionString(string UserID, string Password)
        {
            // Cấu hình lại cho LINQ dùng Windows Auth
            return $"Data Source={DataSource};" +
                  $"Initial Catalog={InitialCatalog};" +
                  $"Integrated Security=True;" + // Dùng quyền Windows
                  $"MultipleActiveResultSets=True;" +
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