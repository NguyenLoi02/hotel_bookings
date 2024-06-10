using hotel_bookings.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace hotel_bookings.Areas.Admin.Controllers
{
    public class FeatureController : Controller
    {
        private HotelBookingEntities db = new HotelBookingEntities();

        // GET: Admin/Feature
        public ActionResult Index(int? page)
        {
            var feature = db.features.ToList();
            int count = 1;
            foreach (var item in feature)
            {
                if (item.id != null)
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
            return View(feature.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult AddFeature()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddFeature(feature feature)
        {

            db.features.Add(feature);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult UpdateFeature(int? id)
        {
            var feature = db.features.Find(id);

            return View(feature);
        }

        [HttpPost]
        public ActionResult UpdateFeature(feature feature)
        {
            var features = db.features.Find(feature.id);
            features.name = feature.name;
            features.icon = feature.icon;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteFeature(int? id)
        {
            var feature = db.features.Find(id);
            if (feature != null)
            {
                db.features.Remove(feature);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult RoomFeature()
        {
            var room = db.rooms.ToList();
            var feature = db.features.ToList();

            var viewModel = new RoomFeatureViewModel
            {
                rooms = room,
                features = feature
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RoomFeature(int room_id, int feature_id)
        {
            if(room_id != null && feature_id != null)
            {
                room_feature room_features = new room_feature();
                room_features.features_id = feature_id;
                room_features.room_id = room_id;
                db.room_feature.Add(room_features);
                db.SaveChanges();
            }
            return RedirectToAction("RoomFeature");
        }
    }
}