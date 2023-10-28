namespace WebAppAPI.DTO
{
    public class UserAdminDTO
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? TelNum { get; set; }
        public string? Address { get; set; }
    }
}
