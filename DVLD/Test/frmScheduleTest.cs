using Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Test
{
    public partial class frmScheduleTest : Form
    {

        private int _LocalDrivingApplication = -1;
        private int _AppointmentID = -1;
        private clsTestTypes.enTestType _TestTypeID = clsTestTypes.enTestType.VisionTest;

        public frmScheduleTest(int localDrivingApplicationID, clsTestTypes.enTestType testType, int appointmentID = -1)
        {
            InitializeComponent();
            _LocalDrivingApplication = localDrivingApplicationID;
            _TestTypeID = testType;
            _AppointmentID = appointmentID;
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestTypeID;
            ctrlScheduleTest1.LoadInfo(_LocalDrivingApplication, _AppointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
