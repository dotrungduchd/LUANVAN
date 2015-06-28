using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DemoHackingApp
{
    public class Config
    {
        public static List<string> GetListExtensions()
        {
            List<string> listExt = new List<string>();
            for (int i = 0; i < FileExtensions.FileExtensionsList.Keys.Count; i++)
                if(FileExtensions.FileExtensionsList[FileExtensions.FileExtensionsList.Keys.ToList()[i]] == "true")
                    listExt.Add(FileExtensions.FileExtensionsList.Keys.ToList()[i]);
            return listExt;
        }

        public static AuthType GetAuthenticationMode()
        {
            if (ConfigurationManager.AppSettings["AuthMode"].ToString() == "Personal")
                return AuthType.Personal;
            return AuthType.Domain;
        }

        public static void SaveListExtensions(List<string> listExt)
        {

        }
    }
}
