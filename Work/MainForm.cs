using System;
using System.Windows.Forms;

namespace Work
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshData();

        }

        void RefreshData()
        {

        }

        private void dgWork_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dgWork_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:    // edit
                    break;
                case Keys.Insert:   // add
                    break;
                case Keys.Delete:   // delete
                    break;
                default:
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.LastUserName = Global.CurrenntUser.UserName;
            Properties.Settings.Default.Save();
        }
    }
}
