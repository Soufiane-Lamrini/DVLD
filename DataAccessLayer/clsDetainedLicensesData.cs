using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class clsDetainedLicensesData
    {

        public static bool GetDetainInfoByID(
           int DetainID, ref int LicenseID, ref DateTime DetainDate, ref float FineFees,
           ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, 
           ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";



            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainID", DetainID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            // The record was found
                            isFound = true;

                            LicenseID = (int)reader["LicenseID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsReleased = (bool)reader["IsReleased"];
                            ReleaseDate = reader["ReleaseDate"] != DBNull.Value ? (DateTime)reader["ReleaseDate"] : DateTime.MaxValue;
                            ReleasedByUserID = reader["ReleasedByUserID"] != DBNull.Value ? (int)reader["ReleasedByUserID"] : -1;
                            ReleaseApplicationID = reader["ReleaseApplicationID"] != DBNull.Value ? (int)reader["ReleaseApplicationID"] : -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
           
            return isFound;
        }

        public static bool GetDetainInfoByLicenseID(
         int LicenseID, ref int DetainID, ref DateTime DetainDate, ref float FineFees,
           ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
           ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            string query = "SELECT * FROM DetainedLicenses WHERE LicenseID = @LicenseID";



            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            // The record was found
                            isFound = true;

                            DetainID = (int)reader["DetainID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsReleased = (bool)reader["IsReleased"];
                            ReleaseDate = reader["ReleaseDate"] != DBNull.Value ? (DateTime)reader["ReleaseDate"] : DateTime.MaxValue;
                            ReleasedByUserID = reader["ReleasedByUserID"] != DBNull.Value ? (int)reader["ReleasedByUserID"] : -1;
                            ReleaseApplicationID = reader["ReleaseApplicationID"] != DBNull.Value ? (int)reader["ReleaseApplicationID"] : -1;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
          
            return isFound;
        }

        public static DataTable GetAllDetains()
        {

            DataTable dt = new DataTable();

            string query = "SELECT * FROM DetainedLicenses_View order by FullName";

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
                // Console.WriteLine("Error: " + ex.Message);
            }
            return dt;
        }

        public static int AddNewDetainedLicense(
            int LicenseID, DateTime DetainDate,
            float FineFees, int CreatedByUserID)
        {
            int DetainID = -1;


            string query = @"INSERT INTO dbo.DetainedLicenses
                               (LicenseID,
                               DetainDate,
                               FineFees,
                               CreatedByUserID,
                               IsReleased
                               )
                            VALUES
                               (@LicenseID,
                               @DetainDate, 
                               @FineFees, 
                               @CreatedByUserID,
                               0
                             );
                            
                            SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {

                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    command.Parameters.AddWithValue("@DetainDate", DetainDate);
                    command.Parameters.AddWithValue("@FineFees", FineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        DetainID = insertedID;
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return DetainID;

        }

        public static bool UpdateDetainedLicense(int DetainID,
           int LicenseID, DateTime DetainDate,
           float FineFees, int CreatedByUserID)
        {

            int rowsAffected = 0;

            string query = @"UPDATE dbo.DetainedLicenses
                              SET LicenseID = @LicenseID, 
                              DetainDate = @DetainDate, 
                              FineFees = @FineFees,
                              CreatedByUserID = @CreatedByUserID,   
                              WHERE DetainID=@DetainID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Parameters.AddWithValue("@DetainedLicenseID", DetainID);
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    command.Parameters.AddWithValue("@DetainDate", DetainDate);
                    command.Parameters.AddWithValue("@FineFees", FineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            return (rowsAffected > 0);
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;

            string query = @" select 1 from DetainedLicenses
                 where LicenseID = @LicenseID and IsReleased = 0;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    IsDetained = (result != null);
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                IsDetained = false;
            }

            return IsDetained;
        }


        public static bool ReleaseDetainedLicense(int DetainID,
                 int ReleasedByUserID, int ReleaseApplicationID)
        {

            int rowsAffected = 0;

            string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleaseApplicationID = @ReleaseApplicationID   
                              WHERE DetainID=@DetainID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainID", DetainID);
                    command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
                    command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
               
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            return (rowsAffected > 0);
        }

    }
}
