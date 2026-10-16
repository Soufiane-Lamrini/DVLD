using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsDetainLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }
        public float FineFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUser CreatedByUserInfo { set; get; }

        public bool IsReleased { set; get; }
        public DateTime ReleasedDate { set; get; }
        public int ReleasedByUserID { set; get; }
        public clsUser ReleasedByUserInfo { set; get; }

        public int ReleasedApplicationID { set; get; }

     

        public clsDetainLicense()

        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleasedDate = DateTime.MaxValue;
            this.ReleasedByUserID = -1;
            this.ReleasedApplicationID = -1;

            Mode = enMode.AddNew;

        }

        private clsDetainLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees,
            int createdByUserID, bool IsReleased, DateTime ReleasedDate, int ReleasedByUserID, 
            int ReleasedApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = createdByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(createdByUserID);
            this.IsReleased = IsReleased;
            this.ReleasedDate = ReleasedDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleasedByUserInfo = clsUser.FindByUserID(ReleasedByUserID);
            this.ReleasedApplicationID = ReleasedApplicationID;

            Mode = enMode.Update;
        }


        private bool _AddNewDetain()
        {
            //call DataAccess Layer 

            this.DetainID = clsDetainedLicensesData.AddNewDetainedLicense(this.LicenseID, this.DetainDate,
               this.FineFees, this.CreatedByUserID);

            return (this.DetainID != -1);
        }

        private bool _UpdateDriverAppointement()
        {
            //call DataAccess Layer 

            return clsDetainedLicensesData.UpdateDetainedLicense(this.DetainID, this.LicenseID, this.DetainDate,
               this.FineFees, this.CreatedByUserID);

        }

        public static clsDetainLicense FindByDetainID(int DetainID)
        {
            // 
            int LicenseID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleasedApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleasedDate = DateTime.Now;
            float FineFees = 0;
            bool IsReleased = false;

            if (clsDetainedLicensesData.GetDetainInfoByID(DetainID, ref LicenseID, ref DetainDate,
                ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleasedDate,
                ref ReleasedByUserID, ref ReleasedApplicationID))
            {
                return new clsDetainLicense(DetainID, LicenseID, 
                    DetainDate, FineFees, CreatedByUserID, IsReleased,
                    ReleasedDate, ReleasedByUserID, ReleasedApplicationID);
            }
            else
                return null;

        }


        public static clsDetainLicense FindByLicenseID(int LicenseID)
        {
            // 
            int DetainID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleasedApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleasedDate = DateTime.Now;
            float FineFees = 0;
            bool IsReleased = false;

            if (clsDetainedLicensesData.GetDetainInfoByLicenseID(LicenseID, ref DetainID, ref DetainDate,
                ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleasedDate,
                ref ReleasedByUserID, ref ReleasedApplicationID))
            {
                return new clsDetainLicense(DetainID, LicenseID,
                    DetainDate, FineFees, CreatedByUserID, IsReleased,
                    ReleasedDate, ReleasedByUserID, ReleasedApplicationID);
            }
            else
                return null;

        }

        public static DataTable GetAllDetains()
        {
            return clsDetainedLicensesData.GetAllDetains();

        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetain())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriverAppointement();

            }

            return false;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicensesData.IsLicenseDetained(LicenseID);
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID,int ApplicationID)
        {
            return clsDetainedLicensesData.ReleaseDetainedLicense(this.DetainID,
                              ReleasedByUserID, ApplicationID);
        }
    }
}
