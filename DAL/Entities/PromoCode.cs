using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class PromoCode
    {
        public PromoCode()
        {
            PromoCodeCustomer = new HashSet<PromoCodeCustomer>();
        }

        public int PromoCodeId { get; set; }
        public int Discount { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PromoCodeCustomer> PromoCodeCustomer { get; set; }
    }
}
