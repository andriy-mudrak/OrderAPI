using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Address
    {
        public Address()
        {
            Order = new HashSet<Order>();
        }

        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public int PostalCode { get; set; }
        public string Street { get; set; }
        public int BuildingNo { get; set; }
        public int? ApartmentNo { get; set; }
        public string City { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
