using System;
using System.Windows.Forms;
using WorkLib.Model;
using WorkLib.Repository;
using System.Collections.ObjectModel;

namespace Work.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lbCurrentUser.Text = Global.CurrenntUser.FullName;
            RefreshUsers();
            RefreshData();
            dgWork.Focus();
        }

        private void RefreshUsers()
        {
            cbUser.Items.Clear();
            if (Global.CurrenntUser.GroupType == GroupType.Administrators)
            {
                cbUser.Items.AddRange(WorkRepository.GetAllUsers());
                cbUser.SelectedItem = Global.CurrenntUser;
            }
            else
            {
                cbUser.Items.Add(Global.CurrenntUser);
                cbUser.SelectedItem = Global.CurrenntUser;
                tabMainControl.TabPages.Remove(tabUsers);
                tabMainControl.TabPages.Remove(tabProjects);
            }
        }

        void RefreshData()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                switch (tabMainControl.SelectedIndex)
                {
                    case 0:     // work hours
                        bsWork.DataSource = new ObservableCollection<WorkHour>(WorkRepository.GetAllWork((cbUser.SelectedItem as User).UserId));
                        SelectedGrid(dgWork);
                        break;
                    case 1:     // users
                        bsUsers.DataSource = new ObservableCollection<User>(WorkRepository.GetAllUsers());
                        SelectedGrid(dgUsers);
                        break;
                    case 2:     // projects
                        bsProjects.DataSource = new ObservableCollection<Project>(WorkRepository.GetAllProjects());
                        SelectedGrid(dgProjects);
                        break;
                    default:
                        break;
                }
                cbUser.Visible = lbUser.Visible =
                    (Global.CurrenntUser.GroupType == GroupType.Administrators
                    && tabMainControl.SelectedIndex == 0);
            }
            catch (AppException ex)
            {
                MessageBox.Show(this, ex.FullMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SelectedGrid(DataGridView dg)
        {
            if (dg.Rows.Count > 0)
            {
                dg.Rows[0].Selected = true;
                if (dg.Rows[0].Cells[1].Visible)
                    dg.Rows[0].Cells[1].Selected = true;
                else
                    dg.Rows[0].Cells[2].Selected = true;
            }
            dg.Select();
        }

        private void GridKeyUpp(object source, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:    // edit
                    DoAction(GridAction.Edit);
                    break;
                case Keys.Insert:   // add
                    DoAction(GridAction.Add);
                    break;
                case Keys.Delete:   // delete
                    DoAction(GridAction.Delete);
                    break;
                case Keys.F5:       // refresh
                    RefreshData();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Performs action (Add, Edidt, Delete)
        /// </summary>
        private void DoAction(GridAction action)
        {
            try
            {
                User user;
                switch (tabMainControl.SelectedIndex)
                {
                    case 0:     // work hour
                        user = cbUser.SelectedItem as User;
                        WorkHour workHour = null;
                        if (dgWork.SelectedRows.Count > 0)
                            workHour = (WorkHour)dgWork.SelectedRows[0].DataBoundItem;
                        switch (action)
                        {
                            case GridAction.Edit:    // edit
                                if (workHour == null)
                                    return;
                                using (var frm = new WorkHourForm(user, workHour))
                                    if (frm.ShowDialog(this) == DialogResult.OK)
                                    {
                                        RefreshData();
                                        SelectRow(frm.WorkHour);
                                    }
                                break;
                            case GridAction.Add:   // add
                                using (var frm = new WorkHourForm(user, 
                                    new WorkHour() 
                                    { 
                                        UserId = user.UserId 
                                    }))
                                    if (frm.ShowDialog(this) == DialogResult.OK)
                                    {
                                        RefreshData();
                                        SelectRow(frm.WorkHour);
                                    }
                                break;
                            case GridAction.Delete:   // delete
                                if (workHour == null)
                                    return;
                                if (MessageBox.Show(this, $"Delete this record?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                {
                                    workHour.Delete();
                                    RefreshData();
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case 1:     // user
                        user = null;
                        if (dgUsers.SelectedRows.Count > 0)
                            user = (User)dgUsers.SelectedRows[0].DataBoundItem;
                        switch (action)
                        {
                            case GridAction.Edit:    // edit
                                if (user == null)
                                    return;
                                using (var frm = new UserForm(user))
                                {
                                    if (frm.ShowDialog(this) == DialogResult.OK)
                                    {
                                        RefreshData();
                                        SelectRow(frm.User);
                                    }
                                }
                                break;
                            case GridAction.Add:   // add
                                using (var frm = new UserForm(new User()))
                                {
                                    if (frm.ShowDialog(this) == DialogResult.OK)
                                    {
                                        RefreshData();
                                        SelectRow(frm.User);
                                    }
                                }
                                break;
                            case GridAction.Delete:   // delete
                                if (user == null)
                                    return;
                                if (MessageBox.Show(this, $"Delete user {user.FullName}?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                {
                                    user.Delete();
                                    RefreshData();
                                }
                                break;
                            default:
                                break;
                        }
                        RefreshUsers();
                        break;
                    case 2:     // project
                        Project project = null;
                        if (dgProjects.SelectedRows.Count > 0)
                            project = (Project)dgProjects.SelectedRows[0].DataBoundItem;
                        switch (action)
                        {
                            case GridAction.Edit:    // edit
                                if (project == null)
                                    return;
                                using (var frm = new ProjectForm(project))
                                {
                                    if (frm.ShowDialog(this) == DialogResult.OK)
                                    {
                                        RefreshData();
                                        SelectRow(frm.Project);
                                    }
                                }
                                break;
                            case GridAction.Add:   // add
                                using (var frm = new ProjectForm(new Project()))
                                {
                                    if (frm.ShowDialog(this) == DialogResult.OK)
                                    {
                                        RefreshData();
                                        SelectRow(frm.Project);
                                    }
                                }
                                break;
                            case GridAction.Delete:   // delete
                                if (project == null)
                                    return;
                                if (MessageBox.Show(this, $"Delete project {project.ProjectName}?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                {
                                    project.Delete();
                                    RefreshData();
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (AppException ex)
            {
                MessageBox.Show(this, ex.FullMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectRow(IEntity entity)
        {
            switch (entity.GetType().Name)
            {
                case "WorkHour":
                    foreach (DataGridViewRow row in dgWork.Rows)
                        if (((WorkHour)row.DataBoundItem).WorkHourID == ((WorkHour)entity).WorkHourID)
                        {
                            row.Selected = true;
                            break;
                        }
                    break;
                case "User":
                    foreach (DataGridViewRow row in dgUsers.Rows)
                        if (((User)row.DataBoundItem).UserId == ((User)entity).UserId)
                        {
                            row.Selected = true;
                            break;
                        }
                    break;
                case "Project":
                    foreach (DataGridViewRow row in dgProjects.Rows)
                        if (((Project)row.DataBoundItem).ProjectId == ((Project)entity).ProjectId)
                        {
                            row.Selected = true;
                            break;
                        }
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

        private void btRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void tabMainControl_Selected(object sender, TabControlEventArgs e)
        {
            RefreshData();
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMainControl.SelectedIndex == 0)
                RefreshData();
        }

        private void GridKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                e.Handled = true;
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            DoAction(GridAction.Edit);
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            DoAction(GridAction.Add);
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            DoAction(GridAction.Delete);
        }
    }
}
