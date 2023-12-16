namespace AdminService.DTO
{
    public class RevenuesStatistical
    {
        public int Month { get; set; }
        public int Count { get; set; }
        public int TotalMoney { get; set; }
        public RevenuesStatistical(int Month, int Count, int totalMoney)
        {
            this.Month = Month;
            this.Count = Count;
            this.TotalMoney = totalMoney;
        }
    }
}
