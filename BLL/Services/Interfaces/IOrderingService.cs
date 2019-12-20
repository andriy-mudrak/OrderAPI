using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL.Services.Interfaces
{
    public interface IOrderingService
    {
        Task<OrderDTO> Create(OrderingDTO model);
    }
}