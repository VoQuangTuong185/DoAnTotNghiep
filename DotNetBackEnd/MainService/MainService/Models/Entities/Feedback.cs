using System.ComponentModel.DataAnnotations;
using WebAppAPI.Models.Bases;

namespace WebAppAPI.Models.Entities
{
    public class Feedback : DbEntity
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        [MaxLength(255)]
        public string? Comments { get; set; }
        public int Votes { get; set; }
        public int OrderId { get; set; }
        public virtual Product product { get; set; }
        public virtual User users { get; set; }
        public virtual Order orders { get; set; }
        [MaxLength(255)]
        public string? AdminReply { get; set; }
        public DateTimeOffset? ReplyDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
