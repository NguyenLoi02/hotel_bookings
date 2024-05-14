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
            var users = db.users.Where(m => m.email == check).FirstOrDefault();
            var id = users.id ;
            var user = db.users.Find(id);
            return View(user);
        }
    }
}