using WebAppAPI.Models.Bases;
using WebAppAPI.Models.Entities.WebAppAPI.Models.Entities;

namespace WebAppAPI.Models.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
        public string? Address { get; set; }
        public double? TotalBill { get; set; }
        public string Payment { get; set; }
        public virtual User? user { get; set; }
        public virtual List<OrderDetail> orderDetails { get; set; }
    }
}
