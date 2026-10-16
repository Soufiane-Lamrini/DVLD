using DVLD_DataAccess;

using System.Data;

namespace Business_Layer
{
    public class clsUser
    {
        public enum enMode { enAdd = 1, enUpdate = 2 };

        public int UserID { private set; get; }
        public int PersonID { set; get; }

        public clsPerson person { set; get; }
        public string UserName { set; get; }
        public string PassWord { set; get; }
        public bool IsActive { set; get; }
       
        public enMode Mode= enMode.enAdd;


        public clsUser()
        {
            Mode = enMode.enAdd;

            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.PassWord = "";
           
        }

        private clsUser(int id, int PersonID, string UserName, string PassWord, bool IsActive)
        {

            this.UserID = id;
            this.PersonID = PersonID;
            this.person = clsPerson.Find(PersonID);
            this.UserName = UserName;
            this.PassWord = PassWord;
            this.IsActive = IsActive;
            Mode = enMode.enUpdate;
            
        }
        public static clsUser FindByUserID(int userID)
        {
            int PersonID = -1;
            string  UserName = "", PassWord = "";
            bool IsActive = false;
            if (clsUserData.GetUserInfoByUserID(userID, ref PersonID, ref UserName, ref PassWord, ref IsActive))

                return new clsUser(userID, PersonID, UserName, PassWord, IsActive);
            else
                return null;

        }

        public static clsUser FindByPersonID(int personID)
        {
            int UserID = -1;
            string PassWord = "", userName = "";
            bool IsActive = false;
            if (clsUserData.GetUserInfoByPersonID(personID, ref UserID, ref userName, ref PassWord, ref IsActive))

                return new clsUser(UserID, personID, userName, PassWord, IsActive);
            else
                return null;

        }

        public static clsUser FindUserByUserName(string UserName)
        {
            int UserID = -1, PersonID = -1;
            string password = "";
            bool IsActive = false;
            if (clsUserData.GetUserInfoByUserName(UserName, ref UserID, ref PersonID,ref password, ref IsActive))

                return new clsUser(UserID, PersonID, UserName, password, IsActive);
            else
                return null;

        }
        public static clsUser FindUserByUserNameAndPassword(string UserName, string password)
        {
            int UserID = -1, PersonID = -1;
            bool IsActive = false;
            if (clsUserData.GetUserInfoByUserNameAndPassword(UserName,  password, ref UserID, ref PersonID, ref IsActive))

                return new clsUser(UserID, PersonID, UserName, password, IsActive);
            else
                return null;

        }
        private bool _AddNewUser()
        {

            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.PassWord, this.IsActive);

            return (this.UserID != -1);

        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.PassWord, this.IsActive);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.enAdd:
                    if (_AddNewUser())
                    {
                        Mode = enMode.enUpdate;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.enUpdate:
                    if (_UpdateUser())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }

            return false;
        }

        public static bool DeleteUser(int ID)
        {
            return clsUserData.DeleteUser(ID);
        }



        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUserData.IsUserExistByUserID(UserID);
        }

        public static bool IsUserExist(string UserName)
        {
            return clsUserData.IsUserExistByUserName(UserName);
        }

        public static bool IsUserExistByPersonId(int personID)
        {
            return clsUserData.IsUserExistByPersonID(personID);
        }

    }
}
