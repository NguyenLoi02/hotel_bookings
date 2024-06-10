using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class RoomFeatureViewModel
    {
        public List<room> rooms { get; set; }
        public List<feature> features { get; set; }
    }
}