using WebAppAPI.Models.Entities;

namespace WebAppAPI.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? TelNum { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public virtual List<UserAPI> UserAPIs { get; set; }
    }
}
