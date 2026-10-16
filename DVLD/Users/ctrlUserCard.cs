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
    public partial class ctrlUserCard : UserControl
    {

        private clsUser _user;

        private int _userID = -1;

        public int UserID
        {
            get { return _userID; }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }



        public void ResetUserInfo()
        {
            ctrlPersonCard1.ResetPersonInfo();
            lblUserName.Text = "";
            lblUserID.Text = "";
            lblIsActive.Text = "";
        }

        public void LoadUserInfo(int userID)
        {
            _userID = userID;
            _user = clsUser.FindByUserID(_userID);
            
            if (_user == null)
            {
                ResetUserInfo();
                MessageBox.Show("No User with UserID = " + userID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillUserInfo();
        }

        private void _FillUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(_user.PersonID);
            lblUserName.Text = _user.UserName;
            lblUserID.Text = _user.UserID.ToString();
            lblIsActive.Text = (_user.IsActive ? "Yes" : "No");
        }

        private void ctrlUserCard_Load(object sender, EventArgs e)
        {

        }
    }
}
