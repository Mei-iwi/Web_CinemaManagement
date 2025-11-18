using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_CinemaManagement.Models.ADO;

namespace Web_CinemaManagement.Areas.Manager.Controllers
{
    [RoutePrefix("api/tickets")]
    public class API_TicketsManagementController : ApiController
    {

        [HttpGet]
        [Route("getTickets")]

        public IHttpActionResult getTickets()
        {
            getInfoTickets getinfo = new getInfoTickets();

            List<InfoTickets_Model> info = getinfo.getInfo();

            return Ok(info);
        }
        [HttpGet]
        [Route("getIn")]
        public IHttpActionResult getIn(int time)
        {
            getInfoTickets getinfo = new getInfoTickets();

            List<InfoTickets_Model> info;

            DateTime today = DateTime.Today;

            if (time == 1)
            {

                info = getinfo.getInfo().Where(t => t.ngaybanve.Date == today).ToList();

            }
            else if (time == 2)
            {
                info = getinfo.getInfo().Where(t => t.ngaybanve.Month == today.Month).ToList();
            }
            else if (time == 3)
            {
                info = getinfo.getInfo().Where(t => t.ngaybanve.Year == today.Year).ToList();
            }
            else
            {
                info = getinfo.getInfo();
            }
            return Ok(info);
        }


        [HttpGet]
        [Route("Search")]

        public IHttpActionResult Search(string id)
        {
            getInfoTickets getinfo = new getInfoTickets();

            List<InfoTickets_Model> info;

            if (string.IsNullOrEmpty(id))
            {
                info = getinfo.getInfo();

            }
            else
            {
                info = getinfo.getInfo().Where(t => t.mave.Contains(id) || t.masuat.Contains(id) || t.malv.Contains(id) || t.makh.Contains(id)).ToList();
            }
            return Ok(info);
        }


    }


}
