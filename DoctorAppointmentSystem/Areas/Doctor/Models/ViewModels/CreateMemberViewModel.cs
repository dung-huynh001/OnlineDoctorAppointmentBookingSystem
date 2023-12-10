using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels
{
    public class CreateMemberViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string nationalID { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime appointmentDate { get; set; }
        public TimeSpan appointmentTime { get; set; }
        public int modeOfConsultant { get; set; }
        public int consultantType { get; set; }
    }
}