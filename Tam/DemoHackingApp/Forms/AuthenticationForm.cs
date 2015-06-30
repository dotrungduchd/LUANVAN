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
            // Validate New User
            if (Users.UsersList.Keys.Contains(tbID.Text))
            {
                if (Users.UsersList[tbID.Text] != Encoding.ASCII.GetString(Signer.Hash(tbPassword.Text)))
                {
                    MessageBox.Show("User is exist but password incorrect");
                    return;
                }
                else
                {
                    MessageBox.Show("Welcome back " + tbID.Text);
                }
            }
            else
            {
                MessageBox.Show("Welcome " + tbID.Text + ". You must remember your ID and password to decrypt your file");

                // Save ID+password if app is remember info
                Users.AddNewUser(tbID.Text, Encoding.ASCII.GetString(Signer.Hash(tbPassword.Text)));
            }

            // Update Global variable
            Global.ID = Global.KEY01 = tbID.Text;
            Global.PASSWORD = Global.KEY02 = tbPassword.Text;

            // Update Rememberme
            Config.UpdateAppSetting(Config.REMEMBER_ME, isRememberMe.ToString());

            Global.METADATA_FILENAME = Global.ID + ".data";
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

        private void btApplyDomain_Click(object sender, EventArgs e)
        {
            // Update global variable
            Global.DOMAIN_PERMIT = DomainPermit;

            // Save to config file
            Config.UpdateAppSetting(Config.DOMAIN_PERMIT, Global.DOMAIN_PERMIT.ToString());

        }

        #endregion
    }
}
