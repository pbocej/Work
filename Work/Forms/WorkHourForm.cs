using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkLib.Model;
using WorkLib.Repository;

namespace Work.Forms
{
    public partial class WorkHourForm : Form
    {
        private User _user;
        private WorkHour _workHour;

        public WorkHourForm(User user, WorkHour workHour)
        {
            InitializeComponent();
            _user = user;
            _workHour = workHour;
        }

        private void WorkHourForm_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            bsUserProjects.DataSource = WorkRepository.GetUserProjects(_user.UserId);
            if (_workHour.WorkHourID == 0) // insert
                if (Properties.Settings.Default.ProjectId != 0)
                    try
                    {
                        // select last project used
                        _workHour.ProjectId = Properties.Settings.Default.ProjectId;
                    }
                    catch { }
                else projectNameComboBox.SelectedIndex = 1;
            workHourBindingSource.DataSource = _workHour;
        }

        public WorkHour WorkHour => _workHour;

        private void btSave_Click(object sender, EventArgs e)
        {
            _workHour.ProjectId = (int)projectNameComboBox.SelectedValue;
            if (ValidateData())
            {
                try
                {
                    if (_workHour.Save() > 0);
                    {
                        DialogResult = DialogResult.OK;
                        Properties.Settings.Default.ProjectId = _workHour.ProjectId ?? 0;
                        Close();
                    }
                }
                catch (AppException ex)
                {
                    MessageBox.Show(this, ex.FullMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            if (fromDateTimePicker.Value > toDateTimePicker.Value)
            {
                MessageBox.Show(this, "Begin date is greather then the end date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fromDateTimePicker.Focus();
            }
            return true;
        }
    }
}
