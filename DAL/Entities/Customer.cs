using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
            PromoCodeCustomer = new HashSet<PromoCodeCustomer>();
        }

        public int CustomerId { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAndress { get; set; }
        public string FirstName { get; set; }
        public string LasyName { get; set; }

        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<PromoCodeCustomer> PromoCodeCustomer { get; set; }
    }
}
