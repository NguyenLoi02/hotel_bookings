using hotel_bookings.Controllers;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Login(admin u)
        {
            if (ModelState.IsValid)
            {
                var Account = db.admins.Where(m => m.username.ToLower() == u.username.ToLower() && m.password == u.password).FirstOrDefault();
                if (Account != null)
                {
                    FormsAuthentication.SetAuthCookie(u.username, false);
                    Session["admin"] = u.username.ToString();
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
            return RedirectToAction("Login", "Access");
        }
    }
}