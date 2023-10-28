using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class Discount : BaseEntity
    {
        public DateTimeOffset StartTime { get; set; }
        public int Persents { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
