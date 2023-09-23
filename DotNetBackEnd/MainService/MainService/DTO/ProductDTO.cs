using WebAppAPI.Models.Entities;

namespace WebAppAPI.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quanity { get; set; }
        public int? SoldQuantity { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
