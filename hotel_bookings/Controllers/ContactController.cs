using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        private HotelBookingEntities db = new HotelBookingEntities();

        public ActionResult Index()
        {
            var detail = db.contact_details.ToList();

            return View(detail);
        }
    }
}