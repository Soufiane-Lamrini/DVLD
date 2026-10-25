using System;
using System.Data;
using System.Data.SqlClient;
namespace DVLD_DataAccess
{
    public static class clsUserData
    {
        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string userName, ref string password, ref bool IsActive)
        {
            bool IsFound = false;

            string query = "Select * from Users Where UserID = @UserID";



            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            IsFound = true;
                            PersonID = (int)reader["PersonID"];
                            userName = (string)reader["UserName"];
                            password = (string)reader["Password"];
                            IsActive = (bool)reader["IsActive"];

                        }
                    }
                }
               
            }
            catch (Exception ex)
            {

                IsFound = false;
            }
           
            return IsFound;

        }

        public static bool GetUserInfoByPersonID(int PersonID, ref int UserID, ref string userName, ref string password, ref bool IsActive)
        {
            bool IsFound = false;

            string query = "Select * from Users Where UserName = @UserName";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            IsFound = true;
                            UserID = (int)reader["UserID"];
                            PersonID = (int)reader["PersonID"];
                            password = (string)reader["Password"];
                            IsActive = (bool)reader["IsActive"];

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                IsFound = false;
            }

            return IsFound;

        }

        public static bool GetUserInfoByUserName(string userName, ref int UserID, ref int PersonID, ref string password, ref bool IsActive)
        {
            bool IsFound = false;

            string query = "Select * from Users Where UserName = @UserName";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            UserID = (int)reader["UserID"];
                            PersonID = (int)reader["PersonID"];
                            password = (string)reader["Password"];
                            IsActive = (bool)reader["IsActive"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                IsFound = false;
            }
          
            return IsFound;

        }


        public static bool GetUserInfoByUserNameAndPassword(string userName, string password, ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool IsFound = false;

            string query = "Select * from Users Where UserName = @UserName and Password = @Password";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            IsFound = true;
                            UserID = (int)reader["UserID"];
                            PersonID = (int)reader["PersonID"];
                            IsActive = (bool)reader["IsActive"];

                        }
                    }
                }
               
             
            }
            catch (Exception ex)
            {

                IsFound = false;
            }

            return IsFound;

        }


        public static int AddNewUser(int PersonID, string userName, string password, bool IsActive)
        {
            int UserID = -1;

            string query = @"INSERT INTO Users (PersonID, UserName, PassWord, IsActive) VALUES (@PersonID, @UserName, @PassWord, @IsActive);
                                        Select SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@PassWord", password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    connection.Open();

                    object result = command.ExecuteScalar();


                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        UserID = insertedID;
                    }
                }

            }
            catch (Exception ex)
            {

            }
           
            return UserID;
        }



        public static bool UpdateUser(int UserID, int PersonID, string userName, string password, bool IsActive)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Users
                            SET PersonID = @PersonID,
                                UserName = @UserName, 
                                PassWord = @PassWord,
                                IsActive = @IsActive
                            WHERE UserID = @userID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userID", UserID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@PassWord", password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }               
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
           
            return (rowsAffected > 0);

        }

        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            string query = "Delete from Users WHERE UserID = @UserID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
               
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
           
            return (rowsAffected > 0);

        }


        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT  Users.UserID, Users.PersonID,
                            FullName = People.FirstName + ' ' + ISNULL( People.SecondName,'') + ' ' + ISNULL( People.ThirdName,'') +' ' + People.LastName,
                             Users.UserName, Users.IsActive
                             FROM  Users INNER JOIN
                                    People ON Users.PersonID = People.PersonID";

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
            }
           
            return dt;
        }

        public static bool IsUserExistByUserID(int UserID)
        {
            bool isFound = false;

            string query = @"Select found = 1 from Users 
                                where UserID = @UserID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        isFound = true;
                    }
                    else
                    {
                        isFound = false;
                    }
                }
               
            }
            catch (Exception ex)
            {
                isFound = false;
            }

            return isFound;
        }

        public static bool IsUserExistByUserName(string UserName)
        {
            bool isFound = false;


            string query = @"Select found = 1 from Users 
                                where UserName = @UserName";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        isFound = true;
                    }
                    else
                    {
                        isFound = false;
                    }
                }
               
            }
            catch (Exception ex)
            {
                isFound = false;
            }
        
            return isFound;
        }

        public static bool IsUserExistByPersonID(int personID)
        {
            bool isFound = false;

            string query = @"Select found = 1 from Users 
                                where PersonID = @personID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@personID", personID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        isFound = true;
                    }
                    else
                    {
                        isFound = false;
                    }
                }
             
            }
            catch (Exception ex)
            {
                isFound = false;
            }
          
            return isFound;
        }


    }
}
