using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LoadConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nDatabase Servers:");
            foreach (var fileExtension in FileExtensions.FileExtensionsList)
            {
                Console.WriteLine(fileExtension);
            }
            Console.ReadKey();
        }
    }
}
