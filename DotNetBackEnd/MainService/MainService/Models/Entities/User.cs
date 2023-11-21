using WebAppAPI.Models.Bases;
using System.ComponentModel.DataAnnotations;
using doantotnghiep.Models.Entities;

namespace WebAppAPI.Models.Entities
{
    public class User : DbEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string LoginName { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10)]
        public string? TelNum { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(255)]
        public string AddressCode { get; set; }
        public bool IsActive { get; set; } = true;
        public int VipsId { get; set; }
        public virtual VIP? vips { get; set; }
        public virtual List<UserAPI>? UserAPIs { get; set; }
        public virtual List<Cart>? U_carts { get; set; }
        public virtual List<Order>? orders { get; set; }
        public virtual List<Feedback>? feedbacks { get; set; }
    }
}
