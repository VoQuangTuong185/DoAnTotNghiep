namespace WebAppAPI.DTO
{
    public class OrderDetailDTO
    {
        public string? ProductName { get; set; }
        public string? Image { get; set; }
        public Double? Price { get; set; }
        public Double Discount { get; set; }
        public int Quantity { get; set; }
    }
}
