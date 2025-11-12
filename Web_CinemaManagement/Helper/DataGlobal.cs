using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Helper
{
    public static class DataGlobal
    {
        public static string UserID;

        public static string Password;

        public static int Position;

        public static void getInformationUser(string user, string password, int position)
        {
            UserID = user;
            Password = password;
            Position = position;
        }

    }
}