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
            Global.GetListExtensions();

            LoadExtenssions();
        }

        private void LoadExtenssions()
        {
            List<string> listExts = Config.GetListExtensions();
            for (int j = 0; j < clbExts.Items.Count; j++)
            {
                clbExts.SetItemCheckState(j, CheckState.Unchecked);
                for (int i = 0; i < listExts.Count; i++)
                {
                    if (listExts[i] == clbExts.Items[j].ToString())
                    {
                        clbExts.SetItemCheckState(j, CheckState.Checked);
                        break;
                    }
                    
                }
            }
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
            //Global.extsForm.Hide();
        }

    }
}
