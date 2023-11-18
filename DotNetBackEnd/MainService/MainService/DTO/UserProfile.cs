using WebAppAPI.Models.Entities;

namespace WebAppAPI.DTO
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? TelNum { get; set; }
        public string? Provinces { get; set; }
        public string? Districts { get; set; }
        public string? Wards { get; set; }
        public int? ProvinceCode { get; set; }
        public int? DistrictCode { get; set; }
        public int? WardCode { get; set; }
        public string? Streets { get; set; }
        public int? Discount { get; set; }

    }
}
