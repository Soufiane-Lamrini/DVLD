using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class clsTestData
    {
        public static bool GetTestInfoByID(
           int TestID, ref int TestAppointmentID, ref bool TestResult,
            ref string Notes,  ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestID", TestID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;

                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];
                            Notes = (string)reader["Notes"];
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

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass
              (int PersonID, int LicenseClassID, int TestTypeID, ref int TestID,
                ref int TestAppointmentID, ref bool TestResult,
                ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT  top 1 Tests.TestID, 
                Tests.TestAppointmentID, Tests.TestResult, 
			    Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                FROM            LocalDrivingLicenseApplications INNER JOIN
                                         Tests INNER JOIN
                                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                         Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                WHERE        (Applications.ApplicantPersonID = @PersonID) 
                        AND (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                        AND ( TestAppointments.TestTypeID=@TestTypeID)
                ORDER BY Tests.TestAppointmentID DESC";


           

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {

                            // The record was found
                            isFound = true;
                            TestID = (int)reader["TestID"];
                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];
                            if (reader["Notes"] == DBNull.Value)

                                Notes = "";
                            else
                                Notes = (string)reader["Notes"];

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

        public static DataTable GetAllTests()
        {

            DataTable dt = new DataTable();

            string query = "SELECT * FROM Tests order by TestID";

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

        public static int AddNewTest( int TestAppointmentID,bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            string query = @"Insert Into Tests (TestAppointmentID,TestResult,
                                                Notes,   CreatedByUserID)
                            Values (@TestAppointmentID,@TestResult,
                                                @Notes,   @CreatedByUserID);
                            
                                UPDATE TestAppointments 
                                SET IsLocked=1 where TestAppointmentID = @TestAppointmentID;

                                SELECT SCOPE_IDENTITY();";

            
           
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);
                    command.Parameters.AddWithValue("@Notes", Notes);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null &&
                        int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Pour voir l'erreur SQL pendant le développement
                //Console.WriteLine(ex.Message);

                return -1;
            }

            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult,
            string Notes, int CreatedByUserID)
        {

            int rowsAffected = 0;

            string query = @"Update  Tests  
                            set TestAppointmentID =@TestAppointmentID, 
                                TestResult = @TestResult,
                                Notes = @Notes,
                                CreatedByUserID = @CreatedByUserID  
                            where TestID=@TestID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestID", TestID);
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);
                    command.Parameters.AddWithValue("@Notes", Notes);
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


        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;

            string query = @"SELECT PassedTestCount = count(TestTypeID)
                         FROM Tests INNER JOIN
                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
						 where LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID and TestResult=1";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                    {
                        PassedTestCount = ptCount;
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return PassedTestCount;

        }


    }
}
