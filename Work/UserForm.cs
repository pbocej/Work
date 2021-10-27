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
    public partial class UserForm : Form
    {
        private User _user;

        public UserForm(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            bsUser.DataSource = _user;
        }

        private void btSave_Click(object sender, EventArgs e)
        {

        }
    }
}
