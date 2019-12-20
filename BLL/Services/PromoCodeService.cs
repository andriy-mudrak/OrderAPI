using AutoMapper;
using BLL.DTOs;
using BLL.Helpers.Queries.Interfaces;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
    public class PromoCodeService : ServiceBase<PromoCodeDTO, PromoCode>, IPromoCodeService
    {
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IPromoCodeQueryCreator _promoCodeQueryCreator;
        private readonly IMapper _mapper;
        public PromoCodeService(IPromoCodeRepository promoCodeRepository, IPromoCodeQueryCreator promoCodeQueryCreator, IMapper mapper) : base(promoCodeRepository, promoCodeQueryCreator, mapper)
        {
            _promoCodeRepository = promoCodeRepository;
            _promoCodeQueryCreator = promoCodeQueryCreator;
            _mapper = mapper;
        }
    }
}