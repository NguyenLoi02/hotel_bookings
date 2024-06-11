using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class RoleViewModel
    {
        public List<admin> admins { get; set; }
        public List<role> roles { get; set; }
        public admin_role admin_role { get; set; }

        public string username { get; set; }
        public string role { get; set; }
        public int adminID { get; set; }
        public int roleID { get; set; }
        public int RowNumber { get; set; }

    }
}