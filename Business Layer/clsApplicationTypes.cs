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
    public class clsApplicationTypes
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;



        public int ApplicationTypeID { private set; get; }
        public string ApplicationTypeTitle { set; get; }
        public float Fees { set; get; }

        private clsApplicationTypes()
        {

            this.ApplicationTypeID = -1;
            this.ApplicationTypeTitle = "";
            this.Fees = 0;
            Mode = enMode.AddNew;
        }
        private clsApplicationTypes(int ApplicationTypeID, string ApplicationTypeTitle, float Fees)
        {

            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.Fees = Fees;
            Mode = enMode.Update;
        }

        public static clsApplicationTypes Find(int ID)
        {

            string ApplicationTypeTitle = "";
            float Fees = 0;
            if (clsApplicationTypesData.GetApplicationTypeInfoByApplicationTypeID(ID, ref ApplicationTypeTitle, ref Fees))

                return new clsApplicationTypes(ID, ApplicationTypeTitle, Fees);
            else
                return null;

        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypesData.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTypeTitle, this.Fees);
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationsTypes();
        }

        private bool _AddNewApplicationType()
        {
            //call DataAccess Layer 

            this.ApplicationTypeID = clsApplicationTypesData.AddNewApplicationType(this.ApplicationTypeTitle, this.Fees);


            return (this.ApplicationTypeID != -1);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicationType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplicationType();

            }

            return false;
        }


    }
}
