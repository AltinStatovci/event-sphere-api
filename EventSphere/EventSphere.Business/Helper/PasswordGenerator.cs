using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace EventSphere.Business.Helper
{
    public static class PasswordGenerator
    {
        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Generates a hash using the provided plain password and salt
        public static byte[] GenerateHash(string plainPass, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                password: plainPass,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
        }

        // Verifies if the entered password matches the stored hash
        public static bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            var enteredHash = GenerateHash(enteredPassword, storedSalt);
            return CryptographicOperations.FixedTimeEquals(enteredHash, storedHash);
        }
    }
}
