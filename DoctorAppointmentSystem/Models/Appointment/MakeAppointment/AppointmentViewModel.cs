using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Appointment.MakeAppointment
{
    public class AppointmentViewModel
    {
        public int doctorID { get; set; }
        public int patientID { get; set; }
        public int scheduleID { get; set; }
        public string doctorName { get; set; }
        public string doctorGender { get; set; }
        public string doctorSpeciality { get; set; }
        public string patientName { get; set; }
        public string patientGender { get; set; }
        public string patientDateOfBirth { get; set; }
        public string consultantTime { get; set; }
        public int consultantType { get; set; }
        public int modeOfConsultant { get; set; }
        public string workingDay { get; set; }
        public string symtoms { get; set; }
        public string existingIllness { get; set; }
        public string drugAlergies { get; set; }
        public string appointmentDate { get; set; }
        public string appointmentTime { get; set; }
        public string dateOfConsultation { get; set; }
        public string appointmentStatus { get; set; }
        public string closeDate { get; set; }
        public string closeBy { get; set; }
        public string note { get; set; }
        public string caseNote { get; set; }
        public string diagnosis { get; set; }
        public string adviceToPatient { get; set; }
        public string labTests { get; set; }
        public string consultationTime { get; set; }
    }
}