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
    public partial class ExtensionsForm : Form
    {
        public ExtensionsForm()
        {
            InitializeComponent();
        }

        List<string> listExt = new List<string>();
        private void clbExts_SelectedIndexChanged(object sender, EventArgs e)
        {
            listExt.Clear();
            foreach (string item in clbExts.CheckedItems)
                listExt.Add(item);            
        }

        private void btApply_Click(object sender, EventArgs e)
        {
            Config.SaveListExtensions(listExt);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            Config.SaveListExtensions(listExt);
            Global.extsForm.Hide();
        }

    }
}
