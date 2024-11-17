using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertStats
{
    public partial class help : Form
    {
        public help()
        {
            InitializeComponent();
        }

        private void help_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void help_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tbPW.Text.Trim() == Properties.Settings.Default.StartPW)
            {
                Properties.Settings.Default.PWallowed = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                if (!Properties.Settings.Default.PWallowed)
                {
                    MessageBox.Show("You must obtain a password to run this application");
                    Application.Exit();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
