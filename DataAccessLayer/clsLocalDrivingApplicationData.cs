using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DVLD_DataAccess
{
    public class clsLocalDrivingLicenseApplicationData
    {

        public static bool GetLocalDrivingLicenseApplicationInfoByID(
            int LocalDrivingLicenseApplicationID, ref int ApplicationID,
            ref int LicenseClassID)
        {
            bool isFound = false;

            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            // The record was found
                            isFound = true;

                            ApplicationID = (int)reader["ApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];
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

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(
         int ApplicationID, ref int LocalDrivingLicenseApplicationID,
         ref int LicenseClassID)
        {
            bool isFound = false;

            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID";

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
                            // The record was found
                            isFound = true;

                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];
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

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {

            DataTable dt = new DataTable();

            string query = @"SELECT *
                              FROM LocalDrivingLicenseApplications_View
                              order by ApplicationDate Desc";


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

        public static int AddNewLocalDrivingLicenseApplication(
            int ApplicationID, int LicenseClassID)
        {

            //this function will return the new person id if succeeded and -1 if not.
            int LocalDrivingLicenseApplicationID = -1;

            string query = @"INSERT INTO LocalDrivingLicenseApplications ( 
                            ApplicationID,LicenseClassID)
                             VALUES (@ApplicationID,@LicenseClassID);
                             SELECT SCOPE_IDENTITY();";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        LocalDrivingLicenseApplicationID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return LocalDrivingLicenseApplicationID;
        }


        public static bool UpdateLocalDrivingLicenseApplication(
            int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            int rowsAffected = 0;

            string query = @"Update  LocalDrivingLicenseApplications  
                            set ApplicationID = @ApplicationID,
                                LicenseClassID = @LicenseClassID
                            where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

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


        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;

            string query = @"Delete LocalDrivingLicenseApplications 
                                where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
           
            return (rowsAffected > 0);
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;

            string query = @" SELECT top 1 TestResult
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";
           
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && bool.TryParse(result.ToString(), out bool returnedResult))
                    {
                        Result = returnedResult;
                    }
                }
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return Result;

        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)

        {
            bool IsFound = false;

            string query = @" SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        IsFound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return IsFound;

        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)

        {
            byte TotalTrialsPerTest = 0;

            string query = @" SELECT TotalTrialsPerTest = count(TestID)
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                       ";

           
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && byte.TryParse(result.ToString(), out byte Trials))
                    {
                        TotalTrialsPerTest = Trials;
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return TotalTrialsPerTest;

        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {

            bool Result = false;

            string query = @" SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)  
                            AND(TestAppointments.TestTypeID = @TestTypeID) and isLocked=0
                            ORDER BY TestAppointments.TestAppointmentID desc";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        Result = true;
                    }
                }
               
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return Result;
        }
    }
}
