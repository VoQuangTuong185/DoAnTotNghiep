using System.ComponentModel.DataAnnotations;
using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class Feedback : BaseEntity
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string? Comments { get; set; }
        public int Votes { get; set; }
        public int OrderId { get; set; }
        public virtual Product product { get; set; }
        public virtual User users { get; set; }
        public string? AdminReply { get; set; }
        public DateTimeOffset? ReplyDate { get; set; }
    }
}
