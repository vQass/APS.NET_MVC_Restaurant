using System;
using System.Security.Cryptography;
using System.Text;

namespace Restauracja_MVC.Providers
{
    public static class Hasher
    {
        public static string GenerateHash(string input, string salt = "")
        {
            using var alg = new HMACSHA256(GetBytes(salt));
            var result = alg.ComputeHash(GetBytes(input));
            return Convert.ToBase64String(result);
        }

        private static byte[] GetBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}
