using DoAnTotNghiep.Models.Entities;
using WebAppAPI.Models.Entities;

namespace WebAppAPI.DTO
{
    public class ProductDTOShow
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public int Quanity { get; set; }
        public int? SoldQuantity { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public bool IsActive { get; set; } = true;
        public string[] ImageDetail { get; set; }
        public virtual Category category { get; set; } = null;
        public virtual Brand brand { get; set; } = null;
        public virtual IList<Cart> P_carts { get; set; } = null;
        public int AverageVote { get; set; }
    }
}
