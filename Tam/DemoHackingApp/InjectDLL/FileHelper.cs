using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoHackingApp
{
    class FileHelper
    {
        /// <summary>
        /// Save IV to file. Use digital signature to authen ID or User domain and integrity data
        /// line 0: file name
        /// line 1: IV
        /// line 2: ID or User in Domain
        /// line 3: Signature
        /// </summary>
        /// <param name="metadataFilePath"></param>
        /// <param name="filePathtToSave"></param>
        /// <param name="IVstring"></param>
        public static void SaveIVToFile(string metadataFilePath, string filePathToSave, string IVstring)
        {
            // Save IV into file
            List<string> allLines = new List<string>();
            allLines.Add(filePathToSave);
            allLines.Add(IVstring);
            allLines.Add(Global.KEY01);
            allLines.Add(Convert.ToBase64String(Signer.Sign(Encoding.ASCII.GetBytes(Global.KEY01), Global.KEY01, Global.KEY02)));
            File.AppendAllLines(metadataFilePath, allLines);
            File.SetAttributes(metadataFilePath, FileAttributes.Hidden);
        }

        /// <summary>
        /// Read IV from file and verify.
        /// line 0: file name
        /// line 1: IV
        /// line 2: ID or User in Domain
        /// line 3: Signature
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="filePathToRead"></param>
        /// <param name="IV"></param>
        /// <returns>true if function load IV and verify success, otherwise return false</returns>
        public static bool ReadIVFromFile(string metadataFilePath, string filePath, ref byte[] IV)
        {
            string[] data;
            data = File.ReadAllLines(metadataFilePath);
            for (int i = data.Length - 1; i >= 0; i--)
            {
                // Compare file name - line 0
                if (filePath.Contains(data[i]))
                {
                    // Check ID or User Domain = KEY01 - line 2
                    if (!string.IsNullOrEmpty(Global.USBDRIVER) && data[i + 2] != null && !data[i + 2].Contains(Global.KEY01))
                        return false;

                    // Get IV - line 1
                    string[] IVnumbers = data[i + 1].Split(' ');
                    for (int j = 0; j < IV.Length; j++)
                    {
                        IV[j] = (byte)int.Parse(IVnumbers[j]);
                    }

                    // Verify IV to authenticate user - line 2 3. Return true if verify success
                    if (data[i + 2] != null && data[i + 3] != null && Signer.Verity(Convert.FromBase64String(data[i + 2]),
                        Convert.FromBase64String(data[i + 3]), Global.KEY01, Global.KEY02))
                        return true;
                    return false;
                }
            }
            return false;
        }
    }
}
