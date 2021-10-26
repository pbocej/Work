using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkLib.Model;
using WorkLib.Repository;

namespace Work
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var user = WorkRepository.Login(tbUserName.Text, tbPassword.Text);
                Global.CurrenntUser = user;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (AppException ex)
            {
                lbError.Text = ex.FullMessage;
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
