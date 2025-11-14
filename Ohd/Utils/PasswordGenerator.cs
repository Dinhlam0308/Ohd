using System;
using System.Linq;

namespace Ohd.Utils
{
    public static class PasswordGenerator
    {
        private const string Lower = "abcdefghijklmnopqrstuvwxyz";
        private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "0123456789";
        private const string Special = "!@#$%^&*";

        public static string Generate(int length = 10)
        {
            if (length < 8)
                length = 8; // đảm bảo tối thiểu 8 ký tự

            var rnd = new Random();
            string pool = Lower + Upper + Numbers + Special;

            // Password random từ pool
            string pass = new string(
                Enumerable.Range(0, length)
                    .Select(_ => pool[rnd.Next(pool.Length)])
                    .ToArray()
            );

            // Đảm bảo đủ tiêu chí Medium
            if (!pass.Any(char.IsLower))
                pass = ReplaceRandom(pass, Lower[rnd.Next(Lower.Length)], rnd);

            if (!pass.Any(char.IsUpper))
                pass = ReplaceRandom(pass, Upper[rnd.Next(Upper.Length)], rnd);

            if (!pass.Any(char.IsDigit))
                pass = ReplaceRandom(pass, Numbers[rnd.Next(Numbers.Length)], rnd);

            if (!pass.Any(c => Special.Contains(c)))
                pass = ReplaceRandom(pass, Special[rnd.Next(Special.Length)], rnd);

            return pass;
        }

        private static string ReplaceRandom(string input, char c, Random rnd)
        {
            int index = rnd.Next(input.Length);
            char[] arr = input.ToCharArray();
            arr[index] = c;
            return new string(arr);
        }
    }
}