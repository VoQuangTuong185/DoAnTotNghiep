using WebAppAPI.Models.Bases;
using WebAppAPI.Models.Entities;

namespace WebAppAPI.Models.Entities
{
    public class VIP : DbEntity
    {
        public int Id { get; set; }
        public double PriceFrom { get; set; }
        public double PriceTo { get; set; }
        public int Discount { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual List<User>? users { get; set; }
    }
}
