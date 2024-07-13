using System.Net.Mail;

namespace RegistrationService.Services
{
    public class EmailValidator: IEmailValidator
    {
        public bool IsEmailValid(string email)
        {
            try
            {
                _ = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
