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
    public class RoomReviewController : Controller
    {
        // GET: Admin/RoomReview
        private HotelBookingEntities db = new HotelBookingEntities();

        public ActionResult Index(int? page)
        {
            var query = from u in db.users
                        join rv in db.rating_view on u.id equals rv.user_id
                        join r in db.rooms on rv.room_id equals r.id
                        select new
                        {
                            rv.id,
                            u.first_name,
                            u.email,
                            rv.review,
                            rv.review_day,
                            r.name,
                        };
            var viewModel = query.ToList().Select(item => new ReviewViewModel
            {

                rating_view_id = (int)item.id,
                room_name = item.name,
                user_name = item.first_name,
                email = item.email,
                review = item.review,
                review_day = item.review_day.HasValue ? item.review_day.Value.ToString("dd-MM-yyyy") : "N/A", // Tương tự, sử dụng tên thuộc tính tương ứng

            }).ToList();
            int count = 1;
            foreach (var item in viewModel)
            {
                if (item.rating_view_id != null)
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
            IPagedList<ReviewViewModel> paginatedViewModel = viewModel.ToPagedList(pageNumber, pageSize);
            return View(paginatedViewModel);
        }
    }
}