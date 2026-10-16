using Business_Layer;
using DVLD.Classes;
using DVLD.License.Local_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Detain_License
{
    public partial class frmDetainLicense : Form
    {
        private int _LicenseID = -1;
        private int _DetainID;
        public frmDetainLicense()
        {
            InitializeComponent();
        }

        public frmDetainLicense(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

        }
        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            if (_LicenseID != -1)
            {
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_LicenseID);
                lblLicenseID.Text = _LicenseID.ToString();
                numFineFees.Focus();
            }

            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblUserName.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected_1(int obj)
        {
            _LicenseID = obj;

            if(_LicenseID == -1)
            {
                return;
            }

            llShowLicenseHistory.Enabled = (_LicenseID != -1);
            llShowLicenseInfo.Enabled = (_LicenseID != -1);
            lblLicenseID.Text = _LicenseID.ToString();

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License is already Detain, Choose another one"
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }



            btnDetain.Enabled = true;

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new 
                frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                 "Are you sure you want to Detain the license?",
                 "Confirm",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            _DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Detain((float)numFineFees.Value,
                    clsGlobal.CurrentUser.UserID);

            if (_DetainID == -1) 
            {
                MessageBox.Show("Faild to Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

           

            lblDetainID.Text = _DetainID.ToString();
            MessageBox.Show("License Detained Successfully with ID = " + _DetainID.ToString(), "License Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnDetain.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;


        }
    }
}
