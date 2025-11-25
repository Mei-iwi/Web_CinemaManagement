using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Helper
{
    public static class GetDataSource_Auto
    {
        public static string DetectServer()
        {
            DataTable servers = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();

            foreach (DataRow row in servers.Rows)
            {
                string serverName = row["ServerName"].ToString();
                string instanceName = row["InstanceName"].ToString();

                string fullServerName = string.IsNullOrEmpty(instanceName)
                    ? serverName
                    : serverName + "\\" + instanceName;

                return fullServerName;
            }

            return @"(localdb)\MSSQLLocalDB";
        }
    }
}