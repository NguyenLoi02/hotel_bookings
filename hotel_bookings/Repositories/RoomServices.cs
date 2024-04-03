using hotel_bookings.Models;
using hotel_bookings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Repositories
{
    public class RoomServices : IRoomServices
    {
        private readonly HotelBookingEntities _dbContext;

        public RoomServices(HotelBookingEntities dbContext)
        {
            _dbContext = dbContext;
        }

        IEnumerable<room> IRoomServices.GetAllRooms()
        {
            return _dbContext.rooms.ToList();
        }
    }
}