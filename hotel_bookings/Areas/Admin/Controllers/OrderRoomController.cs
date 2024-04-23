using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var viewModel = new BookingViewModel
            {
                users = db.users.ToList(),         
                order_services = db.order_service.ToList(),
                services = db.services.ToList(),
                rooms = db.rooms.ToList(),
                booking_orders = db.booking_order.ToList(),
                booking_details = db.booking_details.ToList()
            };

            int count = 1;
            foreach (var item in viewModel.booking_orders)
            {
                if (item.trans_status == 0)
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
            // Lấy danh sách các đặt phòng từ cơ sở dữ liệu
            var viewModel = new BookingViewModel
            {
                users = db.users.ToList(),
                order_services = db.order_service.ToList(),
                services = db.services.ToList(),
                rooms = db.rooms.ToList(),
                booking_orders = db.booking_order.ToList(),
                booking_details = db.booking_details.ToList()
            };

            // Thêm logic tìm kiếm vào đây
            if (!string.IsNullOrEmpty(phonenum))
            {
                // Nếu có số điện thoại được nhập vào biểu mẫu, thực hiện tìm kiếm
                viewModel.users = viewModel.users.Where(u => u.phonenum == phonenum).ToList();
            }

            // Sắp xếp lại số thứ tự nếu cần
            int count = 1;
            foreach (var item in viewModel.booking_orders)
            {
                if (item.trans_status == 0)
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
        public ActionResult booked()
        {
            var viewModel = new BookingViewModel
            {
                users = db.users.ToList(),
                order_services = db.order_service.ToList(),
                services = db.services.ToList(),
                rooms = db.rooms.ToList(),
                booking_orders = db.booking_order.ToList(),
                booking_details = db.booking_details.ToList()
            };

            return View(viewModel);
        }

    }
}