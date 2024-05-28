using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace hotel_bookings.Controllers
{
    public class HomeController : Controller
    {
        private HotelBookingEntities db = new HotelBookingEntities();

        public ActionResult Index()
        {
            var  room_styles = db.room_style.ToList();
            var  news = db.news.ToList();

            var RoomStyleAndNew = new RoomStyleAndNewModel
            {
                room_styles = room_styles ?? new List<room_style>(),
                news = news ?? new List<news>()

            };
            return View(RoomStyleAndNew);
        }


    }
}   