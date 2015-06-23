using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDomain
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get current User login on Windows
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            Console.WriteLine(userName);
            Console.ReadKey();

            

            // Get all user in domain
            // Run in any windows client
            using (var context = new PrincipalContext(ContextType.Domain, "domain.local"))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        Console.WriteLine("First Name: " + de.Properties["givenName"].Value);
                        Console.WriteLine("Last Name : " + de.Properties["sn"].Value);
                        Console.WriteLine("SAM account name   : " + de.Properties["samAccountName"].Value);
                        Console.WriteLine("User principal name: " + de.Properties["userPrincipalName"].Value);
                        Console.WriteLine();
                    }
                }
            }
            Console.ReadLine();

        }
    }
}
