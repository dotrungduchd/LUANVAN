using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadConfig
{
    public static class FileExtensions
    {
        public static IEnumerable<string> FileExtensionsList 
        { 
            get 
            { 
                foreach (var extension in _fileExtensionsList)
                { yield return extension; }
            }
        }

        private static readonly List<string> _fileExtensionsList = new List<string>();

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
                    AddFileExtension(fileExtensions[fileExtensionKey]);

					// If we wanted to, we could also store the Key string as the user-friendly name of the database server with something like:
					// AddDatabaseServer(databaseServerKey, databaseServers[databaseServerKey]);
					// We would just need to modify the DatabaseServersList to be a Dictionary or other structure that can hold 2 string values rather than a List<string>.
				}
			}
		}

        public static void AddFileExtension(string fileExtension)
		{
            if (fileExtension == null)
				return;

            if (!_fileExtensionsList.Contains(fileExtension))
                _fileExtensionsList.Add(fileExtension);
		}

        public static void RemoveFileExtension(string fileExtension)
		{
            if (fileExtension == null)
				return;

            if (_fileExtensionsList.Contains(fileExtension))
                _fileExtensionsList.Remove(fileExtension);
		}
    }
}
