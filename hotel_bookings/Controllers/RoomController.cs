
using hotel_bookings.Models;
using hotel_bookings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace hotel_bookings.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomServices _roomServices;
        private HotelBookingEntities db = new HotelBookingEntities();

        public RoomController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }

        // GET: Room
        public ActionResult Index()
        {
            var room_list = _roomServices.GetAllRooms();
            return View(room_list);
        }
        [HttpPost]
        public ActionResult Index(DateTime check_in, DateTime check_out)
        {
            System.Web.HttpContext.Current.Session.Timeout = 30;
            TimeSpan difference = check_out - check_in;
            double totalDays = difference.TotalDays;
            Session["day"] = totalDays;
            Session["check_in"] = check_in.Date;
            Session["check_out"] = check_out.Date;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult RoomDetail(int id)
        {

            if (id == null || Session["check_in"] == null || Session["check_out"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                DateTime check_in = (DateTime)Session["check_in"];
                ViewBag.CheckIn = check_in;
                DateTime check_out = (DateTime)Session["check_out"];
                ViewBag.CheckOut = check_out;
                double days = (double)Session["day"];
                ViewBag.Day = days;
                var detail = db.rooms.Find(id);
                Session["room_id"] = id;
                Session["room_name"] = detail.name;
                Session["room_price"] = detail.price;
                Session["room_adult"] = detail.adult;
                Session["room_children"] = detail.children;           
                return View(detail);
            }

        }
        public ActionResult RoomService()
        {
            var service = db.services.ToList();
            if (Session["check_in"] == null || Session["check_out"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DateTime check_in = (DateTime)Session["check_in"];
            ViewBag.CheckIn = check_in;
            DateTime check_out = (DateTime)Session["check_out"];
            ViewBag.CheckOut = check_out;
            double days = (double)Session["day"];
            ViewBag.Day = days;
            string room_name = (string)Session["room_name"];
            ViewBag.room_name = room_name;
            int room_price = (int)Session["room_price"];
            ViewBag.room_price = room_price;
            int room_adult = (int)Session["room_adult"];
            ViewBag.room_adult = room_adult;
            int room_children = (int)Session["room_children"];
            ViewBag.room_children = room_children;

            return View(service);
        }
        public ActionResult RoomOrder()
        {
            if (Session["check_in"] == null || Session["check_out"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DateTime check_in = (DateTime)Session["check_in"];
            ViewBag.CheckIn = check_in;
            DateTime check_out = (DateTime)Session["check_out"];
            ViewBag.CheckOut = check_out;
            double days = (double)Session["day"];
            ViewBag.Day = days;
            string room_name = (string)Session["room_name"];
            ViewBag.room_name = room_name;
            int room_price = (int)Session["room_price"];
            ViewBag.room_price = room_price;
            int room_adult = (int)Session["room_adult"];
            ViewBag.room_adult = room_adult;
            int room_children = (int)Session["room_children"];
            ViewBag.room_children = room_children;
            return View();
        }
        [HttpPost]
        public ActionResult RoomOrder(user user)
        {
            DateTime check_in = (DateTime)Session["check_in"];
            DateTime check_out = (DateTime)Session["check_out"];
            double days = (double)Session["day"];
            int room_price = (int)Session["room_price"];
            var room_id = Session["room_id"];

            db.users.Add(user);
            db.SaveChanges();
            var user_id = user.id;

            booking_order bookingOrder = new booking_order();
            bookingOrder.user_id = user_id;
            bookingOrder.trans_money = (int)(days*room_price);
            db.booking_order.Add(bookingOrder);
            db.SaveChanges();

            
            var booking_order_id = bookingOrder.id;
            booking_details booking_details = new booking_details();
            booking_details.booking_order_id = booking_order_id;
            booking_details.room_id = (int)room_id;
            booking_details.check_in = check_in;
            booking_details.check_out = check_out;
            db.booking_details.Add(booking_details);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
