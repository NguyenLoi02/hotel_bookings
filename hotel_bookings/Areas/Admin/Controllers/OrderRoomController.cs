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

        // GET: Admin/OrderRoom
        public ActionResult booking()
        {
            var booking_list = from rooms in db.rooms
                        join booking_details in db.booking_details on rooms.id equals booking_details.room_id
                        join booking_order in db.booking_order on booking_details.booking_order_id equals booking_order.id
                        join order_service in db.order_service on booking_order.id equals order_service.booking_order_id
                        join services in db.services on order_service.service_id equals services.id
                        join users in db.users on booking_order.user_id equals users.id

                        select new
                        {
                            // Select the columns you need from the joined tables
                            name = users.first_name,
                            phone = users.phonenum,
                            booking_order_id = booking_order.id,
                            room_name = rooms.name,
                            room_price = rooms.price,
                            Column4FromTable4 = booking_details.room_number,
                            check_in_room= booking_details.check_in,
                            check_out_room = booking_details.check_out,
                            book_day_room = booking_order.book_day,
                            services_name = services.name

                        };
            return View(booking_list);
        }
    }
}