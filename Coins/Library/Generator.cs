using System;
using System.Security.Cryptography;
using System.Text;

namespace Coins.Library
{
    public class Generator
    {
        public static string StringToHash(string password)
        {
            var hashPassword = SHA1.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = hashPassword.ComputeHash(bytes);

            return Convert.ToBase64String(hashBytes);
        }

        public static string CreateStringKey()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }
    }
}
