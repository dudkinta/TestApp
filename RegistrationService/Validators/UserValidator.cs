using FluentValidation;
using RegistrationService.Models;
using RegistrationService.Services;

namespace RegistrationService.Validators
{
    public class UserValidator : AbstractValidator<RegistrationUserModel>
    {
        public UserValidator(IEmailValidator emailValidator, IPasswordService passwordService)
        {
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Must(emailValidator.IsEmailValid).WithMessage("Invalid email format.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Must(passwordService.IsPasswordValid).WithMessage("Invalid password format.");

        }
    }
}
