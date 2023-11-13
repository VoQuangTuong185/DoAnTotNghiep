namespace doantotnghiep.DTO
{
    public class FeedbackShowDetail
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string? LoginName { get; set; }
        public string? UserName { get; set; }
        public string? Comments { get; set; }
        public int Votes { get; set; }
        public int OrderId { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? AdminReply { get; set; }
    }
}

