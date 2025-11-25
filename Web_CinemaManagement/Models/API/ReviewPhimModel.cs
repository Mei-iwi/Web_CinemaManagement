using System;
using System.Collections.Generic;

namespace Web_CinemaManagement.Models.API
{
    // Class map dữ liệu JSON từ API TMDB
    public class TMDBReviewResponse
    {
        public int id { get; set; }
        public int page { get; set; }
        public List<ReviewItem> results { get; set; }
    }

    public class ReviewItem
    {
        public string author { get; set; }
        public AuthorDetails author_details { get; set; }
        public string content { get; set; }
        public string created_at { get; set; }
        public string url { get; set; }
    }

    public class AuthorDetails
    {
        public string name { get; set; }
        public string username { get; set; }
        public string avatar_path { get; set; }
        public double? rating { get; set; }
    }

    // VIEW MODEL MỚI: Dùng để hiển thị ra View (Gồm Tên phim + Ảnh + List Review của nó)
    public class PhimReviewViewModel
    {
        public string TenPhim { get; set; }
        public string HinhAnh { get; set; }
        public List<ReviewItem> Reviews { get; set; }
    }
}
