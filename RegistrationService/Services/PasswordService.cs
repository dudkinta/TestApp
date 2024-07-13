﻿using Microsoft.AspNetCore.Identity;

namespace RegistrationService.Services
{
    public class PasswordService: IPasswordService
    {
        private readonly PasswordHasher<string> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<string>();
        }

        public string HashPassword(string user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(string user, string hashPassword, string password)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, hashPassword, password);
            return verificationResult == PasswordVerificationResult.Success;
        }
    }
}