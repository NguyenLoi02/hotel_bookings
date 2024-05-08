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
        [HttpPost]
        public ActionResult Index(string name, string email, string message)
        {
            user_question question = new user_question();
            question.name = name;
            question.email = email;
            question.message = message;
            db.user_question.Add(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}