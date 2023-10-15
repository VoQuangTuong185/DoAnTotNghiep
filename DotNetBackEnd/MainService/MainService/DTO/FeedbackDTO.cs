using WebAppAPI.Models.Entities;

namespace DoAnTotNghiep.DTO
{
    public class FeedbackDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string? Comments { get; set; }
        public int Votes { get; set; }
        public int OrderId { get; set; }
    }
}
