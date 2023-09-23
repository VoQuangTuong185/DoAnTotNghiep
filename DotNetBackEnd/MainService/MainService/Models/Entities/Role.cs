using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class Role : DbEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<UserAPI> UserRoles { get; set; }
    }
}
