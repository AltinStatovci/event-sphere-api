using System.Text.RegularExpressions;

namespace EventSphere.Business.Validator.password;

public class PasswordValidator : IPasswordValidator
{
    public void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length <= 7)
        {
            throw new ArgumentException("Password must be more than 7 characters long.");
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            throw new ArgumentException("Password must contain at least one uppercase letter.");
        }

        if (!Regex.IsMatch(password, @"\d"))
        {
            throw new ArgumentException("Password must contain at least one number.");
        }
    }
}