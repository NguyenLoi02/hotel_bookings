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
    
    public partial class room_sale
    {
        public int sale_id { get; set; }
        public int room_id { get; set; }
        public Nullable<System.DateTime> start_day { get; set; }
        public Nullable<System.DateTime> end_day { get; set; }
    
        public virtual room room { get; set; }
        public virtual sale sale { get; set; }
    }
}
