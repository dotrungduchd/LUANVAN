using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DemoHackingApp
{
    public class Config
    {
        public static string AUTH_TYPE = "AuthType";
        public static string DOMAIN_PERMIT = "DomainPermit";
        public static string REMEMBER_ME = "RememberMe";
        public static string ID = "ID";
        public static string PASSWORD = "Password";


        #region Extensions

        /// <summary>
        /// Get list extensions is enable from config
        /// </summary>
        /// <returns></returns>
        public static List<string> GetListExtensions()
        {
            List<string> listExt = new List<string>();
            for (int i = 0; i < FileExtensions.FileExtensionsList.Keys.Count; i++)
                if(FileExtensions.FileExtensionsList[FileExtensions.FileExtensionsList.Keys.ToList()[i]] == "True")
                    listExt.Add(FileExtensions.FileExtensionsList.Keys.ToList()[i]);
            return listExt;
        }

        /// <summary>
        /// Get authenticate from config file
        /// </summary>
        /// <returns>Personal or Domain mode</returns>
        public static AuthType GetAuthenticationMode()
        {
            if (ConfigurationManager.AppSettings["AuthMode"].ToString() == "Personal")
                return AuthType.Personal;
            return AuthType.Domain;
        }

        public static void SaveListExtensions(List<string> listExt)
        {

        }

        #endregion

        #region AppSetting
        public static string GetAppSetting(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
                return null;
            return ConfigurationManager.AppSettings[key].ToString();
        }

        /// <summary>
        /// Add new section appSetting, update if it is existed
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSetting(string key, string value)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                UpdateAppSetting(key, value);
                return;
            }
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Update section appSetting, add new if not exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        public static void UpdateAppSetting(string key, string newValue)
        {
            if (ConfigurationManager.AppSettings[key] == null){
                AddAppSetting(key, newValue);
                return;
            }
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = newValue;
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void DeleteAppSetting(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
                return;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }

        #endregion


    }
}
