﻿
using Google.Apis.Gmail.v1;
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
using System.Threading.Tasks;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using hotel_bookings.Common;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Services.Description;
namespace hotel_bookings.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomServices;
        private readonly IVnPayService _vnPayService;
        private HotelBookingEntities db = new HotelBookingEntities();

        public RoomController(IRoomService roomServices, IVnPayService vnPayService)
        {
            _roomServices = roomServices;
            _vnPayService = vnPayService;
        }

        // GET: Room
        public ActionResult Index(int? page)
        {
            //string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Common/templateEmail.html"), Encoding.UTF8);
            //contentCustomer = contentCustomer.Replace("{{MaBooking}}", "BK0001");
            //contentCustomer = contentCustomer.Replace("{{TenPhong}}", "Room 1 style 1");
            //contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", "Lợi");
            //try
            //{
            //    string subject = "Test Email";
            //    string body = "This is a test email sent from the ASP.NET MVC application.";
            //    string to = "user.email"; // Update with the recipient's email address

            //    GoogleAuthentication.SendEmail("Hotel HL", contentCustomer.ToString(), "nguyenvanloihd88@gmail.com");
            //    Console.WriteLine("Email sent successfully.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("$\"Failed to send email");

            //    // Log the exception here
            //}
            // Số lượng mục trên mỗi trang
            int pageSize = 6;
            var roomStyle = db.room_style.ToList();
            ViewBag.roomStyleList = roomStyle;
            var roomFeatures = db.features.Take(4).ToList();
            ViewBag.roomFeatureList = roomFeatures;
            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            var room_list = _roomServices.GetAllRooms();
            return View(room_list.ToPagedList(pageNumber, pageSize));
        }
       
        [HttpPost]
        public ActionResult search(int? page,DateTime check_in, DateTime check_out, int adult = 1, int children = 1)
        {
          
            var availableRooms = _roomServices.CheckRoom(check_in, check_out, adult, children, null);
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
            Session["availableRooms"] = availableRooms;


            return View(availableRooms.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult filter(int? page, int? filterRoom, List<int> selectedFeatures, int? priceFrom, int ? priceTo)
        {

            var ddd = selectedFeatures;

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

            
            var availableRooms = (List<room>)Session["availableRooms"];
            
                if (availableRooms != null && availableRooms.Count() > 0)
                {
                    if (bookedRoomIds != null && bookedRoomIds.Count > 0)
                    {
                        var availableRoomss = availableRooms
                            .Where(r => bookedRoomIds.Contains(r.room_style_id))
                            .ToList();
                        if (priceFrom != null && priceTo != null)
                        {
                            var availableRoomssss = availableRoomss
                                .Where(r => r.price >= priceFrom && r.price <= priceTo)
                                .ToList();
                            return View(availableRoomssss.ToPagedList(pageNumber, pageSize));
                        }
                    

                    }
                    if (priceFrom != null && priceTo != null)
                    {
                        var availableRoomssss = availableRooms
                            .Where(r => r.price >= priceFrom && r.price <= priceTo)
                            .ToList();
                        return View(availableRoomssss.ToPagedList(pageNumber, pageSize));
                    }
                return View(availableRooms.ToPagedList(pageNumber, pageSize));
            }
           


            var availableRoomsss = db.rooms
               .Where(r => bookedRoomIds.Contains(r.room_style_id))
               .ToList();
            if (priceFrom != null && priceTo != null)
            {
                var availableRoomss = availableRoomsss
                    .Where(r => r.price >= priceFrom && r.price <= priceTo)
                    .ToList();
                return View(availableRoomss.ToPagedList(pageNumber, pageSize));
            }
            return View(availableRoomsss.ToPagedList(pageNumber, pageSize));


        }
       
        [HttpGet]
       
        public ActionResult RoomDetails(int id)
        {
            var detail = db.rooms.Find(id);
            return View(detail);
        }
        public ActionResult RoomReview(int id)
        {
            //string currentUrl = HttpContext.Request.Url.AbsoluteUri;

            var viewModel = new ReviewViewModel
            {
                users = db.users.ToList(),
                rating_views = db.rating_view.Where(x => x.room_id == id).ToList(),
             };
            Session["room_id"] = id;
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RoomReview(string review)
        {
            int room_id = (int)Session["room_id"];
            var check = (string)Session["user"];
            if (check != null)
            {
                var user = db.users.Where(m => m.email == check).FirstOrDefault();
                var user_id = user.id;
                var user_name = user.first_name;
                var user_profile = user.profile;
                var user_review = review;
                DateTime currentDate = DateTime.Now;
                DateTime? reviewDay = null;
                string currentDateAsString = currentDate.ToString("dd/MM/yyyy");

                if (DateTime.TryParseExact(currentDateAsString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    reviewDay = parsedDate;
                }
                rating_view rating_views = new rating_view();
                rating_views.user_id = user_id;
                rating_views.room_id = room_id;
                rating_views.review = review;
                rating_views.review_day = reviewDay;
                rating_views.seen = 1;
                db.rating_view.Add(rating_views);
                db.SaveChanges();
                var successJson = new { success = true, name = user_name, profile = user_profile, review = user_review, date = currentDate.ToString() };
                return Content(JsonConvert.SerializeObject(successJson), "application/json");
            }
            return RedirectToAction("Login", "Access");


        }
        private static List<room> list_room = new List<room>();
        private static List<int> room_id = new List<int>();
        public ActionResult RoomService(int id)
        {

            var availableRooms = (List<room>)Session["availableRooms"];

            if (!room_id.Contains(id))
            {
                room_id.Add(id);
                var details = availableRooms.Where(r => r.id == id).FirstOrDefault();
                list_room.Add(details);
            }
            var service = db.services.ToList();
            if (Session["check_in"] == null || Session["check_out"] == null)
            {   
                return RedirectToAction("Index");
            }
            var roomPriceAll = 0;
            foreach (var item in list_room)
            {
                roomPriceAll += (int)item.price;
            }
            Session["roomPriceAll"] = roomPriceAll;

            var viewModel = new ServiceRoomViewModel
            {
                services = service,
                rooms = list_room,
                roomPriceAll = roomPriceAll
            };
            var detail = availableRooms
                    .Where(r => r.id == id).FirstOrDefault();
           
            DateTime check_in = (DateTime)Session["check_in"];
            ViewBag.CheckIn = check_in.ToString("dd-MM-yyyy");
            DateTime check_out = (DateTime)Session["check_out"];
            ViewBag.CheckOut = check_out.ToString("dd-MM-yyyy");
            double days = (double)Session["day"];
            ViewBag.Day = days;
           

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RoomService(List<SelectedData> selectedItems)
        {
            //room_id.Clear();
            //list_room.Clear();
            if (selectedItems != null && selectedItems.Count() > 0)
            {
                // Lưu mảng selectedOptions vào Session
                Session["SelectedOptions"] = selectedItems;
            }
            return RedirectToAction("RoomOrder");
        }
        [HttpGet]
        public ActionResult RoomOrder()
        {
            if (Session["email"] != null && Session["first_name"]!= null && Session["last_name"] != null)
            {
                ViewBag.email = Session["emails"];
                ViewBag.first_name = Session["first_names"];
                ViewBag.last_name = Session["last_names"];

            }
            else
            {
                ViewBag.email = "";
                ViewBag.first_name = "";
                ViewBag.last_name = "";
            }
            if (Session["check_in"] == null || Session["check_out"] == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.listRoom = list_room;
            ViewBag.roomPriceAll = Session["roomPriceAll"];
            var selectedOptions = Session["SelectedOptions"] as List<SelectedData>;
            var itemList = new List<SelectedData>();
            var servicePrice = 0;
            if (selectedOptions == null)
            {
                ViewBag.servicePrice = 0;
            }
            if (selectedOptions != null)
            {
                // Lặp qua từng phần tử trong mảng selectedOptions
                foreach (var optionId in selectedOptions)
                {
                    // Thực hiện truy vấn vào cơ sở dữ liệu sử dụng optionId
                    var item = db.services.FirstOrDefault(items => items.id == optionId.ItemId);
                    servicePrice += (int)item.price*optionId.Quantity;
                    // Kiểm tra xem phần tử có tồn tại
                    if (item != null)
                    {
                        var selectedData = new SelectedData
                        {
                            Services = item, // Chú ý: Đây có thể không chính xác, phụ thuộc vào cấu trúc của lớp SelectedData
                            ItemId = optionId.ItemId,
                            Quantity = optionId.Quantity
                        };

                        // Thêm đối tượng SelectedData vào danh sách itemList
                        itemList.Add(selectedData);

                    }
                    ViewBag.servicePrice = servicePrice;
                }
            }

            Session["servicePrice"] = (double)servicePrice;

            // Kiểm tra và xử lý giá trị nếu cần
            if (selectedOptions != null)
            {
                // Gán giá trị vào ViewBag để truyền qua view
                ViewBag.SelectedOptions = selectedOptions;
            }

            DateTime check_in = (DateTime)Session["check_in"];
            ViewBag.CheckIn = check_in.ToString("dd-MM-yyyy");
            DateTime check_out = (DateTime)Session["check_out"];
            ViewBag.CheckOut = check_out.ToString("dd-MM-yyyy");
            double days = (double)Session["day"];
            ViewBag.Day = days;
            
            return View(itemList);
        }
        [HttpPost]
        public ActionResult RoomOrder(user user, bool? vnPay)
        {
            list_room.Clear();
            room_id.Clear();
            double servicePrice = (double)Session["servicePrice"];
            int room_price = (int)Session["roomPriceAll"];
            double days = (double)Session["day"];
            double trans_money = room_price * days + servicePrice;
            Session["last_name"] = user.last_name;
            Session["first_name"] = user.first_name;
            Session["email"] = user.email;
            Session["phonenum"] = user.phonenum;
            Session["trans_money"] = trans_money;

            if (vnPay == true)
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = trans_money,
                    CreatedDate = DateTime.Now,
                    Description = $"{user.first_name} {user.phonenum}",
                    FullName = user.first_name,
                    OrderId = new Random().Next(1000, 100000),
                };

                // Gọi phương thức CreatePaymentUrl của _vnPayService với HttpContextBase
                var paymentUrl = _vnPayService.CreatePaymentUrl(vnPayModel);

                // Chuyển hướng người dùng đến URL thanh toán
                return Redirect(paymentUrl);
            }

            return RedirectToAction("Index");

        }
        public ActionResult PaymentFail()
        {
            return View();  
        }
        public ActionResult PaymentSuccess()
        {
            DateTime check_in = (DateTime)Session["check_in"];
            DateTime check_out = (DateTime)Session["check_out"];
            var room_id = Session["room_id"] as int?;
            var last_name = (string)Session["last_name"];
            var first_name = (string)Session["first_name"];
            var email = (string)Session["email"];
            var phonenum = (string)Session["phonenum"];
            var trans_money = (double)Session["trans_money"];
            user user = new user();
            user.last_name = last_name;
            user.first_name = first_name;
            user.email = email;
            user.last_name = last_name;
            user.phonenum = phonenum;
            db.users.Add(user);
            db.SaveChanges();
            var user_id = user.id;
            string roomName = db.rooms.Where(r => r.id == room_id).Select(r => r.name).FirstOrDefault();
            string firstName = db.users.Where(r => r.id == user_id).Select(r => r.first_name).FirstOrDefault();
            DateTime currentDate = DateTime.Now;
            int room_price = (int)Session["room_price"];


            booking_order bookingOrder = new booking_order();
            bookingOrder.user_id = user_id;
            bookingOrder.booking_status = 0;
            bookingOrder.book_day = currentDate;
            bookingOrder.trans_money = (int)trans_money;
            db.booking_order.Add(bookingOrder);
            db.SaveChanges();

            //var selectedOptions = Session["SelectedOptions"] as int[];
            var selectedOptions = Session["SelectedOptions"] as List<SelectedData>;

            if (selectedOptions != null)
            {
                // Lặp qua từng phần tử trong mảng selectedOptions
                foreach (var option in selectedOptions)
                {
                    // Thực hiện truy vấn vào cơ sở dữ liệu sử dụng optionId
                    var item = db.services.FirstOrDefault(items => items.id == option.ItemId);
                    // Kiểm tra xem phần tử có tồn tại
                    if (item != null)
                    {
                        order_service order_service = new order_service();
                        order_service.booking_order_id = bookingOrder.id;
                        order_service.service_id = option.ItemId;
                        order_service.order_count = option.Quantity;
                        db.order_service.Add(order_service);
                        db.SaveChanges();
                    }
                }
            }
            Session["servicePrice"] = null;
            var booking_order_id = bookingOrder.id;
            booking_details booking_details = new booking_details();
            booking_details.booking_order_id = booking_order_id;
            booking_details.room_id = (int)room_id;
            booking_details.check_in = check_in;
            booking_details.check_out = check_out;
            booking_details.price = room_price;
            db.booking_details.Add(booking_details);
            db.SaveChanges();


            
                string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Common/templateEmail.html"));
            contentCustomer = contentCustomer.Replace("{{MaBooking}}", Convert.ToString(booking_order_id));
            contentCustomer = contentCustomer.Replace("{{TenPhong}}", roomName);
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", firstName);
            contentCustomer = contentCustomer.Replace("{{ngayDat}}", currentDate.ToString("dd/MM/yyyy"));
            contentCustomer = contentCustomer.Replace("{{giaPhong}}", room_price.ToString());
            contentCustomer = contentCustomer.Replace("{{thanhTien}}", trans_money.ToString());
            contentCustomer = contentCustomer.Replace("{{soDienThoai}}", phonenum);
            contentCustomer = contentCustomer.Replace("{{email}}", email);
            string htmlTemplate = @"
                 <tr>
                   <th scope=""row"" colspan=""2""
                       style=""font-family: 'Times New Roman'; color: #636363; border: 1px solid #e5e5e5; vertical-align: middle; padding: 12px; text-align: left; border-top-width: 4px"">
                          {{tenDichVu}}
                  </th>
                <td style=""font-family: 'Times New Roman'; color: #636363; border: 1px solid #e5e5e5; vertical-align: middle; padding: 12px; text-align: left; border-top-width: 4px"">
                <span>{{giaTien}}&nbsp;<span>₫</span></span>
                </td>
               </tr>
                              ";
            string dichVuThem = "";
            if (selectedOptions != null)
            {
                foreach (var option in selectedOptions)
                {
                    // Thực hiện truy vấn vào cơ sở dữ liệu sử dụng optionId
                    var item = db.services.FirstOrDefault(items => items.id == option.ItemId);
                    // Kiểm tra xem phần tử có tồn tại
                    if (item != null)
                    {

                        string row = htmlTemplate
                        .Replace("{{tenDichVu}}", item.name)
                        .Replace("{{giaTien}}", (item.price* option.Quantity).ToString());
                        dichVuThem += row;
                    }
                }
                contentCustomer = contentCustomer.Replace("{{dichVuThem}}", dichVuThem);
            }else
                contentCustomer = contentCustomer.Replace("{{dichVuThem}}", "");

            try
            {
               
                GoogleAuthentication.SendEmail("Hotel HL", contentCustomer.ToString(), email);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("$\"Failed to send email");

                // Log the exception here
            }
            
            return View(bookingOrder.id);
        }
        public ActionResult PaymentCallBack()
        {
            var respoonse = _vnPayService.PaymentExecute(Request.QueryString);
            if(respoonse == null || respoonse.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {respoonse.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            TempData["Message"] = $"Thanh toán VN Pay thành công";

            return RedirectToAction("PaymentSuccess");

        }
    }
}
