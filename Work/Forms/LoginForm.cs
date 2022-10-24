using System;
using System.Windows.Forms;
using WorkLib.Model;
using WorkLib.Repository;

namespace Work.Forms
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

        private void LoginForm_Load(object sender, EventArgs e)
        {
            tbUserName.Text = Properties.Settings.Default.LastUserName;
        }
    }
}
