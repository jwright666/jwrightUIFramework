using App_CommonLibraries;
using App_CommonLibraries.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jwrightUIFramework.UI
{
    public partial class LookupForm : AbstractForm
    {
        public bool jobSearch; 
        private DataTable _dtSource;
        

        public LookupForm(string tableName)
        {
            InitializeComponent();
            dgvMasterList.AutoGenerateColumns = false;
            if (!jobSearch)
                _dtSource = SearchClass.GetSearchData(tableName, Convert.ToInt32(txtRowCount.Text.Trim()));
            else
                _dtSource = Employee.GetEmployees().Tables[0];
        }

        private void RefreshDataSource(int rowCount)
        {
            try
            {                
                DataView view = new DataView(_dtSource);
                if (!string.IsNullOrEmpty(txtCode.Text.ToString().Trim()))
                    view.RowFilter = "Code like '%" + txtCode.Text.ToString().Trim() + "%'";
                if (!string.IsNullOrEmpty(txtDescription.Text.ToString().Trim()))
                    view.RowFilter = "Description like '%" + txtDescription.Text.ToString().Trim() + "%'";

                dgvMasterList.DataSource = view;
            }
            catch (Exception ex) { MessageBox.Show(ex.InnerException.Message.ToString(), "Alert"); }
        }

        private void FilterText_Change(object sender, EventArgs e)
        {
            try
            {
                RefreshDataSource(Convert.ToInt32(txtRowCount.Text.Trim()));
            }
            catch (Exception ex) { MessageBox.Show(ex.InnerException.Message.ToString(), "Alert"); }
        }
    }
}
