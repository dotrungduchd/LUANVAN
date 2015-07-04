using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoHackingApp
{
    public class ProcessInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 InClientPID)
        {
            Console.WriteLine("FileMon has been installed in target {0}.\r\n", InClientPID);            
        }

        public void OnProcessing(Int32 InClientPID, String[] InFileNames)
        {
            for (int i = 0; i < InFileNames.Length; i++)
            {
                Console.WriteLine(InFileNames[i]);
            }
        }

        public void ReportException(Exception InInfo)
        {
            Console.WriteLine("The target process has reported an error:\r\n" + InInfo.ToString());

        }
        public void Ping()
        {
        }
        public string[] getExtensions()
        {
            return Global.extensions.ToArray();
        }
        public string[] getUSBDrives()
        {
            if(Global.USBDrives.Count> 0)
                return Global.USBDrives.ToArray();
            return null;
        }

        public string[] getDefaultPrograms()
        {
            return Global.defaultPrograms.ToArray();
        }

        public byte[] getIV(string filePath)
        {
            if (Global.FileInformations.Keys.Contains(filePath))
            {
                return Global.FileInformations[filePath];
            }
            return null;
        }

        public void addIV(string filePath, byte[] IV)
        {
            if (!Global.FileInformations.Keys.Contains(filePath))
                Global.FileInformations.Add(filePath, IV);
            else
                Global.FileInformations[filePath] = IV;
        }
        public string[] getUSBLetter()
        {
            return Global.USBDrives.ToArray();
        }
        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public string getKey02()
        {
            return Global.getKey02();
        }
    }    
}
