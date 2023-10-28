using WebAppAPI.Models.Entities;

namespace WebAppAPI.DTO
{
    public class UserPermisson
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public Role? role { get; set; }
        public User? user { get; set; }
    }
}
