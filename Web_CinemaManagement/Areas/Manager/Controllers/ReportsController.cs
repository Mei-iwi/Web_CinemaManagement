using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web_CinemaManagement.Models.ModelLinq;
using Web_CinemaManagement.Models.Report;
using ClosedXML.Excel;
using Rotativa;
using Microsoft.AspNetCore.Mvc;

namespace Web_CinemaManagement.Areas.Manager.Controllers
{
    public class ReportsController : Controller
    {
        private readonly CinemaManegementLinqDataContext db = new CinemaManegementLinqDataContext();

        // =============================
        // INDEX
        // =============================
        [HttpGet]
        public ActionResult Index(DateTime? from, DateTime? to, string mode = "day")
        {
            if (!from.HasValue || !to.HasValue)
            {
                var today = DateTime.Today;
                from = new DateTime(today.Year, today.Month, 1);
                to = today;
            }

            var model = BuildReport(from.Value, to.Value, mode);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ReportViewModel input)
        {
            if (!input.FromDate.HasValue || !input.ToDate.HasValue)
            {
                ModelState.AddModelError("", "Vui lòng chọn thời gian thống kê.");
                return View(new ReportViewModel());
            }

            var model = BuildReport(input.FromDate.Value, input.ToDate.Value, input.Mode);
            return View(model);
        }

        // =============================
        // BUILD REPORT
        // =============================
        private ReportViewModel BuildReport(DateTime from, DateTime to, string mode)
        {
            var tickets = db.VEs
                .Where(v => v.NGAYBANVE >= from && v.NGAYBANVE <= to)
                .ToList();

            var ticketIds = tickets.Select(v => v.MAVE).ToList();

            var services = db.DANGKies
                .Where(d => ticketIds.Contains(d.MAVE))
                .ToList();

            int totalTickets = tickets.Count();
            decimal ticketRevenue = tickets.Sum(v => (decimal?)v.LOAIVE.DONGIA) ?? 0;

            int totalServiceItems = services.Sum(s => s.SOLUONG ?? 0);
            decimal serviceRevenue = services.Sum(s => (decimal?)((s.SOLUONG ?? 0) * s.DICHVU.DONGIA)) ?? 0;

            decimal totalRevenue = ticketRevenue + serviceRevenue;

            List<ReportItem> items = new List<ReportItem>();

            switch (mode.ToLower())
            {
                case "month":
                    items = tickets
                        .GroupBy(v => new { v.NGAYBANVE.Year, v.NGAYBANVE.Month })
                        .Select(g => new ReportItem
                        {
                            Label = $"{g.Key.Month:00}/{g.Key.Year}",
                            Revenue = g.Sum(x => (decimal?)x.LOAIVE.DONGIA) ?? 0
                        })
                        .ToList();
                    break;

                case "movie":
                    items = tickets
                        .GroupBy(v => v.SUATCHIEU.PHIM.TENPHIM)
                        .Select(g => new ReportItem
                        {
                            Label = g.Key,
                            Revenue = g.Sum(x => (decimal?)x.LOAIVE.DONGIA) ?? 0
                        })
                        .ToList();
                    break;

                case "employee":
                    items = tickets
                        .GroupBy(v => v.NHANVIEN.HOTENNV)
                        .Select(g => new ReportItem
                        {
                            Label = g.Key,
                            Revenue = g.Sum(x => (decimal?)x.LOAIVE.DONGIA) ?? 0
                        })
                        .ToList();
                    break;

                default:
                    items = tickets
                        .GroupBy(v => v.NGAYBANVE.Date)
                        .Select(g => new ReportItem
                        {
                            Label = g.Key.ToString("dd/MM/yyyy"),
                            Revenue = g.Sum(x => (decimal?)x.LOAIVE.DONGIA) ?? 0
                        })
                        .ToList();
                    break;
            }

            return new ReportViewModel
            {
                FromDate = from,
                ToDate = to,
                Mode = mode,

                TotalTickets = totalTickets,
                TotalServiceItems = totalServiceItems,
                TicketRevenue = ticketRevenue,
                ServiceRevenue = serviceRevenue,
                TotalRevenue = totalRevenue,

                Items = items
            };
        }

        // =============================
        // EXPORT EXCEL
        // =============================
        public ActionResult ExportExcel(DateTime from, DateTime to, string mode = "day")
        {
            var model = BuildReport(from, to, mode);

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("BaoCao");

                ws.Cell(1, 1).Value = "BÁO CÁO DOANH THU";

                ws.Cell(3, 1).Value = "Từ ngày";
                ws.Cell(3, 2).Value = model.FromDate?.ToString("dd/MM/yyyy");

                ws.Cell(4, 1).Value = "Đến ngày";
                ws.Cell(4, 2).Value = model.ToDate?.ToString("dd/MM/yyyy");

                // Tổng quan
                ws.Cell(6, 1).Value = "Tổng vé bán";
                ws.Cell(6, 2).Value = model.TotalTickets;

                ws.Cell(7, 1).Value = "Doanh thu vé";
                ws.Cell(7, 2).Value = model.TicketRevenue;

                ws.Cell(8, 1).Value = "Doanh thu dịch vụ";
                ws.Cell(8, 2).Value = model.ServiceRevenue;

                ws.Cell(9, 1).Value = "Tổng doanh thu";
                ws.Cell(9, 2).Value = model.TotalRevenue;

                // Border tổng quan
                ws.Range("A6:B9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A6:B9").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Header chi tiết
                ws.Cell(11, 1).Value = "Nhãn";
                ws.Cell(11, 2).Value = "Doanh thu";

                ws.Range("A11:B11").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A11:B11").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                int row = 12;
                foreach (var it in model.Items)
                {
                    ws.Cell(row, 1).Value = it.Label;
                    ws.Cell(row, 2).Value = it.Revenue;

                    ws.Range($"A{row}:B{row}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range($"A{row}:B{row}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    row++;
                }

                using (var ms = new System.IO.MemoryStream())
                {
                    wb.SaveAs(ms);
                    ms.Position = 0;

                    string fileName = $"BaoCao_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                    return File(
                        ms.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName
                    );
                }
            }
        }

        // =============================
        // EXPORT PDF
        // =============================
        public ActionResult ExportPDF(DateTime from, DateTime to, string mode = "day")
        {
            var model = BuildReport(from, to, mode);

            return new ViewAsPdf("PdfReport", model)
            {
                FileName = $"BaoCao_{DateTime.Now:yyyyMMddHHmmss}.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }
    }
}