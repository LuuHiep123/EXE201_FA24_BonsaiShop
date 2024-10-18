using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class User
    {
        public User()
        {
            OrderActivities = new HashSet<OrderActivity>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string Gender { get; set; }
        public string ImgUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? RatingCount { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<OrderActivity> OrderActivities { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
