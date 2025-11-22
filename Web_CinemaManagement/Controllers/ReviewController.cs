using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Web_CinemaManagement.Models.API;
using Web_CinemaManagement.Models.ModelLinq;

namespace Web_CinemaManagement.Controllers
{
    public class ReviewController : Controller
    {
        CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

        public async Task<ActionResult> Index(string searchName)
        {
            // 1. Dropdown
            var listTenPhim = db.PHIMs.Select(p => p.TENPHIM).OrderBy(n => n).ToList();
            ViewBag.ListPhim = new SelectList(listTenPhim);

            // 2. Lấy danh sách phim
            List<PHIM> moviesToProcess = new List<PHIM>();

            if (!string.IsNullOrEmpty(searchName))
            {
                var movie = db.PHIMs.FirstOrDefault(p => p.TENPHIM == searchName);
                if (movie != null) moviesToProcess.Add(movie);
                ViewBag.TenPhimHienTai = searchName;
            }
            else
            {
                // Lấy hết phim để quét
                moviesToProcess = db.PHIMs.OrderByDescending(p => p.NGAYCAPNHAT).ToList();
                ViewBag.TenPhimHienTai = "Tất cả phim có đánh giá";
            }

            string apiKey = "e9e9d8da18ae29fc430845952232787c";
            var model = new List<PhimReviewViewModel>();

            using (HttpClient client = new HttpClient())
            {
                foreach (var phim in moviesToProcess)
                {
                    var phimVM = new PhimReviewViewModel
                    {
                        TenPhim = phim.TENPHIM,
                        HinhAnh = phim.HINH_ANH,
                        Reviews = new List<ReviewItem>()
                    };

                    try
                    {
                        string tmdbId = "";
                        string lowerName = phim.TENPHIM.ToLower();

                        // --- Fix cứng các phim tên sai ---
                        if (lowerName.Contains("minion")) tmdbId = "211672";
                        else if (lowerName.Contains("kungfu") || lowerName.Contains("kung fu")) tmdbId = "1011985";
                        else if (lowerName.Contains("avengers")) tmdbId = "299534";
                        else if (lowerName.Contains("annabelle")) tmdbId = "460465";

                        if (string.IsNullOrEmpty(tmdbId))
                        {
                            string queryName = HttpUtility.UrlEncode(phim.TENPHIM);
                            string searchUrl = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={queryName}&language=en-US";

                            HttpResponseMessage searchRes = await client.GetAsync(searchUrl);
                            if (searchRes.IsSuccessStatusCode)
                            {
                                string searchJson = await searchRes.Content.ReadAsStringAsync();
                                dynamic searchData = JsonConvert.DeserializeObject(searchJson);
                                if (searchData.results != null && searchData.results.Count > 0)
                                {
                                    var resultList = (IEnumerable<dynamic>)searchData.results;
                                    var bestMatch = resultList.OrderByDescending(x => (int)x.vote_count).FirstOrDefault();
                                    if (bestMatch != null) tmdbId = bestMatch.id;
                                }
                            }
                        }

                        // --- Lấy Review ---
                        if (!string.IsNullOrEmpty(tmdbId))
                        {
                            string urlViet = $"https://api.themoviedb.org/3/movie/{tmdbId}/reviews?api_key={apiKey}&language=vi-VN";
                            HttpResponseMessage responseVN = await client.GetAsync(urlViet);
                            if (responseVN.IsSuccessStatusCode)
                            {
                                var dataVN = JsonConvert.DeserializeObject<TMDBReviewResponse>(await responseVN.Content.ReadAsStringAsync());
                                if (dataVN?.results != null) phimVM.Reviews.AddRange(dataVN.results);
                            }

                            string urlEng = $"https://api.themoviedb.org/3/movie/{tmdbId}/reviews?api_key={apiKey}&language=en-US";
                            HttpResponseMessage responseEng = await client.GetAsync(urlEng);
                            if (responseEng.IsSuccessStatusCode)
                            {
                                var dataEng = JsonConvert.DeserializeObject<TMDBReviewResponse>(await responseEng.Content.ReadAsStringAsync());
                                if (dataEng?.results != null) phimVM.Reviews.AddRange(dataEng.results);
                            }

                            phimVM.Reviews = phimVM.Reviews.OrderByDescending(x => x.created_at).Take(10).ToList();
                        }
                    }
                    catch { }

                    // --- 3. ĐIỀU KIỆN HIỂN THỊ ---

                    if (!string.IsNullOrEmpty(searchName))
                    {
                        // Trường hợp 1: Người dùng chọn CỤ THỂ 1 phim
                        // -> Luôn hiện (để nếu rỗng thì báo "Chưa có review" cho người dùng biết)
                        model.Add(phimVM);
                    }
                    else
                    {
                        // Trường hợp 2: Người dùng chọn "XEM TẤT CẢ"
                        // -> Chỉ hiện phim nào CÓ REVIEW. Phim rỗng thì ẩn luôn cho đẹp.
                        if (phimVM.Reviews.Count > 0)
                        {
                            model.Add(phimVM);
                        }
                    }
                }
            }

            return View(model);
        }
    }
}
