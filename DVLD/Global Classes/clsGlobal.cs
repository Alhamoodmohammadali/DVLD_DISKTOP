using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Buisness;
using Microsoft.Win32;

namespace DVLD.Classes
{
    public static class clsGlobal
    {
        public static clsUser CurrentUser;
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            string keyPath = @"SOFTWARE\YourSoftware";
            string _Username = "Username";
            string _Password = "Password";
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (RegistryKey key = baseKey.OpenSubKey(keyPath, true))
                    {
                        if ((key != null) && (Username != null || Username != string.Empty || Password != null || Password != string.Empty))
                        {
                            key.DeleteValue(_Username);
                            key.DeleteValue(_Password);
                        }
                        else
                        {
                            keyPath = @"HKEY_CURRENT_USER\SOFTWARE\YourSoftware";
                            try
                            {
                                // Write the value to the Registry
                                Registry.SetValue(keyPath, _Username, Username, RegistryValueKind.String);
                                Registry.SetValue(keyPath, _Password, Password, RegistryValueKind.String);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"An error occurred 1 : {ex.Message}");
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred 2 : {ex.Message}");
                return false;
            }
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\YourSoftware";
            string _Username = "Username";
            string _Password = "Password";
            try
            {
                // Read the value from the Registry
                string _User = Registry.GetValue(keyPath, _Username, null) as string;
                string _Pass = Registry.GetValue(keyPath, _Password, null) as string;
                if (_User != null && _Pass != null)
                {
                    Username = _User;
                    Password = _Pass;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred3: {ex.Message}");
                return false;

            }
        }
    }
}
