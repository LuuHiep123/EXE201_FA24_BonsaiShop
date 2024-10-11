using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string CouponId { get; set; }
        public double TotalPrice { get; set; }
        public double ShipPrice { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public virtual Coupon Coupon { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
