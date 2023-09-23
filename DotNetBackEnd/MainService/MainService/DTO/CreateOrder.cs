namespace WebAppAPI.DTO
{
    public class CreateOrder
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? TelNum { get; set; }
        public string? Email { get; set; }
        public string? Provinces { get; set; }
        public string? Districts { get; set; }
        public string? Wards { get; set; }
        public string? Streets { get; set; }
        public string? Payment { get; set; }
    }
}
