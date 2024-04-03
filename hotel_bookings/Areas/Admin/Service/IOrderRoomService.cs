using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace hotel_bookings.Areas.Admin.Service
{
    public interface IOrderRoomService
    {
        IEnumerable<BookingViewModel> GetAllBooking();
        IEnumerable<BookingViewModel> GetAllBooked();
    }
}
