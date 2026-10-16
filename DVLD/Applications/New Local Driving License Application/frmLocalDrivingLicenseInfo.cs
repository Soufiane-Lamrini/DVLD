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
    public partial class frmLocalDrivingLicenseInfo : Form
    {
        public frmLocalDrivingLicenseInfo(int LDLAppID)
        {
            InitializeComponent();
            ctrlLocalDrivingLicenseAppInfo1.LoadLocalDrivingAppInfoByLocalDrivingLicenseID(LDLAppID);
        }

        private void frmLocalDrivingLicenseInfo_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
