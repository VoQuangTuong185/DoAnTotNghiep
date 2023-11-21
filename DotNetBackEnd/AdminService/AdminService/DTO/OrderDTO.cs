namespace WebAppAPI.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int ProductCount { get; set; }
        public double? TotalBill { get; set; }
        public string Payment { get; set; }
        public string? Address { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public int? DiscountVIP { get; set; }
        public DateTimeOffset orderDate { get; set; }
    }
}
