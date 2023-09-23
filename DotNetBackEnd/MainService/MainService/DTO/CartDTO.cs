namespace WebAppAPI.DTO
{
    public class CartDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string CategoryName { get; set; }   
        public int Quanity { get; set; }
    }
}
