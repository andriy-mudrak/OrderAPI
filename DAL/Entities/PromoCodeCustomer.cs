namespace DAL.Entities
{
    public partial class PromoCodeCustomer
    {
        public int PromoCodeId { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual PromoCode PromoCode { get; set; }
    }
}
