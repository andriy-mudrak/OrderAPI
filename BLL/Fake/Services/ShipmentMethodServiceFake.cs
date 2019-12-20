using System;
using System.Collections.Generic;
using System.Linq;
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
            return new ShipmentModelDTO()
            {
                TrackingNumber = GenerateTrackingNo(),
                Address = request.Address,
                Method= request.Method
            };
        }

        public async Task<ShipmentModelDTO> Cancel(ShipmentModelDTO request)
        {
            return request;
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