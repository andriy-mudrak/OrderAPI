using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Helpers.Extensions;
using BLL.Helpers.Queries.Interfaces;
using DAL.Entities;

namespace BLL.Helpers.Queries
{
    public class CustomerQueryCreator : ICustomerQueryCreator

    {
        public async Task<Expression<Func<Customer, bool>>> Get(int customerId)
        {
            return dto => customerId.IsZero() || dto.CustomerId == customerId;
        }
    }
}