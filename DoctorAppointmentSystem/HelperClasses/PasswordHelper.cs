using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DoctorAppointmentSystem.HelperClasses
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            Config.Salt = "doctorappointmentsystem";
            using (var sha256 = SHA256.Create())
            {
                password = password + Config.Salt;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "");

                return hashedPassword;
            }
        }

        public static bool VerifyPassword(string password, string hashedpassword)
        {
            bool verified = false;
            password = HashPassword(password);
            if (password.Equals(hashedpassword.Trim()))
            {
                verified = true;
            }
            return verified;
        }
    }
}