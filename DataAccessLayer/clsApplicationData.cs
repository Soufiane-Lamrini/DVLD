using DVLD_DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class clsApplicationData
    {
        public static bool GetApplicationInfoByApplicationID(int ApplicationID, ref int PersonID, ref int ApplicationTypeID,
     ref DateTime DateDemande, ref byte Status, ref DateTime LastStatusDate, ref float PaidFees, ref int CreatedByUserID)
        {
            bool IsFound = false;

            string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            PersonID = (int)reader["ApplicantPersonID"];
                            DateDemande = (DateTime)reader["ApplicationDate"];
                            ApplicationTypeID = (int)reader["ApplicationTypeID"];
                            Status = (byte)reader["ApplicationStatus"];
                            LastStatusDate = (DateTime)reader["LastStatusDate"];
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                IsFound = false;
            }

            return IsFound;
        }
        public static int AddApplication(int PersonID, int ApplicationTypeID, DateTime DateDemande,
                  byte Status, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int ApplicationID = -1;


            string query = @"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID) 
                                    VALUES (@AppPersonID, @AppDate, @AppTypeID, @AppStatus, @LastStatusDate, @PaidFees, @UserID);
                                SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AppPersonID", PersonID);
                    command.Parameters.AddWithValue("@AppDate", DateDemande);
                    command.Parameters.AddWithValue("@AppTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@AppStatus", Status);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@UserID", CreatedByUserID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        ApplicationID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return ApplicationID;
        }

        public static bool UpdateApplication(int AppID, int PersonID, int ApplicationTypeID, DateTime DateDemande,
                  byte Status, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int rowsAffected = 0;


            string query = @"UPDATE Applications
                            SET ApplicantPersonID = @AppPersonID,
                                ApplicationDate = @AppDate, 
                                ApplicationTypeID = @AppTypeID,
                                ApplicationStatus = @AppStatus,
                                LastStatusDate = @LastStatusDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID
                               
                            WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", AppID);
                    command.Parameters.AddWithValue("@AppPersonID", PersonID);
                    command.Parameters.AddWithValue("@AppDate", DateDemande);
                    command.Parameters.AddWithValue("@AppTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@AppStatus", Status);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            int rowsAffected = 0;


            string query = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            

            return (rowsAffected > 0);
        }

        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT * from Applications";


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
                Console.WriteLine(ex.ToString());
            }
           
            return dt;
        }

        public static bool IsApplicationExistByApplicationID(int ApplicationID)
        {
            bool isFound = false;

            string query = "SELECT 1 FROM Applications WHERE ApplicationID = @ApplicationID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    isFound = (result != null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                isFound = false;
            }
            

            return isFound;
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {

            //incase the ActiveApplication ID !=-1 return true.
            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);
        }

        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;

            string query = "SELECT ActiveApplicationID=ApplicationID FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID and ApplicationTypeID=@ApplicationTypeID and ApplicationStatus=1";



            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    connection.Open();
                    object result = command.ExecuteScalar();


                    if (result != null && int.TryParse(result.ToString(), out int AppID))
                    {
                        ActiveApplicationID = AppID;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return ActiveApplicationID;
            }
           

            return ActiveApplicationID;
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            string query = @"SELECT ActiveApplicationID=Applications.ApplicationID  
                            From
                            Applications INNER JOIN
                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE ApplicantPersonID = @ApplicantPersonID 
                            and ApplicationTypeID=@ApplicationTypeID 
							and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                            and ApplicationStatus=1";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    connection.Open();
                    object result = command.ExecuteScalar();


                    if (result != null && int.TryParse(result.ToString(), out int AppID))
                    {
                        ActiveApplicationID = AppID;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return ActiveApplicationID;
            }

            return ActiveApplicationID;
        }

        public static bool UpdateStatus(int ApplicationID, byte NewStatus)
        {

            int rowsAffected = 0;

            string query = @"Update  Applications  
                            set 
                                ApplicationStatus = @NewStatus, 
                                LastStatusDate = @LastStatusDate
                            where ApplicationID=@ApplicationID;";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                   
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@NewStatus", NewStatus);
                    command.Parameters.AddWithValue("LastStatusDate", DateTime.Now);

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
