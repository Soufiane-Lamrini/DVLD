using System;
using System.Data;
using System.Data.SqlClient;


namespace DVLD_DataAccess
{
    public static class clsCountriesData
    {
        public static bool GetCountryInfoByCountryID(int CountryID, ref string CountryName)
        {
            bool IsFound = false;

            string query = "Select * from Countries Where CountryID = @CountryID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())

                        if (reader.Read())
                        {
                            IsFound = true;
                            CountryName = (string)reader["CountryName"];
                        }
                }
            }
            catch (Exception ex)
            {

                IsFound = false;
            }
          
            return IsFound;
        }

        public static bool GetCountryInfoByCountryName(string CountryName, ref int CountryID)
        {
            bool IsFound = false;

            string query = "Select * from Countries Where CountryName = @countryName";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@countryName", CountryName);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            CountryID = (int)reader["CountryID"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                IsFound = false;
            }
           
            return IsFound;
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            string query = "Select * from Countries";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                { 
                connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            
            return dt;
        }

        public static bool IsCountryExistByCountryID(int CountryID)
        {
            bool isFound = false;

            string query = @"Select found = 1 from Countries 
                                where CountryID = @CountryID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            
            return isFound;
        }

        public static bool IsCountryExistByCountryName(string CountryName)
        {
            bool isFound = false;

            string query = @"Select found = 1 from Countries 
                                where CountryName = @countryName";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@countryName", CountryName);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        isFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
            }
          
            return isFound;
        }

    }
}
