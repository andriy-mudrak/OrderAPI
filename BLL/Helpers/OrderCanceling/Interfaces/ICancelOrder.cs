using System.Threading.Tasks;

namespace BLL.Helpers.OrderCanceling.Interfaces
{
    public interface ICancelOrder
    {
        Task InvokeAsync(int orderId);
    }
}