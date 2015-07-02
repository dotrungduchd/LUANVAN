using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InjectDLL;

namespace DemoHackingApp
{
    public partial class AuthenticationForm : Form
    {
        public AuthenticationForm()
        {
            InitializeComponent();

            // Load Previous Setting
            if (Global.AUTH_TYPE == (int)AuthType.Personal)
            {
                pnDomain.Enabled = false;
                rbPersonnal.Checked = true;

                if (Config.GetAppSetting(Config.REMEMBER_ME) == "True")
                    isRememberMe = true;
                if (isRememberMe)
                {
                    tbID.Text = Global.ID;
                    tbPassword.Text = Global.PASSWORD;
                    cbRememberMe.Checked = isRememberMe;
                }
            }
            else
            {
                pnPersonal.Enabled = false;
                rbDomain.Checked = true;

                rbOnlyMe.Checked = Global.DOMAIN_PERMIT == (int)DomainPermission.OnlyMe;
                rbAllUserDomain.Checked = Global.DOMAIN_PERMIT == (int)DomainPermission.AllUserInDomain;
                rbSomeOneDomain.Checked = Global.DOMAIN_PERMIT == (int)DomainPermission.SomeUserInDomain;
                DomainPermit = Global.DOMAIN_PERMIT;
            }

        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {


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
            isRememberMe = cbRememberMe.Checked;

        }

        /// <summary>
        /// Authenticate user, save config to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAuth_Click(object sender, EventArgs e)
        {
            if (rbPersonnal.Checked == true)
            {
                Global.AUTH_TYPE = (int)AuthType.Personal;

                #region Persional
                // Validate
                if (tbID.Text == "" || tbPassword.Text == "")
                {
                    MessageBox.Show("ID and Password can not be null");
                    return;
                }

                if (tbID.Text.Length < 3 || tbPassword.Text.Length < 3)
                {
                    MessageBox.Show("ID and Password length at least 4 charactor");
                    return;
                }

                // Update Global ID Password variable
                Global.ID = Global.KEY01 = tbID.Text;
                Global.PASSWORD = Global.KEY02 = tbPassword.Text;
                Global.REMEMBER_ME = isRememberMe;

                // Save ID+password if app is remember info
                Config.AddAppSetting(Config.ID, Global.ID);
                Config.AddAppSetting(Config.PASSWORD, Global.PASSWORD);
                Config.UpdateAppSetting(Config.REMEMBER_ME, isRememberMe.ToString());
                #endregion
            }
            else if (rbDomain.Checked == true)
            {
                Global.AUTH_TYPE = (int)AuthType.Domain;

                #region Domain
                // Update global variable
                Global.DOMAIN_PERMIT = DomainPermit;
                Global.USER_DOMAIN_NAME = Global.KEY01 = Global.GetUserDomainName();
                Global.DOMAIN_NAME = Global.KEY02 = Global.GetDomainName();

                if (Global.DOMAIN_NAME == "")
                {
                    MessageBox.Show("Your are not in Active Domain");
                    return;
                }

                // Save to config file
                Config.UpdateAppSetting(Config.DOMAIN_PERMIT, Global.DOMAIN_PERMIT.ToString());

                #endregion
            }
            Config.UpdateAppSetting(Config.AUTH_TYPE, Global.AUTH_TYPE.ToString());

            Main.UpdateAesKey();
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
