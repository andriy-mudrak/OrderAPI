using AutoMapper;
using BLL.DTOs;
using BLL.Helpers.Queries.Interfaces;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
    public class CustomerService : ServiceBase<CustomerDTO, Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerQueryCreator _customerQueryCreator;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, ICustomerQueryCreator customerQueryCreator, IMapper mapper) : base(customerRepository, customerQueryCreator, mapper)
        {
            _customerRepository = customerRepository;
            _customerQueryCreator = customerQueryCreator;
            _mapper = mapper;
        }
    }
}