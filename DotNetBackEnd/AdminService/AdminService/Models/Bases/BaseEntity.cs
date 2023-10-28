using System;
using System.ComponentModel.DataAnnotations;
namespace WebAppAPI.Models.Bases
{
    public class DbEntity
    {
    }
    public class BaseEntity : DbEntity
    {
        public int Id { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
