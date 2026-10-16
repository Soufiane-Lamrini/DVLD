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

namespace DVLD.Users
{
    public partial class frmAddUpdateUser : Form
    {

        clsUser _user;
        int _userID = -1;
        enum enMode { enAdd, enEdit }
        enMode Mode;

        public frmAddUpdateUser()
        {
            InitializeComponent();
            Mode = enMode.enAdd;
        }

        public frmAddUpdateUser(int userID)
        {
            InitializeComponent();
            _userID = userID;
            Mode = enMode.enEdit;
        }

        private void _ResetDefaultValue()
        {
            if (Mode == enMode.enAdd)
            {
                _user = new clsUser();
                this.Text = "Add New User";
                lblTitle.Text = "Add New User";
                ctrlPersonCardWithFilter1.FilterFocus();
                btnSave.Enabled = false;
                tpLoginInfo.Enabled = false;

            }
            else
            {
                lblTitle.Text = "Update New User";
                this.Text = "Update User";


                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;

            }

            chkIsActive.Checked = true;
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";

        }

        private void _loadData()
        {
            _user = clsUser.FindByUserID(_userID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_user == null) {
                MessageBox.Show("There are no users with this ID: " + _userID.ToString(), "User Not Found", MessageBoxButtons.OK, 
                    MessageBoxIcon.Exclamation);
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_user.PersonID);
            lblUserID.Text = _userID.ToString();
            txtUserName.Text = _user.UserName;
            txtPassword.Text = _user.PassWord;
            txtConfirmPassword.Text = _user.PassWord;
            chkIsActive.Checked = _user.IsActive;

        }
        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();
            if (Mode == enMode.enEdit)
            {
                _loadData();
            }
        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {

            if (Mode == enMode.enAdd)
            {
                int personID = ctrlPersonCardWithFilter1.PersonID;

                if (clsUser.IsUserExistByPersonId(personID))
                {
                    MessageBox.Show("This Person already have a user account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }
                else if (personID == -1)
                {
                    MessageBox.Show("Please select a valid person", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.Focus();
                }
                else
                {
                    
                    tpLoginInfo.Enabled = true;
                    btnSave.Enabled = true;
                    tcUserInfo.SelectedTab = tpLoginInfo;
                }


            }
            else if (Mode == enMode.enEdit)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tpLoginInfo;
                return;
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            }

            if (Mode == enMode.enAdd)
            {
                if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "This username is already used by another exist user!");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }

            }
            else
            {
                if (_user.UserName != txtUserName.Text.Trim() && clsUser.IsUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "This username is already used by another exist user!, Please seecte another.");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _user.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _user.UserName = txtUserName.Text.Trim();
            _user.PassWord = txtPassword.Text.Trim();
            if(chkIsActive.Checked)
            {
                _user.IsActive = true;
            }
            else
            {
                _user.IsActive = false;
            }
            if (_user.Save())
            {
                lblUserID.Text = _user.UserID.ToString();

                Mode = enMode.enEdit;
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }
    }
}
