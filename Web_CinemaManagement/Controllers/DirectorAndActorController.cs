using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ADO;
using Web_CinemaManagement.Services;

namespace Web_CinemaManagement.Controllers
{
    public class DirectorAndActorController : Controller
    {
        // GET: DirectorAndActor
        private readonly TmdbService _tmdb;

        public DirectorAndActorController()
        {
            _tmdb = new TmdbService();
        }

        public async Task<ActionResult> Credits(int id)
        {
            MovieCredits credits = await _tmdb.GetMovieCredits(id);

            return View(credits);
        }
    }
}