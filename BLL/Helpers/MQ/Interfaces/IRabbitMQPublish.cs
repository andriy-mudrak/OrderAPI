using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL.Helpers.MQ.Interfaces
{
    public interface IRabbitMQPublish
    {
        //Task Send(string queue, string data);
        //void SendPayment(PaymentModel payment);
        void Close();
        void Send<T>(T payment, string routingKey) where T : class;
    }
}