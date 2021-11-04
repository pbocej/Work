
namespace Work.Forms
{
    partial class WorkHourForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label dateLabel;
            System.Windows.Forms.Label fromLabel;
            System.Windows.Forms.Label toLabel;
            System.Windows.Forms.Label projectNameLabel;
            System.Windows.Forms.Label subjectLabel;
            System.Windows.Forms.Label descriptionLabel;
            this.workHourBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dateDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.projectNameComboBox = new System.Windows.Forms.ComboBox();
            this.subjectTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.btSave = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.bsUserProjects = new System.Windows.Forms.BindingSource(this.components);
            dateLabel = new System.Windows.Forms.Label();
            fromLabel = new System.Windows.Forms.Label();
            toLabel = new System.Windows.Forms.Label();
            projectNameLabel = new System.Windows.Forms.Label();
            subjectLabel = new System.Windows.Forms.Label();
            descriptionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.workHourBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUserProjects)).BeginInit();
            this.SuspendLayout();
            // 
            // dateLabel
            // 
            dateLabel.AutoSize = true;
            dateLabel.Location = new System.Drawing.Point(23, 23);
            dateLabel.Name = "dateLabel";
            dateLabel.Size = new System.Drawing.Size(33, 13);
            dateLabel.TabIndex = 1;
            dateLabel.Text = "Date:";
            // 
            // fromLabel
            // 
            fromLabel.AutoSize = true;
            fromLabel.Location = new System.Drawing.Point(23, 75);
            fromLabel.Name = "fromLabel";
            fromLabel.Size = new System.Drawing.Size(33, 13);
            fromLabel.TabIndex = 3;
            fromLabel.Text = "From:";
            // 
            // toLabel
            // 
            toLabel.AutoSize = true;
            toLabel.Location = new System.Drawing.Point(150, 75);
            toLabel.Name = "toLabel";
            toLabel.Size = new System.Drawing.Size(23, 13);
            toLabel.TabIndex = 5;
            toLabel.Text = "To:";
            // 
            // projectNameLabel
            // 
            projectNameLabel.AutoSize = true;
            projectNameLabel.Location = new System.Drawing.Point(364, 22);
            projectNameLabel.Name = "projectNameLabel";
            projectNameLabel.Size = new System.Drawing.Size(43, 13);
            projectNameLabel.TabIndex = 9;
            projectNameLabel.Text = "Project:";
            // 
            // subjectLabel
            // 
            subjectLabel.AutoSize = true;
            subjectLabel.Location = new System.Drawing.Point(361, 71);
            subjectLabel.Name = "subjectLabel";
            subjectLabel.Size = new System.Drawing.Size(46, 13);
            subjectLabel.TabIndex = 11;
            subjectLabel.Text = "Subject:";
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(23, 115);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(63, 13);
            descriptionLabel.TabIndex = 13;
            descriptionLabel.Text = "Description:";
            // 
            // workHourBindingSource
            // 
            this.workHourBindingSource.DataSource = typeof(WorkLib.Model.WorkHour);
            // 
            // dateDateTimePicker
            // 
            this.dateDateTimePicker.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.workHourBindingSource, "Date", true));
            this.dateDateTimePicker.Location = new System.Drawing.Point(62, 19);
            this.dateDateTimePicker.Name = "dateDateTimePicker";
            this.dateDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.dateDateTimePicker.TabIndex = 0;
            // 
            // fromDateTimePicker
            // 
            this.fromDateTimePicker.CustomFormat = "H:mm";
            this.fromDateTimePicker.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.workHourBindingSource, "From", true));
            this.fromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromDateTimePicker.Location = new System.Drawing.Point(62, 69);
            this.fromDateTimePicker.Name = "fromDateTimePicker";
            this.fromDateTimePicker.ShowUpDown = true;
            this.fromDateTimePicker.Size = new System.Drawing.Size(83, 20);
            this.fromDateTimePicker.TabIndex = 1;
            // 
            // toDateTimePicker
            // 
            this.toDateTimePicker.CustomFormat = "H:mm";
            this.toDateTimePicker.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.workHourBindingSource, "To", true));
            this.toDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDateTimePicker.Location = new System.Drawing.Point(179, 69);
            this.toDateTimePicker.Name = "toDateTimePicker";
            this.toDateTimePicker.ShowUpDown = true;
            this.toDateTimePicker.Size = new System.Drawing.Size(83, 20);
            this.toDateTimePicker.TabIndex = 2;
            // 
            // projectNameComboBox
            // 
            this.projectNameComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.workHourBindingSource, "ProjectName", true));
            this.projectNameComboBox.DataSource = this.bsUserProjects;
            this.projectNameComboBox.DisplayMember = "ProjectName";
            this.projectNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.projectNameComboBox.FormattingEnabled = true;
            this.projectNameComboBox.Location = new System.Drawing.Point(410, 19);
            this.projectNameComboBox.Name = "projectNameComboBox";
            this.projectNameComboBox.Size = new System.Drawing.Size(244, 21);
            this.projectNameComboBox.TabIndex = 3;
            this.projectNameComboBox.ValueMember = "ProjectId";
            // 
            // subjectTextBox
            // 
            this.subjectTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.workHourBindingSource, "Subject", true));
            this.subjectTextBox.Location = new System.Drawing.Point(410, 68);
            this.subjectTextBox.Name = "subjectTextBox";
            this.subjectTextBox.Size = new System.Drawing.Size(244, 20);
            this.subjectTextBox.TabIndex = 4;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.workHourBindingSource, "Description", true));
            this.descriptionTextBox.Location = new System.Drawing.Point(26, 131);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.descriptionTextBox.Size = new System.Drawing.Size(628, 161);
            this.descriptionTextBox.TabIndex = 5;
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(579, 315);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 6;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(450, 315);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 7;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // bsUserProjects
            // 
            this.bsUserProjects.DataSource = typeof(WorkLib.Model.UserProject);
            // 
            // WorkHourForm
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(683, 357);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btSave);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(subjectLabel);
            this.Controls.Add(this.subjectTextBox);
            this.Controls.Add(projectNameLabel);
            this.Controls.Add(this.projectNameComboBox);
            this.Controls.Add(toLabel);
            this.Controls.Add(this.toDateTimePicker);
            this.Controls.Add(fromLabel);
            this.Controls.Add(this.fromDateTimePicker);
            this.Controls.Add(dateLabel);
            this.Controls.Add(this.dateDateTimePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkHourForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Work";
            this.Load += new System.EventHandler(this.WorkHourForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.workHourBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUserProjects)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource workHourBindingSource;
        private System.Windows.Forms.DateTimePicker dateDateTimePicker;
        private System.Windows.Forms.DateTimePicker fromDateTimePicker;
        private System.Windows.Forms.DateTimePicker toDateTimePicker;
        private System.Windows.Forms.ComboBox projectNameComboBox;
        private System.Windows.Forms.TextBox subjectTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.BindingSource bsUserProjects;
    }
}