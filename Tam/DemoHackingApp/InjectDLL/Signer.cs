using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InjectDLL
{
    /// <summary>
    /// This class is digital sign
    /// </summary>
    public class Signer
    {
        // Key of Application to authenticate user and integrity of data
        private const string keySign = "abcd1234@#$%";
        private const string saltSign = "!@#$%^&*()";
        static Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(keySign, Encoding.ASCII.GetBytes(saltSign));
        public static string KeySign = Encoding.ASCII.GetString(key.GetBytes(1024));
        public static byte[] Hash(string content)
        {
            byte[] contentByte = Encoding.ASCII.GetBytes(content);
            using (HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                return hashAlgorithm.ComputeHash(contentByte);
            }
        }

        public static byte[] Sign(byte[] content, string key)
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = key;

            //Create a new instance of the RSACryptoServiceProvider class.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048, cspParams);

            byte[] data = RSA.Encrypt(content, false);
            return data;
        }

        public static bool Verity(byte[] content, byte[] signature, string key)
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = key;

            //Create a new instance of the RSACryptoServiceProvider class.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048, cspParams);

            byte[] data = RSA.Decrypt(content, false);

            if (Encoding.ASCII.GetString(data) == Encoding.ASCII.GetString(signature))
                return true;
            return false;
        }
    }
}
