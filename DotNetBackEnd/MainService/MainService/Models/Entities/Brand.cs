using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class Brand : DbEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string BrandName { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual List<Product> products { get; set; }
    }
}
