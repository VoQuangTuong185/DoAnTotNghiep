using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class UserAPI : DbEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }       
        public virtual User user { get; set; }
        public virtual Role role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
