using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs;
using BLL.Fake.Models.Item;
using BLL.Fake.Services.Interfaces;
using BLL.Helpers.Queries.Interfaces;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using MicroserviceMessages;
using RawRabbit;

namespace BLL.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly IShipmentMethodService _shipmentService;
        private readonly IItemService _itemService;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerQueryCreator _customerQueryCreator;
        private readonly IOrderQueryCreator _orderQueryCreator;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IBusClient _busClient;

        public OrderingService(IShipmentMethodService shipmentService, IItemService itemService,
            ICustomerRepository customerRepository, ICustomerQueryCreator customerQueryCreator,
            IOrderQueryCreator orderQueryCreator, IBusClient busClient, IOrderRepository orderRepository, IMapper mapper)
        {
            _customerQueryCreator = customerQueryCreator;
            _customerRepository = customerRepository;
            _shipmentService = shipmentService;
            _itemService = itemService;
            _orderQueryCreator = orderQueryCreator;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _busClient = busClient;
        }

        public async Task<OrderDTO> Create(OrderingDTO model)
        {
            var paymentTotal = model.Items.Sum(x => x.Price);
            var order = new OrderDTO()
            {
                CreateDate = DateTime.Now,
                Currency = model.OrderInfo.Currency,
                CustomerId = model.OrderInfo.CustomerId,
                TrackingNo = "as",// TODO: перемістити трекінг намбер в іншу таблицю
                AddressId = model.Shipment.Address.AddressId,
                ShippingTotal = model.Shipment.Method.Price,
                ShippingMethod = model.Shipment.Method.Name,
                PaymentTotal = paymentTotal,
                SubTotal = model.Shipment.Method.Price + paymentTotal,
                Status = "???" //TODO : what is that?
            };
            var orderResponse = await _orderRepository.Create(_mapper.Map<Order>(order));

            var items = await _itemService.Reservation(new ItemRequestModel() { Items = model.Items, OrderId = orderResponse.OrderId });
            if (items.Any(x => x.Status == false)) throw new Exception();//TODO: Create custom exception

            var shipmentRequest = model.Shipment;
            shipmentRequest.OrderId = orderResponse.OrderId;
            var shipment = await _shipmentService.Create(model.Shipment);

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

            await _busClient.PublishAsync(GetPayment(paymentTotal, model, customer, orderResponse));
            
            return order;
        }

        public static PaymentModel GetPayment(int paymentTotal, OrderingDTO model, IEnumerable<Customer> customer, Order orderResponse)
        {
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
            return payment;
        }
    }
}