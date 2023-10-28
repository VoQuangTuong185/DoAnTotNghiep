using Microsoft.Extensions.Hosting;
using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class Brand : DbEntity
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual List<Product> products { get; set; }
    }
}
