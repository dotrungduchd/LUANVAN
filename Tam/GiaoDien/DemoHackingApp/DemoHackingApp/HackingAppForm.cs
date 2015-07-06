using DemoHackingApp.Properties;
using EasyHook;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Windows.Forms;
using System.Resources;

namespace DemoHackingApp
{
    public partial class HackingAppForm : Form
    {
        public HackingAppForm()
        {
            InitializeComponent();

            Global.Initialization();

            // Authentication
            InitAuthenticate();

            // Set panel
            pnExtension.Hide();
            pnAuthenticate.Show();

            // File extensinon
            InitExtension();
        }


        #region SystemStray and Register
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea
                        (IntPtr hwnd, ref MARGINS margins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        static String ChannelName = null;
        NotifyIcon notifyIcon;
        public void InitNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Visible = false;
            notifyIcon.Icon = Resources.tho7mau_oYb_1;
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            notifyIcon.ContextMenuStrip = CreateMenuStripForNotifyIcon();
        }

        public ContextMenuStrip CreateMenuStripForNotifyIcon()
		{
			// Add the default menu options.
			ContextMenuStrip menu = new ContextMenuStrip();
			ToolStripMenuItem item;
			ToolStripSeparator sep;

			// Windows Explorer.
			item = new ToolStripMenuItem();
			item.Text = "Explorer";
			item.Click += new EventHandler(Explorer_Click);
			item.Image = Resources.Explorer;
			menu.Items.Add(item);

			// About.
			item = new ToolStripMenuItem();
			item.Text = "About";
			item.Click += new EventHandler(About_Click);
			item.Image = Resources.About;
			menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			menu.Items.Add(sep);

			// Exit.
			item = new ToolStripMenuItem();
			item.Text = "Exit";
			item.Click += new System.EventHandler(Exit_Click);
			item.Image = Resources.exit;
			menu.Items.Add(item);

			return menu;
		}

		/// <summary>
		/// Handles the Click event of the Explorer control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Explorer_Click(object sender, EventArgs e)
		{
            notifyIcon.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
		}

		/// <summary>
		/// Handles the Click event of the About control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void About_Click(object sender, EventArgs e)
		{
            MessageBox.Show("System Protection - New beta for everyone");
		}

		/// <summary>
		/// Processes a menu item.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Exit_Click(object sender, EventArgs e)
		{
			// Quit without further ado.
			Application.Exit();
		}

        #endregion

        #region Origion

        private void HackingAppForm_Load(object sender, EventArgs e)
        {
            InitNotifyIcon();
            this.Icon = Resources.tho7mau_oYb_1;

            #region Registry
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\data.tam"))
            {
                File.Create(Directory.GetCurrentDirectory() + @"\data.tam");
            }

            if (registryKey.GetValue("DemoHackingApp", null) == null)
            {
                registryKey.SetValue("DemoHackingApp", Application.ExecutablePath);
            }
            #endregion

            #region USB
            lock (Global.USBExist)
            {
                DriveInfo[] drives = DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Removable).ToArray();
                for (int i = 0; i < drives.Length; i++)
                {
                    Global.USBDrives.Add(drives[i].Name);
                    Global.USBExist.Add(false);
                }
            }

            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent " +
                                               "WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent " +
                                               "WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
            removeWatcher.Start();
            #endregion

            RemoteHooking.IpcCreateServer<ProcessInterface>(ref ChannelName, WellKnownObjectMode.Singleton);

            Process process = Process.GetProcessesByName("explorer").FirstOrDefault();
            string LibraryPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(ProcessInterface).Assembly.Location), "InjectDLL.dll");
            if (process != null)
            {
                RemoteHooking.Inject(process.Id, LibraryPath, LibraryPath, ChannelName);
            }
            else
            {
                int processId = 0;
                RemoteHooking.CreateAndInject("explorer.exe", "", 0, LibraryPath, LibraryPath, out processId, ChannelName);
            }
            this.WindowState = FormWindowState.Minimized;
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
            }
        }   
        
        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            while (true)
            {
                DriveInfo[] drives = DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Removable).ToArray();
                if (drives.Length <= Global.USBDrives.Count)
                    continue;
                lock (Global.USBExist)
                {
                    for (int i = 0; i < drives.Length; i++)
                    {
                        if (!Global.USBDrives.Contains(drives[i].Name))
                        {
                            if (notifyIcon.Visible)
                            {
                                notifyIcon.ShowBalloonTip(1000, "New notification", "Insert new device: " + drives[i].Name, ToolTipIcon.Info);
                            }
                            //lbEventLog.Items.Add("Insert new device: " + drives[i].Name + Environment.NewLine);
                            Global.USBDrives.Add(drives[i].Name);
                            Global.USBExist.Add(false);                            
                        }
                    }
                }
                break;
            }
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            lock (Global.USBExist)
            {               
                DriveInfo[] drives = DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Removable).ToArray();
                for (int i = 0; i < drives.Length; i++)
                {
                    int idx = -1;
                    if ((idx = Global.USBDrives.IndexOf(drives[i].Name)) != -1)
                    {
                        Global.USBExist[idx] = true;
                    }
                }
                int removeID = Global.USBExist.IndexOf(false);
                if (removeID != -1)
                {
                    if (notifyIcon.Visible)
                    {
                        notifyIcon.ShowBalloonTip(1000, "New notification", "Remove device :" + Global.USBDrives[removeID], ToolTipIcon.Info);
                    }
                    //lbEventLog.Items.Add("Remove device :" + Global.USBDrives[removeID] + Environment.NewLine);                    
                    Global.USBDrives.RemoveAt(removeID);
                    Global.USBExist.RemoveAt(removeID);
                }
                for (int i = 0; i < Global.USBExist.Count; i++)
                {
                    Global.USBExist[i] = false;
                }
            }
        }

        private void HackingAppForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(1000, "New notification", "I am in system tray", ToolTipIcon.None);                
                this.Hide();
            }
            else
            {
                notifyIcon.Visible = false;
            }
        }

        #endregion

        #region Authentication

        public void InitAuthenticate()
        {
            // Load Previous Setting
            if (Global.AUTH_TYPE == (int)AuthType.Personal)
            {
                pnDomain.Enabled = false;
                rbPersonal.Checked = true;

                if (Global.REMEMBER_ME.ToString() == "True")
                    isRememberMe = true;

                if (isRememberMe)
                {
                    SetPersonalText(Global.ID, string.Empty);
                    lbHello.Text = string.Format("Hello {0}", Global.ID);
                    pnLogin.Show();
                    btPersonalLogin.Text = "Logout";

                    isAuth = true;
                }
                else
                {
                    SetPersonalText(string.Empty, string.Empty);
                    pnLogin.Hide();
                    btPersonalLogin.Text = "Login";

                    isAuth = false;
                }

                cbRememberMe.Checked = isRememberMe;

            }
            else if (Global.AUTH_TYPE == (int)AuthType.Domain)
            {
                pnPersonal.Enabled = false;
                rbDomain.Checked = true;

                DomainPermit = Global.DOMAIN_PERMIT;

                pnLogin.Hide();
            }

            lbAuthMode.Text = string.Format("You are using {0} authentication mode",
                Global.AUTH_TYPE == (int)AuthType.Personal ? "personal" : "domain");
        }

        private bool Authenticate()
        {
            if (rbPersonal.Checked == true)
            {

                #region Persional
                // Validate
                if (tbID.Text == "" || tbPassword.Text == "")
                {
                    MessageBox.Show("Username and Password can not be emty");
                    return false;
                }

                if (tbID.Text.Length < 3 || tbPassword.Text.Length < 3)
                {
                    MessageBox.Show("Username and Password length at least 4 charactor");
                    return false;
                }

                // Valid
                Global.AUTH_TYPE = (int)AuthType.Personal;
                AppSettings.UpdateAppSetting(Config.AUTH_TYPE, Global.AUTH_TYPE.ToString());

                // Update Global ID Password variable
                Global.ID = Global.KEY01 = tbID.Text;
                Global.PASSWORD = Global.KEY02 = tbPassword.Text;

                isRememberMe = cbRememberMe.Checked;
                Global.REMEMBER_ME = isRememberMe;
                AppSettings.UpdateAppSetting(Config.REMEMBER_ME, isRememberMe.ToString());
                if (isRememberMe)
                {
                    // Save ID+password if app is remember info
                    AppSettings.UpdateAppSetting(Config.ID, Global.ID);
                    AppSettings.UpdateAppSetting(Config.PASSWORD, Global.PASSWORD);
                }
                else
                {
                    AppSettings.UpdateAppSetting(Config.ID, string.Empty);
                    AppSettings.UpdateAppSetting(Config.PASSWORD, string.Empty);

                }


                lbAuthMode.Text = string.Format("You are using {0} authentication mode", "persional");
                #endregion

                return true;
            }
            else if (rbDomain.Checked == true)
            {

                #region Domain
                // Away valied
                Global.AUTH_TYPE = (int)AuthType.Domain;
                AppSettings.UpdateAppSetting(Config.AUTH_TYPE, Global.AUTH_TYPE.ToString());

                // Update global variable
                //Global.DOMAIN_PERMIT = DomainPermit;
                //Config.UpdateAppSetting(Config.DOMAIN_PERMIT, Global.DOMAIN_PERMIT.ToString());
                Global.USER_DOMAIN_NAME = Global.KEY01 = Global.GetUserDomainName();
                Global.DOMAIN_NAME = Global.KEY02 = Global.GetDomainName();

                if (Global.DOMAIN_NAME == "")
                {
                    MessageBox.Show("Your are not in Active Domain");
                    return false;
                }

                AppSettings.UpdateAppSetting(Config.USER_DOMAIN_NAME, Global.USER_DOMAIN_NAME);
                AppSettings.UpdateAppSetting(Config.DOMAIN_NAME, Global.DOMAIN_NAME);

                lbAuthMode.Text = string.Format("You are using {0} authentication mode", "domain");
                #endregion

                return true;
            }

            return false;
        }

        #region PERSONAL

        static bool isAuth = false;
        private void btPersonalLogin_Click(object sender, EventArgs e)
        {
            if (!isAuth)
            { 
                // Login
                isAuth = Authenticate();
                 if (isAuth)
                {
                    SetPersonalText(Global.ID, string.Empty);
                    btPersonalLogin.Text = "Logout";
                    lbHello.Text = string.Format("Hello {0}", Global.ID);
                    pnLogin.Show();
                }
            }
            else 
            {
                // Logout
                isAuth = false;
                ResetPersonalAuth();
            }

        }
        private void SetPersonalText(string id, string pas)
        {
            tbID.Text = id;
            tbPassword.Text = pas;
        }

        bool isRememberMe = false;
        private void cbRememberMe_CheckedChanged(object sender, EventArgs e)
        {
            isRememberMe = cbRememberMe.Checked;
        }

        private void ResetPersonalAuth()
        {
            SetPersonalText(string.Empty, string.Empty);
            btPersonalLogin.Text = "Login";
            Global.REMEMBER_ME = isRememberMe = false;
            AppSettings.UpdateAppSetting(Config.ID, string.Empty);
            AppSettings.UpdateAppSetting(Config.PASSWORD, string.Empty);
            AppSettings.UpdateAppSetting(Config.REMEMBER_ME, false.ToString());

            pnLogin.Hide();

        }


        private void rbPersonal_CheckedChanged(object sender, EventArgs e)
        {
            pnPersonal.Enabled = true;
            pnDomain.Enabled = false;

            pnDomain.Hide();
            pnPersonal.Show();
        }

        #endregion

        #region DOMAIN
        private void rbDomain_CheckedChanged(object sender, EventArgs e)
        {
            pnDomain.Enabled = true;
            pnPersonal.Enabled = false;

            pnPersonal.Hide();
            pnDomain.Show();
        }

        private void btDomainApply_Click(object sender, EventArgs e)
        {
            if (Authenticate())
            {
                MessageBox.Show("Change is applied. You are working in domain: " + Global.DOMAIN_NAME);
                ResetPersonalAuth();
            }
        }

        int DomainPermit = (int)DomainPermission.OnlyMe;
        private void rbOnlyMe_CheckedChanged(object sender, EventArgs e)
        {
            DomainPermit = (int)DomainPermission.OnlyMe;
        }

        private void rbAllUserDomain_CheckedChanged(object sender, EventArgs e)
        {
            DomainPermit = (int)DomainPermission.AllUserInDomain;
        }

        private void rbSomeOneDomain_CheckedChanged(object sender, EventArgs e)
        {
            DomainPermit = (int)DomainPermission.SomeUserInDomain;
        }
        #endregion

        private void lbAuthMode_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region FileExtension

        static List<string> listExt = new List<string>();

        private void InitExtension()
        {
            foreach (string s in Global.extensions)
            {
                listExt.Add(s);
            }

            if(listExt.Contains(".doc"))
                cbDoc.Checked = true;
            if (listExt.Contains(".docx"))
                cbDocx.Checked = true;
            if (listExt.Contains(".xls"))
                cbXls.Checked = true;
            if (listExt.Contains(".xlsx"))
                cbXlsx.Checked = true;
            if (listExt.Contains(".ppt"))
                cbPpt.Checked = true;
            if (listExt.Contains(".pptx"))
                cbPptx.Checked = true;
            if (listExt.Contains(".rar"))
                cbRar.Checked = true;
            if (listExt.Contains(".zip"))
                cbZip.Checked = true;
            if (listExt.Contains(".pdf"))
                cbPdf.Checked = true;
        }
        
        private void UpdateExtension(string fileExtension, bool extChecked)
        {
            if (extChecked)
            {
                if (!listExt.Contains(fileExtension))
                {
                    listExt.Add(fileExtension);
                }
            }
            else
            {
                if (listExt.Contains(fileExtension))
                {
                    listExt.Remove(fileExtension);
                }
            }

            Global.extensions.Clear();
            Global.extensions.AddRange(listExt);
            FileExtensions.SaveListExtensions(Global.extensions);

        }

        #region CheckBox
        private void cbDoc_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExtension(".doc", cbDoc.Checked);
        }
        
        private void cbXls_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExtension(".xls", cbXls.Checked);
        }

        private void cbPpt_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExtension(".ppt", cbPpt.Checked);
        }

        private void cbRar_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExtension(".rar", cbRar.Checked);
        }

        private void cbDocx_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExtension(".docx", cbDocx.Checked);
        }

        private void cbXlsx_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExtension(".xlsx", cbXlsx.Checked);
        }

        private void cbPptx_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExtension(".pptx", cbPptx.Checked);
        }

        private void cbZip_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExtension(".zip", cbZip.Checked);
        }

        private void cbPdf_CheckedChanged(object sender, EventArgs e)
        {
            UpdateExtension(".pdf", cbPdf.Checked);
        }
        #endregion

        #endregion

        #region Main menu
        private void btAuthenticate_Click(object sender, EventArgs e)
        {
            pnExtension.Hide();
            pnAuthenticate.Show();
        }

        private void btFileExtension_Click(object sender, EventArgs e)
        {
            pnAuthenticate.Hide();
            pnExtension.Show();
        }

        private void btAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a demo application for protecting data.");
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Global.Log);
        }



        #endregion

    }
}
