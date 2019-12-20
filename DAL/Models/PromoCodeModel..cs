using System;

namespace DAL.Models
{
    public class PromoCodeModel
    {
        public int PromoCodeId { get; set; }
        public int Discount { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
    }
}
