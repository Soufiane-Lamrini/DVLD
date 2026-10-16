using DataAccessLayer;
using DVLD_Buisness;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Business_Layer
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID { set; get; }
        public clsTestTypes.enTestType TestTypeID { set; get; }
        public clsTestTypes testTypes { set; get; }
        public int LocalDrivingLicenseApplicationID {  set; get; }
        public DateTime AppointementDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsLocked {  set; get; }
        public int RetakeTestApplicationID {  set; get; }
        public clsApplication RetakeTestAppInfo { set; get; }


        public int TestID
        {
            get { return _GetTestID(); }

        }
        public clsTestAppointment()

        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestTypes.enTestType.VisionTest;
            this.AppointementDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.RetakeTestApplicationID = -1;

            Mode = enMode.AddNew;

        }

        private clsTestAppointment(int TestAppointmentID, clsTestTypes.enTestType TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointementDate, float PaidFees, int CreatedByUserID, bool IsLocked, 
                int RetakeTestApplicationID)
        {
            this.TestAppointmentID = TestAppointmentID; ;
            this.TestTypeID = TestTypeID;
            this.testTypes = clsTestTypes.Find(this.TestTypeID);
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointementDate = AppointementDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            this.RetakeTestAppInfo = clsApplication.FindBaseApplication(RetakeTestApplicationID);

            Mode = enMode.Update;
        }

        private bool _AddNewTestAppointement()
        {
            //call DataAccess Layer 

            this.TestAppointmentID = clsTestAppointmentsData.AddNewTestAppointment
                (
                (int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointementDate, 
                    this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);

            return (this.TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointement()
        {
            //call DataAccess Layer 

            return clsTestAppointmentsData.UpdateTestAppointment
                (
                this.TestAppointmentID, (int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointementDate, 
                    this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);

        }

        public static clsTestAppointment FindByTestAppointmentID(int TestAppointmentID)
        {
            // 
            int  TestTypeID = 1, LocalDrivingApplicationID = -1, CreatedByUserID = -1, RetakeTestApplicationID = -1;
             DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            bool IsLocked = false;
           
            if (clsTestAppointmentsData.GetTestAppointmentInfoByID
                (TestAppointmentID, ref TestTypeID, ref LocalDrivingApplicationID, ref AppointmentDate,
                    ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestTypes.enTestType)TestTypeID, LocalDrivingApplicationID, AppointmentDate, PaidFees, CreatedByUserID,
                           IsLocked, RetakeTestApplicationID);
            }
            else
                return null;

        }

        public static clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, clsTestTypes.enTestType TestTypeID)
        {
            int TestAppointmentID = -1;
            DateTime AppointmentDate = DateTime.Now; float PaidFees = 0;
            int CreatedByUserID = -1; bool IsLocked = false; int RetakeTestApplicationID = -1;

            if (clsTestAppointmentsData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, (int)TestTypeID,
                ref TestAppointmentID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
             AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            else
                return null;

        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentsData.GetAllTestAppointments();

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

        public static DataTable GetAllTestAppointementsPerTestType(int LDLAppID, clsTestTypes.enTestType TestTypeID)
        {
            return clsTestAppointmentsData.GetTestAppointmentsPerTestTypes(LDLAppID, (int)TestTypeID);
        }

        private int _GetTestID()
        {
            return clsTestAppointmentsData.GetTestID(this.TestAppointmentID);
        }
    }
}
