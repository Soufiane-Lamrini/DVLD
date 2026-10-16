using DataAccessLayer;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsTest
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestID { set; get; }
        public  int TestAppointmentID { set; get; }
        public clsTestAppointment TestAppointmentInfo { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }
        public int CreatedByUserID { set; get; }
        

        public clsTest()

        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }

        private clsTest(int TestID, int TestAppointmentID, bool TestResult,
            string Notes, int CreatedByUserID)
        {
            this.TestID = TestID; ;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = 
                clsTestAppointment.FindByTestAppointmentID(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
           
            Mode = enMode.Update;
        }

        private bool _AddNewTestAppointement()
        {
            //call DataAccess Layer 

            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID, this.TestResult,
                this.Notes, this.CreatedByUserID);

            return (this.TestID != -1);
        }

        private bool _UpdateTestAppointement()
        {
            //call DataAccess Layer 

            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID, 
                this.TestResult, this.Notes, this.CreatedByUserID);

        }

        public static clsTest FindByTestID(int TestID)
        {
            // 
            int TestAppointmentID = 1, CreatedByUserID = -1;
            string Notes = "";
            bool TestResult = false;

            if (clsTestData.GetTestInfoByID
                (TestID, ref TestAppointmentID, ref TestResult,
                    ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            else
                return null;

        }

        public static clsTest GetLastTestGetLastTestByPersonAndTestTypeAndLicenseClass
            (int PersonID, int LicenseClassID, clsTestTypes.enTestType TestTypeID)
        {
            int TestID = -1, TestAppointmentID = -1;
            string Notes = "";
            int CreatedByUserID = -1; bool TestResult = false;

            if (clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass(PersonID,LicenseClassID, (int)TestTypeID,
                ref TestID, ref TestAppointmentID,ref TestResult, ref Notes, ref CreatedByUserID))

                return new clsTest(TestID, TestAppointmentID, TestResult,
             Notes, CreatedByUserID);
            else
                return null;

        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();

        }
        public bool Save()
        {
            
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointement())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestAppointement();

            }

            return false;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }
    }
}
