using System;
using System.Text;
using System.Security.Cryptography;
namespace DVLD.Global_Classes
{
    public class Securty
    {
        static public string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
