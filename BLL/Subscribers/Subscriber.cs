using System;
using System.Threading.Tasks;
using BLL.Helpers.OrderCanceling.Interfaces;
using BLL.Subscribers.Interfaces;
using MicroserviceMessages;
using RawRabbit;

namespace BLL.Subscribers
{
    public class Subscriber : ISubscriber
    {
        private static IBusClient _client;
        private readonly ICancelOrder _cancelOrder;

        public Subscriber(IBusClient client, ICancelOrder cancelOrder)
        {
            _client = client;
            _cancelOrder = cancelOrder;
        }

        public async Task Start()
        {
            await _client.SubscribeAsync<ResponseBasic>((requested) => RunTask(requested));
        }

        private async Task RunTask(ResponseBasic response)
        {
            if (!response.Status) await _cancelOrder.InvokeAsync(response.OrderId);

        }
    }
}