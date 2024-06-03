using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace hotel_bookings.Models.Service
{
    public interface IRoomService
    {
        IEnumerable<room> GetAllRooms();
        IEnumerable<room> SearchRooms(string a);
        IEnumerable<room> CheckRoom(DateTime check_in, DateTime check_out,int adult,int children, int? roomStyleId);
        IEnumerable<room> RoomFilter(int? room_style_id);

        void AddRoom(room room);
        room UpdateRoom(int id);
        void UpdateRoom(room room);
        void DeleteRoom(int id);
    }
}
