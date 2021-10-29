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
using System.ComponentModel.DataAnnotations;

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
            LoadData();
        }

        private void LoadData()
        {
            bsUser.DataSource = _user;
            ((ListBox)lbProjects).DataSource = WorkRepository.GetAllProjects();
            ((ListBox)lbProjects).DisplayMember = "ProjectName";
            ((ListBox)lbProjects).ValueMember = "ProjectId";
            for (int i = 0; i < lbProjects.Items.Count; i++)
            {
                var project = lbProjects.Items[i] as Project;
                if (_user.UserProjects.Any(p => p.ProjectId == project.ProjectId))
                    lbProjects.SetItemChecked(i, true);
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            var vc = new ValidationContext(_user, null, null);
            IList<ValidationResult> errors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(_user, vc, errors, false))
            {
                var sb = new StringBuilder();
                errors.ToList().ForEach(error => sb.AppendLine(error.ErrorMessage));
                MessageBox.Show(this, sb.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _user.Save();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public override bool ValidateChildren()
        {
            return base.ValidateChildren();
        }
    }
}
