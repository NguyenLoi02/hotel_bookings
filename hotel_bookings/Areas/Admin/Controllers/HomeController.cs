using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Areas.Admin.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        private HotelBookingEntities db = new HotelBookingEntities();
        public ActionResult Index()
        {

            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Access");
            }
            return View();

        }

        public ActionResult CheckQuyen()
        {


            return View();

        }
    }
}