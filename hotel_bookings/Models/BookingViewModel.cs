using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;


namespace hotel_bookings.Models
{

    public class BookingViewModel
    {
        public List<booking_order> booking_orders {  get; set; }
        public List<user> users {  get; set; }
        public List<booking_details> booking_details {  get; set; }
        public List<room> rooms {  get; set; }
        public List<order_service> order_services {  get; set; }
        public List<service> services {  get; set; }
        public int RowNumber { get; set; }

        public int booking_order_id { get; set; }
        public string user_name { get; set; }
        public string phone { get; set; }
        public string room_name { get; set; }
        public int room_price { get; set; }
        public string service_name { get; set; }
        public int service_price { get; set; }
        public int service_count { get; set; }
        public string check_in { get; set; }
        public string check_out { get; set; }
        public string book_date { get; set; }




    }
}