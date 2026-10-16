using Business_Layer;
using DVLD.License.Local_License;
using DVLD.Licenses;
using DVLD.Licenses.Detain_License;
using DVLD.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Release_Detained_License
{
    public partial class frmManageListDetainedLicenses : Form
    {
        DataTable _dtAllDetainedLicenses;
        public frmManageListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageListDetainedLicenses_Load(object sender, EventArgs e)
        {

            _dtAllDetainedLicenses = clsDetainLicense.GetAllDetains();

            dgvListDetainedLicense.DataSource = _dtAllDetainedLicenses;
            lblRecordsCount.Text = dgvListDetainedLicense.Rows.Count.ToString();

            if (dgvListDetainedLicense.Rows.Count > 0)
            {
                dgvListDetainedLicense.Columns[0].HeaderText = "D.ID";
                dgvListDetainedLicense.Columns[0].Width = 90;

                dgvListDetainedLicense.Columns[1].HeaderText = "L.ID";
                dgvListDetainedLicense.Columns[1].Width = 90;

                dgvListDetainedLicense.Columns[2].HeaderText = "D.Date";
                dgvListDetainedLicense.Columns[2].Width = 160;

                dgvListDetainedLicense.Columns[3].HeaderText = "Is Released";
                dgvListDetainedLicense.Columns[3].Width = 110;

                dgvListDetainedLicense.Columns[4].HeaderText = "Fine Fees";
                dgvListDetainedLicense.Columns[4].Width = 110;

                dgvListDetainedLicense.Columns[5].HeaderText = "Release Date";
                dgvListDetainedLicense.Columns[5].Width = 160;

                dgvListDetainedLicense.Columns[6].HeaderText = "N.No.";
                dgvListDetainedLicense.Columns[6].Width = 90;

                dgvListDetainedLicense.Columns[7].HeaderText = "Full Name";
                dgvListDetainedLicense.Columns[7].Width = 330;

                dgvListDetainedLicense.Columns[8].HeaderText = "Release App.ID";
                dgvListDetainedLicense.Columns[8].Width = 150;

            }

            cbFilterBy.SelectedIndex = 0;

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None" && cbFilterBy.Text != "Is Released");
            cbIsReleased.Visible = (cbFilterBy.Text == "Is Released");

            _dtAllDetainedLicenses.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvListDetainedLicense.Rows.Count.ToString();

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            if (cbIsReleased.Visible)
            {
                cbIsReleased.SelectedIndex = 0;
                cbIsReleased.Focus();
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {

            string FilterColumn = "IsReleased";
            string FilterValue = cbIsReleased.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }


            if (FilterValue == "All")
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
            else
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecordsCount.Text = dgvListDetainedLicense.Rows.Count.ToString();

        }

 

        private void txtFilterValue_TextChanged_1(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilterBy.Text)
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Release Application ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;
                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvListDetainedLicense.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "ReleaseApplicationID" || FilterColumn == "DetainID")
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvListDetainedLicense.Rows.Count.ToString();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Detain ID" || cbFilterBy.Text == "Release Application ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnRelesed_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void btnDetained_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NationalNo = dgvListDetainedLicense.CurrentRow.Cells[6].Value.ToString();
            frmShowPersonInfo frm = new frmShowPersonInfo(NationalNo);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvListDetainedLicense.CurrentRow.Cells[1].Value;
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvListDetainedLicense.CurrentRow.Cells[1].Value;
            int PersonID = clsLicense.FindByLicenseID(LicenseID).DriverInfo.PersonID;
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvListDetainedLicense.CurrentRow.Cells[1].Value;
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(LicenseID);
            frm.ShowDialog();
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled = !(bool)dgvListDetainedLicense.CurrentRow.Cells[3].Value;
        }
    }
}
