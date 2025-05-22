using System.Security.Cryptography;

namespace BudgetManager.Helpers
{
    public class PasswordHelper
    {
        public static byte[] HashPassword(string password, byte[] salt, int iterations = 10000, int hashSize = 32)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(hashSize);
            }
        }
        public static bool VerifyPassword(string enteredPassword, string storedSalt, string storedPassword)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);
            byte[] hashOfInput = HashPassword(enteredPassword, salt);
            return Convert.ToBase64String(hashOfInput) == storedPassword;
        }
    }
}
