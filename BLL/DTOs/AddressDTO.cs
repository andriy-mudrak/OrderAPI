namespace BLL.DTOs
{
    public class AddressDTO
    {
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public int PostalCode { get; set; }
        public string Street { get; set; }
        public int BuildingNo { get; set; }
        public int? ApartmentNo { get; set; }
        public string City { get; set; }

    }
}
