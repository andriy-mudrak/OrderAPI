//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AutoMapper;
//using BLL.DTOs;
//using BLL.Fake.Models.Item;
//using BLL.Fake.Models.Shipment;
//using BLL.Fake.Services.Interfaces;
//using BLL.Helpers.Queries.Interfaces;
//using BLL.Services.Interfaces;
//using Chronicle;
//using DAL.Entities;
//using DAL.Repositories.Interfaces;

//namespace BLL.Services
//{
//    public class SagaData
//    {
//        public bool IsItemReceived { get; set; }
//        public bool IsShipmentReceived { get; set; }
//        public bool IsOrderReceived { get; set; }
//        public bool IsPaymentReceived { get; set; }
//        public IEnumerable<ItemReservedModel> Items { get; set; }
//        public ShipmentModelDTO Shipment { get; set; }
//        public OrderDTO Order { get; set; }
//        public PaymentModel Payment { get; set; }
//    }


//    public class SagaOrderTransaction : Saga<SagaData>,
//        ISagaStartAction<IEnumerable<ItemModelDTO>>,
//        ISagaAction<ShipmentModelDTO>,
//        ISagaAction<OrderInfoDTO>,
//        ISagaAction<PaymentInfoDTO>
//    {
//        private readonly IShipmentMethodService _shipmentService;
//        private readonly IItemService _itemService;
//        private readonly IPaymentService _paymentService;
//        private readonly ICustomerRepository _customerRepository;
//        private readonly ICustomerQueryCreator _customerQueryCreator;
//        private readonly IOrderQueryCreator _orderQueryCreator;
//        private readonly IOrderRepository _orderRepository;
//        private readonly IMapper _mapper;

//        public SagaOrderTransaction(IShipmentMethodService shipmentService, IItemService itemService,
//            ICustomerRepository customerRepository, ICustomerQueryCreator customerQueryCreator, IPaymentService paymentService,
//            IOrderQueryCreator orderQueryCreator, IOrderRepository orderRepository, IMapper mapper)
//        {
//            _customerQueryCreator = customerQueryCreator;
//            _customerRepository = customerRepository;
//            _shipmentService = shipmentService;
//            _itemService = itemService;
//            _paymentService = paymentService;
//            _orderQueryCreator = orderQueryCreator;
//            _orderRepository = orderRepository;
//            _mapper = mapper;
//        }

//        public async Task HandleAsync(IEnumerable<ItemModelDTO> models, ISagaContext context)
//        {
//            Data.Items = await _itemService.Reservation(models);
//            if (Data.Items.Any(x => x.Status == false)) throw new Exception();//TODO: Create custom exception

//            Data.IsItemReceived = true;

//            CompleteSaga();
//            await Task.CompletedTask;
//        }

//        public async Task CompensateAsync(IEnumerable<ItemModelDTO> models, ISagaContext context)
//        {
//            await _itemService.CancelReservation(models);
//            await Task.CompletedTask;
//        }

//        public async Task HandleAsync(ShipmentModelDTO shipment, ISagaContext context)
//        {
//            Data.Shipment = await _shipmentService.Create(shipment);
//            await Task.CompletedTask;
//        }

//        public async Task CompensateAsync(ShipmentModelDTO shipment, ISagaContext context)
//        {
//            await _shipmentService.Cancel(Data.Shipment);
//            await Task.CompletedTask;
//        }

//        public async Task HandleAsync(OrderInfoDTO model, ISagaContext context)
//        {
//            var paymentTotal = Data.Items.Sum(x => x.Price);

//            var order = new OrderDTO()
//            {
//                CreateDate = DateTime.Now,
//                Currency = model.Currency,
//                CustomerId = model.CustomerId,
//                TrackingNo = Data.Shipment.TrackingNumber,
//                AddressId = Data.Shipment.Address.AddressId,
//                ShippingTotal = Data.Shipment.Method.Price,
//                ShippingMethod = Data.Shipment.Method.Name,
//                PaymentTotal = paymentTotal,
//                SubTotal = Data.Shipment.Method.Price + paymentTotal,
//                Status = "???" //TODO : what is that?
//            };
//            Data.Order = _mapper.Map<OrderDTO>(await _orderRepository.Create(_mapper.Map<Order>(order)));

//            Data.IsOrderReceived = true;

//            CompleteSaga();
//            await Task.CompletedTask;
//        }

//        public async Task CompensateAsync(OrderInfoDTO message, ISagaContext context)
//        {
//            await _orderRepository.Delete(_mapper.Map<Order>(Data.Order));
//            await Task.CompletedTask;
//        }

//        public async Task HandleAsync(PaymentInfoDTO paymentInfo, ISagaContext context)
//        {
//            var customerQuery = await _customerQueryCreator.Get(paymentInfo.CustomerId);
//            var customer = await _customerRepository.Get(customerQuery);

//            var payment = new PaymentModel()
//            {
//                Amount = (long)Data.Order.PaymentTotal,
//                CardToken = paymentInfo.Token,
//                Currency = paymentInfo.Currency,
//                UserId = paymentInfo.CustomerId,
//                Email = customer.LastOrDefault().EmailAndress,
//                SaveCard = paymentInfo.SaveCard,
//                OrderId = Data.Order.OrderId,
//                VendorId = 1, // TODO: create in DB
//            };

//            Data.Payment = await _paymentService.Charge(payment);
//            Data.IsPaymentReceived = true;

//            CompleteSaga();
//            await Task.CompletedTask;
//        }

//        public async Task CompensateAsync(PaymentInfoDTO payment, ISagaContext context)
//        {
//            await _paymentService.Refund(new PaymentModel()
//            {
//                Email = Data.Payment.Email,
//                OrderId = Data.Payment.OrderId,
//            });
//            await Task.CompletedTask;
//        }

//        private void CompleteSaga()
//        {
//            if (Data.IsItemReceived
//                && Data.IsPaymentReceived
//                && Data.IsShipmentReceived
//                && Data.IsOrderReceived)
//            {
//                Complete();
//            }
//        }

//    }
//}