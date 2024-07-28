using System;

namespace ExpressServiceWeb.Models
{
    public class DailyStatisticsViewModel
    {
        public DateTime Date { get; set; }
        public decimal DailyRevenue { get; set; }
        public int DailyOrders { get; set; }
    }
}
