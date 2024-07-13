namespace RegistrationService.Models
{
    public class RegistrationUserModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
    }
}
