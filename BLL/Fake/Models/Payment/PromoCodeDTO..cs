using System;

namespace BLL.DTOs
{
    public class PromoCodeDTO
    {
        public int PromoCodeId { get; set; }
        public double Value { get; set; }
        public double Percentage { get; set; }
        public DateTime EndDate { get; set; }
    }
}
