using THUCTAPTOTNGHIEP.Models.Entities;
using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quanity { get; set; }
        public int SoldQuantity { get; set; }
        public int CategoryId { get; set; }
        public virtual Category category { get; set; }
        public int BrandId { get; set; }
        public virtual Brand brand { get; set; }
        public virtual IList<Cart> P_carts { get; set; }
    }
}
