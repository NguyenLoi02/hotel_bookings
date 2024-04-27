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
            if (!string.IsNullOrEmpty(room.avatar))
            {
                string oldImagePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Assets/images/"), room.avatar);
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }

            // Set the new image filename
            string filename = Path.GetFileNameWithoutExtension(room.ImageUpload.FileName);
            string extension = Path.GetExtension(room.ImageUpload.FileName);
            filename = filename + extension;
            room.avatar = filename;

            // Save the new image file
            filename = Path.Combine(HttpContext.Current.Server.MapPath("~/Assets/img/room/"), filename);
            room.ImageUpload.SaveAs(filename);
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

        public IEnumerable<room> CheckRoom(DateTime check_in, DateTime check_out)
        {
            var bookedRoomIds = _dbContext.booking_details
            .Where(b => !(b.check_in >= check_out || b.check_out <= check_in))
            .Select(b => b.id)
            .ToList();

            var availableRooms = _dbContext.rooms
                .Where(r => !bookedRoomIds.Contains(r.id) && r.quantity > 0)
                .ToList();
            return availableRooms;
        }

        public IEnumerable<room> RoomFilter(int? room_style_id)
        {

            var bookedRoomIds = _dbContext.room_style
            .Where(b => b.id== room_style_id)
            .Select(b => b.id)
            .ToList();

            var availableRooms = _dbContext.rooms
                .Where(r => !bookedRoomIds.Contains(r.id))
                .ToList();
            return availableRooms;
        }
    }
}