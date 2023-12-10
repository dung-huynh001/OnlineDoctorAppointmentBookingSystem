using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.HelperClasses
{
    public class Config
    {
        private static string salt;
        public static string Salt
        {
            get
            {
                return salt == null ? string.Empty : salt;
            }
            set
            {
                salt = value;
            }
        }
    }
}