using Google;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Controllers
{
    public class RoomStyleController : Controller
    {
        // GET: RoomStyle
        private HotelBookingEntities db = new HotelBookingEntities();

        public ActionResult Index(int id)
        {
            var room_styles = db.room_style.FirstOrDefault(x => x.id == id);
            var rooms = db.rooms.Where(x => x.room_style_id == id).ToList();
            var room_images = db.room_images.Where(x => x.room_id == id).ToList();

            var RoomStyleAndNew = new RoomStyleAndRoomModel
            {
                room_styles = room_styles != null ? new List<room_style> { room_styles } : new List<room_style>(),
                rooms = rooms ?? new List<room>(),
                room_img = room_images ?? new List<room_images>()

            };
            return View(RoomStyleAndNew);
        }
    }
}