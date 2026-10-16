using DataAccessLayer;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsTestTypes
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest };
        public clsTestTypes.enTestType ID { set; get; }
        public string TestTypeTitle { set; get; }
        public string TestTypeDescription { set; get; }

        public float Fees { set; get; }

        private clsTestTypes()
        {

            this.ID = clsTestTypes.enTestType.VisionTest;
            this.TestTypeTitle = "";
            this.TestTypeDescription = "";
            this.Fees = 0;
            Mode = enMode.AddNew;
        }
        private clsTestTypes(enTestType TestTypeID, string TestTypeTitle, string TestTypeDescription, float Fees)
        {

            this.ID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.Fees = Fees;
            Mode = enMode.Update;
        }

        public static clsTestTypes Find(enTestType ID)
        {

            string TestTypeTitle = "", TestTypeDescription = "";
            float Fees = 0;
            if (TestTypesData.GetTestTypeInfoByTestTypeID((int)ID, ref TestTypeTitle, ref TestTypeDescription, ref Fees))

                return new clsTestTypes(ID, TestTypeTitle, TestTypeDescription, Fees);
            else
                return null;

        }

        private bool _UpdateTestType()
        {
            return TestTypesData.UpdateTestType((int)this.ID, this.TestTypeTitle, this.TestTypeDescription, this.Fees);
        }

        public static DataTable GetAllTestTypes()
        {
            return TestTypesData.GetAllTestsTypes();
        }

        private bool _AddNewTestType()
        {
            //call DataAccess Layer 

            this.ID = (enTestType) TestTypesData.AddNewTestType(this.TestTypeTitle, this.TestTypeDescription, this.Fees);


            return (this.TestTypeTitle != "");
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestType();

            }

            return false;
        }

    }
}
