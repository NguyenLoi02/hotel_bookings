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
        public int RowNumber { get; set; }
    }
}