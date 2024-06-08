using hotel_bookings.Models.Service;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Storage.v1.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using static iTextSharp.text.pdf.AcroFields;
using Rotativa;

namespace hotel_bookings.Areas.Admin.Controllers
{
    //[Authorize]

    public class OrderRoomController : Controller
    {
        private HotelBookingEntities db = new HotelBookingEntities();

        // GET: Admin/OrderRoom
        [HttpGet]
        public ActionResult booking()
        {
            var query = from u in db.users
                        join bo in db.booking_order on u.id equals bo.user_id
                        join os in db.order_service on bo.id equals os.booking_order_id
                        join sv in db.services on os.service_id equals sv.id
                        join bd in db.booking_details on bo.id equals bd.booking_order_id
                        join r in db.rooms on bd.room_id equals r.id
                        where bo.booking_status == 0
                        select new
                        {
                            bo.id,
                            u.first_name,
                            u.phonenum,
                            r.name,
                            bd.price,
                            ServiceName = sv.name,
                            ServicePrice = sv.price,
                            bd.check_in,
                            bd.check_out,
                            bo.book_day
                        };

            var viewModel = query.ToList().Select(item => new BookingViewModel
            {
                booking_order_id = item.id, // Sử dụng tên thuộc tính tương ứng với kiểu ẩn danh
                user_name = item.first_name, // Tương tự, sử dụng tên thuộc tính tương ứng
                phone = item.phonenum, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_name = item.name, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_price = (int)item.price, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_name = item.ServiceName, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_price = (int)item.ServicePrice, // Tương tự, sử dụng tên thuộc tính tương ứng
                check_in = item.check_in.HasValue ? item.check_in.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                check_out = item.check_out.HasValue ? item.check_out.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                book_date = item.book_day.HasValue ? item.book_day.Value.ToString("dd-MM-yyyy") : "N/A" // Tương tự, sử dụng tên thuộc tính tương ứng
            }).ToList();

            int count = 1;
            foreach (var item in viewModel)
            {
                if (item.booking_order_id != null)
                {

                    item.RowNumber = count;
                    count++;
                }
                else
                {
                    // Reset count if trans_status is not 0
                    count = 1;
                }
            }
            return View(viewModel);
        }
        [HttpPost] // Đánh dấu phương thức chỉ được gọi khi gửi dữ liệu bằng phương thức POST
        public ActionResult booking(string phonenum)
        {

            var querys = from u in db.users
                         join bo in db.booking_order on u.id equals bo.user_id
                         join os in db.order_service on bo.id equals os.booking_order_id
                         join sv in db.services on os.service_id equals sv.id
                         join bd in db.booking_details on bo.id equals bd.booking_order_id
                         join r in db.rooms on bd.room_id equals r.id
                         where u.phonenum == phonenum && bo.booking_status == 0
                         select new
                         {
                             bo.id,
                             u.first_name,
                             u.phonenum,
                             r.name,
                             bd.price,
                             ServiceName = sv.name,
                             ServicePrice = sv.price,
                             bd.check_in,
                             bd.check_out,
                             bo.book_day
                         };
            var viewModels = querys.ToList().Select(item => new BookingViewModel
            {
                booking_order_id = item.id, // Sử dụng tên thuộc tính tương ứng với kiểu ẩn danh
                user_name = item.first_name, // Tương tự, sử dụng tên thuộc tính tương ứng
                phone = item.phonenum, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_name = item.name, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_price = (int)item.price, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_name = item.ServiceName, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_price = (int)item.ServicePrice, // Tương tự, sử dụng tên thuộc tính tương ứng
                check_in = item.check_in.HasValue ? item.check_in.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                check_out = item.check_out.HasValue ? item.check_out.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                book_date = item.book_day.HasValue ? item.book_day.Value.ToString("dd-MM-yyyy") : "N/A" // Tương tự, sử dụng tên thuộc tính tương ứng
            }).ToList();
            return View(viewModels);

        
        }

        public ActionResult booked()
        {

            var query = from u in db.users
                        join bo in db.booking_order on u.id equals bo.user_id
                        join os in db.order_service on bo.id equals os.booking_order_id
                        join sv in db.services on os.service_id equals sv.id
                        join bd in db.booking_details on bo.id equals bd.booking_order_id
                        join r in db.rooms on bd.room_id equals r.id
                        where bo.booking_status == 1
                        select new
                        {
                            bo.id,
                            u.first_name,
                            u.phonenum,
                            r.name,
                            bd.price,
                            ServiceName = sv.name,
                            ServicePrice = sv.price,
                            bd.check_in,
                            bd.check_out,
                            bo.book_day
                        };

            var viewModel = query.ToList().Select(item => new BookingViewModel
            {
                booking_order_id = item.id, // Sử dụng tên thuộc tính tương ứng với kiểu ẩn danh
                user_name = item.first_name, // Tương tự, sử dụng tên thuộc tính tương ứng
                phone = item.phonenum, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_name = item.name, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_price = (int)item.price, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_name = item.ServiceName, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_price = (int)item.ServicePrice, // Tương tự, sử dụng tên thuộc tính tương ứng
                check_in = item.check_in.HasValue ? item.check_in.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                check_out = item.check_out.HasValue ? item.check_out.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                book_date = item.book_day.HasValue ? item.book_day.Value.ToString("dd-MM-yyyy") : "N/A" // Tương tự, sử dụng tên thuộc tính tương ứng
            }).ToList();

            return View(viewModel);
        }
        [HttpPost] // Đánh dấu phương thức chỉ được gọi khi gửi dữ liệu bằng phương thức POST
        public ActionResult booked(string phonenum)
        {

            var querys = from u in db.users
                         join bo in db.booking_order on u.id equals bo.user_id
                         join os in db.order_service on bo.id equals os.booking_order_id
                         join sv in db.services on os.service_id equals sv.id
                         join bd in db.booking_details on bo.id equals bd.booking_order_id
                         join r in db.rooms on bd.room_id equals r.id
                         where u.phonenum == phonenum && bo.booking_status == 1
                         select new
                         {
                             bo.id,
                             u.first_name,
                             u.phonenum,
                             r.name,
                             bd.price,
                             ServiceName = sv.name,
                             ServicePrice = sv.price,
                             bd.check_in,
                             bd.check_out,
                             bo.book_day
                         };
            var viewModels = querys.ToList().Select(item => new BookingViewModel
            {
                booking_order_id = item.id, // Sử dụng tên thuộc tính tương ứng với kiểu ẩn danh
                user_name = item.first_name, // Tương tự, sử dụng tên thuộc tính tương ứng
                phone = item.phonenum, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_name = item.name, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_price = (int)item.price, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_name = item.ServiceName, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_price = (int)item.ServicePrice, // Tương tự, sử dụng tên thuộc tính tương ứng
                check_in = item.check_in.HasValue ? item.check_in.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                check_out = item.check_out.HasValue ? item.check_out.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                book_date = item.book_day.HasValue ? item.book_day.Value.ToString("dd-MM-yyyy") : "N/A" // Tương tự, sử dụng tên thuộc tính tương ứng
            }).ToList();
            return View(viewModels);

        }

        public ActionResult DeleteBooking(int bookingOrderId)
        {
            // Tìm đơn đặt phòng cần xóa từ cơ sở dữ liệu
            var bookingOrder = db.booking_order.Find(bookingOrderId);

            if (bookingOrder == null)
            {
                // Xử lý trường hợp không tìm thấy đơn đặt phòng
                return HttpNotFound();
            }

            try
            {
                // Xóa đơn đặt phòng từ cơ sở dữ liệu
                db.booking_order.Remove(bookingOrder);
                db.SaveChanges();

                // Chuyển hướng về trang danh sách đơn đặt phòng hoặc trang chính
                return RedirectToAction("booking");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                return View("Error", ex);
            }
        }
        public ActionResult DeleteBooked(int bookingOrderId)
        {
            // Tìm đơn đặt phòng cần xóa từ cơ sở dữ liệu
            var bookingOrder = db.booking_order.Find(bookingOrderId);

            if (bookingOrder == null)
            {
                // Xử lý trường hợp không tìm thấy đơn đặt phòng
                return HttpNotFound();
            }

            try
            {
                // Xóa đơn đặt phòng từ cơ sở dữ liệu
                db.booking_order.Remove(bookingOrder);
                db.SaveChanges();

                // Chuyển hướng về trang danh sách đơn đặt phòng hoặc trang chính
                return RedirectToAction("booked");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                return View("Error", ex);
            }
        }
        public ActionResult StatusBooking(int bookingOrderId)
        {
            // Tìm đơn đặt phòng cần xóa từ cơ sở dữ liệu
            var bookingOrder = db.booking_order.Find(bookingOrderId);

            if (bookingOrder == null)
            {
                // Xử lý trường hợp không tìm thấy đơn đặt phòng
                return HttpNotFound();
            }

            try
            {
                bookingOrder.booking_status = 1;
                db.SaveChanges();
                // Chuyển hướng về trang danh sách đơn đặt phòng hoặc trang chính
                return RedirectToAction("booking");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                return View("Error", ex);
            }
        }

        public ActionResult GenerateInvoicePdf(int bookingOrderId)
        {
            var querys = from u in db.users
                         join bo in db.booking_order on u.id equals bo.user_id
                         join os in db.order_service on bo.id equals os.booking_order_id
                         join sv in db.services on os.service_id equals sv.id
                         join bd in db.booking_details on bo.id equals bd.booking_order_id
                         join r in db.rooms on bd.room_id equals r.id
                         where bo.id == bookingOrderId 
                         select new
                         {
                             bo.id,
                             u.first_name,
                             u.phonenum,
                             r.name,
                             bd.price,
                             ServiceName = sv.name,
                             ServicePrice = sv.price,
                             bd.check_in,
                             bd.check_out,
                             bo.book_day
                         };
            var viewModels = querys.AsEnumerable().Select(item => new BookingViewModel
            {
                booking_order_id = item.id, // Sử dụng tên thuộc tính tương ứng với kiểu ẩn danh
                user_name = item.first_name, // Tương tự, sử dụng tên thuộc tính tương ứng
                phone = item.phonenum, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_name = item.name, // Tương tự, sử dụng tên thuộc tính tương ứng
                room_price = (int)item.price, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_name = item.ServiceName, // Tương tự, sử dụng tên thuộc tính tương ứng
                service_price = (int)item.ServicePrice, // Tương tự, sử dụng tên thuộc tính tương ứng
                check_in = item.check_in.HasValue ? item.check_in.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                check_out = item.check_out.HasValue ? item.check_out.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                book_date = item.book_day.HasValue ? item.book_day.Value.ToString("dd-MM-yyyy") : "N/A" // Tương tự, sử dụng tên thuộc tính tương ứng
            });
            var firstViewModel = viewModels.FirstOrDefault();


            

            return new ViewAsPdf("~/Areas/Admin/Views/Shared/Invoice.html", firstViewModel)
            {
                FileName = "Invoice.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };

        }
        
    }
}