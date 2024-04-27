﻿
using hotel_bookings.Models;
using hotel_bookings.Models.Service;
using Microsoft.Ajax.Utilities;
using Ninject.Planning.Targets;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using WebGrease.Css.Extensions;

namespace hotel_bookings.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomServices;
        private HotelBookingEntities db = new HotelBookingEntities();

        public RoomController(IRoomService roomServices)
        {
            _roomServices = roomServices;
        }

        // GET: Room
        public ActionResult Index(int? page)
        {
            // Số lượng mục trên mỗi trang
            int pageSize = 6;
            var roomStyle = db.room_style.ToList();
            ViewBag.roomStyleList = roomStyle;
            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            var room_list = _roomServices.GetAllRooms();
            return View(room_list.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult filter(int? page,int filterRoom)
        {
            // Số lượng mục trên mỗi trang
            int pageSize = 6;
            var roomStyle = db.room_style.ToList();

            ViewBag.roomStyleList = roomStyle;

            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            var bookedRoomIds = db.room_style
             .Where(b => b.id == filterRoom)
             .Select(b => b.id)
             .ToList();

            var availableRooms = db.rooms
                .Where(r => bookedRoomIds.Contains(r.room_style_id))
                .ToList();
            return View(availableRooms.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult search(int? page,DateTime check_in, DateTime check_out)
        {
          
            var availableRooms = _roomServices.CheckRoom(check_in, check_out);


            // Số lượng mục trên mỗi trang
            int pageSize = 6;
            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            System.Web.HttpContext.Current.Session.Timeout = 30;
            TimeSpan difference = check_out - check_in;
            double totalDays = difference.TotalDays;
            Session["day"] = totalDays;
            Session["check_in"] = check_in.Date;
            Session["check_out"] = check_out.Date;
            ViewBag.CheckIn = check_in;
            ViewBag.CheckIn = check_out;

            return View(availableRooms.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult RoomFilter(int? page,int? room_style_id)
        {

            var availableRooms = _roomServices.RoomFilter(room_style_id);


            // Số lượng mục trên mỗi trang
            int pageSize = 6;
            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            System.Web.HttpContext.Current.Session.Timeout = 30;
           

            return View(availableRooms.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        //public ActionResult RoomDetail(int id)
        //{

        //    if (id == null || Session["check_in"] == null || Session["check_out"] == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        DateTime check_in = (DateTime)Session["check_in"];
        //        ViewBag.CheckIn = check_in;
        //        DateTime check_out = (DateTime)Session["check_out"];
        //        ViewBag.CheckOut = check_out;
        //        double days = (double)Session["day"];
        //        ViewBag.Day = days;
        //        var detail = db.rooms.Find(id);
        //        Session["room_id"] = id;
        //        Session["room_name"] = detail.name;
        //        Session["room_price"] = detail.price;
        //        Session["room_adult"] = detail.adult;
        //        Session["room_children"] = detail.children;           
        //        return View(detail);
        //    }

        //}
        public ActionResult RoomDetails(int id)
        {
            var detail = db.rooms.Find(id);
            return View(detail);
        }
        public ActionResult RoomService(int id)
        {
            var service = db.services.ToList();
            if (Session["check_in"] == null || Session["check_out"] == null)
            {
                return RedirectToAction("Index");
            }
            var detail = db.rooms.Find(id);
            Session["room_id"] = id;
            Session["room_name"] = detail.name;
            Session["room_price"] = detail.price;
            Session["room_adult"] = detail.adult;
            Session["room_children"] = detail.children;
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
                return RedirectToAction("Index");
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
            var room_id = Session["room_id"] as int?;
            db.users.Add(user);
            db.SaveChanges();
            var user_id = user.id;
            string roomName = db.rooms.Where(r => r.id == room_id).Select(r => r.name).FirstOrDefault();
            string firstName = db.users.Where(r => r.id == user_id).Select(r => r.first_name).FirstOrDefault();

            DateTime currentDate = DateTime.Now;
            var trans_money = room_price * days;
            booking_order bookingOrder = new booking_order();
            bookingOrder.user_id = user_id;
            bookingOrder.booking_status = 0;
            bookingOrder.book_day = currentDate;
            bookingOrder.trans_money = (int)trans_money;
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

            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Common/send2.html"));
            contentCustomer = contentCustomer.Replace("{{MaBooking}}", Convert.ToString(booking_order_id));
            contentCustomer = contentCustomer.Replace("{{TenPhong}}", roomName);
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", firstName);
            hotel_bookings.Common.Common.SendMail("hotelL", "Đơn Hàng #" + Convert.ToString(booking_order_id), contentCustomer, user.email);
            return RedirectToAction("Index");
        }
    }
}
