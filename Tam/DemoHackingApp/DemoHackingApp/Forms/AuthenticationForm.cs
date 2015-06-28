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
        }

        private void rbPersonnal_CheckedChanged(object sender, EventArgs e)
        {
            pnDomain.Enabled = false;
        }

        private void rbDomain_CheckedChanged(object sender, EventArgs e)
        {
            pnPersonal.Enabled = true;
        }
    }
}
