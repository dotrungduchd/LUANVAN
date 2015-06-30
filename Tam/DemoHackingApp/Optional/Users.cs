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
    public static class Users
    {
        public static Dictionary<string, string> UsersList = new Dictionary<string, string>();


        /// <summary>
        /// Constructor.
        /// </summary>
        static Users()
        {
            var Users = ConfigurationManager.GetSection("Users") as NameValueCollection;
            if (Users != null)
            {
                foreach (string userKey in Users.AllKeys)
                {
                    UsersList.Add(userKey, Users[userKey]);
                }
            }
        }

        public static void AddNewUser(string key, string value)
        {
            if (UsersList.Keys.Contains(key))
                return;
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            var node = xmlDoc.CreateElement("add");
            node.SetAttribute("key", key);
            node.SetAttribute("value", value);

            xmlDoc.SelectSingleNode("//Users").AppendChild(node);
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("Users");

            UsersList.Add(key, value);
        }

        public static void EditExistUser(string key, string newValue)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            xmlDoc.SelectSingleNode("//Users/add[@key='" + key + "']").Attributes["value"].Value = newValue;
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("Users");

            UsersList.Remove(key);
            UsersList.Add(key, newValue);
        }

        public static void DeleteExistUser(string key)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            XmlNode node = xmlDoc.SelectSingleNode("//Users/add[@key='" + key + "']");
            if (node == null) return;
            node.ParentNode.RemoveChild(node);

            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("Users");

            UsersList.Remove(key);
        }

    }
}
