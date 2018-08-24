using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Lykke.Service.Assets.Tools
{
    public static class StringExtensions
    {
        private static readonly MD5 MD5Calculator;

        static StringExtensions()
        {
            MD5Calculator = MD5.Create();
        }

        public static string GetMD5(this string @string)
        {
            var stringBytes = Encoding.ASCII.GetBytes(@string);
            var stringHash = MD5Calculator.ComputeHash(stringBytes);

            var sb = new StringBuilder();

            foreach (var @byte in stringHash)
            {
                sb.Append(@byte.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
