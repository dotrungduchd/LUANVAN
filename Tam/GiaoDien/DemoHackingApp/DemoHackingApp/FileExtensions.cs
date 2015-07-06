using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DemoHackingApp
{
    public enum ExtensionSupportedType : int
    {
        Yes = 1,
        No = 0
    }

    public static class FileExtensions
    {
        public static Dictionary<string, string> FileExtensionsList = new Dictionary<string, string>();


        /// <summary>
        /// Constructor.
        /// </summary>
        static FileExtensions()
        {
            FileExtensionsList = Config.GetDictionarySetting(Config.FILEEXTENSION);
        }

        /// <summary>
        /// Get list extensions is enable from config
        /// </summary>
        /// <returns></returns>
        public static List<string> GetListExtensionsSupported()
        {
            List<string> listExt = new List<string>();
            Dictionary<string, string> fileExtensions = Config.GetDictionarySetting(Config.FILEEXTENSION);

            for (int i = 0; i < fileExtensions.Count; i++)
                if (fileExtensions.Values.ToList()[i] == ((int)ExtensionSupportedType.Yes).ToString())
                    listExt.Add(fileExtensions.Keys.ToList()[i]);
            return listExt;
        }

        public static void SaveListExtensions(List<string> listExt)
        {
            List<string> fileExt = FileExtensions.FileExtensionsList.Keys.ToList();
            for (int j = 0; j < listExt.Count; j++)
            {
                if (fileExt.Contains(listExt[j]))
                {
                    FileExtensions.UpdateExistExtension(listExt[j], ((int)ExtensionSupportedType.Yes).ToString());
                }
                else
                {
                    FileExtensions.UpdateExistExtension(listExt[j], ((int)ExtensionSupportedType.No).ToString());
                }
            }
        }

        public static void AddNewExtension(string key, string value)
        {
            Config.AddNewSetting(Config.FILEEXTENSION, key, value);

            FileExtensionsList.Add(key, value);
        }

        public static void UpdateExistExtension(string key, string newValue)
        {
            Config.UpdateExistSetting(Config.FILEEXTENSION, key, newValue);

            FileExtensionsList.Remove(key);
            FileExtensionsList.Add(key, newValue);
        }

        public static void DeleteExistExtension(string key)
        {
            Config.DeleteExistSetting(Config.FILEEXTENSION, key);

            FileExtensionsList.Remove(key);
        }

    }
}
