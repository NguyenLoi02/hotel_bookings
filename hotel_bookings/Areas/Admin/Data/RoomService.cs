using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace hotel_bookings.Areas.Admin.Data
{
    public class RoomService : IRoomService
    {
        private List<room> _rooms;
        private readonly HotelBookingEntities _dbContext;

        public RoomService(HotelBookingEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<room> GetAllRooms()
        {
            return _dbContext.rooms.ToList();
        }
        
        public void AddRoom(room room)
        {
            _dbContext.rooms.Add(room);
            _dbContext.SaveChanges();
        }
        public room UpdateRoom(int id)
        {
            if (id != null)
            {
                return _dbContext.rooms.Find(id);
            }
            return null;
        }
        public void UpdateRoom(room room)
        {
            _dbContext.Entry(room).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public void DeleteRoom(int id)
        {
            var roomToDelete = _dbContext.rooms.Find(id);
            if (roomToDelete != null)
            {
                _dbContext.rooms.Remove(roomToDelete);
                _dbContext.SaveChanges();
            }
        }

    }
}