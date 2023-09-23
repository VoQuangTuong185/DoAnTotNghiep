using WebAppAPI.Models.Entities;

namespace WebAppAPI.DTO
{
    public class RegisterUserDTO
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public string? TelNum { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
