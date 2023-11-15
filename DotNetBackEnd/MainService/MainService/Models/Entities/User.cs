using WebAppAPI.Models.Bases;
using System.ComponentModel.DataAnnotations;
using doantotnghiep.Models.Entities;

namespace WebAppAPI.Models.Entities
{
    public class User : DbEntity
    {
        [Key]
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string? TelNum { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AddressCode { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual VIP? vips { get; set; }
        public virtual List<UserAPI>? UserAPIs { get; set; }
        public virtual List<Cart>? U_carts { get; set; }
        public virtual List<Order>? orders { get; set; }
        public virtual List<Feedback>? feedbacks { get; set; }
    }
}
