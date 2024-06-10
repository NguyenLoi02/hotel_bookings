using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class RoomSaleViewModel
    {
        public List<room> rooms { get; set; }
        public List<room_sale> room_sales { get; set; }
        public List<sale> sales { get; set; }

        public int room_id { get; set; }
        public int sale_id { get; set; }
        public string room_name { get; set; }
        public string sale_name { get; set; }
        public string start_day { get; set; }
        public string end_day { get; set; }
        public int RowNumber { get; set; }

    }
}