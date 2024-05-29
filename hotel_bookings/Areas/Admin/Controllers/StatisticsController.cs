using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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




            int month = 5 /* Giá trị tháng bạn muốn lọc, ví dụ: 5 cho tháng 5 */;
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
                .Where(x => x.BookingOrder != null && x.BookingOrder.book_day != null && x.BookingOrder.book_day.Value.Month == month || x.BookingOrder == null)
                .GroupBy(x => x.Room.name)
                .Select(g => new total
                {
                    name = g.Key,
                    Price = g.Sum(x => x.BookingDetail != null ? x.BookingDetail.price ?? 0 : 0)
                })
                .ToList();



            var allMonths = Enumerable.Range(1, 12);

            var totalMonthsData = db.rooms
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
                .Where(x => x.BookingOrder != null && x.BookingOrder.book_day != null)
                .GroupBy(x => x.BookingOrder.book_day.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalRevenue = g.Sum(x => x.BookingDetail != null ? x.BookingDetail.price ?? 0 : 0)
                })
                .ToList();

            // Kết hợp với danh sách tất cả các tháng
            var monthlyTotalsWithAllMonths = allMonths
                .GroupJoin(totalMonthsData,
                    allMonth => allMonth,
                    total => total.Month,
                    (allMonth, monthlyTotalGroup) => new { Month = allMonth, MonthlyTotalGroup = monthlyTotalGroup })
                .Select(result => new hotel_bookings.Models.totalMonths
                {
                    Month = $"Tháng {result.Month}",                   
                    Price = (int)(result.MonthlyTotalGroup.FirstOrDefault()?.TotalRevenue ?? 0) // Chuyển đổi kiểu dữ liệu từ decimal? sang int
                })
                .OrderBy(result => result.Month, new MonthComparer())
                .ToList();

            var viewModel = new StatisticsViewModel
            {
                GenderStats = genderStats,
                list_room_styles = list_room_styles,
                totals = totals,
                totalMonths = monthlyTotalsWithAllMonths
            };


            return View(viewModel);
        }

        public class MonthComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                var months = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
                return Array.IndexOf(months, x).CompareTo(Array.IndexOf(months, y));
            }
        }
    }
}