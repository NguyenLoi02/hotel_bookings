using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;


namespace hotel_bookings.Models
{

    public class BookingViewModel
    {

        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public int BookingOrderId { get; set; }
        public string RoomName { get; set; }
        public int RoomPrice { get; set; }
        public int RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; }
        public string ServiceName { get; set; }
    }
}