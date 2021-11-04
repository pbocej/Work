using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkLib.Data;
using WorkLib.Model;
using WorkLib.Repository;
using System.ComponentModel.DataAnnotations;

namespace Work.Forms
{
    public partial class UserForm : Form
    {
        private User _user;
        private bool _passwordChanged = false;

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
            using (var context = new DbContext())
            {
                userGroupIdCombobox.DataSource = WorkRepository.GetUserGroups(context);
                userGroupIdCombobox.DisplayMember = "GroupName";
                userGroupIdCombobox.ValueMember = "UserGroupId";
                ((ListBox)lbProjects).DataSource = WorkRepository.GetAllProjects(context);
            }
            ((ListBox)lbProjects).DisplayMember = "ProjectName";
            ((ListBox)lbProjects).ValueMember = "ProjectId";
            bsUser.DataSource = _user;
            for (int i = 0; i < lbProjects.Items.Count; i++)
            {
                var project = lbProjects.Items[i] as Project;
                if (_user.UserProjects.Any(p => p.ProjectId == project.ProjectId))
                    lbProjects.SetItemChecked(i, true);
            }
            if (_user.UserId == 0) // new user
                userGroupIdCombobox.SelectedValue = 1;
            else
                userGroupIdCombobox.SelectedValue = _user.UserGroupId;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    _user.UserGroupId = (int)userGroupIdCombobox.SelectedValue;
                    var up = new List<UserProject>();
                    foreach (var item in lbProjects.CheckedItems)
                        up.Add(new UserProject((Project)item)
                        {
                            UserId = _user.UserId,
                            Owned = true
                        });
                    _user.UserProjects = up.ToArray();
                    using (var context = new DbContext())
                    {
                        var userId = _user.Save(context);
                        if (_user.Password == null)
                            _user.Password = string.Empty;
                        if (_passwordChanged)
                            _user.ChangePassword(userId, _user.Password, context);
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (AppException ex)
            {
                MessageBox.Show(this, ex.FullMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private bool ValidateData()
        {
            var vc = new ValidationContext(_user, null, null);
            IList<ValidationResult> errors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(_user, vc, errors, false))
            {
                var sb = new StringBuilder();
                foreach (var error in errors)
                    sb.AppendLine(error.ErrorMessage);
                MessageBox.Show(this, sb.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // password
            if (_passwordChanged && passwordTextBox.Text != password2TextBox.Text)
            {
                MessageBox.Show(this, "Passwords does not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            _passwordChanged = true;
        }

        public User User
        {
            get { return _user; }
        }
    }
}
