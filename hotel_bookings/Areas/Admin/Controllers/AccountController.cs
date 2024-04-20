using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Areas.Admin.Controllers
{
    //[Authorize]

    public class AccountController : Controller
    {
        // GET: Admin/Account
        private HotelBookingEntities db = new HotelBookingEntities();
        [Authorize(Roles = "admin")]

        public ActionResult Index()
        {
            var account = db.admins.ToList();
            int count = 1;
            foreach (var item in account)
            {
                item.RowNumber = count;
                count++;
            }
            return View(account);
        }
    }
}