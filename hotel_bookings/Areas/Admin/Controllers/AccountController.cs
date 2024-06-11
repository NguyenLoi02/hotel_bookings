using hotel_bookings.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

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
        [HttpGet]
        public ActionResult AddAccount()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult AddAccount(admin admin)
        {
            db.admins.Add(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteAccount(int? id)
        {
            var admin = db.admins.Find(id);
            if (admin != null)
            {
                db.admins.Remove(admin);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        
    }
}