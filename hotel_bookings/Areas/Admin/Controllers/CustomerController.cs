using hotel_bookings.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace hotel_bookings.Areas.Admin.Controllers
{
    //[Authorize]

    public class CustomerController : Controller
    {
        // GET: Admin/Customer
        private HotelBookingEntities db = new HotelBookingEntities();
 

        [Authorize(Roles = "admin,manager,user")]

        public ActionResult Index(int? page)
        {
            // Số lượng mục trên mỗi trang
            int pageSize = 10;

            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            var customer = db.users.ToList();
            int count = 1;
            foreach (var item in customer)
            {
                item.RowNumber = count;
                count++;
            }
            return View(customer.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult Index(int? page, string sdt)
        {
            // Số lượng mục trên mỗi trang
            int pageSize = 10;

            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            var customer = db.users.Where(r => r.phonenum == sdt).ToList();
            int count = 1;
            foreach (var item in customer)
            {
                item.RowNumber = count;
                count++;
            }
            return View(customer.ToPagedList(pageNumber, pageSize));
        }
    }
}