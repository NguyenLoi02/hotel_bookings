using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        private HotelBookingEntities db = new HotelBookingEntities();

        public ActionResult Index()
        {
            var check = (string)Session["user"];
            var user = db.users.Where(m => m.email == check).ToList();
            return View(user);
        }
    }
}