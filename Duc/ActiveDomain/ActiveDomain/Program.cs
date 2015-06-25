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
            byte[] iv = Signer.Sign(Encoding.ASCII.GetBytes("ivneemyeuoi"), "key", "salt");
            string ivstring = Convert.ToBase64String(iv);
            byte[] iv2 = Convert.FromBase64String(ivstring);
            Console.WriteLine(Signer.Verity(iv2, Encoding.ASCII.GetBytes("ivneemyeuoi"), "key", "salt"));
        }

        public static void TestDomain()
        {
            // Get current User login on Windows
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            Console.WriteLine(userName);
            // print: DOMAIN/tamtq

            // Gets the network domain name associated with the current user
            string userdomain = Environment.UserDomainName;
            Console.WriteLine(userdomain);
            // print: DOMAIN

            // Get current full Domain 
            // Gets an object that provides information about the local computer's network connectivity and traffic statistics.
            string domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            Console.WriteLine(domain);
            Console.ReadKey();
            // print: domain.local



            // Get all user in domain
            // Run in any windows client
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, domain))
                {
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    {
                        foreach (var result in searcher.FindAll())
                        {
                            DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                            Console.WriteLine("First Name: " + de.Properties["givenName"].Value);
                            Console.WriteLine("Last Name : " + de.Properties["sn"].Value);
                            Console.WriteLine("SAM account name   : " + de.Properties["samAccountName"].Value); // print: tamtq
                            Console.WriteLine("User principal name: " + de.Properties["userPrincipalName"].Value); //print: tamtq@domain.local
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            Console.ReadLine();

        }
    }
}
