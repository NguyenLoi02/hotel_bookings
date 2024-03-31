using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_bookings.Areas.Admin.Service
{
    public interface IRoomService
    {
        IEnumerable<room> GetAllRooms();
        void AddRoom(room room);
        room UpdateRoom(int id);
        void UpdateRoom(room room);
        void DeleteRoom(int id);
    }
}
