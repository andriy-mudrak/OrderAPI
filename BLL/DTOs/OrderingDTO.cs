using System.Collections.Generic;
using BLL.Fake.Models.Item;
using BLL.Fake.Models.Shipment;

namespace BLL.DTOs
{
    public class OrderingDTO
    {
        public PaymentInfoDTO OrderInfo { get; set; }
        //public string PromoCode { get; set; }
        public IEnumerable<ItemModelDTO> Items { get; set; }
        public ShipmentModelDTO Shipment { get; set; }
    }
}