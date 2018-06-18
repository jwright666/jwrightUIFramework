
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using App_CommonLibraries.Objects;
using App_CommonLibraries.Utilities;

namespace jwrightUIFramework.UI
{
    public partial class BaseEntryForm : AbstractForm
    {
        public User user;
        public string module;
        public ItemAccess moduleAccess;
        public FormMode formMode = FormMode.Browse;

        private bool mouseDown;
        private Point lastLocation;
        private bool isMaximized;

        public BaseEntryForm()
        {
            InitializeComponent();
            if (this.WindowState != FormWindowState.Maximized)
                isMaximized = false;
        }

        #region Mouse Event
        private void EntryForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void EntryForm_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void EntryForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panelTop_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (isMaximized)
            {
                this.WindowState = FormWindowState.Normal;
                isMaximized = false;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                isMaximized = true;
            }
        }
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to close the form?", "Confirmation!", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No)
                e.Cancel = true;
        }

        #region Button Controls
        protected virtual bool AfterNewClicked() { return true; }
        protected virtual bool AfterModifyClicked() { return true; }
        protected virtual bool AfterCancelClicked() { return true; }
        protected virtual bool AfterSaveClicked() { return true; }
        protected virtual bool AfterDeleteClicked() { return true; }
        protected virtual bool AfterPrintClicked() { return true; }
        protected void ManageUserAccess(string module, User user)
        {
            this.user = user;
            this.module = module;
            moduleAccess = UserAccess.GetUserAccessRightsForModuleItem(user, module);
            if (moduleAccess != null)
            {
                if (moduleAccess.Add == false)
                {
                    btnNew.Enabled = false;
                }
                if (moduleAccess.Edit == false)
                {
                    btnEdit.Enabled = false;
                }
                if (moduleAccess.Print == false)
                {
                    btnPreview.Enabled = false;
                }
                if (moduleAccess.Delete == false)
                {
                    btnDelete.Enabled = false;
                }
            }
        }
        protected virtual void EnableAllButtonsForFormMode()
        {
            switch (formMode)
            {
                case FormMode.Browse:
                    {
                        btnNew.Enabled = true;
                        btnEdit.Enabled = true;
                        btnCancel.Enabled = false;
                        btnSave.Enabled = false;
                        btnDelete.Enabled = true;
                        btnPreview.Enabled = true;
                        panelMain.Enabled = false;
                        break;
                    }
                case FormMode.Add:
                    {
                        btnNew.Enabled = false;
                        btnEdit.Enabled = false;
                        btnCancel.Enabled = true;
                        btnSave.Enabled = true;
                        btnDelete.Enabled = false;
                        btnPreview.Enabled = false;
                        panelMain.Enabled = true;
                        break;
                    }
                case FormMode.Edit:
                    {
                        btnNew.Enabled = false;
                        btnEdit.Enabled = false;
                        btnCancel.Enabled = true;
                        btnSave.Enabled = true;
                        btnDelete.Enabled = false;
                        btnPreview.Enabled = false;
                        panelMain.Enabled = true;
                        break;
                    }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (AfterNewClicked())
                {
                    formMode = FormMode.Add;
                    EnableAllButtonsForFormMode();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.InnerException.Message.ToString(), "Alert"); }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (AfterModifyClicked())
                {
                    formMode = FormMode.Edit;
                    EnableAllButtonsForFormMode();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.InnerException.Message.ToString(), "Alert"); }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (AfterDeleteClicked())
                {
                    formMode = FormMode.Browse;
                    EnableAllButtonsForFormMode();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.InnerException.Message.ToString(), "Alert"); }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (AfterSaveClicked())
                {
                    formMode = FormMode.Browse;
                    EnableAllButtonsForFormMode();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.InnerException.Message.ToString(), "Alert"); }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (AfterCancelClicked())
                {
                    formMode = FormMode.Browse;
                    EnableAllButtonsForFormMode();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.InnerException.Message.ToString(), "Alert"); }
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        { 
            try
            {
                LookupForm lookup = new LookupForm("CustVend_Code");
                lookup.jobSearch = true;
                lookup.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Alert"); }
        }
    }
}
