using BLL.DTOs;

namespace BLL.Helpers.MQ.Interfaces
{
    public interface IRpcClient
    {
        string Call(PaymentModel message);
        void Close();
    }
}