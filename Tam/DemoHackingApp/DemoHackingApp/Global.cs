using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoHackingApp
{
    public class Global
    {
        public static List<string> extensions = new List<string> { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".rar", ".zip" };
        public static List<int> hookProcessed = new List<int>();
        public static List<string> USBDrives = new List<string>();
        public static List<bool> USBExist = new List<bool>();
        public static List<string> defaultPrograms = new List<string>();
        public static Dictionary<string, byte[]> FileInformations = new Dictionary<string, byte[]>();

        #region Authentication
        public static string ID = "";
        public static string PASSWORD = "";
        public static string DOMAIN_NAME = "";
        public static string USER = "";

        public static int AUTH_TYPE = (int)AuthType.Personal;
        public static bool REMEMBER_ME = false;
        public static int DOMAIN_PERMIT = (int)DomainPermission.OnlyMe;

        public static string KEY01 = "";
        public static string KEY02 = "";

        public static string METADATA_FILENAME = "";

        public static AuthenticationForm authForm = new AuthenticationForm();
        public static ExtensionsForm extsForm = new ExtensionsForm();
        public static void GetListExtensions()
        {
            extensions = Config.GetListExtensions();
        }

        public static void Initialization()
        {
            GetListExtensions();
            getDefaultPrograms();

            try
            {
                AUTH_TYPE = int.Parse(Config.GetAppSetting(Config.AUTH_TYPE));
                DOMAIN_PERMIT = int.Parse(Config.GetAppSetting(Config.DOMAIN_PERMIT));
                REMEMBER_ME = bool.Parse(Config.GetAppSetting(Config.REMEMBER_ME));
                ID = Config.GetAppSetting(Config.ID);
                PASSWORD = Config.GetAppSetting(Config.PASSWORD);
            }
            catch
            {
            }

        }
        #endregion

        public static List<string> getDefaultPrograms()
        {
            List<string> defaultPrograms = new List<string>();

            RegistryKey oHKCR = Registry.ClassesRoot;
            RegistryKey oOpenCmd,oProgID;

            for (int i = 0; i < extensions.Count; i++)
            {
                try
                {
                    oProgID = oHKCR.OpenSubKey(extensions[i]);
                    string proId = oProgID.GetValue(null).ToString();
                    oProgID.Close();
                    oOpenCmd = oHKCR.OpenSubKey(proId + "\\shell\\open\\command");
                    string strExe = oOpenCmd.GetValue(null).ToString();
                    oOpenCmd.Close();
                    if (!defaultPrograms.Contains(strExe))
                    {
                        defaultPrograms.Add(strExe);
                    }
                }
                catch (Exception)
                {
                }
            }
            return defaultPrograms;
            
        }


    }
}
