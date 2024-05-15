using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        private HotelBookingEntities db = new HotelBookingEntities();

        public ActionResult Index()
        {
            var detail = db.news.ToList();

            return View(detail);
        }
        public ActionResult NewsDetail(int id)
        {
            var contents = db.contents.Where(x=>x.news_id == id).ToList();
            return View(contents);
        }
    }
}