using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Services.Interfaces;
using Flurl.Http;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<PaymentModel> Charge(PaymentModel payment)
        {
            var response = await "http://localhost:53783/api/payment"
                .PostJsonAsync(new PaymentModel()
                {
                    CardToken = "tok_visa",
                    Currency = "usd",
                    Amount = 10000,
                    UserId = 2222,
                    OrderId = 2222,
                    VendorId = 2222,
                    Email = "admin@mail.com",
                    SaveCard = false,
                    Type = "charge"
                }); // TODO: add custom exception for payment

            return payment;
        }

        public async Task<PaymentModel> Refund(PaymentModel payment)
        {
            var response = await "http://localhost:53783/api/payment"
                .PostJsonAsync(new PaymentModel()
                {
                    OrderId = payment.OrderId,
                    Email = payment.Email,
                    Type = "refund"
                }); 

            return payment;
        }
    }
}