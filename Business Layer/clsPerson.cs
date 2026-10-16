using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Business_Layer
{
    public class clsPerson
    {
        public enum enMode { enAdd = 1, enUpdate = 2 };

        public int PersonID { private set; get; }
        public string NationalNo { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName { set; get; }
        public DateTime DateOfBirth { set; get; }
        public byte Gendor { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }
        private string _ImagePath;
        public clsCountry CountryInfo;
        public string ImagePath
        {
            set { _ImagePath = value; }
            get { return _ImagePath; }
        }
        public enMode Mode = enMode.enAdd;


        public clsPerson()
        {
            Mode = enMode.enAdd;

            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gendor = 0;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.ImagePath = "";
            this.NationalityCountryID = -1;

        }

        private clsPerson(int id, string nationalNo, string firstName, string secondName, string thirdName, string lastName,
                                  DateTime dateOfBirth, byte gendor, string address, string phone,
                                       string email, int countryId, string imagePath)
        {

            this.PersonID = id;
            this.NationalNo = nationalNo;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.FullName = FirstName + " " + SecondName + " " + ThirdName + " " + LastName;
            this.DateOfBirth = dateOfBirth;
            this.Gendor = gendor;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.NationalityCountryID = countryId;
            this.ImagePath = imagePath;
            this.CountryInfo = clsCountry.FindByCountryID(countryId);
            Mode = enMode.enUpdate;
        }
        public static clsPerson Find(int ID)
        {

            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int CountryID = -1;
            byte Gendor = 0;
            if (clsPersonData.GetPersonInfoByPersonID(ID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                         ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath))

                return new clsPerson(ID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                            DateOfBirth, Gendor, Address, Phone, Email, CountryID, ImagePath);
            else
                return null;

        }

        public static clsPerson FindPersonByNationalNo(string NationalNo)
        {
            int PersonID = -1;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int CountryID = -1;
            byte Gendor = 0;
            if (clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                         ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath))

                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                            DateOfBirth, Gendor, Address, Phone, Email, CountryID, ImagePath);
            else
                return null;

        }
        private bool _AddNewPerson()
        {

            this.PersonID = clsPersonData.AddPerson(this.NationalNo, this.FirstName, this.SecondName,
                this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor, this.Address, this.Phone,
                this.Email, this.NationalityCountryID, this.ImagePath);

            return (this.PersonID != -1);

        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName,
                this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor, this.Address, this.Phone,
                this.Email, this.NationalityCountryID, this.ImagePath);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.enAdd:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.enUpdate;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.enUpdate:
                    if (_UpdatePerson())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }

            return false;
        }

        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }



        public static DataTable GetAllPersons()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool IsPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExistByPersonID(PersonID);
        }

        public static bool IsPersonExistbyNationalNo(string NationalNo)
        {
            return clsPersonData.IsPersonExistByNationalNo(NationalNo);
        }

    }
}
