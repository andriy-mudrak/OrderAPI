using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Fake.Models.Item;

namespace BLL.Fake.Services.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<ItemModelDTO>> GetAll();
        Task<IEnumerable<ItemReservedModel>> Reservation(ItemRequestModel request);
        Task<IEnumerable<ItemModelDTO>> CancelReservation(int orderId);
    }
}