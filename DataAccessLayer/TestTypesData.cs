using DVLD_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayer
{
    public static class TestTypesData
    {
        public static bool GetTestTypeInfoByTestTypeID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription, ref float TestFees)
        {
            bool IsFound = false;

            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            TestTypeTitle = (string)reader["TestTypeTitle"];
                            TestTypeDescription = (string)reader["TestTypeDescription"];
                            TestFees = Convert.ToSingle(reader["TestTypeFees"]);
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                IsFound = false;
            }
          
            return IsFound;
        }

        public static int AddNewTestType(string Title, string TestTypeDescription, float Fees)
        {
            int TestTypeID = -1;

            string query = @"Insert Into TestTypes (TestTypeTitle, TestTypeDescription, TestFees)
                            Values (@Title, @Description, @Fees)
                            
                            SELECT SCOPE_IDENTITY();";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Description", TestTypeDescription);
                    command.Parameters.AddWithValue("@Fees", Fees);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestTypeID = insertedID;
                    }
                }            
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return TestTypeID;

        }

        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, float Fees)
        {
            int rowsAffected = 0;

            string query = @"UPDATE TestTypes
                            SET TestTypeTitle = @TestTypeTitle,
                                TestTypeDescription = @TestTypeDescription,               
                                TestTypeFees = @TestFees                               
                            WHERE TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString)) 
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
                    command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
                    command.Parameters.AddWithValue("@TestFees", Fees);
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

        public static DataTable GetAllTestsTypes()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT * FROM TestTypes order by TestTypeID";

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

    }
}
