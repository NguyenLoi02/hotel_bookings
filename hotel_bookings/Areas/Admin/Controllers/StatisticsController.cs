using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace hotel_bookings.Areas.Admin.Controllers
{
    public class StatisticsController : Controller
    {
        private HotelBookingEntities db = new HotelBookingEntities();

        // GET: Admin/Statistics
        public ActionResult Index()
        {
            var users = db.users.ToList();

            var genderStats = users.GroupBy(u => u.gender)
                                   .Select(g => new GenderStat
                                   {
                                       Gender = g.Key,
                                       Count = g.Count()
                                   }).ToList();

            var rooms = db.rooms.ToList();
            var room_styles = db.room_style.ToList();

            var list_room_style = from room in rooms
                        join style in room_styles on room.room_style_id equals style.id
                        group style by new { style.id, style.name } into g
                        select new list_room_style
                        {
                            name = g.Key.name,
                            Count = g.Count()
                        };
            var list_room_styles = list_room_style.ToList();



            //   var totals = (
            //   from room in db.rooms
            //   join bookingDetail in db.booking_details on room.id equals bookingDetail.room_id into bdGroup
            //   from bd in bdGroup.DefaultIfEmpty()
            //   join bookingOrder in db.booking_order on bd != null ? bd.booking_order_id : (int?)null equals bookingOrder.id into boGroup
            //   from bo in boGroup.DefaultIfEmpty()
            //   where bo == null || (bo.book_day.HasValue && bo.book_day.Value.Month == 5)
            //   group new { room, bo } by room.name into g
            //   select new total
            //   {
            //       name = g.Key,
            //       Price = g.Sum(x => x.bo != null ? x.room.price : 0).GetValueOrDefault()
            //   }
            //).ToList();




            //var totals = db.rooms
            //.GroupJoin(db.booking_details,
            //    room => room.id,
            //    bookingDetail => bookingDetail.room_id,
            //    (room, bookingDetails) => new { Room = room, BookingDetails = bookingDetails })
            //.SelectMany(
            //    x => x.BookingDetails.DefaultIfEmpty(),
            //    (x, bookingDetail) => new { x.Room, BookingDetail = bookingDetail })
            //.GroupJoin(db.booking_order,
            //    x => x.BookingDetail.booking_order_id,
            //    bookingOrder => bookingOrder.id,
            //    (x, bookingOrders) => new { x.Room, x.BookingDetail, BookingOrders = bookingOrders })
            //.SelectMany(
            //    x => x.BookingOrders.DefaultIfEmpty(),
            //    (x, bookingOrder) => new { x.Room, x.BookingDetail, BookingOrder = bookingOrder })
            //.Where(x => x.BookingOrder != null && x.BookingOrder.book_day != null && x.BookingOrder.book_day.Value.Month == 5 || x.BookingOrder == null)
            //.GroupBy(x => x.Room.name)
            //.Select(g => new total
            //{
            //    name = g.Key,
            //    Price = g.Sum(x => x.BookingDetail != null ? x.BookingDetail.price ?? 0 : 0)
            //})
            //.ToList();
            var totals = db.rooms
                .GroupJoin(db.booking_details,
                    room => room.id,
                    bookingDetail => bookingDetail.room_id,
                    (room, bookingDetails) => new { Room = room, BookingDetails = bookingDetails })
                .SelectMany(
                    x => x.BookingDetails.DefaultIfEmpty(),
                    (x, bookingDetail) => new { x.Room, BookingDetail = bookingDetail })
                .GroupJoin(db.booking_order,
                    x => x.BookingDetail.booking_order_id,
                    bookingOrder => bookingOrder.id,
                    (x, bookingOrders) => new { x.Room, x.BookingDetail, BookingOrders = bookingOrders })
                .SelectMany(
                    x => x.BookingOrders.DefaultIfEmpty(),
                    (x, bookingOrder) => new { x.Room, x.BookingDetail, BookingOrder = bookingOrder })
                .Where(x => x.BookingOrder != null && x.BookingOrder.book_day != null && x.BookingOrder.book_day.Value.Month == 5 || x.BookingOrder == null)
                .GroupBy(x => x.Room.name)
                .Select(g => new total
                {
                    name = g.Key,
                    Price = g.Sum(x => x.BookingDetail != null ? x.BookingDetail.price ?? 0 : 0)
                })
                .ToList();


            var viewModel = new StatisticsViewModel
            {
                GenderStats = genderStats,
                list_room_styles = list_room_styles
            };

            return View(viewModel);
        }
        //public JsonResult GetTotals(int? month)
        //{
        //    var totals = db.rooms
        //        .GroupJoin(db.booking_details,
        //            room => room.id,
        //            bookingDetail => bookingDetail.room_id,
        //            (room, bookingDetails) => new { Room = room, BookingDetails = bookingDetails })
        //        .SelectMany(
        //            x => x.BookingDetails.DefaultIfEmpty(),
        //            (x, bookingDetail) => new { x.Room, BookingDetail = bookingDetail })
        //        .GroupJoin(db.booking_order,
        //            x => x.BookingDetail.booking_order_id,
        //            bookingOrder => bookingOrder.id,
        //            (x, bookingOrders) => new { x.Room, x.BookingDetail, BookingOrders = bookingOrders })
        //        .SelectMany(
        //            x => x.BookingOrders.DefaultIfEmpty(),
        //            (x, bookingOrder) => new { x.Room, x.BookingDetail, BookingOrder = bookingOrder })
        //        .Where(x => x.BookingOrder != null && x.BookingOrder.book_day != null && x.BookingOrder.book_day.Value.Month == month || x.BookingOrder == null)
        //        .GroupBy(x => x.Room.name)
        //        .Select(g => new total
        //        {
        //            name = g.Key,
        //            Price = g.Sum(x => x.BookingDetail != null ? x.BookingDetail.price ?? 0 : 0)
        //        })
        //        .ToList();

        //    // Trả về dữ liệu dưới dạng JSON
        //    return Json(totals, JsonRequestBehavior.AllowGet);
        //}

    }
}