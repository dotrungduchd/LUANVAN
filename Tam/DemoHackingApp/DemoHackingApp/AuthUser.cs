using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoHackingApp
{
    /// <summary>
    /// This class authenticate user by id+password or domain+username
    /// </summary>
    public class AuthUser
    {
        public static bool Authenticate(AuthType authType)
        {
            switch (authType)
            {
                case AuthType.Personal:

                    break;
                case AuthType.Domain:

                    break;
                default:
                    return false;
            }
            return false;
        }
    }

    public enum AuthType : int
    {
        Personal = 0,
        Domain = 1
    }

    public enum FilePermit : int
    {
        OnlyMe = 0,
        AllUserInDomain = 1,
        OnlySomeUserInDomain = 2
    }
}
