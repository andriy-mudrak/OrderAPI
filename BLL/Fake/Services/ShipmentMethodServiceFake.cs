using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Fake.Constants;
using BLL.Fake.Models.Shipment;
using BLL.Fake.Services.Interfaces;

namespace BLL.Fake.Services
{
    public class ShipmentMethodServiceFake : IShipmentMethodService
    {
        private static int AddressId = 0;
        private static Dictionary<int, ShipmentModelDTO> transactions = new Dictionary<int, ShipmentModelDTO>();


        public async Task<IEnumerable<ShipmentMethod>> GetMethods(ShipmentModel request)
        {
            //calculate using start and finish address from request
            return new List<ShipmentMethod>()
            {
                new ShipmentMethod(){ Name = ShipmentConstants.DHL, Price = ShipmentConstants.DHL_PRICE},
                new ShipmentMethod(){ Name = ShipmentConstants.FED_EX, Price = ShipmentConstants.FED_EX_PRICE},
                new ShipmentMethod(){ Name = ShipmentConstants.UPS, Price = ShipmentConstants.UPS_PRICE},
            };
        }

        public async Task<ShipmentModelDTO> Create(ShipmentModelDTO request)
        {
            request.Address.AddressId = ++AddressId;
            request.TrackingNumber = GenerateTrackingNo();

            transactions.Add(request.OrderId, request);

            return request;
        }

        public async Task<ShipmentModelDTO> Cancel(int orderId)
        {
            var shipment = transactions[orderId];
            transactions.Remove(orderId);
            return shipment;
        }

        private string GenerateTrackingNo()
        {
            var random = new Random();
            var trackingNumber = new StringBuilder(16);
            for (int i = 0; i < 16; i++)
            {
                trackingNumber.Append(random.Next(0, 9).ToString());
            }

            return trackingNumber.ToString();
        }
    }
}