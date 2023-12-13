namespace AdminService.DTO
{
    public class OrderStatistical
    {
        public string Status { get; set; }
        public int Count { get; set; }
        public OrderStatistical(string Status, int Count)
        {
            this.Status = Status;
            this.Count = Count; 
        }
    }
}
