using System;

namespace BLL.DTOs
{
    public class PromoCodeDTO
    {
        public int PromoCodeId { get; set; }
        public int Discount { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
    }
}
