using System;
using System.Collections.Generic;
using System.Configuration;
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

            LoadAppSetting();

            Application.Run(new HackingAppForm());
            //Application.Run(new AuthenticationForm());
            //Application.Run(new ExtensionsForm());

        }

        /// <summary>
        /// Load Application Setting
        /// Authentication type
        /// ID + Password if remember
        /// Domain field
        /// </summary>
        private static void LoadAppSetting()
        {
            Global.Initialization();
        }
    }
}
