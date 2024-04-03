using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Areas.Admin.Data
{
    public class OrderRoomService : IOrderRoomService
    {
        private readonly HotelBookingEntities _dbContext;

        public OrderRoomService(HotelBookingEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BookingViewModel> GetAllBooking()
        {
            var booking_list = from rooms in _dbContext.rooms
                               join booking_details in _dbContext.booking_details on rooms.id equals booking_details.room_id
                               join booking_order in _dbContext.booking_order on booking_details.booking_order_id equals booking_order.id
                               join order_service in _dbContext.order_service on booking_order.id equals order_service.booking_order_id
                               join services in _dbContext.services on order_service.service_id equals services.id
                               join users in _dbContext.users on booking_order.user_id equals users.id
                               select new BookingViewModel
                               {
                                   FirstName = users.first_name,
                                   PhoneNumber = users.phonenum,
                                   BookingOrderId = booking_order.id,
                                   RoomName = rooms.name,
                                   RoomPrice = (int)rooms.price,
                                   RoomNumber = (int)booking_details.room_number,
                                   CheckInDate = (DateTime)booking_details.check_in,
                                   CheckOutDate = (DateTime)booking_details.check_out,
                                   BookingDate = (DateTime)booking_order.book_day,
                                   ServiceName = services.name
                               };
            return booking_list.ToList();
        }

        public IEnumerable<BookingViewModel> GetAllBooked()
        {
            var booking_list = from rooms in _dbContext.rooms
                               join booking_details in _dbContext.booking_details on rooms.id equals booking_details.room_id
                               join booking_order in _dbContext.booking_order on booking_details.booking_order_id equals booking_order.id
                               join order_service in _dbContext.order_service on booking_order.id equals order_service.booking_order_id
                               join services in _dbContext.services on order_service.service_id equals services.id
                               join users in _dbContext.users on booking_order.user_id equals users.id
                               select new BookingViewModel
                               {
                                   FirstName = users.first_name,
                                   PhoneNumber = users.phonenum,
                                   BookingOrderId = booking_order.id,
                                   RoomName = rooms.name,
                                   RoomPrice = (int)rooms.price,
                                   RoomNumber = (int)booking_details.room_number,
                                   CheckInDate = (DateTime)booking_details.check_in,
                                   CheckOutDate = (DateTime)booking_details.check_out,
                                   BookingDate = (DateTime)booking_order.book_day,
                                   ServiceName = services.name
                               };
            return booking_list.ToList();
        }
    }
}