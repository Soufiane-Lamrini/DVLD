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
    public class clsLicenseClasses
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public float Fees { set; get; }

        private clsLicenseClasses()
        {

            this.ID = 2;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 0;
            this.DefaultValidityLength = 0;
            this.Fees = 0;
            Mode = enMode.AddNew;
        }
        private clsLicenseClasses(int licenseClasseID, string ClassName, string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float Fees)
        {

            this.ID = licenseClasseID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.Fees = Fees;
            Mode = enMode.Update;
        }

        public static clsLicenseClasses Find(int ID)
        {

            string ClassName = "", ClassDescription = "";
            float Fees = 0;
            byte MinimumAllowedAge = 0, DefaultValidityLength = 0;
            if (clsLicenseClassesData.GetLicenseClassInfoByID(ID, ref ClassName, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength,ref Fees))

                return new clsLicenseClasses(ID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, Fees);
            else
                return null;

        }

        public static clsLicenseClasses Find(string ClassName)
        {
            int LicenseClassID = -1; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; float ClassFees = 0;

            if (clsLicenseClassesData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, ref ClassDescription,
                    ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

                return new clsLicenseClasses(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;

        }
        private bool _UpdateLicenseclasse()
        {
            return clsLicenseClassesData.UpdateLicenseClass((byte)this.ID, this.ClassName, this.ClassDescription, this.MinimumAllowedAge, this.DefaultValidityLength, this.Fees);
        }

        public static DataTable GetAllLicenseclasses()
        {
            return clsLicenseClassesData.GetAllLicenseClasses();
        }

        private bool _AddNewLicenseclasse()
        {
            //call DataAccess Layer 

            this.ID = clsLicenseClassesData.AddNewLicenseClass(this.ClassName, this.ClassDescription, this.MinimumAllowedAge, this.DefaultValidityLength, this.Fees);


            return (this.ClassName != "");
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicenseclasse())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicenseclasse();

            }

            return false;
        }

    }
}

