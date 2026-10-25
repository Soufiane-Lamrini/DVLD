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
    public static class clsTestAppointmentsData
    {
        public static bool GetTestAppointmentInfoByID(
           int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID,
             ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;

                            TestTypeID = (int)reader["TestTypeID"];
                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = (bool)reader["IsLocked"];
                            RetakeTestApplicationID = reader["RetakeTestApplicationID"] != DBNull.Value ? (int)reader["RetakeTestApplicationID"] : -1;
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

        public static bool GetLastTestAppointment(
           int LocalDrivingLicenseApplicationID, int TestTypeID,
          ref int TestAppointmentID, ref DateTime AppointmentDate,
          ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString);

            string query = @"SELECT       top 1 *
                FROM            TestAppointments
                WHERE        (TestTypeID = @TestTypeID) 
                AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                order by TestAppointmentID Desc";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                        RetakeTestApplicationID = -1;
                    else
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];


                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllTestAppointments()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString);

            string query = @"select * from TestAppointments_View
                                  order by AppointmentDate Desc";


            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }

        public static DataTable GetTestAppointmentsPerTestTypes(int LDLAppID, int TestTypeID)
        {

            DataTable dt = new DataTable();

            string query = @"SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked
                              FROM TestAppointments where LocalDrivingLicenseApplicationID=@LDLAppID 
                                    And TestTypeID = @TestTypeID";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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

        public static int AddNewTestAppointment( int TestTypeID, int LocalDrivingLicenseApplicationID,
                DateTime AppointmentDate, float PaidFees, int CreatedByUserID,  bool IsLocked,
                    int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            string query = @"INSERT INTO TestAppointments
                    (
                        TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate,
                        PaidFees, CreatedByUserID, IsLocked,  RetakeTestApplicationID
                    )
                    VALUES
                    (
                        @TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate,
                        @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestApplicationID
                    );

                    SELECT SCOPE_IDENTITY();";



            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {


                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@IsLocked", IsLocked);

                    if (RetakeTestApplicationID == -1)
                    {
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
                    }

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null &&
                        int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestAppointmentID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Pour voir l'erreur SQL pendant le développement
                //Console.WriteLine(ex.Message);

                return -1;
            }
           
            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment( int TestAppointmentID,
            int TestTypeID, int LocalDrivingLicenseApplicationID,
           DateTime AppointmentDate, float PaidFees, int CreatedByUserID,
             bool IsLocked, int RetakeTestApplicationID)
        {

            int rowsAffected = 0;

            string query = @"Update  TestAppointments  
                            set TestTypeID =@TestTypeID, 
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                AppointmentDate = @AppointmentDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID,
                                IsLocked = @IsLocked,
                                RetakeTestApplicationID = @RetakeTestApplicationID   
                            where TestAppointmentID=@TestAppointmentID";


          

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@IsLocked", IsLocked);
                    if (RetakeTestApplicationID == -1)
                    {
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
                    }

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

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;

            string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestID = insertedID;
                    }
                }             
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return TestID;

        }

    }
}
