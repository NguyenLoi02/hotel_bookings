//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace hotel_bookings.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class order_service
    {
        public int service_id { get; set; }
        public int booking_order_id { get; set; }
        public string note { get; set; }
    
        public virtual booking_order booking_order { get; set; }
        public virtual service service { get; set; }
    }
}