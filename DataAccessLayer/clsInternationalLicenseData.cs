using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace DataAccessLayer
{
    public static class clsInternationalLicenseData
    {
        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID, ref int ApplicationID,
          ref int DriverID, ref int IssuedUsingLocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate,
           ref bool IsActive,ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            // The record was found
                            isFound = true;

                            ApplicationID = (int)reader["ApplicationID"];
                            DriverID = (int)reader["DriverID"];
                            IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                            IssueDate = (DateTime)reader["IssueDate"];
                            ExpirationDate = (DateTime)reader["ExpirationDate"];
                            IsActive = (bool)reader["IsActive"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];

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


        public static DataTable GetAllInternationalLicenses()
        {

            DataTable dt = new DataTable();

            string query = @"SELECT    InternationalLicenseID, ApplicationID,DriverID, +
                              IssuedUsingLocalLicenseID , IssueDate,
                                ExpirationDate, IsActive
                                from InternationalLicenses 
                                order by IsActive, ExpirationDate desc";


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


        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {

            DataTable dt = new DataTable();

            string query = @"SELECT InternationalLicenseID
                      ,ApplicationID
                      ,IssuedUsingLocalLicenseID
                      ,IssueDate
                      ,ExpirationDate
                      ,IsActive
                  FROM InternationalLicenses where DriverID = @DriverID;
                ";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

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

        public static int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int InternationalLicenseID = -1;

            string query = @" Update InternationalLicenses 
                               set IsActive=0
                               where DriverID=@DriverID;
                            Insert Into InternationalLicenses (ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                            IssueDate, ExpirationDate, IsActive, CreatedByUserID)
                            Values (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate,
                               @ExpirationDate, @IsActive, @CreatedByUserID);
                            
                                SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null &&
                        int.TryParse(result.ToString(), out int insertedID))
                    {
                        InternationalLicenseID = insertedID;
                    }
                }

            }
            catch (Exception ex)
            {
                // Pour voir l'erreur SQL pendant le développement
                //Console.WriteLine(ex.Message);

                return -1;
            }
            return InternationalLicenseID;
        }

        public static bool UpdateInternationalLicense(int InternationalLicenseID, 
            int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {

            int rowsAffected = 0;

            string query = @"Update  InternationalLicenses  
                            set ApplicationID =@ApplicationID, 
                                DriverID = @DriverID,
                                IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                                IssueDate = @IssueDate,
                                ExpirationDate = @ExpirationDate,
                                IsActive = @IsActive,
                                CreatedByUserID = @CreatedByUserID
                            where InternationalLicenseID=@InternationalLicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
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


        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;


            string query = @"  
                            SELECT Top 1 InternationalLicenseID
                            FROM InternationalLicenses 
                            where DriverID=@DriverID and GetDate() between IssueDate and ExpirationDate 
                            order by ExpirationDate Desc;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        InternationalLicenseID = insertedID;
                    }
                }              
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return InternationalLicenseID;
        }

       
    }
}
