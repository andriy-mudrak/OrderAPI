//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using AutoMapper;
//using BLL.DTOs;
//using BLL.Helpers.Queries.Interfaces;
//using BLL.Services.Interfaces;
//using DAL.Entities;
//using DAL.Repositories.Interfaces;
//using Flurl.Http;

//namespace BLL.Services
//{
//    public class OrderService : IOrderService
//    {
//        private readonly IOrderRepository _orderRepository;
//        private readonly IOrderQueryCreator _orderQueryCreator;
//        private readonly IMapper _mapper;

//        public OrderService(IOrderRepository orderRepository, IOrderQueryCreator orderQueryCreator, IMapper mapper) 
//        {
//            _orderRepository = orderRepository;
//            _orderQueryCreator = orderQueryCreator;
//            _mapper = mapper;
//        }


//        private async Task CreatePayment(OrderDTO model)
//        {
//            var response = await "http://localhost:53783/api/payment"
//                .PostJsonAsync(new PaymentModel()
//                {
//                    CardToken = "tok_visa",
//                    Currency = "usd",
//                    Amount = 10000,
//                    UserId = 2222,
//                    OrderId = 2222,
//                    VendorId = 2222,
//                    Email = "admin@mail.com",
//                    SaveCard = false,
//                    Type = "charge"
//                });

//            if (!response.IsSuccessStatusCode) await _orderRepository.Delete(_mapper.Map<Order>(model));
//        }

//        public async Task<IEnumerable<OrderDTO>> Get(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<OrderDTO> Detele(OrderDTO model)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<OrderDTO> Create(OrderDTO model)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<IEnumerable<OrderDTO>> Update(IEnumerable<OrderDTO> model)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}