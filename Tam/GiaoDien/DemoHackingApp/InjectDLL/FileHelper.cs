using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InjectDLL
{
    public static class FileHelper
    {
        private const int NUM_ROW = 3;
        public static void SaveFileInfo(string metadataFilePath, List<string> listData)
        {
            File.AppendAllLines(metadataFilePath, listData);
            File.SetAttributes(metadataFilePath, FileAttributes.Hidden);
        }

        public static List<string> ReadFileInfo(string metadataFilePath, string filePathToRead)
        {
            List<string> listData = new List<string>();
            List<string> data = File.ReadAllLines(metadataFilePath).ToList();
            for (int i = 0; i < data.Count; i += NUM_ROW)
            {
                if (data[i].Contains(filePathToRead))
                {
                    listData.AddRange(data.GetRange(i, NUM_ROW));
                    break;
                }
            }

            return listData;
        }

    }
}
