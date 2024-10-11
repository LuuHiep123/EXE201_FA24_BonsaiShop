using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Coupon
    {
        public Coupon()
        {
            Orders = new HashSet<Order>();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
