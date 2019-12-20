namespace BLL.Fake.Models.Shipment
{
    public class ShipmentModelDTO
    {
        public string TrackingNumber { get; set; }
        public ShipmentModel Address { get; set; }
        public ShipmentMethod Method { get; set; }

    }
}