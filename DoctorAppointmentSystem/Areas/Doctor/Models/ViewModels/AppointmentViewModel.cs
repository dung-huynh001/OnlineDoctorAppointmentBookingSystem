using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public int appointmentID { get; set; }
        public int patientID { get; set; }
        public string patientName { get; set; }
        public string patientGender { get; set; }
        public string patientDateOfBirth { get; set; }
        public string dateOfConsultation { get; set; }
        public string consultationTime { get; set; }
        public string consultationDay { get; set; }
        public string appointmentDate { get; set; }
        public string appointmentTime { get; set; }
        public string appointmentDay { get; set; }
        public string appointmentStatus { get; set; }
    }
}