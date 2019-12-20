using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Fake.Models.Shipment;

namespace BLL.Fake.Services.Interfaces
{
    public interface IShipmentMethodService
    {
        Task<IEnumerable<ShipmentMethod>> GetMethods(ShipmentModel request);
        Task<ShipmentModelDTO> Create(ShipmentModelDTO request);
        Task<ShipmentModelDTO> Cancel(ShipmentModelDTO request);
    }
}