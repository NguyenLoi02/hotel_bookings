using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Areas.Admin.Controllers
{
    public class OrderRoomController : Controller
    {
        private HotelBookingEntities db = new HotelBookingEntities();
        private readonly IOrderRoomService _orderRoomService;

        public OrderRoomController(IOrderRoomService roomService)
        {
            _orderRoomService = roomService;
        }
        // GET: Admin/OrderRoom
        public ActionResult booking()
        {
            var booking_list = _orderRoomService.GetAllBooking();
            return View(booking_list.ToList());
        }
        public ActionResult booked()
        {
            var booked_list = _orderRoomService.GetAllBooked();
            return View(booked_list.ToList());
        }
    }
}