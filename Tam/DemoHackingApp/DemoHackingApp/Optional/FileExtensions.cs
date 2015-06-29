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
    public static class FileExtensions
    {
        public static Dictionary<string, string> FileExtensionsList = new Dictionary<string, string>();


        /// <summary>
        /// Constructor.
        /// </summary>
        static FileExtensions()
        {
            // Grab the Database Servers listed in the App.config and add them to our list.
            var fileExtensions = ConfigurationManager.GetSection("FileExtensions") as NameValueCollection;
            if (fileExtensions != null)
            {
                foreach (string fileExtensionKey in fileExtensions.AllKeys)
                {
                    FileExtensionsList.Add(fileExtensionKey, fileExtensions[fileExtensionKey]);
                }
            }
        }

        public static void AddNewExtension(string key, string value)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            var node = xmlDoc.CreateElement("add");
            node.SetAttribute("key", key);
            node.SetAttribute("value", value);

            xmlDoc.SelectSingleNode("//FileExtensions").AppendChild(node);
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("FileExtensions");

            FileExtensionsList.Add(key, value);
        }

        public static void EditExistExtension(string key, string newValue)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            xmlDoc.SelectSingleNode("//FileExtensions/add[@key='" + key + "']").Attributes["value"].Value = newValue;
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("FileExtensions");

            FileExtensionsList.Remove(key);
            FileExtensionsList.Add(key, newValue);
        }

        public static void DeleteExistextension(string key)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            XmlNode node = xmlDoc.SelectSingleNode("//FileExtensions/add[@key='" + key + "']");
            if (node == null) return;
            node.ParentNode.RemoveChild(node);

            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("FileExtensions");

            FileExtensionsList.Remove(key);
        }

    }
}
