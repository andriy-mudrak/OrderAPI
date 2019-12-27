namespace DAL.Entities
{
    public partial class OrderItem
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public int Uop { get; set; }

        public virtual Order Order { get; set; }
    }
}
