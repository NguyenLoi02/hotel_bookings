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

        public ActionResult Index()
        {
            var RoomStyleAndNew = new RoomStyleAndNewModel
            {
                room_styles = db.room_style.ToList(),
                news = db.news.ToList(),
            };
            return View(RoomStyleAndNew);
        }
    }
}