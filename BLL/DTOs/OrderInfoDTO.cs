namespace BLL.DTOs
{
    public class OrderInfoDTO
    {
        public int CustomerId { get; set; }
        public string Token { get; set; }
        public string Currency { get; set; }
        public bool SaveCard { get; set; }
    }
}