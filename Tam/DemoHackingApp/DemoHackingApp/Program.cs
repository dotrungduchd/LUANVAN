using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DemoHackingApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new HackingAppForm());
            //Application.Run(new AuthenticationForm());
            //Application.Run(new ExtensionsForm());
            Config.GetListExtensions();
        }
    }
}
