using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hotel_bookings.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        // GET: Admin/News
        public ActionResult Index(int? page)
        {
            // Số lượng mục trên mỗi trang
            int pageSize = 8;

            // Số trang hiện tại (nếu không có sẽ mặc định là 1)
            int pageNumber = (page ?? 1);
            var news = _newsService.GetAllNews();

            return View(news.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult AddNews()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNews(news news)
        {

            _newsService.AddNews(news);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult UpdateNews(int id)
        {
            var news = _newsService.UpdateNews(id);
            return View(news);
        }
        [HttpPost]
        public ActionResult UpdateNews(news news)
        {
            _newsService.UpdateNews(news);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult DeleteNews(int id)
        {
            _newsService.DeleteNews(id);
            return RedirectToAction("Index");
        }
    }
}