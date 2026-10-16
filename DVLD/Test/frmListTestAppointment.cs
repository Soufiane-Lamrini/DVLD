using Business_Layer;
using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Business_Layer.clsTestTypes;

namespace DVLD.Test
{
    public partial class frmListTestAppointment : Form
    {

        private DataTable _dtAllTestAppointmentPerTestType;

        private int _localDrivingApplicationID = -1;
        private clsTestTypes.enTestType _TestTypeID = clsTestTypes.enTestType.VisionTest;
        public frmListTestAppointment(int localDrivingApplicationID, clsTestTypes.enTestType testTypeID)
        {
            InitializeComponent();
            _localDrivingApplicationID = localDrivingApplicationID;
            _TestTypeID = testTypeID;
        }

        private void _LoadImageAndTitle()
        {
            switch (_TestTypeID) {
                case clsTestTypes.enTestType.VisionTest:
                    this.Text = "List Vision Test Appointment";
                    pbImage.Image = Resources.Vision_512;
                    lblTitle.Text = "Vision Test";
                    break;
                case clsTestTypes.enTestType.StreetTest:
                    this.Text = "List Street Test Appointment";
                    pbImage.Image = Resources.Street_Test_32;
                    lblTitle.Text = "Vision Test";
                    break;
                case clsTestTypes.enTestType.WrittenTest:
                    this.Text = "List Written Test Appointment";
                    pbImage.Image = Resources.Written_Test_512;
                    lblTitle.Text = "Written Test";
                    break;
            }

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListTestAppointment_Load(object sender, EventArgs e)
        {
            _LoadImageAndTitle();
            ctrlLocalDrivingLicenseAppInfo1.LoadLocalDrivingAppInfoByLocalDrivingLicenseID(_localDrivingApplicationID);

            _dtAllTestAppointmentPerTestType = clsTestAppointment.GetAllTestAppointementsPerTestType(_localDrivingApplicationID, _TestTypeID);
            dgvAllTestAppointment.DataSource = _dtAllTestAppointmentPerTestType;

            lblRecordsCount.Text = dgvAllTestAppointment.Rows.Count.ToString();
            if (dgvAllTestAppointment.Rows.Count > 0)
            {

                dgvAllTestAppointment.Columns[0].HeaderText = "Appointment ID";
                dgvAllTestAppointment.Columns[0].Width = 150;

                dgvAllTestAppointment.Columns[1].HeaderText = "Appointment Date";
                dgvAllTestAppointment.Columns[1].Width = 200;

                dgvAllTestAppointment.Columns[2].HeaderText = "Paid Fees";
                dgvAllTestAppointment.Columns[2].Width = 150;

                dgvAllTestAppointment.Columns[3].HeaderText = "Is Locked";
                dgvAllTestAppointment.Columns[3].Width = 100;


            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_localDrivingApplicationID);


            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            //---
            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestTypeID);

            if (LastTest == null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_localDrivingApplicationID, _TestTypeID);
                frm1.ShowDialog();
                frmListTestAppointment_Load(null, null);
                return;
            }

            //if person already passed the test s/he cannot retak it.
            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest
                (LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestTypeID);
            frm2.ShowDialog();
            frmListTestAppointment_Load(null, null);

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppointmentID = (int)dgvAllTestAppointment.CurrentRow.Cells[0].Value;
            frmScheduleTest frm = new frmScheduleTest(_localDrivingApplicationID, _TestTypeID, AppointmentID);
            frm.ShowDialog();
            frmListTestAppointment_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppointmentID = (int)dgvAllTestAppointment.CurrentRow.Cells[0].Value;
            frmTakeTest frm = new frmTakeTest(AppointmentID, _TestTypeID);
            frm.ShowDialog();
            frmListTestAppointment_Load(null, null);
        }
    }
}
