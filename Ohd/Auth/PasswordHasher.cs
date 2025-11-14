using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ohd.Utils
{
    public static class PasswordHasher
    {
        // Hash with SHA-256 -> returns binary bytes
        public static byte[] Hash(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            using var sha = SHA256.Create();
            return sha.ComputeHash(bytes);
        }

        public static bool Verify(string password, byte[] storedHash)
        {
            if (storedHash == null) return false;
            var computed = Hash(password);
            return computed.SequenceEqual(storedHash);
        }
    }
}