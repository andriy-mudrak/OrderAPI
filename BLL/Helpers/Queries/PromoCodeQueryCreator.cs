using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Helpers.Extensions;
using BLL.Helpers.Queries.Interfaces;
using DAL.Entities;

namespace BLL.Helpers.Queries
{
    public class PromoCodeQueryCreator : IPromoCodeQueryCreator
    {
        public async Task<Expression<Func<PromoCode , bool>>> Get(int promoCodeId)
        {
            return dto => promoCodeId.IsZero() || dto.PromoCodeId == promoCodeId;
        }
    }
}