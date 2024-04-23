using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Models;
using PagedList;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList.Mvc;

namespace hotel_bookings.Areas.Admin.Controllers
{
    //[Authorize]
    [AllowAnonymous]

    public class RoomController : Controller
    {
        private HotelBookingEntities db = new HotelBookingEntities();

        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        [HttpGet]
        //public ActionResult Index(int? page)
        //{
        //    // Số lượng mục trên mỗi trang
        //    int pageSize = 8;

        //    // Số trang hiện tại (nếu không có sẽ mặc định là 1)
        //    int pageNumber = (page ?? 1);
        //    var rooms = _roomService.GetAllRooms();

        //    return View(rooms.ToPagedList(pageNumber, pageSize));
        //}
        public ActionResult Index()
        {
            var SearchView = new SearchViewModel
            {
                rooms = db.rooms.ToList(),
                room_Styles = db.room_style.ToList(),
            };
            int count = 1;
            foreach (var item in SearchView.rooms)
            {
                item.RowNumber = count;
                count++;
            }
            return View(SearchView);
        }
        [HttpPost]
        public ActionResult Index(int? room_style_id, int? status)
        {
            
            var query = db.rooms.AsQueryable();

            if (room_style_id != null)
            {
                query = query.Where(r => r.room_style_id == room_style_id);
            }

            if (status != null)
            {
                query = query.Where(r => r.status == status);
            }

            var searchView = new SearchViewModel
            {
                rooms = query.ToList(),
                room_Styles = db.room_style.ToList(),
            };
            int count = 1;
            foreach (var item in searchView.rooms)
            {
                item.RowNumber = count;
                count++;
            }

            return View(searchView);
        }


        [HttpGet]
        public ActionResult AddRoom()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRoom(room room)  
        {
            
            _roomService.AddRoom(room);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult UpdateRoom(int id)
        {
            var room = _roomService.UpdateRoom(id);
            return View(room);
        }
        [HttpPost]
        public ActionResult UpdateRoom(room room)
        {
            _roomService.UpdateRoom(room);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult DeleteRoom(int id)
        {
            _roomService.DeleteRoom(id);
            return RedirectToAction("Index");
        }
        
    }
}