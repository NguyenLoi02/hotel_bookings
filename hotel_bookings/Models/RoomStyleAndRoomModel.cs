using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class RoomStyleAndRoomModel
    {
        public List<room_style> room_styles { get; set; }
        public List<room> rooms { get; set; }
        public List<room_images> room_img { get; set; }
    }
}