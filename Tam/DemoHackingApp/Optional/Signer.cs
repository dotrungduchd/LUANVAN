using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DemoHackingApp
{
    /// <summary>
    /// This class is digital sign
    /// </summary>
    public class Signer
    {
        public static byte[] Hash(string content)
        {
            byte[] contentByte = Encoding.ASCII.GetBytes(content);
            using (HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                return hashAlgorithm.ComputeHash(contentByte);
            }
        }

        public static byte[] Sign(byte[] content, string id, string password)
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = id + password;

            //Create a new instance of the RSACryptoServiceProvider class.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048, cspParams);

            byte[] data = RSA.Encrypt(content, false);
            return data;
        }

        public static bool Verity(byte[] content, byte[] signature, string id, string password)
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = id + password;

            //Create a new instance of the RSACryptoServiceProvider class.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048, cspParams);

            byte[] data = RSA.Decrypt(content, false);

            if (Encoding.ASCII.GetString(data) == Encoding.ASCII.GetString(signature))
                return true;
            return false;
        }
    }
}
