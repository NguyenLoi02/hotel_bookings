using Google.Apis.Gmail.v1.Data;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace hotel_bookings.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        private HotelBookingEntities db = new HotelBookingEntities();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(user u)
        {
            if (ModelState.IsValid)
            {
                var Account = db.users.Where(m => m.email.ToLower() == u.email.ToLower() && m.password == u.password).FirstOrDefault();
                if (Account != null)
                {
                    FormsAuthentication.SetAuthCookie(u.email, false);
                    Session["user"] = u.email.ToString();
                    if (Account != null)
                    {

                        return RedirectToAction("Index", "Home");
                        //return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Access");
                    }
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Register(user user)
        {

           
            db.users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login", "Access");
        }

    }
}