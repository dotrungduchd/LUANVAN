using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoHackingApp
{
    public class AppSettings
    {
        public static string GetAppSetting(string key)
        {
            return Config.GetSetting(Config.APPSETTING, key);
        }

        /// <summary>
        /// Add new section appSetting, update if it is existed
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSetting(string key, string value)
        {
            Config.AddNewSetting(Config.APPSETTING, key, value);
        }

        /// <summary>
        /// Update section appSetting, add new if not exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        public static void UpdateAppSetting(string key, string newValue)
        {
            Config.UpdateExistSetting(Config.APPSETTING, key, newValue);
        }

        public static void DeleteAppSetting(string key)
        {
            Config.DeleteExistSetting(Config.APPSETTING, key);
        }
    }
}
