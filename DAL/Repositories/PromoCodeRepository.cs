using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class PromoCodeRepository : RepositoryBase<PromoCode>, IPromoCodeRepository
    {
        public PromoCodeRepository(OrderDbTestContext context) : base(context)
        {
        }
    }
}
