using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_CinemaManagement.Models.ModelLinq;
using System.Configuration;

namespace Web_CinemaManagement.Controllers
{
    [RoutePrefix("api/seats")]
    public class APISeatController : ApiController
    {
        string connnectinostring;

        CinemaManegementLinqDataContext db;

        public APISeatController()
        {
            connnectinostring = ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
            db = new CinemaManegementLinqDataContext(connnectinostring);
        }

        [HttpGet]
        [Route("getseats")]

        public IHttpActionResult getSeats(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "PC00000001";
            }

            var seats = db.CT_GHE_PHONGs.Where(t => t.MAPHONG == id).Select(
                g => new
                {
                    g.MAGHE,
                    g.TRANGTHAI
                }
                ).ToList();


            return Ok(seats);
        }

        [HttpGet]
        [Route("getTime")]
        public IHttpActionResult getTime(string id = "P00000001")
        {

            var times = db.SUATCHIEUs.Where(t => t.MAPHIM == id).Select(
                t => new
                {
                    t.MASUAT,
                    t.PHONGCHIEU.TENPHONG,
                    t.MAPHONG,
                    t.GIOBATDAU,
                    t.GIOKETTHUC,
                    t.NGAYCHIEU,
                }).ToList();

            return Ok(times);
        }

        [HttpGet]
        [Route("Quatity")]
        public IHttpActionResult getQuatity()
        {
            var lv = db.LOAIVEs.Select(t => new
            {
                t.MALV,
                t.TENLV
            }).ToList();
            return Ok(lv);
        }

    }

}



