using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Areas.Admin.Data
{
    public class OrderRoomService : IOrderRoomService
    {
        private readonly HotelBookingEntities _dbContext;

        public OrderRoomService(HotelBookingEntities dbContext)
        {
            _dbContext = dbContext;
        }

       
    }
    
}