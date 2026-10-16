using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsDriver
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int DriverID { set; get; }
        public int PersonID { set; get; }
        public clsPerson PersonInfo { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate {  set; get; }



        public clsDriver()

        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;

            Mode = enMode.AddNew;

        }

        private clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            this.DriverID = driverID; 
            this.PersonID = personID;
            this.PersonInfo =
                clsPerson.Find(PersonID);
            this.CreatedByUserID = createdByUserID;
            this.CreatedDate = createdDate;
            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            //call DataAccess Layer 

            this.DriverID = clsDriverData.AddNewDriver(this.PersonID, this.CreatedByUserID);

            return (this.DriverID != -1);
        }

        private bool _UpdateDriver()
        {
            //call DataAccess Layer 

            return clsDriverData.UpdateDriver(this.DriverID, this.PersonID,
                this.CreatedByUserID);

        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            // 
            int PersonID = 1, CreatedByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByID
                (DriverID, ref PersonID, ref CreatedByUserID, ref createdDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, createdDate);
            }
            else
                return null;

        }

        public static clsDriver FindByPersonID(int PersonID)
        {
            // 
            int DriverID = -1, CreatedByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByPersonID
                (PersonID, ref DriverID, ref CreatedByUserID, ref createdDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, createdDate);
            }
            else
                return null;

        }
        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();

        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }

            return false;
        }


        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }

        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverLicenses(DriverID);
        }
    }
}
