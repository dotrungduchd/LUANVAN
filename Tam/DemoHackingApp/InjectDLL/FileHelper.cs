using DemoHackingApp;
using InjectDLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InjectDLL
{
    public class FileHelper
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
        public static void SaveIVToFile(string metadataFilePath, string filePathToSave, string IVstring, string key02)
        {
            if (!File.Exists(metadataFilePath))
                File.Create(metadataFilePath);

            string hashKey02String = Convert.ToBase64String(Signer.Hash(key02));
            // Save IV into file
            List<string> allLines = new List<string>();
            allLines.Add(filePathToSave);
            allLines.Add(IVstring);
            allLines.Add(hashKey02String);
            allLines.Add(Convert.ToBase64String(Signer.Sign(Convert.FromBase64String(hashKey02String), Signer.KeySign)));
            File.AppendAllLines(metadataFilePath, allLines);
            File.SetAttributes(metadataFilePath, FileAttributes.Hidden);
        }

        /// <summary>
        /// Read IV from file and verify.
        /// line 0: file name
        /// line 1: IV
        /// line 2: Hash Password or Domain name
        /// line 3: Signature of line 2
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="filePathToRead"></param>
        /// <param name="IV"></param>
        /// <returns>true if function load IV and verify success, otherwise return false</returns>
        public static bool ReadIVFromFile(string metadataFilePath, string filePath, ref byte[] IV, string key02)
        {
            if (!File.Exists(metadataFilePath))
            {
                File.Create(metadataFilePath);
                return false;
            }

            string hashKey02String = Convert.ToBase64String(Signer.Hash(key02));
            string[] data;
            data = File.ReadAllLines(metadataFilePath);

            if (data == null) return false;

            for (int i = data.Length - 1; i >= 0; i--)
            {
                // Step 0 - Compare file name - line 0
                if (filePath.Contains(data[i]))
                {

                    // Step 1 - hash password or Domain name == hash(KEY02) - line 2
                    if (data[i + 2] == null || (data[i + 2] != null && !data[i + 2].Contains(hashKey02String)))
                        return false;

                    // Step 2 - Verify hash password or Domain name to authenticate user - line 2 3. Return true if verify success
                    if (data[i + 2] == null || data[i + 3] == null || !Signer.Verity(Convert.FromBase64String(data[i + 2]),
                        Convert.FromBase64String(data[i + 3]), Signer.KeySign))
                        return false;

                    // Step 3 - Get IV - line 1
                    if (data[i + 1] == null) return false;
                    string[] IVnumbers = data[i + 1].Split(' ');
                    if (IVnumbers.Length != IV.Length) return false;
                    for (int j = 0; j < IV.Length; j++)
                    {
                        IV[j] = (byte)int.Parse(IVnumbers[j]);
                    }

                    return true;
                }
            }
            return false;
        }
    }
}
