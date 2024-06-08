using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class SelectedData
    {
        public service Services { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}