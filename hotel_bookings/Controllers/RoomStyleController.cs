using Google;
using hotel_bookings.Models;
using hotel_bookings.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Controllers
{
    public class RoomStyleController : Controller
    {
        private readonly IRoomService _roomServices;
        private readonly IVnPayService _vnPayService;
        private HotelBookingEntities db = new HotelBookingEntities();

        public RoomStyleController(IRoomService roomServices, IVnPayService vnPayService)
        {
            _roomServices = roomServices;
            _vnPayService = vnPayService;
        }

        // GET: RoomStyle

        public ActionResult Index(int id)
        {
            var room_styles = db.room_style.FirstOrDefault(x => x.id == id);
            var rooms = db.rooms.Where(x => x.room_style_id == id).ToList();
            var room_images = db.room_images.Where(x => x.room_id == id).ToList();
            Session["id"] = id;
            var RoomStyleAndNew = new RoomStyleAndRoomModel
            {
                room_styles = room_styles != null ? new List<room_style> { room_styles } : new List<room_style>(),
                rooms = rooms ?? new List<room>(),
                room_img = room_images ?? new List<room_images>()

            };
            return View(RoomStyleAndNew);
        }
        [HttpPost]
        public ActionResult search(int? page, DateTime check_in, DateTime check_out, int adult = 1, int children = 1)
        {
            var roomId = (int)Session["id"];
            var availableRooms = _roomServices.CheckRoom(check_in, check_out, adult, children, roomId);
            System.Web.HttpContext.Current.Session.Timeout = 30;
            TimeSpan difference = check_out - check_in;
            double totalDays = difference.TotalDays;
            Session["day"] = totalDays;
            Session["check_in"] = check_in.Date;
            Session["check_out"] = check_out.Date;
            ViewBag.CheckIn = check_in;
            ViewBag.CheckIn = check_out;
            Session["availableRooms"] = availableRooms;


            return View(availableRooms);
        }
    }
}