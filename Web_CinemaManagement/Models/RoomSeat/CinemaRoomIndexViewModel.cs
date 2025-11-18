using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Models.RoomSeat
{
    public class CinemaRoomIndexViewModel
    {
        public IEnumerable<PHONGCHIEU> PhongChieus { get; set; }
        public IEnumerable<GHE> Ghes { get; set; }
    }
}