using Microsoft.AspNetCore.Identity;
using Backend.Models;

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
        return _passwordHasher.VerifyHashedPassword(user, hashedPassword, passwordToCheck) != PasswordVerificationResult.Failed;
    }
}
