using DoAnTotNghiep.Models.Entities;
using WebAppAPI.Models.Entities;

namespace AdminService.DTO
{
    public class ProductHomeDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public int Quanity { get; set; }
        public int SoldQuantity { get; set; }
        public int CategoryId { get; set; }
        public virtual Category category { get; set; }
        public int BrandId { get; set; }
        public virtual Brand brand { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
