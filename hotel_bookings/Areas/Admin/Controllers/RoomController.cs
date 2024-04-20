using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Models;
using PagedList;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Areas.Admin.Controllers
{
    //[Authorize]

    public class RoomController : Controller
    {
        private HotelBookingEntities db = new HotelBookingEntities();

        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        [HttpGet]
        public ActionResult Index(int? page)
        {
            // Số lượng mục trên mỗi trang
            int pageSize = 8;

            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            var rooms = _roomService.GetAllRooms();

            return View(rooms.ToPagedList(pageNumber, pageSize));
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