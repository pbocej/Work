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
    public partial class ProjectForm : Form
    {
        private Project _project;

        public ProjectForm(Project project)
        {
            _project = project;
            InitializeComponent();
        }

        private void ProjectForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            projectBindingSource.DataSource = _project;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                _project.Save();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        new private bool Validate()
        {
            var vc = new ValidationContext(_project, null, null);
            IList<ValidationResult> errors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(_project, vc, errors, false))
            {
                var sb = new StringBuilder();
                foreach (var error in errors)
                    sb.AppendLine(error.ErrorMessage);
                MessageBox.Show(this, sb.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        public Project Project
        {
            get { return _project; }
        }
    }
}
