using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

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

        public static string getEFConnectionString(string user, string pass)
        {
            string providerConn = $"Data Source={DataSource};" +
                                  $"Initial Catalog={InitialCatalog};" +
                                  $"User ID={user};Password={pass};" +
                                  $"MultipleActiveResultSets=True;" +
                                  $"TrustServerCertificate=True;" +
                                  $"Application Name=EntityFramework";

            string efConn = $"metadata=res://DatabaseEF/Models.ModelEF.CinemaManagementEF.csdl|" +
                               $"res://DatabaseEF/Models.ModelEF.CinemaManagementEF.ssdl|" +
                               $"res://DatabaseEF/Models.ModelEF.CinemaManagementEF.msl;" +
                               $"provider=System.Data.SqlClient;" +
                               $"provider connection string=\"{providerConn}\"";
            return efConn;
        }

        public static string GetSessionEFConnection()
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.Session["UserID"] != null &&
                HttpContext.Current.Session["Password"] != null)
            {
                string user = HttpContext.Current.Session["UserID"].ToString();
                string pass = HttpContext.Current.Session["Password"].ToString();
                return getEFConnectionString(user, pass);
            }
            else
            {
                // default
                return getEFConnectionString("JustWatch", "Abc12345!");
            }
        }

        public static string getLinqConnectionString(string UserID, string Password)
        {
            return $"Data Source={DataSource};" +
                  $"Initial Catalog={InitialCatalog};" +
                  $"User ID={UserID};" +
                  $"Password={Password};" +
                  $"MultipleActiveResultSets=True;" +
                  $"TrustServerCertificate=True;" +
                  $"Application Name=EntityFramework";
        }


    }
}