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
    public static class clsLicenseData
    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, 
            ref int DriverID, ref int LicenseClass, ref DateTime IssueDate, ref DateTime ExpirationDate,
            ref string Notes, ref float PaidFees, ref bool IsActive, 
            ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";


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

                            ApplicationID = (int)reader["ApplicationID"];
                            DriverID = (int)reader["DriverID"];
                            LicenseClass = (int)reader["LicenseClass"];
                            IssueDate = (DateTime)reader["IssueDate"];
                            ExpirationDate = (DateTime)reader["ExpirationDate"];
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "";
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            IsActive = (bool)reader["IsActive"];
                            IssueReason = (byte)reader["IssueReason"];
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

     
        public static DataTable GetAllLicenses()
        {

            DataTable dt = new DataTable();

            string query = "SELECT * FROM Licenses order by LicenseID";

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


        public static DataTable GetDriverLicenses(int DriverID)
        {

            DataTable dt = new DataTable();

            string query = @"SELECT     
                           Licenses.LicenseID,
                           ApplicationID,
		                   LicenseClasses.ClassName, Licenses.IssueDate, 
		                   Licenses.ExpirationDate, Licenses.IsActive
                           FROM Licenses INNER JOIN
                                LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID=@DriverID
                            Order By IsActive Desc, ExpirationDate Desc";


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

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass, 
            DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees, 
            bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;

            string query = @"Insert Into Licenses (ApplicationID, DriverID, LicenseClass,
                            IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason,
                            CreatedByUserID)
                            Values (@ApplicationID, @DriverID, @LicenseClass, @IssueDate,
                               @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason,
                                    @CreatedByUserID);
                            
                                SELECT SCOPE_IDENTITY();";


           
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                    command.Parameters.AddWithValue("@Notes", Notes != "" ? Notes : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        LicenseID = insertedID;
                    }
                }
               
            }
            catch (Exception ex)
            {
                // Pour voir l'erreur SQL pendant le développement
                //Console.WriteLine(ex.Message);

                return -1;
            }
           
            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees,
            bool IsActive, int IssueReason, int CreatedByUserID)
        {

            int rowsAffected = 0;

            string query = @"Update  Licenses  
                            set ApplicationID =@ApplicationID, 
                                DriverID = @DriverID,
                                LicenseClass = @LicenseClass,
                                IssueDate = @IssueDate,
                                ExpirationDate = @ExpirationDate,
                                Notes = @Notes,
                                PaidFees = @PaidFees,
                                IsActive = @IsActive,
                                IssueReason = @IssueReason,
                                CreatedByUserID = @CreatedByUserID
                            where LicenseID=@LicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                    command.Parameters.AddWithValue("@Notes", Notes);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
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


        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            string query = @"SELECT        Licenses.LicenseID
                            FROM Licenses INNER JOIN
                                                     Drivers ON Licenses.DriverID = Drivers.DriverID
                            WHERE  
                             
                             Licenses.LicenseClass = @LicenseClass 
                              AND Drivers.PersonID = @PersonID
                              And IsActive=1;";

           

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        LicenseID = insertedID;
                    }
                }
                
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {

            int rowsAffected = 0;

            string query = @"UPDATE Licenses
                           SET 
                              IsActive = 0
                             
                         WHERE LicenseID=@LicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

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
