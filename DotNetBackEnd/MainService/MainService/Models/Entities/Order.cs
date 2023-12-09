using System.ComponentModel.DataAnnotations;
using WebAppAPI.Models.Bases;
using WebAppAPI.Models.Entities.WebAppAPI.Models.Entities;

namespace WebAppAPI.Models.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
        public double TotalBill { get; set; }
        [MaxLength(50)]
        public string Payment { get; set; }
        public int DiscountVIP { get; set; }
        [MaxLength(255)]
        public string? Remark { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        public virtual User? user { get; set; }
        public virtual List<OrderDetail> orderDetails { get; set; }
        public virtual List<Feedback> feedbacks { get; set; }
    }
}
