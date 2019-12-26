//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using BLL.DTOs;
//using BLL.Fake.Services.Interfaces;

//namespace BLL.Fake.Services
//{
//    public class PromoCodeServiceFake : IPromoCodeService
//    {
//        private static Dictionary<string, PromoCodeDTO> promoCodes = new Dictionary<string, PromoCodeDTO>()
//        {
//            { "Promo20", new PromoCodeDTO { PromoCodeId = 1, Value=0, Percentage=20, EndDate = DateTime.Now.AddYears(1)} },
//            { "First10", new PromoCodeDTO { PromoCodeId = 2,Value=10, Percentage=0, EndDate = DateTime.Now.AddYears(1)} },
//        };


//        public async Task<Dictionary<string, PromoCodeDTO>> GetPromos()
//        {
//            return promoCodes;
//        }

//        public async Task<string> UsePromo(string promo)
//        {
//            throw new System.NotImplementedException();
//        }

//        public async Task<string> CreatePromo(string name, PromoCodeDTO promo)
//        {
//            promoCodes.
//        }
//    }
//}