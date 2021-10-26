using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
