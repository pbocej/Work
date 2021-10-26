using System;
using System.Windows.Forms;
using WorkLib.Model;
using WorkLib.Repository;
using System.Collections.ObjectModel;

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
                        break;
                    case 1:     // users
                        bsUsers.DataSource = new ObservableCollection<User>(WorkRepository.GetAllUsers());
                        break;
                    case 2:     // projects
                        bsProjects.DataSource = new ObservableCollection<Project>(WorkRepository.GetAllProjects());
                        break;
                    default:
                        break;
                }
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
    }
}
