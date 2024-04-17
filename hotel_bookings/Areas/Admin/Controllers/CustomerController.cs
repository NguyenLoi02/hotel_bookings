using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Admin/Customer
        private HotelBookingEntities db = new HotelBookingEntities();

        public ActionResult Index()
        {
            var customer = db.users.ToList();
            int count = 1;
            foreach (var item in customer)
            {
                item.RowNumber = count;
                count++;
            }
            return View(customer);
        }
    }
}