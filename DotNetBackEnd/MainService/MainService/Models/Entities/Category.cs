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

        [Required]
        public int ExternalID { get; set; }

        [Required]
        public string CategoryName { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
