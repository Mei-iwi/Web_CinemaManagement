using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Models.ADO
{
    public class TicketModel
    {
        public string MASUAT { get; set; }      // Mã suất chiếu
        public string MAGHE { get; set; }       // Mã ghế

        public string MAPHONG { get; set; }

        public string LOAIVE { get; set; }
    }
}
