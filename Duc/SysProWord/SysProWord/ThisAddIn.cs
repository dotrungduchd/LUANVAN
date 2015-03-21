using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using System.Security.Cryptography;
using System.IO;


namespace SysProWord
{
    public partial class ThisAddIn
    {

        byte[] _data, _key, _IV; // Data, Key, IV
        TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
        AesCryptoServiceProvider myAes = new AesCryptoServiceProvider();

        public void Init()
        {
            // If random key so next time program won't decrypt correct
            string k = "215840236279510254623514", iv = "6528450965284509";
            tDES.BlockSize = 64;
            tDES.Padding = PaddingMode.PKCS7;
            myAes.BlockSize = 128;
            myAes.Padding = PaddingMode.PKCS7;
            _key = new byte[24];
            _IV = new byte[16];
            _key = UTF8Encoding.UTF8.GetBytes(k);
            _IV = UTF8Encoding.UTF8.GetBytes(iv);
            //tDES.Key = _key;
            //tDES.IV = _IV;
            myAes.Key = _key;
            myAes.IV = _IV;

        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Init();


            this.Application.DocumentOpen += Application_DocumentOpen;

            //this.Application.DocumentBeforeSave += Application_DocumentBeforeSave;

            this.Application.DocumentBeforeClose += Application_DocumentBeforeClose;

        }

        void Application_DocumentBeforeClose(Word.Document Doc, ref bool Cancel)
        {
            if (Cancel) return;

            string sData = Doc.Content.Text.Remove(Doc.Content.Text.Length-1, 1);

            //byte[] Data = EncryptTextToMemory(sData, tDES.Key, tDES.IV);

            byte[] Data = EncryptStringToBytes_Aes(sData, myAes.Key, myAes.IV);

            Doc.Content.Text = new UnicodeEncoding().GetString(Data);
            
        }

        // Open Doc
        void Application_DocumentOpen(Word.Document Doc)
        {
            byte[] Data = new UnicodeEncoding().GetBytes(Doc.Content.Text.Remove(Doc.Content.Text.Length-1, 1));

            //Doc.Content.Text = DecryptTextFromMemory(Data, tDES.Key, tDES.IV);
            Doc.Content.Text = DecryptStringFromBytes_Aes(Data, myAes.Key, myAes.IV);
        }

        // Before save Doc
        void Application_DocumentBeforeSave(Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {

        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        // AES
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }


        // tDES
        public static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                // Create a CryptoStream using the MemoryStream  
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write);

                // Convert the passed string to a byte array. 
                byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(toEncrypt, 0, toEncrypt.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the  
                // MemoryStream that holds the  
                // encrypted data. 
                byte[] ret = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                // Return the encrypted buffer. 
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }

        }

        public static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a new MemoryStream using the passed  
                // array of encrypted data.
                MemoryStream msDecrypt = new MemoryStream(Data);

                // Create a CryptoStream using the MemoryStream  
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read);

                // Create buffer to hold the decrypted data. 
                byte[] fromEncrypt = new byte[Data.Length];

                // Read the decrypted data out of the crypto stream 
                // and place it into the temporary buffer.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                //Convert the buffer into a string and return it. 
                return new ASCIIEncoding().GetString(fromEncrypt);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
