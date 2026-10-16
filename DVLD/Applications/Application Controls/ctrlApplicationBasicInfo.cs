using Business_Layer;
using DVLD.Classes;
using DVLD.People;
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
using static DVLD.People.frmAddUpdatePerson;

namespace DVLD.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        private int _AppID = -1;
        private clsApplication _clsApp;

        public int AppID {
            get { return _AppID; }
        }

        public void ResetAppInfo()
        {
            _AppID = -1;
            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblFees.Text = "[????]";
            lblType.Text = "[????]";
            lblApplicantFullName.Text = "[????]";
            lblAppDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblUserName.Text = "[????]";          
        }

        private void _FillAppInfo()
        {
            linkLblPersonInfo.Enabled = true;
            _AppID = _clsApp.ApplicationID;
            lblApplicationID.Text = _clsApp.ApplicationID.ToString();
            lblStatus.Text = _clsApp.StatusText;
            lblFees.Text = _clsApp.PaidFees.ToString();
            lblType.Text = clsApplicationTypes.Find(_clsApp.ApplicationTypeID).ApplicationTypeTitle;
            lblApplicantFullName.Text = clsPerson.Find(_clsApp.ApplicantPersonID).FullName;
            lblAppDate.Text = clsFormat.DateToShort(_clsApp.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(_clsApp.LastStatusDate);
            lblUserName.Text = _clsApp.CreatedByUserInfo.UserName;
           
        }


        public void LoadAppInfo(int AppID)
        {
            _clsApp = clsApplication.FindBaseApplication(AppID);
            if (_clsApp == null)
            {
                ResetAppInfo();
                MessageBox.Show("No Application with Application ID = " + AppID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillAppInfo();
        }


        private void ctrlApplicationBasicInfo_Load(object sender, EventArgs e)
        {

        }

        private void linkLblPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_clsApp.ApplicantPersonID);
            frm.ShowDialog();
            
        }
    }
}
