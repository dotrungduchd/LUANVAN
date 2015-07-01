using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoHackingApp
{
    public partial class AuthenticationForm : Form
    {
        public AuthenticationForm()
        {
            InitializeComponent();

            if (Global.AUTH_TYPE == (int)AuthType.Personal)
            {
                pnDomain.Enabled = false;
                rbPersonnal.Checked = true;

                tbID.Text = Global.ID;
            }
            else
            {
                pnPersonal.Enabled = false;
                rbDomain.Checked = true;

                rbOnlyMe.Checked = Global.DOMAIN_PERMIT == (int)DomainPermission.OnlyMe;
                rbAllUserDomain.Checked = Global.DOMAIN_PERMIT == (int)DomainPermission.AllUserInDomain;
                rbSomeOneDomain.Checked = Global.DOMAIN_PERMIT == (int)DomainPermission.SomeUserInDomain;
            }

        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            if (Config.GetAppSetting(Config.REMEMBER_ME) == "True")
                isRememberMe = true;
            cbRememberMe.Enabled = isRememberMe;


        }

        #region Personal Auth
        private void rbPersonnal_CheckedChanged(object sender, EventArgs e)
        {
            pnPersonal.Enabled = true;
            pnDomain.Enabled = false;
        }

        bool isRememberMe = false;
        private void cbRememberMe_CheckedChanged(object sender, EventArgs e)
        {
            isRememberMe = cbRememberMe.Enabled;

        }

        /// <summary>
        /// Authenticate user, save config to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAuth_Click(object sender, EventArgs e)
        {
            #region Persional
            // Update Global ID Password variable
            Global.ID = tbID.Text;
            Global.PASSWORD = tbPassword.Text;
            Global.REMEMBER_ME = isRememberMe;

            // Update Rememberme
            Config.UpdateAppSetting(Config.REMEMBER_ME, isRememberMe.ToString());

            // Save ID+password if app is remember info
            Config.AddAppSetting(Config.ID, Global.ID);
            Config.AddAppSetting(Config.PASSWORD, Global.PASSWORD);
            #endregion

            #region Domain
            // Update global variable
            Global.DOMAIN_PERMIT = DomainPermit;

            // Save to config file
            Config.UpdateAppSetting(Config.DOMAIN_PERMIT, Global.DOMAIN_PERMIT.ToString());

            #endregion
        }

        #endregion

        #region Domain Auth
        private void rbDomain_CheckedChanged(object sender, EventArgs e)
        {
            pnDomain.Enabled = true;
            pnPersonal.Enabled = false;
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
    }
}
