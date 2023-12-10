using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels
{
    public class MemberDetailViewModel
    {
        public int appointmentID { get; set; }
        public string fullName { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string nationalID { get; set; }
        public string avatar { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string modeOfConsutant { get; set; }
        public string consultantType { get; set; }
        public string dateOfConsultation { get; set; }
        public string consultationTime { get; set; }
        public string appointmentDate { get; set; }
        public string appointmentTime { get; set; }
        public string symtoms { get; set; }
        public string existingIllness { get; set; }
        public string drugAlergies { get; set; }
    }
}