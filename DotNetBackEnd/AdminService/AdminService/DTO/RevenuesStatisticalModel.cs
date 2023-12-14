namespace AdminService.DTO
{
    public class RevenuesStatisticalModel
    {
        public string Month { get; set; }
        public int TotalMoney { get; set; }
        public RevenuesStatisticalModel(string Month, int totalMoney)
        {
            this.Month = Month;
            this.TotalMoney = totalMoney;
        }
    }
}
