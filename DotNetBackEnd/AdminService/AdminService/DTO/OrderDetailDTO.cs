namespace WebAppAPI.DTO
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Image { get; set; }
        public Double? Price { get; set; }
        public Double Discount { get; set; }
        public int? Votes { get; set; }
        public int Quantity { get; set; }
        public string? Comments { get; set; }
    }
}
