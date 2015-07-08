using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DemoHackingApp
{
    /// <summary>
    /// This class load all config from file
    /// </summary>
    public class Config
    {
        public static string AUTH_TYPE = "AuthType";
        public static string REMEMBER_ME = "RememberMe";
        public static string ID = "ID";
        public static string PASSWORD = "Password";
        public static string USER_DOMAIN_NAME = "UserDomainName";
        public static string DOMAIN_NAME = "DomainName";
        public static string DOMAIN_PERMIT = "DomainPermit";

        public static string APPSETTING = "//appSettings";
        public static string FILEEXTENSION = "//FileExtensions";
        public static string XMLPATH = "Config.xml";
        #region Extensions

        

        /// <summary>
        /// Get authenticate from config file
        /// </summary>
        /// <returns>Personal or Domain mode</returns>
        public static AuthType GetAuthenticationMode()
        {
            string authType = GetSetting(Config.APPSETTING, Config.AUTH_TYPE);
            if (authType == AuthType.Personal.ToString())
                return AuthType.Personal;
            else if (authType == AuthType.Domain.ToString())
                return AuthType.Domain;
            return AuthType.Personal;
        }

        

        #endregion

        #region XML Setting
        public static Dictionary<string, string> GetDictionarySetting(string settingPath)
        {
            Dictionary<string, string> dictionarySetting = new Dictionary<string, string>();

            var xmlDoc = new XmlDocument();
            //xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            xmlDoc.Load(XMLPATH);

            var xmlList = xmlDoc.SelectNodes(settingPath + "/add");
            foreach (XmlElement node in xmlList)
            {
                string key = node.Attributes[0].Value;
                string value = node.Attributes[1].Value;
                dictionarySetting.Add(key, value);
            }

            return dictionarySetting;
        }

        public static string GetSetting(string settingPath, string key)
        {
            var xmlDoc = new XmlDocument();
            //xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            xmlDoc.Load(XMLPATH);

            return xmlDoc.SelectSingleNode(settingPath + "/add[@key='" + key + "']").Attributes["value"].Value;
        }

        public static void AddNewSetting(string settingPath, string key, string value)
        {
            var xmlDoc = new XmlDocument();
            //xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            xmlDoc.Load(XMLPATH);

            var node = xmlDoc.CreateElement("add");
            node.SetAttribute("key", key);
            node.SetAttribute("value", value);

            xmlDoc.SelectSingleNode(settingPath).AppendChild(node);
            xmlDoc.Save(XMLPATH);
            
        }

        public static void UpdateExistSetting(string settingPath, string key, string newValue)
        {
            var xmlDoc = new XmlDocument();
            // xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            xmlDoc.Load(XMLPATH);

            xmlDoc.SelectSingleNode(settingPath + "/add[@key='" + key + "']").Attributes["value"].Value = newValue;
            xmlDoc.Save(XMLPATH);

        }

        public static void DeleteExistSetting(string settingPath, string key)
        {
            var xmlDoc = new XmlDocument();
            //xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            xmlDoc.Load(XMLPATH);

            XmlNode node = xmlDoc.SelectSingleNode(settingPath + "/add[@key='" + key + "']");
            if (node == null) return;
            node.ParentNode.RemoveChild(node);

            xmlDoc.Save(XMLPATH);

        }
        #endregion

        


    }
}
