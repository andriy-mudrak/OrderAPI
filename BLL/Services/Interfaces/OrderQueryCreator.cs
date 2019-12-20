using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Helpers.Extensions;
using BLL.Helpers.Queries.Interfaces;
using DAL.Entities;

namespace BLL.Helpers.Queries
{
    public class OrderQueryCreator: IOrderQueryCreator
    {
        public async Task<Expression<Func<Order, bool>>> Get(int orderId)
        {
            return dto => orderId.IsZero() || dto.OrderId == orderId;
        }
    }
}