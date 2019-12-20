using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string ShippingMethod { get; set; }
        public int AddressId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Currency { get; set; }
        public double PaymentTotal { get; set; }
        public double ShippingTotal { get; set; }
        public double SubTotal { get; set; }
        public string Status { get; set; }
        public int Tax { get; set; }
        public string TrackingNo { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
