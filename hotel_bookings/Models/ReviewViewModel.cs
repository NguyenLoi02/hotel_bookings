using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings.Models
{
    public class ReviewViewModel
    {
        internal rating_view rating_view;

        public List<user> users { get; set; }
        public List<rating_view> rating_views { get; set; }
    }
}