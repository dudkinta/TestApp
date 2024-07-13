namespace RegistrationService.Services
{
    public interface IPasswordService
    {
        string HashPassword(string user, string password);
        bool VerifyPassword(string user, string hashPassword, string password);
    }
}
