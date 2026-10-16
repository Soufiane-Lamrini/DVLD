using Business_Layer;
using DVLD.Classes;
using DVLD.Controls;
using DVLD.Properties;
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
using static Business_Layer.clsTestTypes;

namespace DVLD.Test.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        private enum enCreationMode { FirstTime = 0, RetakeTest }
        private enCreationMode _CreationMode = enCreationMode.FirstTime;
        private enum enMode { Add = 1, Edit = 2 };

        enMode _Mode = enMode.Add;



        private int _testAppointmentID = -1;
        private clsTestAppointment _TestAppointment;
        private clsTestTypes.enTestType _TestTypeID = clsTestTypes.enTestType.VisionTest;   
        private int _localDrivingAppID = -1;
        private clsLocalDrivingLicenseApplication _localDrivingApplication;

        public clsTestTypes.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {

                    case clsTestTypes.enTestType.VisionTest:
                        {
                            gbTestAppointmentInfo.Text = "Vision Test";
                            pbTestType.Image = Resources.Vision_512;
                            break;
                        }

                    case clsTestTypes.enTestType.WrittenTest:
                        {
                            gbTestAppointmentInfo.Text = "Written Test";
                            pbTestType.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestTypes.enTestType.StreetTest:
                        {
                            gbTestAppointmentInfo.Text = "Street Test";
                            pbTestType.Image = Resources.driving_test_512;
                            break;


                        }
                }
            }
        }

        public void ManageImageAndTitle(clsTestTypes.enTestType testType)
        {
            switch (testType)
            {
                case clsTestTypes.enTestType.VisionTest:
                    pbTestType.Image = Resources.Vision_512;
                    gbTestAppointmentInfo.Text = "Vision Test";
                    break;
                case clsTestTypes.enTestType.WrittenTest:
                    pbTestType.Image = Resources.Written_Test_512;
                    gbTestAppointmentInfo.Text = "Written Test";
                    break;
                case clsTestTypes.enTestType.StreetTest:
                    pbTestType.Image = Resources.Schedule_Test_512;
                    gbTestAppointmentInfo.Text = "Street Test";
                    break;
                default:
                    pbTestType.Image = Resources.Vision_512;
                    gbTestAppointmentInfo.Text = "Vision Test";
                    break;
            }
        }
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }



        public void LoadInfo(int localDrivingAppID, int appointmentID)
        {
            if(appointmentID == -1)
            {
                _Mode = enMode.Add;
            }
            else
            {
                _Mode = enMode.Edit;
            }

            _testAppointmentID = appointmentID;
            _localDrivingAppID = localDrivingAppID;
            _localDrivingApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(localDrivingAppID);

            if (_localDrivingApplication == null)
            {
                MessageBox.Show("No application with UserID = " + appointmentID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            //Has the person already taken this test?
            if (_localDrivingApplication.DoesAttendTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetakeTest;
            else
                _CreationMode = enCreationMode.FirstTime;



            if (_CreationMode == enCreationMode.RetakeTest)
            {
                lblRetakeTestAppFees.Text = clsApplicationTypes.Find((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestID.Text = "0";
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeTestAppFees.Text = "0";
                lblRetakeTestID.Text = "N/A";
            }



            lblDrivingLicenseAppID.Text = localDrivingAppID.ToString();
            lblLicenseClassName.Text = _localDrivingApplication.LicenseClassInfo.ClassName;
            lblApplicantFullName.Text = _localDrivingApplication.PersonFullName;
            lblTrialTest.Text =
                _localDrivingApplication.TotalTrialsPerTest(_TestTypeID).ToString();
           
            if(_Mode == enMode.Add)
            {
                lblTestFees.Text = clsTestTypes.Find(_TestTypeID).Fees.ToString();
                dateAppointmentDate.MinDate = DateTime.Now;
                lblRetakeTestID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_FillAppointmentInfo())
                    return;
            }

            lblTotalFees.Text = (Convert.ToSingle(lblTestFees.Text) + Convert.ToSingle(lblRetakeTestAppFees.Text)).ToString();


            if (!_HandleActiveTestAppointmentConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePrviousTestConstraint())
                return;

        }

        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.Add && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_localDrivingAppID, _TestTypeID))
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dateAppointmentDate.Enabled = false;
                return false;
            }

            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            //if appointment is locked that means the person already sat for this test
            //we cannot update locked appointment
            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment loacked.";
                dateAppointmentDate.Enabled = false;
                btnSave.Enabled = false;
                return false;

            }
            else
                lblUserMessage.Visible = false;

            return true;
        }
        private bool _FillAppointmentInfo()
        {
            _TestAppointment = clsTestAppointment.FindByTestAppointmentID(_testAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _testAppointmentID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblTestFees.Text = _TestAppointment.PaidFees.ToString();

            //we compare the current date with the appointment date to set the min date.
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointementDate) < 0)
            { 
                dateAppointmentDate.MinDate = DateTime.Now; 
            }  else
            {
                dateAppointmentDate.MinDate = _TestAppointment.AppointementDate;
            }
            dateAppointmentDate.Value = _TestAppointment.AppointementDate;

            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRetakeTestAppFees.Text = "0";
                lblRetakeTestID.Text = "N/A";
            }
            else
            {
                lblRetakeTestAppFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestID.Text = _TestAppointment.RetakeTestApplicationID.ToString();

            }
            return true;
        }

       

        private bool _HandlePrviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannno apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch (TestTypeID)
            {
                case clsTestTypes.enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    lblUserMessage.Visible = false;

                    return true;

                case clsTestTypes.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.
                    if (!_localDrivingApplication.DoesPassTestType(clsTestTypes.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dateAppointmentDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dateAppointmentDate.Enabled = true;
                    }


                    return true;

                case clsTestTypes.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    if (!_localDrivingApplication.DoesPassTestType(clsTestTypes.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dateAppointmentDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dateAppointmentDate.Enabled = true;
                    }


                    return true;

            }
            return true;

        }
        private bool _HandleRetakeApplication()
        {
            //this will decide to create a seperate application for retake test or not.
            // and will create it if needed , then it will linkit to the appoinment.
            if (_Mode == enMode.Add && _CreationMode == enCreationMode.RetakeTest)
            {
                //incase the mode is add new and creation mode is retake test we should create a seperate application for it.
                //then we linke it with the appointment.

                //First Create Applicaiton 
                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = _localDrivingApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationTypes.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;

            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _localDrivingApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointementDate = dateAppointmentDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblTestFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.Edit;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
