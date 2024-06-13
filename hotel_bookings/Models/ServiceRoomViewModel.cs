using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class ServiceRoomViewModel
    {
        public List<service> services { get; set; }
        public List<room> rooms { get; set; }
        public int roomPriceAll { get; set; }
        public service Service { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

    }
}