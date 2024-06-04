using hotel_bookings.Models.Service;
using hotel_bookings.Controllers;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections;

namespace hotel_bookings.Models.Data
{
    public class RoomService : IRoomService
    {
        private List<room> _rooms;
        private readonly HotelBookingEntities _dbContext;

        public RoomService(HotelBookingEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<room> GetAllRooms()
        {

            var rooms = _dbContext.rooms.ToList();
            int count = 1;
            foreach (var item in rooms)
            {
                item.RowNumber = count;
                count++;
            }
            return rooms;
        }
        public IEnumerable<room> SearchRooms(string a)
        {
            var SearchView = new SearchViewModel
            {
                rooms = _dbContext.rooms.ToList(),
                room_Styles = _dbContext.room_style.ToList(),
            };
            return SearchView.rooms;
        }
        public void AddRoom(room room)
        {
            if (room.ImageUpload != null && room.ImageUpload.ContentLength > 0)
            {
                try
                {
                    string filename = Path.GetFileNameWithoutExtension(room.ImageUpload.FileName);
                    string extension = Path.GetExtension(room.ImageUpload.FileName);
                    filename = filename + extension;
                    room.avatar = filename;
                    string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Assets/img/room/"), filename);
                    room.ImageUpload.SaveAs(filePath);
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi khi lưu ảnh
                    // Ví dụ: Ghi log, thông báo người dùng, vv.
                    // Ví dụ: Logger.Error(ex.Message);
                    // Ví dụ: ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu ảnh.");
                }
            }

            try
            {
               room.status = 0;
                _dbContext.rooms.Add(room);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi lưu phòng
                // Ví dụ: Ghi log, thông báo người dùng, vv.
                // Ví dụ: Logger.Error(ex.Message);
                // Ví dụ: ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu phòng.");
            }

        }
        public room UpdateRoom(int id)
        {
            if (id != null)
            {
                return _dbContext.rooms.Find(id);
            }
            return null;
        }
        public void UpdateRoom(room room)
        {
            // Kiểm tra nếu có tệp ảnh mới được tải lên
            if (room.ImageUpload != null && room.ImageUpload.ContentLength > 0)
            {
                // Xóa ảnh cũ nếu tồn tại
                if (!string.IsNullOrEmpty(room.avatar))
                {
                    string oldImagePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Assets/images/"), room.avatar);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                // Thiết lập tên mới cho hình ảnh
                string filename = Path.GetFileNameWithoutExtension(room.ImageUpload.FileName);
                string extension = Path.GetExtension(room.ImageUpload.FileName);
                filename = filename + extension;
                room.avatar = filename;

                // Lưu tập tin hình ảnh mới
                string newImagePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Assets/img/room/"), filename);
                room.ImageUpload.SaveAs(newImagePath);
            }


            // Cập nhật thông tin phòng trong cơ sở dữ liệu
            _dbContext.Entry(room).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void DeleteRoom(int id)
        {
            var roomToDelete = _dbContext.rooms.Find(id);
            if (roomToDelete != null)
            {
                _dbContext.rooms.Remove(roomToDelete);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<room> CheckRoom(DateTime check_in, DateTime check_out, int adult , int children,int? roomStyleId)
        {
            // Lấy danh sách ID phòng và số lần phòng đó được đặt trong khoảng thời gian
            var bookedRoomsCount = _dbContext.booking_details
                .Where(b => !(b.check_in >= check_out || b.check_out <= check_in))
                .GroupBy(b => b.room_id)
                .Select(g => new
                {
                    RoomId = g.Key,
                    BookingCount = g.Count()
                })
                .ToList();

            // Lấy danh sách ID của các phòng đã đặt từ bookedRoomsCount
            var bookedRoomIds = bookedRoomsCount.Select(b => b.RoomId).ToList();

            // Lấy danh sách ID phòng được sale trong thời gian tìm kiếm
            var query = (from rs in _dbContext.room_sale
                        join s in _dbContext.sales on rs.sale_id equals s.id
                        where rs.start_day <=check_in && rs.end_day >= check_out
                        select new
                        {
                            RoomId = rs.room_id,
                            Percent = s.percents
                        }).ToList();
            var roomSaleIds = query.Select(b => b.RoomId).ToList();
            // Lấy danh sách các phòng chưa sale(các phòng càn lại trong danh sách phòng)
            var room_sales = _dbContext.rooms
                .Where(r => !roomSaleIds.Contains(r.id))
                .ToList();

            // tìm kiếm phòng đã đặt dựa trên ID phòng đã đặt và Cập nhật giá phòng 
            foreach (var roomSale in query)
            {
                var room = _dbContext.rooms.SingleOrDefault(r => r.id == roomSale.RoomId);
                if (room != null)
                {
                    room.price = room.price * roomSale.Percent / 100;
                    room_sales.Add(room);
                }
            }
            room_sales = room_sales.OrderBy(room => room.id).ToList();
            // Lấy danh sách các phòng có sẵn (các phòng càn lại trong danh sách phòng sau khi đã check sale)
            var availableRooms = room_sales
                .Where(r => !bookedRoomIds.Contains(r.id) && r.quantity > 0)
                .ToList();

            // tìm kiếm phòng đã đặt dựa trên ID phòng đã đặt và Cập nhật số lượng phòng dựa trên số lần đặt phòng
            foreach (var bookedRoom in bookedRoomsCount)
            {
                var room = _dbContext.rooms.SingleOrDefault(r => r.id == bookedRoom.RoomId);
                if (room != null)
                {
                    room.quantity -= bookedRoom.BookingCount;
                    if (room.quantity >= 0)
                    {
                        availableRooms.Add(room);
                    }
                }
            }
            // kiểm tra số người lớn và trẻ em dựa trên biến adult, children
            availableRooms = availableRooms
             .Where(room => room.adult >= adult && room.children >= children)
             .ToList();
            if (roomStyleId.HasValue)
            {
                var availableRoom = availableRooms.Where(r => r.room_style_id == roomStyleId).ToList();
                availableRoom = availableRoom.OrderBy(room => room.id).ToList();
                return availableRoom;
            }
            //Sắp xếp danh sách phòng theo ID phòng
            availableRooms = availableRooms.OrderBy(room => room.id).ToList();
            return availableRooms;
        }

        //public IEnumerable<room> RoomFilter(int? room_style_id)
        //{

        //    var bookedRoomIds = _dbContext.room_style
        //    .Where(b => b.id== room_style_id)
        //    .Select(b => b.id)
        //    .ToList();

        //    var availableRooms = _dbContext.rooms
        //        .Where(r => !bookedRoomIds.Contains(r.id))
        //        .ToList();
        //    return availableRooms;
        //}
    }
}