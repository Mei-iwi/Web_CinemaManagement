using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_CinemaManagement.Models.Report
{
    public class ReportViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Mode { get; set; }

        public int TotalTickets { get; set; }
        public int TotalServiceItems { get; set; }
        public decimal TicketRevenue { get; set; }
        public decimal ServiceRevenue { get; set; }
        public decimal TotalRevenue { get; set; }

        public List<ReportItem> Items { get; set; }
    }

    public class ReportItem
    {
        public string Label { get; set; }
        public decimal Revenue { get; set; }
    }
}
