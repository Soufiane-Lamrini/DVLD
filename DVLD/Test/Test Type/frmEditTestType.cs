using Business_Layer;
using DVLD.Classes;
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
    public partial class frmEditTestType : Form
    {
        int _testTypeID;
        clsTestTypes _testTypes;

        public frmEditTestType(int TestTypeID)
        {
            InitializeComponent();
            _testTypeID = TestTypeID;
        }

        private void loadData()
        {
            lblTestTypeID.Text = _testTypeID.ToString();
            txtTitle.Text = _testTypes.TestTypeTitle;
            txtDescription.Text = _testTypes.TestTypeDescription;
            txtFees.Text = _testTypes.Fees.ToString();
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _testTypes = clsTestTypes.Find((clsTestTypes.enTestType)_testTypeID);
            if (_testTypes != null)
            {
                loadData();
            }
            else
            {
                MessageBox.Show("ID not Found.",
                    "Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDescription, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtDescription, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFees, null);

            }
            ;


            if (!clsValidatoin.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _testTypes.TestTypeTitle = txtTitle.Text.Trim();
            _testTypes.TestTypeDescription = txtDescription.Text.Trim();
            _testTypes.Fees = float.Parse(txtFees.Text.Trim());

            if (_testTypes.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
