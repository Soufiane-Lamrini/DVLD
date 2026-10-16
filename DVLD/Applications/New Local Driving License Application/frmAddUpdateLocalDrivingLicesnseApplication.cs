using Business_Layer;
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

namespace DVLD.Applications.New_Local_Driving_License_Application
{
    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {

        
        clsLocalDrivingLicenseApplication _clsLDLApp;
        private int _LDLAppID = -1;
        private int _SelectedPersonID = -1;

        private enum enMode { enAdd, enEdit }
        enMode Mode;

        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            Mode = enMode.enAdd;
        }

        public frmAddUpdateLocalDrivingLicesnseApplication(int LDLAppID)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
            Mode = enMode.enEdit;
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dtCountries = clsLicenseClasses.GetAllLicenseclasses();

            foreach (DataRow row in dtCountries.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }
        private void _ResetDefualtValues()
        {
            //this will initialize the reset the defaule values
            _FillLicenseClassesInComboBox();


            if (Mode == enMode.enAdd)
            {

                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _clsLDLApp = new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;
                cbLicenseClass.SelectedIndex = 2;
                lblFees.Text = clsApplicationTypes.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;

            }

        }

        private void _loadData()
        {
            _clsLDLApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LDLAppID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_clsLDLApp == null)
            {
                MessageBox.Show("There are no local driving application with this ID: " + _LDLAppID.ToString(), "Application Not Found", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_clsLDLApp.ApplicantPersonID);
            lblLocalDrivingLicebseApplicationID.Text = _clsLDLApp.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_clsLDLApp.ApplicationDate);
            lblFees.Text = _clsLDLApp.PaidFees.ToString();
            lblCreatedByUser.Text = clsUser.FindByUserID(_clsLDLApp.CreatedByUserID).UserName;
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClasses.Find(_clsLDLApp.LicenseClassID).ClassName);

        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
            if (Mode == enMode.enEdit)
            {
                _loadData();
            }
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if (Mode == enMode.enAdd)
            {
                if (ctrlPersonCardWithFilter1.PersonID == -1)
                {
                    MessageBox.Show("Please select a valid person", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.Focus();
                }
                else
                {

                    tpApplicationInfo.Enabled = true;
                    btnSave.Enabled = true;
                    tcApplicationInfo.SelectedTab = tpApplicationInfo;
                }


            }
            else if (Mode == enMode.enEdit)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tpApplicationInfo;
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = clsLicenseClasses.Find(cbLicenseClass.Text.Trim()).ID;


            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }


            //check if user already have issued license of the same driving  class.
            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {

                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _clsLDLApp.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _clsLDLApp.ApplicationDate = DateTime.Now;
            _clsLDLApp.ApplicationTypeID = 1;
            _clsLDLApp.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _clsLDLApp.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _clsLDLApp.LastStatusDate = DateTime.Now;
            _clsLDLApp.PaidFees = Convert.ToSingle(lblFees.Text);
            _clsLDLApp.LicenseClassID = LicenseClassID;


            if (_clsLDLApp.Save())
            {
                lblLocalDrivingLicebseApplicationID.Text = _clsLDLApp.LocalDrivingLicenseApplicationID.ToString();

                Mode = enMode.enEdit;
                lblTitle.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}

