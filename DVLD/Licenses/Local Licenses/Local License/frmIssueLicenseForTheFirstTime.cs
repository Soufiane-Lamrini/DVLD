using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.License.Issue_license_for_the_first_time
{
    public partial class frmIssueLicenseForTheFirstTime : Form
    {

        private int _LocalDrivingLicenseAppID = -1;
        private clsLocalDrivingLicenseApplication _localDrivingLicenseApplication;
        public frmIssueLicenseForTheFirstTime(int localDrivingLicenseAppID)
        {
            InitializeComponent();
            _LocalDrivingLicenseAppID = localDrivingLicenseAppID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void frmIssueLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();

            _localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseAppID);

            if( _localDrivingLicenseApplication == null )
            {
                MessageBox.Show("No application with id: [" + _LocalDrivingLicenseAppID.ToString() + "]",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;
            }

            if(!_localDrivingLicenseApplication.PassedAllTests())
            {

                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _localDrivingLicenseApplication.GetActiveLicenseID();
            if (LicenseID != -1)
            {

                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }
            ctrlLocalDrivingLicenseAppInfo1.LoadLocalDrivingAppInfoByLocalDrivingLicenseID(_LocalDrivingLicenseAppID);

        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            int LicenseID = _localDrivingLicenseApplication.IssueLicenseForTheFirtTime(txtNotes.Text.Trim(),
                clsGlobal.CurrentUser.UserID);
            if ( LicenseID != -1) {

                MessageBox.Show("License Issued Successfully with ID = " + LicenseID.ToString(), "Succeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("License not Issued", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
