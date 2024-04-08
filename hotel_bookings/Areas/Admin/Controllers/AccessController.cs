using hotel_bookings.Controllers;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Areas.Admin.Controllers
{
    public class AccessController : Controller
    {
        // GET: Admin/Login
        private HotelBookingEntities db = new HotelBookingEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var Account = db.admins.SingleOrDefault(m => m.username.ToLower() == username.ToLower() && m.password == password);
            if (Account != null)
            {
                Session["user"] = Account;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();

            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Access");
        }
    }
}