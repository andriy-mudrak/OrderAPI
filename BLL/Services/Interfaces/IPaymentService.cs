using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentModel> Charge(PaymentModel payment);
        Task<PaymentModel> Refund(PaymentModel payment);
    }
}