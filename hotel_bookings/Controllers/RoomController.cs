
using hotel_bookings.Models;
using hotel_bookings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomServices _roomServices;
        private HotelBookingEntities db = new HotelBookingEntities();

        public RoomController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }

        // GET: Room
        public ActionResult Index()
        {
            var room_list = _roomServices.GetAllRooms();
            return View(room_list);
        }
        public ActionResult RoomDetail(int id)
        {
            if (id != null)
            {
                var detail = db.rooms.Find(id);
                return View(detail);
            }
            return RedirectToAction("Index");
        }
    }
}