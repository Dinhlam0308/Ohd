using System.Security.Cryptography;
using System.Text;

namespace Ohd.Utils
{
    public static class TokenHasher
    {
        public static byte[] Hash(string token)
        {
            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(token));
            }
        }
    }
}