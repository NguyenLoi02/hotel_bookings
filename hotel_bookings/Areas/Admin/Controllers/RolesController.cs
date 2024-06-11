using hotel_bookings.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static iTextSharp.text.pdf.AcroFields;

namespace hotel_bookings.Areas.Admin.Controllers
{
    public class RolesController : Controller
    {
        private HotelBookingEntities db = new HotelBookingEntities();

        // GET: Admin/Roles
        public ActionResult Index(int? page)
        {
            var query = from a in db.admins
                        join ar in db.admin_role on a.id equals ar.admin_id
                        join r in db.roles on ar.role_id equals r.id
                        select new
                        {
                            adminID = a.id,
                            roleID = r.id,
                            r.name,
                            a.username
                        };
            var viewModel = query.ToList().Select(item => new RoleViewModel
            {
                adminID = item.adminID,
                roleID = item.roleID,
                username = item.username,
                role = item.name,
            }).ToList();
            int count = 1;
            foreach (var item in viewModel)
            {
                if (item.adminID != null && item.roleID != null)
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
            IPagedList<RoleViewModel> paginatedViewModel = viewModel.ToPagedList(pageNumber, pageSize);
            return View(paginatedViewModel);
        }
        [HttpGet]
        public ActionResult AddRole()
        {
            var admins = db.admins.ToList();
            var roles = db.roles.ToList();

            var viewModel = new RoleViewModel
            {
                admins = admins,
                roles = roles
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddRole(admin_role admin_Role)
        {
            db.admin_role.Add(admin_Role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult UpdateRole(int adminID, int roleID)
        {
            var admin_role = db.admin_role.Where(x => x.admin_id == adminID && x.role_id == roleID).FirstOrDefault();
            var admins = db.admins.ToList();
            var roles = db.roles.ToList();
            var query = from a in db.admins
                        join ar in db.admin_role on a.id equals ar.admin_id
                        where ar.admin_id == adminID && ar.role_id == roleID
                        select new
                        {
                            username = a.username,
                            adminID = a.id,
                        };

            var result = query.FirstOrDefault();
            Session["adminID"] = adminID;
            Session["roleID"] = roleID;
            var viewModel = new RoleViewModel
            {
                username = result.username,
                adminID = result.adminID,
                admins = admins,
                roles = roles,
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult UpdateRole(admin_role admin_Roles)
        {
            var adminID = (int)Session["adminID"];
            var roleID = (int)Session["roleID"];
            var admin_role = db.admin_role.Where(x => x.admin_id == adminID && x.role_id == roleID).FirstOrDefault();
            if (admin_role != null)
            {
                admin_role.role_id = admin_Roles.role_id;

                db.SaveChanges(); // Lưu các thay đổi vào cơ sở dữ liệu
            }
            return RedirectToAction("Index");
        }
    }
}