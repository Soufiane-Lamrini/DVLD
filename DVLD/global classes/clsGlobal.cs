using Business_Layer;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace DVLD.Classes
{
    internal static  class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\Software\DVLD";

            try
            {
                Registry.SetValue(keyPath, "UserName", Username, RegistryValueKind.String);
                Registry.SetValue(keyPath, "Password", Password, RegistryValueKind.String);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }
        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                string keyPath = @"HKEY_CURRENT_USER\Software\DVLD";

                Username = Registry.GetValue(keyPath, "UserName", null) as string;
                Password = Registry.GetValue(keyPath, "Password", null) as string;

                return Username != null && Password != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
