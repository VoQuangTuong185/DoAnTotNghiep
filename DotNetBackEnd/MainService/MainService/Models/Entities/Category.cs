using System.ComponentModel.DataAnnotations;
using WebAppAPI.Models.Bases;
using WebAppAPI.Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DoAnTotNghiep.Models.Entities
{
    public class Category : DbEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
