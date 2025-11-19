using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_CinemaManagement.Helper;
using Web_CinemaManagement.Models.ModelLinq;
using PagedList;
using Web_CinemaManagement.Models.Phim;
using System.Data.SqlClient;
using System.Configuration;
namespace Web_CinemaManagement.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QL_RAP_PHIMConnectionString"].ConnectionString;
        public ActionResult Dashboard(int? page)
        {
            int position = Session["Position"] != null ? (int)Session["Position"] : -1;

            CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

            List<PHIM> p = db.PHIMs.ToList();

            if (position == -1)
            {
                Session["UserID"] = "JustWatch";
                Session["Password"] = "Abc12345!";
                Session["Position"] = -1;
            }

            // Phân trang
            int pageNumber = page ?? 1;
            int pageSize = 8;
            var pageList = p.ToPagedList(pageNumber, pageSize);

         

            return View(pageList);
        }
        public ActionResult Detail(string id)
        {

            CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound("Mã phim không hợp lệ");
            }

            var phim = new PhimDetailViewModel();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string sqlQuery = "SELECT * FROM dbo.fn_GetChiTietPhim(@MaPhim)";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhim", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                             
                                phim.MaPhim = reader["MAPHIM"].ToString();
                                phim.TenPhim = reader["TENPHIM"].ToString();
                                phim.HinhAnh = reader["HINH_ANH"].ToString();

                              
                                phim.ThoiLuong = reader["THOILUONG"] != DBNull.Value ? (TimeSpan)reader["THOILUONG"] : TimeSpan.Zero;
                                phim.NgayKhoiChieu = reader["NGAYKHOICHIEU"] != DBNull.Value ? (DateTime)reader["NGAYKHOICHIEU"] : DateTime.Now;

                                phim.NhaSX = reader["NHASX"].ToString();
                                phim.TenTheLoai = reader["TENTHELOAI"].ToString();

                                
                                phim.NoiDung = reader["NOIDUNG"].ToString();
                                phim.DaoDien = reader["DAODIEN"].ToString();
                                phim.DienVien = reader["DIENVIEN"].ToString();
                                phim.QuocGia = reader["QUOCGIA"].ToString();
                                phim.GioiHanTuoi = reader["GIOIHAN_TUOI"].ToString();
                            }
                            else
                            {
                                return HttpNotFound("Không tìm thấy phim này trong Database");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Content("Lỗi kết nối: " + ex.Message);
                }
                // LẤY 3 PHIM KHÁC ĐỂ HIỂN THỊ BÊN PHẢI
                string sqlSidebar = @"SELECT TOP 3 MAPHIM, TENPHIM, HINH_ANH, GIOIHAN_TUOI 
                                      FROM PHIM 
                                      WHERE MAPHIM <> @CurrentId 
                                      ORDER BY NEWID()"; 

                using (SqlCommand cmd = new SqlCommand(sqlSidebar, conn))
                {
                    cmd.Parameters.AddWithValue("@CurrentId", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            phim.PhimDeCu.Add(new PhimDetailViewModel
                            {
                                MaPhim = reader["MAPHIM"].ToString(),
                                TenPhim = reader["TENPHIM"].ToString(),
                                HinhAnh = reader["HINH_ANH"].ToString(),
                                GioiHanTuoi = reader["GIOIHAN_TUOI"].ToString()
                            });
                        }
                    }
                }
            }
            return View(phim);
        }
    }
}