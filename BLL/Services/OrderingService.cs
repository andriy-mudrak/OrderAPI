using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs;
using BLL.Fake.Models.Item;
using BLL.Fake.Services.Interfaces;
using BLL.Helpers.MQ.Interfaces;
using BLL.Helpers.Queries.Interfaces;
using BLL.Services.Interfaces;
using Chronicle;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BLL.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly IShipmentMethodService _shipmentService;
        private readonly IItemService _itemService;
        private readonly IPaymentService _paymentService;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerQueryCreator _customerQueryCreator;
        private readonly IOrderQueryCreator _orderQueryCreator;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublish _rpcClient;

        public OrderingService(IShipmentMethodService shipmentService, IItemService itemService,
            ICustomerRepository customerRepository, ICustomerQueryCreator customerQueryCreator, IPaymentService paymentService,
            IOrderQueryCreator orderQueryCreator, IOrderRepository orderRepository, IMapper mapper, IRabbitMQPublish rpcClient)
        {
            _customerQueryCreator = customerQueryCreator;
            _customerRepository = customerRepository;
            _shipmentService = shipmentService;
            _itemService = itemService;
            _paymentService = paymentService;
            _orderQueryCreator = orderQueryCreator;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _rpcClient = rpcClient;
        }

        public async Task<OrderDTO> Create(OrderingDTO model)
        {
            var items = await _itemService.Reservation(model.Items);
            if (items.Any(x => x.Status == false)) throw new Exception();//TODO: Create custom exception




            var shipment =  await _shipmentService.Create(model.Shipment);




            var paymentTotal = items.Sum(x => x.Price);
            var order = new OrderDTO()
            {
                CreateDate = DateTime.Now,
                Currency = model.OrderInfo.Currency,
                CustomerId = model.OrderInfo.CustomerId,
                TrackingNo = shipment.TrackingNumber,
                AddressId = model.Shipment.Address.AddressId,
                ShippingTotal = model.Shipment.Method.Price,
                ShippingMethod = model.Shipment.Method.Name,
                PaymentTotal = paymentTotal,
                SubTotal = model.Shipment.Method.Price + paymentTotal,
                Status = "???" //TODO : what is that?
            };
            var orderResponse = await _orderRepository.Create(_mapper.Map<Order>(order));




            var customerQuery = await _customerQueryCreator.Get(model.OrderInfo.CustomerId);
            var customer = await _customerRepository.Get(customerQuery);

            var payment = new PaymentModel()
            {
                Amount = paymentTotal,
                CardToken = model.OrderInfo.Token,
                Currency = model.OrderInfo.Currency,
                UserId = model.OrderInfo.CustomerId,
                Email = customer.LastOrDefault().EmailAndress,
                SaveCard = model.OrderInfo.SaveCard,
                OrderId = orderResponse.OrderId,
                VendorId = 1, // TODO: create in DB,
                Type = "charge"
            };

            // _messageHandler.Send(payment, "payment");
            _rpcClient.Send(payment, "payment");

            //var result =  await _paymentService.Charge(payment);


            //_messageHandler.Close();
            return new OrderDTO();
            //var items = await _itemService.Reservation(model.Items);
            //if (items.Any(x=>x.Status == false)) throw new Exception();//TODO: Create custom exception

            //var shipmentResponse = await _shipmentService.Create(model.Shipment);

            //var customerQuery = await _customerQueryCreator.Get(model.OrderInfo.CustomerId);
            //var customer = await _customerRepository.Get(customerQuery);

            //var paymentTotal = items.Sum(x => x.Price);

            //var order = new Order()
            //{
            //    CreateDate = DateTime.Now,
            //    Currency = model.OrderInfo.Currency,
            //    CustomerId = model.OrderInfo.CustomerId,
            //    TrackingNo = shipmentResponse.TrackingNumber,
            //    AddressId = shipmentResponse.Address.AddressId,
            //    ShippingTotal = shipmentResponse.Method.Price,
            //    ShippingMethod = shipmentResponse.Method.Name,
            //    PaymentTotal = paymentTotal,
            //    SubTotal = shipmentResponse.Method.Price + paymentTotal,
            //    Status = "???" //TODO : what is that?
            //};
            //var orderResponse = _mapper.Map<OrderDTO>(await _orderRepository.Create(order));

            //var payment = new PaymentModel()
            //{
            //    Amount = paymentTotal,
            //    CardToken = model.OrderInfo.Token,
            //    Currency = model.OrderInfo.Currency,
            //    UserId = model.OrderInfo.CustomerId,
            //    Email = customer.LastOrDefault().EmailAndress,
            //    SaveCard = model.OrderInfo.SaveCard,
            //    Type = "charge",
            //    OrderId = orderResponse.OrderId,
            //    VendorId = 1, // TODO: create in DB
            //};

            //var paymentResponse = await _paymentService.Charge(payment);
            //return (orderResponse);
        }
    }
}