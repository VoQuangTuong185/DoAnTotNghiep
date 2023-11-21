using DoAnTotNghiep.Models.Entities;
using System.ComponentModel.DataAnnotations;
using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class Product : BaseEntity
    {
        [MaxLength(255)]
        public string ProductName { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        [MaxLength(255)]
        public string Image { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public int Quanity { get; set; }
        public int SoldQuantity { get; set; }
        public int CategoryId { get; set; }
        public virtual Category category { get; set; }
        public int BrandId { get; set; }
        public virtual Brand brand { get; set; }
        public virtual IList<Cart> P_carts { get; set; }
        public string? ImageDetail { get; set; }
        public ICollection<Feedback> feedbacks { get; set; } = new List<Feedback>();
    }
}
