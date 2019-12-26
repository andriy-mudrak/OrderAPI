using AutoMapper;
using BLL.DTOs;
using BLL.Fake.Models.Item;
using DAL.Entities;

namespace BLL.Helpers.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PromoCodeDTO, PromoCode>();
            CreateMap<PromoCodeDTO, PromoCode>().ReverseMap();

            CreateMap<OrderDTO, Order>();
            CreateMap<OrderDTO, Order>().ReverseMap();

            CreateMap<ItemModelDTO, ItemReservedModel>();
            CreateMap<ItemModelDTO, ItemReservedModel>().ReverseMap();
        }
    }
}