using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account.Register
{
    public class PatientViewModel
    {
        public string fullName { get; set; }
        public string nationalID { get; set; }
        public string dateOfBirth { get; set; }
        public int gender { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
    }
}