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
    public static class clsDriverData
    {
        public static bool GetDriverInfoByID(
           int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;

                            PersonID = (int)reader["PersonID"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            CreatedDate = (DateTime)reader["CreatedDate"];
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

        public static bool GetDriverInfoByPersonID(
         int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";



            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())

                        if (reader.Read())
                        {

                            // The record was found
                            isFound = true;

                            DriverID = (int)reader["DriverID"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            CreatedDate = (DateTime)reader["CreatedDate"];
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

        public static DataTable GetAllDrivers()
        {

            DataTable dt = new DataTable();

            string query = "SELECT * FROM Drivers_View order by FullName";


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

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;   

            string query = @"Insert Into Drivers (PersonID, CreatedByUserID, CreatedDate)
                            Values (@PersonID, @CreatedByUserID, @CreatedDate);
                            
                                SELECT SCOPE_IDENTITY();";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null &&
                        int.TryParse(result.ToString(), out int insertedID))
                    {
                        DriverID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                // Pour voir l'erreur SQL pendant le développement
                //Console.WriteLine(ex.Message);

                return -1;
            }
           
            return DriverID;
        }

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {

            int rowsAffected = 0;

            string query = @"Update  Drivers  
                            set PersonID =@PersonID, 
                                CreatedByUserID = @CreatedByUserID
                            where DriverID=@DriverID";

            try
            {
              
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
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


    
    }
}
