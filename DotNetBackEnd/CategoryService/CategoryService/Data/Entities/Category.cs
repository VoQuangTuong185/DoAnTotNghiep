using System.ComponentModel.DataAnnotations;

namespace CategoryService.Data.Entities
{
    public class Category : DbEntity
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
