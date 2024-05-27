using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class StatisticsViewModel
    {
        public List<GenderStat> GenderStats { get; set; }
        public List<list_room_style> list_room_styles { get; set; }
        public List<total> totals { get; set; }
    }
    public class GenderStat
    {
        public int? Gender { get; set; }
        public int Count { get; set; }
    }

    public class list_room_style
    {
        public string name { get; set; }
        public int Count { get; set; }
    }
    public class total
    {
        public string name { get; set; }
        public int Price { get; set; }
    }
}