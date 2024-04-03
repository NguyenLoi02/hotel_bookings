using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_bookings.Services
{
    public interface IRoomServices
    {
        IEnumerable<room> GetAllRooms();
    }
}
