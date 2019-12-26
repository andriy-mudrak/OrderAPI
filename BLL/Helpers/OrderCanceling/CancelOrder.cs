using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BLL.Fake.Services.Interfaces;
using BLL.Helpers.OrderCanceling.Interfaces;
using BLL.MessagesTest;
using MicroserviceMessages;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit.Enrichers.HttpContext;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Instantiation;
using RawRabbit.Operations.Publish;
using RawRabbit.Operations.Publish.Context;
using RawRabbit.Operations.Publish.Middleware;
using RawRabbit.Pipe;
using RawRabbit.Pipe.Middleware;

//using RawRabbit;

namespace BLL.Helpers.OrderCanceling
{
    public class CancelOrder : ICancelOrder
    {
        private static IBusClient _client;
        private readonly IItemService _itemService;
        private readonly IShipmentMethodService _shipmentMethod;
        private readonly IConfiguration _configuration;
        private RawRabbitConfiguration GetRawRabbitConfiguration()
        {
            var section = _configuration.GetSection("RawRabbit");
            if (!section.GetChildren().Any())
            {
                throw new ArgumentException($"Unable to configuration section 'RawRabbit'. Make sure it exists in the provided configuration");
            }
            var test = section.Get<RawRabbitConfiguration>();
            return test;
        }
        public CancelOrder(IBusClient client, IItemService itemService, IShipmentMethodService shipmentMethod, IConfiguration configuration)
        {
            _configuration = configuration;
            _client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = GetRawRabbitConfiguration(),
                Plugins = p => p
                    .UseStateMachine()
                    .UseGlobalExecutionId()
                    .UseHttpContext()
                    .UseMessageContext(c =>
                    {
                        return new MessageContext
                        {
                            Source = c.GetHttpContext().Request.GetDisplayUrl()
                        };
                    })
            });
            _shipmentMethod = shipmentMethod;
            _itemService = itemService;
        }

        public async Task InvokeAsync(int orderId)
        {
            try
            {
               
                await _client.PublishAsync(GetModel(orderId));
                await _itemService.CancelReservation(orderId);
                await _shipmentMethod.Cancel(orderId);
            }
            catch (Exception e)
            {

            }
        }

        private static PaymentModel GetModel(int orderId)
        {
            var test = new MicroserviceMessages.PaymentModel()
            {
                OrderId = orderId,
                Type = "refund",
                Amount = 0,
                CardToken = "",
                Currency = "usd",
                Email = "mail",
                SaveCard = false,
                UserId = 12,
                VendorId = 12
            };
            return test;
        }
    }
}