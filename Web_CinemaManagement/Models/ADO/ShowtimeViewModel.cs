using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Models.ADO
{
    public class ShowtimeViewModel
    {
        public DateTime Date { get; set; }
        public List<MovieViewModel> MovieGroups { get; set; }

    }
}