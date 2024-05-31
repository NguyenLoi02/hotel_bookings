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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class user
    {
        public user()
        {
            this.booking_order = new HashSet<booking_order>();
            this.rating_view = new HashSet<rating_view>();
            this.user_question = new HashSet<user_question>();
        }
    
        public int id { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string phonenum { get; set; }
        //[Required]
        public string email { get; set; }
        public string address { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public string profile { get; set; }
        //[Required]
        public string password { get; set; }
        [NotMapped]


        public string ConfirmPassword { get; set; }
        public Nullable<System.DateTime> date_sign { get; set; }
        public Nullable<int> gender { get; set; }
        public int RowNumber { get; set; }
    
        public int Count { get; set; }

    
        public virtual ICollection<booking_order> booking_order { get; set; }
        public virtual ICollection<rating_view> rating_view { get; set; }
        public virtual ICollection<user_question> user_question { get; set; }

    }
}
