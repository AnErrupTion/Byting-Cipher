using System;
using System.Security.Cryptography;
using System.Text;

namespace Byting_Encrypt_Algorithm
{
    public class SecureRandoms
    {
        private static readonly RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();

        public static int Number(int maxValue, int minValue = 0)
        {
            if (minValue >= maxValue) throw new ArgumentOutOfRangeException(nameof(minValue));
            long diff = maxValue - minValue;
            long upperBound = uint.MaxValue / diff * diff;
            uint ui;
            do { ui = Unsigned(); } while (ui >= upperBound);
            return (int)(minValue + (ui % diff));
        }

        public static string String(int size)
        {
            byte[] bytes = Bytes(size);
            StringBuilder sb = new StringBuilder(bytes.Length);
            foreach (byte b in bytes) sb.Append(b.ToString("x2"));
            char[] chars = sb.ToString().Remove(bytes.Length, bytes.Length).ToCharArray();
            sb.Clear();
            foreach (char c in chars)
                switch (Number(2))
                {
                    case 0: sb.Append(c.ToString().ToLower()); break;
                    case 1: sb.Append(c.ToString().ToUpper()); break;
                }
            return sb.ToString();
        }

        public static uint Unsigned()
        {
            byte[] randomBytes = Bytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        public static byte[] Bytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            csp.GetBytes(buffer);
            return buffer;
        }
    }
}
