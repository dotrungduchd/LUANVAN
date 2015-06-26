using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ActiveDomain
{
    class FileHelper
    {
        /// <summary>
        /// Save IV to file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="filePathtToSave"></param>
        /// <param name="IVstring"></param>
        public static void SaveIVToFile(string filePath, string filePathEncrypted, string IVstring)
        {
            // Save IV into file
            List<string> allLines = new List<string>();
            allLines.Add(filePathEncrypted);
            allLines.Add(IVstring);
            allLines.Add(Convert.ToBase64String(Signer.Sign(Encoding.ASCII.GetBytes(IVstring), Global.ID, Global.PASSWORD)));
            File.AppendAllLines(filePath, allLines);
            File.SetAttributes(filePath, FileAttributes.Hidden);
        }

        /// <summary>
        /// Read IV from file and verify.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="filePathToRead"></param>
        /// <param name="IV"></param>
        /// <returns>IV if success, otherwise return null</returns>
        public static void ReadIVFromFile(string filePath, string filePathEncrypted, ref byte[] IV)
        {
            string[] data = File.ReadAllLines(filePath);
            for (int i = data.Length - 1; i >= 0; i--)
            {
                if (filePathEncrypted.Contains(data[i]))
                {
                    // Get IV
                    if (data[i + 1] != null){
                        string[] IVnumbers = data[i + 1].Split(' ');
                        for (int j = 0; j < IV.Length; j++)
                        {
                            IV[j] = (byte)int.Parse(IVnumbers[j]);
                        }
                        // Verify IV to authenticate user
                        if (data[i + 2] != null && Signer.Verity(IV, Convert.FromBase64String(data[i + 2]), Global.KEY01, Global.KEY02))
                            break;
                    }

                    IV = null;
                    break;
                }
            }
        }
    }
}
