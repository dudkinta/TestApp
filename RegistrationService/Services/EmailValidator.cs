using System.Net.Mail;
using System.Text.RegularExpressions;

namespace RegistrationService.Services
{
    public class EmailValidator : IEmailValidator
    {
        public bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                _ = new MailAddress(email);

                // более строгая проверка, т.к. стандартный MailAddress не проверят наличие субдомена и пропускает спецсимволы
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                return emailRegex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
}
