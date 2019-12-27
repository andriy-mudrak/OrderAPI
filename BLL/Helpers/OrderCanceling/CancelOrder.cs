using System;
using System.Threading.Tasks;
using BLL.Fake.Services.Interfaces;
//using BLL.Fake.Services.Interfaces;
using BLL.Helpers.OrderCanceling.Interfaces;
using MicroserviceMessages;
using RawRabbit;


namespace BLL.Helpers.OrderCanceling
{
    public class CancelOrder : ICancelOrder
    {
        private static IBusClient _client;
        private readonly IItemService _itemService;
        private readonly IShipmentMethodService _shipmentMethod;

        public CancelOrder(IBusClient client, IItemService itemService, IShipmentMethodService shipmentMethod)
        {

            _client = client;
            _shipmentMethod = shipmentMethod;
            _itemService = itemService;
        }

        public async Task InvokeAsync(int orderId)
        {
            try
            {
                await _client.PublishAsync(GetMessage(orderId));
                await _itemService.CancelReservation(orderId);
                await _shipmentMethod.Cancel(orderId);
            }
            catch (Exception ex)
            {

            }
          
        }

        private static PaymentModel GetMessage(int orderId)
        {
            return  new PaymentModel
            {
                OrderId = orderId,
                Type = "refund",
            };
        }
    }
}