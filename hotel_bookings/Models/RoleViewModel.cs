using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class RoleViewModel
    {
        public List<admin> admins { get; set; }
        public List<admin_role> admin_Roles { get; set; }
        public List<role> roles { get; set; }
    }
}