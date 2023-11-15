using WebAppAPI.Models.Bases;
using WebAppAPI.Models.Entities;

namespace doantotnghiep.Models.Entities
{
    public class VIP : DbEntity
    {
        public int Id { get; set; }
        public double PriceFrom { get; set; }
        public double PriceTo { get; set; }
        public int Discount { get; set; }
        public virtual List<User>? users { get; set; }
    }
}
