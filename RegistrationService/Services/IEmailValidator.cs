namespace RegistrationService.Services
{
    public interface IEmailValidator
    {
        bool IsEmailValid(string email);
    }
}
