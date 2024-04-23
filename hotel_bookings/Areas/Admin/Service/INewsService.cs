using hotel_bookings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_bookings.Areas.Admin.Service
{
    public interface INewsService
    {
        IEnumerable<news> GetAllNews();
        void AddNews(news News);
        news UpdateNews(int id);
        void UpdateNews(news News);
        void DeleteNews(int id);
    }
}
