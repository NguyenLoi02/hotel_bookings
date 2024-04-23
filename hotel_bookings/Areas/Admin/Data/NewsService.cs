using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace hotel_bookings.Areas.Admin.Data
{
    public class NewsService : INewsService
    {
        private readonly HotelBookingEntities _dbContext;

        public NewsService(HotelBookingEntities dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<news> GetAllNews()
        {
            var news = _dbContext.news.ToList();
            int count = 1;
            foreach (var item in news)
            {
                item.RowNumber = count;
                count++;
            }
            return news;
        }

        public void AddNews(news News)
        {
            if (News.ImageUpload != null && News.ImageUpload.ContentLength > 0)
            {
                try
                {
                    string filename = Path.GetFileNameWithoutExtension(News.ImageUpload.FileName);
                    string extension = Path.GetExtension(News.ImageUpload.FileName);
                    filename = filename + extension;
                    News.image = filename;
                    string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Assets/img/news/"), filename);
                    News.ImageUpload.SaveAs(filePath);
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
                _dbContext.news.Add(News);
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

        public void DeleteNews(int id)
        {
            var newsToDelete = _dbContext.news.Find(id);
            if (newsToDelete != null)
            {
                _dbContext.news.Remove(newsToDelete);
                _dbContext.SaveChanges();
            }
        }

       
        public news UpdateNews(int id)
        {
            if (id != null)
            {
                return _dbContext.news.Find(id);
            }
            return null;
        }

        public void UpdateNews(news News)
        {
            if (!string.IsNullOrEmpty(News.image))
            {
                string oldImagePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Assets/images/"), News.image);
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }

            // Set the new image filename
            string filename = Path.GetFileNameWithoutExtension(News.ImageUpload.FileName);
            string extension = Path.GetExtension(News.ImageUpload.FileName);
            filename = filename + extension;
            News.image = filename;

            // Save the new image file
            filename = Path.Combine(HttpContext.Current.Server.MapPath("~/Assets/img/news/"), filename);
            News.ImageUpload.SaveAs(filename);
            _dbContext.Entry(News).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}