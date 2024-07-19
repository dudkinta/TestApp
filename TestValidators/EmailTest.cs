using RegistrationService.Services;

namespace TestValidators
{
    public class Tests
    {
        IEmailValidator emailValidator;

        [SetUp]
        public void Setup()
        {
            emailValidator = new EmailValidator();
        }

        [Test]
        public void ValidEmail_WithStandardFormat()
        {
            var email = "example@example.com";
            var result = emailValidator.IsEmailValid(email);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidEmail_WithSubdomain()
        {
            var email = "example@mail.example.com";
            var result = emailValidator.IsEmailValid(email);
            Assert.IsTrue(result);
        }

        [Test]
        public void InvalidEmail_WithMissingAtSymbol()
        {
            var email = "example.com";
            var result = emailValidator.IsEmailValid(email);
            Assert.IsFalse(result);
        }

        [Test]
        public void InvalidEmail_WithMultipleAtSymbols()
        {
            var email = "example@@example.com";
            var result = emailValidator.IsEmailValid(email);
            Assert.IsFalse(result);
        }

        [Test]
        public void InvalidEmail_WithInvalidDomain()
        {
            var email = "example@example";
            var result = emailValidator.IsEmailValid(email);
            Assert.IsFalse(result);
        }

        [Test]
        public void InvalidEmail_WithSpecialCharacters()
        {
            var email = "example@exa!mple.com";
            var result = emailValidator.IsEmailValid(email);
            Assert.IsFalse(result);
        }

        [Test]
        public void InvalidEmail_WithEmptyString()
        {
            var email = "";
            var result = emailValidator.IsEmailValid(email);
            Assert.IsFalse(result);
        }

        [Test]
        public void InvalidEmail_WithWhitespace()
        {
            var email = " ";
            var result = emailValidator.IsEmailValid(email);
            Assert.IsFalse(result);
        }

        [Test]
        public void NullEmail()
        {
            string email = null;
            var result = emailValidator.IsEmailValid(email);
            Assert.IsFalse(result);
        }
    }
}