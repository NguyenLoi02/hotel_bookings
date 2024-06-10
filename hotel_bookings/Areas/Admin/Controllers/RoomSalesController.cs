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
    public class RoomSalesController : Controller
    {
        // GET: Admin/RoomSales
        private HotelBookingEntities db = new HotelBookingEntities();

        [HttpGet]
        public ActionResult Index(int? page)
        {
            var query = from r in db.rooms
                        join rs in db.room_sale on r.id equals rs.room_id
                        join s in db.sales on rs.sale_id equals s.id
                        select new
                        {
                            room_id = r.id,
                            sale_id = s.id,
                            r.name,
                            sale_name = s.name,
                            rs.start_day,
                            rs.end_day
                        };
            var viewModel = query.ToList().Select(item => new RoomSaleViewModel
            {

                room_id = item.room_id,
                sale_id = item.sale_id,
                room_name = item.name,
                sale_name = item.sale_name,
                start_day = item.start_day.HasValue ? item.start_day.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng
                end_day = item.end_day.HasValue ? item.end_day.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng

            }).ToList();
            int count = 1;
            foreach (var item in viewModel)
            {
                if (item.room_id != null)
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
            // Số lượng mục trên mỗi trang
            int pageSize = 8;

            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            IPagedList<RoomSaleViewModel> paginatedViewModel = viewModel.ToPagedList(pageNumber, pageSize);
            return View(paginatedViewModel);
        }

        [HttpGet]
        public ActionResult AddSales()
        {
            var room = db.rooms.ToList();
            var sales = db.sales.ToList();

            var viewModel = new RoomSaleViewModel
            {
                rooms = room,
                sales = sales
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddSales(room_sale room_Sale, DateTime? tuNgay, DateTime? denNgay)
        {
            if(room_Sale != null && tuNgay != null && denNgay != null) {
                room_sale room_sales = new room_sale();
                room_sales.room_id = room_Sale.room_id;
                room_sales.sale_id = room_Sale.sale_id;
                room_sales.start_day = tuNgay;
                room_sales.end_day = denNgay;
                db.room_sale.Add(room_sales);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteSales(int? room_id, int? sale_id)
        {
            var feature = db.room_sale.Where(x => x.room_id == room_id && x.sale_id == sale_id).FirstOrDefault();
            if (feature != null)
            {
                db.room_sale.Remove(feature);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}