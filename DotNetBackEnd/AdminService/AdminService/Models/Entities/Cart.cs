using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class Cart : DbEntity
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }       
        public int Quantity { get; set; }
        public virtual User User { get; set; }
        public virtual Product product { get; set; } 
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
