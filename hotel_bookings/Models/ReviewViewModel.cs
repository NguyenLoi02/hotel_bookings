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

        public int rating_view_id { get; set; }
        public string room_name { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string review { get; set; }
        public string review_day { get; set; }
        public int RowNumber { get; set; }

    }
}