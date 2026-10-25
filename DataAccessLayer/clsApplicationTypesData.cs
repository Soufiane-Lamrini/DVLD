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
    public static class clsApplicationTypesData
    {
        public static bool GetApplicationTypeInfoByApplicationTypeID(int ApplicationTypeID, ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            bool IsFound = false;

            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            IsFound = true;
                            ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                            ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
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

        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ApplicationTypeID = -1;


            string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)
                            
                            SELECT SCOPE_IDENTITY();";


           
            try
            {
                using (SqlConnection connection = new SqlConnection((clsDataAccessSetting.connectionString)))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                    command.Parameters.AddWithValue("@ApplicationFees", Fees);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        ApplicationTypeID = insertedID;
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }


            return ApplicationTypeID;

        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float Fees)
        {
            int rowsAffected = 0;

            string query = @"UPDATE ApplicationTypes
                            SET ApplicationTypeTitle = @ApplicationTypeTitle,
                                ApplicationFees = @ApplicationFees                               
                            WHERE ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection((clsDataAccessSetting.connectionString)))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
                    command.Parameters.AddWithValue("@ApplicationFees", Fees);

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

        public static DataTable GetAllApplicationsTypes()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT * FROM ApplicationTypes order by ApplicationTypeTitle";

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
