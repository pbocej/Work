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

        }
    }
}
