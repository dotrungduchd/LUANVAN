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

namespace DemoHackingApp
{
    public partial class HackingAppForm : Form
    {
        public HackingAppForm()
        {
            InitializeComponent();
        }

        #region Import dll
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

        #endregion

        #region Notification and Event Form
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
			item.Image = Resources.Exit;
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

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
            }
        }   

        #endregion

        /// <summary>
        /// Start Hook when application load, get USB Drive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HackingAppForm_Load(object sender, EventArgs e)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            InitNotifyIcon();
            this.Icon = Resources.tho7mau_oYb_1;
            Global.defaultPrograms = Global.getDefaultPrograms();
            
            if(registryKey.GetValue("DemoHackingApp",null)==null)
            {
                registryKey.SetValue("DemoHackingApp", Application.ExecutablePath);
            }
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

            RemoteHooking.IpcCreateServer<ProcessInterface>(ref ChannelName, WellKnownObjectMode.SingleCall);

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

        #region Device Event
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
                            lbEventLog.Items.Add("Insert new device: " + drives[i].Name + Environment.NewLine);
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
                    lbEventLog.Items.Add("Remove device :" + Global.USBDrives[removeID] + Environment.NewLine);                    
                    Global.USBDrives.RemoveAt(removeID);
                    Global.USBExist.RemoveAt(removeID);
                }
                for (int i = 0; i < Global.USBExist.Count; i++)
                {
                    Global.USBExist[i] = false;
                }
            }
        }

        #endregion

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

        private void btExtensions_Click(object sender, EventArgs e)
        {

        }

        private void btAuthenticate_Click(object sender, EventArgs e)
        {
            Global.authForm = new AuthenticationForm();
            Global.authForm.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
