namespace EventSphere.Business.Helper;

 public interface IPasswordGenerator
    {
        byte[] GenerateSalt();
        byte[] GenerateHash(string plainPass, byte[] salt);
        bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt);
    }