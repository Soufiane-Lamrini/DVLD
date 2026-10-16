using Business_Layer;
using DVLD.License.Local_License;
using DVLD.People;
using DVLD_Buisness;
using System;
using System.Windows.Forms;

namespace DVLD.Applications.New_Local_Driving_License_Application
{
    public partial class ctrlLocalDrivingLicenseAppInfo : UserControl
    {
        public ctrlLocalDrivingLicenseAppInfo()
        {
            InitializeComponent();
        }

        private clsLocalDrivingLicenseApplication _clsLDLApp;
        private int _LDLAppID = -1;
        private int _LicenseID = -1;

        public int LDLAppID
        {
            get { return _LDLAppID; }
        }

        public void ResetLocalDrivingAppInfo()
        {
            _LDLAppID = -1;
            _LicenseID = -1;
            _clsLDLApp = null;

            linkLblLicenseInfo.Enabled = false;

            ctrlApplicationBasicInfo1.ResetAppInfo();
            lblLocalDrivingLicenseAppID.Text = "[?????]";
            lblLicenseClass.Text = "[?????]";
            lblPassedTests.Text = "[?????]";
        }

        public void LoadLocalDrivingAppInfoByLocalDrivingLicenseID(int LDLAppID)
        {
            _LDLAppID = LDLAppID;
            _clsLDLApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LDLAppID);

            if (_clsLDLApp == null)
            {
                ResetLocalDrivingAppInfo();
                MessageBox.Show("No Local driving application with Local driving Application ID = " + LDLAppID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDriverApplicationInfo();
        }

        public void LoadLocalDrivingAppInfoByApplicationID(int AppID)
        {
            _clsLDLApp = clsLocalDrivingLicenseApplication.FindByApplicationID(AppID);

            if (_clsLDLApp == null)
            {
                ResetLocalDrivingAppInfo();
                MessageBox.Show("No Local driving application with Application ID = " + AppID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LDLAppID = _clsLDLApp.LocalDrivingLicenseApplicationID;
            _FillLocalDriverApplicationInfo();
        }

        private void _FillLocalDriverApplicationInfo()
        {
            _LicenseID = _clsLDLApp.GetActiveLicenseID();

            // In case there is a license, enable the show link.
            linkLblLicenseInfo.Enabled = (_LicenseID != -1);

            ctrlApplicationBasicInfo1.LoadAppInfo(_clsLDLApp.ApplicationID);
            lblLocalDrivingLicenseAppID.Text = _clsLDLApp.LocalDrivingLicenseApplicationID.ToString();
            lblPassedTests.Text = _clsLDLApp.GetPassedTestCount().ToString() + "/3";

            var licenseClass = clsLicenseClasses.Find(_clsLDLApp.LicenseClassID);
            lblLicenseClass.Text = licenseClass?.ClassName ?? "[?????]";
        }

        private void linkLblLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_LicenseID == -1)
                return;

            // Adaptez le nom de la form et son constructeur à votre projet.
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        // À brancher sur le clic du lien "Show Person Info" (et non sur Load).
        private void _ShowApplicantInfo()
        {
            if (_clsLDLApp == null)
                return;

            frmShowPersonInfo frm = new frmShowPersonInfo(_clsLDLApp.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}