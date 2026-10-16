using DVLD_DataAccess;

using System.Data;


namespace Business_Layer
{
    public class clsCountry
    {
        public enum enMode { enAdd = 1, enUpdate = 2 };

        public int CountryID { private  set; get; }
        public string CountryName { set; get; }
        public enMode Mode { set; get; }


        public clsCountry()
        {
            Mode = enMode.enAdd;

            this.CountryID = -1;
            this.CountryName = "";
          
        }

        private clsCountry(int id, string CountryName)
        {

            this.CountryID = id;
            this.CountryName = CountryName;
           

            Mode = enMode.enUpdate;
        }
        public static clsCountry FindByCountryID(int ID)
        {

            string CountryName = "";
            if (clsCountriesData.GetCountryInfoByCountryID(ID, ref CountryName))

                return new clsCountry(ID, CountryName);
            else
                return null;

        }

        public static clsCountry FindByCountyName(string countryName)
        {

            int CountryID = -1;
            if (clsCountriesData.GetCountryInfoByCountryName(countryName, ref CountryID))

                return new clsCountry(CountryID, countryName);
            else
                return null;

        }

        public static DataTable GetAllCountrys()
        {
            return clsCountriesData.GetAllCountries();
        }

        public static bool IsCountryExistByID(int CountryID)
        {
            return clsCountriesData.IsCountryExistByCountryID(CountryID);
        }

        public static bool IsCountryExistByIDCountryName(string CountryName)
        {
            return clsCountriesData.IsCountryExistByCountryName(CountryName);
        }
    }
}
