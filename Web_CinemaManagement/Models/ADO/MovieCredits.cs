using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Models.ADO
{
    public class MovieCredits
    {
        public List<Cast> Cast { get; set; }
        public List<Crew> Crew { get; set; }
    }
}