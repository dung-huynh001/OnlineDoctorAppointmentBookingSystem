using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Appointment.History
{
    public class HistoryViewModel
    {
        public int appointmentID { get; set; }
        public string doctorName { get; set; }
        public string consultationDay { get; set; }
        public string dateOfConsultation { get; set; }
        public string appointmentStatus { get; set; }
        public string consultationTime { get; set; }
    }
}