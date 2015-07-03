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
        public static string currentDomain;

        #region Authentication
        public static string ID = "";
        public static string PASSWORD = "";
        public static string DOMAIN_NAME = "";
        public static string USER_DOMAIN_NAME = "";
        public static string USBDRIVER = "";

        public static int AUTH_TYPE = (int)AuthType.Personal;
        public static bool REMEMBER_ME = false;
        public static int DOMAIN_PERMIT = (int)DomainPermission.OnlyMe;

        public static string KEY01 = "ABC"; // = ID = USER_DOMAIN_NAME
        public static string KEY02 = "XYZ"; // = PASSWORD = DOMAIN_NAME

        public static string METADATA_FILENAME = "data.tam";

        public static void GetListExtensions()
        {
            extensions = Config.GetListExtensions();
        }

        public static string GetDomainName()
        {
            // Gets an object that provides information about the local computer's network connectivity and traffic statistics.
            string domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            // print: domain.local
            return domain;
        }

        public static string GetUserDomainName()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            // print: DOMAIN/tamtq
            return userName;
        }

        public static void Initialization()
        {
            // File extensions
            GetListExtensions();
            getDefaultPrograms();

            // Authentication
            DOMAIN_NAME = GetDomainName();
            USER_DOMAIN_NAME = GetUserDomainName();

            ID = Config.GetAppSetting(Config.ID);
            PASSWORD = Config.GetAppSetting(Config.PASSWORD);

            try
            {
                AUTH_TYPE = int.Parse(Config.GetAppSetting(Config.AUTH_TYPE));
                REMEMBER_ME = bool.Parse(Config.GetAppSetting(Config.REMEMBER_ME));
                ID = Config.GetAppSetting(Config.ID);
                PASSWORD = Config.GetAppSetting(Config.PASSWORD);
                USER_DOMAIN_NAME = Config.GetAppSetting(Config.USER_DOMAIN_NAME);
                DOMAIN_NAME = Config.GetAppSetting(Config.DOMAIN_NAME);
                DOMAIN_PERMIT = int.Parse(Config.GetAppSetting(Config.DOMAIN_PERMIT));
                if (AUTH_TYPE == (int)AuthType.Personal)
                {
                    KEY01 = ID;
                    KEY02 = PASSWORD;
                }
                else if (AUTH_TYPE == (int)AuthType.Domain)
                {
                    KEY01 = USER_DOMAIN_NAME;
                    KEY02 = DOMAIN_NAME;
                }
            }
            catch
            {
            }

            UpdateAuthType((AuthType)AUTH_TYPE);


        }

        public static void UpdateAuthType(AuthType authType)
        {
            if (authType == (int)AuthType.Personal)
            {
                KEY01 = ID;
                KEY02 = PASSWORD;
            }
            else
            {
                KEY01 = USER_DOMAIN_NAME;
                KEY02 = DOMAIN_NAME;
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
