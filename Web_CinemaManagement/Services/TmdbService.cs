using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Web_CinemaManagement.Models.ADO;

namespace Web_CinemaManagement.Services
{
    public class TmdbService
    {

        private readonly string apiKey = "ea1f69716055c7535b1e5359b6e274c8";
        private readonly string baseUrl = "https://api.themoviedb.org/3";

        public async Task<MovieCredits> GetMovieCredits(int movieId)
        {
            string url = $"{baseUrl}/movie/{movieId}/credits?api_key={apiKey}";

            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                JObject json = JObject.Parse(response);

                var credits = new MovieCredits
                {
                    Cast = json["cast"].Select(c => new Cast
                    {
                        Id = (int)c["id"],
                        Name = (string)c["name"],
                        Character = (string)c["character"],
                        ProfilePath = (string)c["profile_path"]
                    }).ToList(),

                    Crew = json["crew"].Select(c => new Crew
                    {
                        Id = (int)c["id"],
                        Name = (string)c["name"],
                        Job = (string)c["job"]
                    }).ToList()
                };

                return credits;
            }
        }
    }
}