using Microsoft.AspNetCore.Identity;
using Backend.Modules.Users.Domain.Entities;
namespace Backend.Shared.Services {

    public class PasswordService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string hashedPassword, string passwordToCheck)
        {
            var userInfo = new User(user.Name, user.Email);
            
            return _passwordHasher.VerifyHashedPassword(userInfo, hashedPassword, passwordToCheck) != PasswordVerificationResult.Failed;
        }
    }
}
