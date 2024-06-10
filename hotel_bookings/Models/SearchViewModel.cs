using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class SearchViewModel
    {

        public List<room> rooms { get; set; }
        public List<room_style> room_Styles { get; set; }

        public int room_id { get; set; }
        public string image { get; set; }
        public string room_name { get; set; }
        public int adult { get; set; }
        public int children { get; set; }
        public int price { get; set; }
        public int room_count { get; set; }
        public int status { get; set; }
        public string room_style { get; set; }
        public int room_style_id { get; set; }
        public int RowNumber { get; set; }
    }
}